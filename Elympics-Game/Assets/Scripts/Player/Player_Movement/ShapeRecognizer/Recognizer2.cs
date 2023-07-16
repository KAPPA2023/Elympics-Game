using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using WobbrockLib;
using WobbrockLib.Extensions;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class Recognizer2 : ScriptableObject
{
    #region Members

    public const int NumPoints = 64;
    private const float DX = 250f;
    public static readonly SizeF SquareSize = new SizeF(DX, DX);
    public static readonly double Diagonal = Math.Sqrt(DX * DX + DX * DX);
    public static readonly double HalfDiagonal = 0.5 * Diagonal;
    public static readonly PointF Origin = new PointF(0f, 0f);
    private static readonly double Phi = 0.5 * (-1.0 + Math.Sqrt(5.0)); // Golden Ratio

    // batch testing
    private const int NumRandomTests = 100;
    //public event ProgressEventHandler ProgressChangedEvent;

    //private Hashtable _gestures;
    private Dictionary<string, Unistroke> _gestures;

    #endregion

    #region Constructor

    public Recognizer2()
    {
        _gestures = new Dictionary<string, Unistroke>(256);
    }

    #endregion

    #region Recognition

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timepoints"></param>
    /// <param name="protractor"></param>
    /// <returns></returns>
    public NBestList Recognize(List<TimePointF> timepoints, bool protractor) // candidate points
    {

        double I = GeotrigEx.PathLength(timepoints) / (NumPoints - 1); // interval distance between points
        List<PointF> points = TimePointF.ConvertList(SeriesEx.ResampleInSpace(timepoints, I));
        double radians = GeotrigEx.Angle(GeotrigEx.Centroid(points), points[0], false);
        points = GeotrigEx.RotatePoints(points, -radians);
        points = GeotrigEx.ScaleTo(points, SquareSize);
        points = GeotrigEx.TranslateTo(points, Origin, true);
        List<double> vector = Unistroke.Vectorize(points); // candidate's vector representation


        NBestList nbest = ScriptableObject.CreateInstance<NBestList>();
        foreach (Unistroke u in _gestures.Values)
        {
            if (protractor) // Protractor extension by Yang Li (CHI 2010)
            {
                double[] best = OptimalCosineDistance(u.Vector, vector);
                double score = 1.0 / best[0];

                nbest.AddResult(u.Name, score, best[0], best[1]); // name, score, distance, angle
            }
            else // original $1 angular invariance search -- Golden Section Search (GSS)
            {
                double[] best = GoldenSectionSearch(
                    points, // to rotate
                    u.Points, // to match
                    GeotrigEx.Degrees2Radians(-45.0), // lbound
                    GeotrigEx.Degrees2Radians(+45.0), // ubound
                    GeotrigEx.Degrees2Radians(2.0) // threshold
                );

                double score = 1.0 - best[0] / HalfDiagonal;
                nbest.AddResult(u.Name, score, best[0], best[1]); // name, score, distance, angle
            }
        }

        nbest.SortDescending(); // sort descending by score so that nbest[0] is best result
        return nbest;
    }

    // From http://www.math.uic.edu/~jan/mcs471/Lec9/gss.pdf
    private double[] GoldenSectionSearch(List<PointF> pts1, List<PointF> pts2, double a, double b, double threshold)
    {
        double x1 = Phi * a + (1 - Phi) * b;
        List<PointF> newPoints = GeotrigEx.RotatePoints(pts1, x1);
        double fx1 = PathDistance(newPoints, pts2);

        double x2 = (1 - Phi) * a + Phi * b;
        newPoints = GeotrigEx.RotatePoints(pts1, x2);
        double fx2 = PathDistance(newPoints, pts2);

        double i = 2.0; // calls to pathdist
        while (Math.Abs(b - a) > threshold)
        {
            if (fx1 < fx2)
            {
                b = x2;
                x2 = x1;
                fx2 = fx1;
                x1 = Phi * a + (1 - Phi) * b;
                newPoints = GeotrigEx.RotatePoints(pts1, x1);
                fx1 = PathDistance(newPoints, pts2);
            }
            else
            {
                a = x1;
                x1 = x2;
                fx1 = fx2;
                x2 = (1 - Phi) * a + Phi * b;
                newPoints = GeotrigEx.RotatePoints(pts1, x2);
                fx2 = PathDistance(newPoints, pts2);
            }

            i++;
        }

        return new double[3]
            { Math.Min(fx1, fx2), GeotrigEx.Radians2Degrees((b + a) / 2.0), i }; // distance, angle, calls to pathdist
    }

    /// <summary>
    /// From Protractor by Yang Li, published at CHI 2010. See http://yangl.org/protractor/. 
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    private double[] OptimalCosineDistance(List<double> v1, List<double> v2)
    {
        double a = 0.0;
        double b = 0.0;
        for (int i = 0; i < Math.Min(v1.Count, v2.Count); i += 2)
        {
            a += v1[i] * v2[i] + v1[i + 1] * v2[i + 1];
            b += v1[i] * v2[i + 1] - v1[i + 1] * v2[i];
        }

        double angle = Math.Atan(b / a);
        double distance = Math.Acos(a * Math.Cos(angle) + b * Math.Sin(angle));
        return new double[3] { distance, GeotrigEx.Radians2Degrees(angle), 0.0 }; // distance, angle, calls to pathdist
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path1"></param>
    /// <param name="path2"></param>
    /// <returns></returns>
    public static double PathDistance(List<PointF> path1, List<PointF> path2)
    {
        double distance = 0;
        for (int i = 0; i < Math.Min(path1.Count, path2.Count); i++)
        {
            distance += GeotrigEx.Distance(path1[i], path2[i]);
        }

        return distance / path1.Count;
    }

    // continues to rotate 'pts1' by 'step' degrees as long as points become ever-closer 
    // in path-distance to pts2. the initial distance is given by D. the best distance
    // is returned in array[0], while the angle at which it was achieved is in array[1].
    // array[3] contains the number of calls to PathDistance.
    private double[] HillClimbSearch(List<PointF> pts1, List<PointF> pts2, double D, double step)
    {
        double i = 0.0;
        double theta = 0.0;
        double d = D;
        do
        {
            D = d; // the last angle tried was better still
            theta += step;
            List<PointF> newPoints = GeotrigEx.RotatePoints(pts1, GeotrigEx.Degrees2Radians(theta));
            d = PathDistance(newPoints, pts2);
            i++;
        } while (d <= D);

        return new double[3] { D, theta - step, i }; // distance, angle, calls to pathdist
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pts1"></param>
    /// <param name="pts2"></param>
    /// <param name="writer"></param>
    /// <returns></returns>
    private double[] FullSearch(List<PointF> pts1, List<PointF> pts2, StreamWriter writer)
    {
        double bestA = 0d;
        double bestD = PathDistance(pts1, pts2);

        for (int i = -180; i <= +180; i++)
        {
            List<PointF> newPoints = GeotrigEx.RotatePoints(pts1, GeotrigEx.Degrees2Radians(i));
            double d = PathDistance(newPoints, pts2);
            if (writer != null)
            {
                writer.WriteLine("{0}\t{1:F3}", i, Math.Round(d, 3));
            }

            if (d < bestD)
            {
                bestD = d;
                bestA = i;
            }
        }

        writer.WriteLine("\nFull Search (360 rotations)\n{0:F2}{1}\t{2:F3} px", Math.Round(bestA, 2), (char)176,
            Math.Round(bestD, 3)); // calls, angle, distance
        return new double[3] { bestD, bestA, 360.0 }; // distance, angle, calls to pathdist
    }

    #endregion

    #region Gestures & Xml

    public int NumGestures
    {
        get { return _gestures.Count; }
    }

    public List<Unistroke> Gestures
    {
        get
        {
            List<Unistroke> list = new List<Unistroke>(_gestures.Values);
            list.Sort();
            return list;
        }
    }

    public void ClearGestures()
    {

        _gestures.Clear();
    }

    public bool SaveGesture(string filename, List<TimePointF> points)
    {
        // add the new prototype with the name extracted from the filename.
        string name = Unistroke.ParseName(filename);
        if (_gestures.ContainsKey(name))
            _gestures.Remove(name);
        Unistroke newPrototype = new Unistroke(name, points);
        _gestures.Add(name, newPrototype);

        // do the xml writing
        bool success = true;
        XmlTextWriter writer = null;
        try
        {
            // save the prototype as an Xml file
            writer = new XmlTextWriter(filename, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Gesture");
            writer.WriteAttributeString("Name", name);
            writer.WriteAttributeString("NumPts", XmlConvert.ToString(points.Count));
            writer.WriteAttributeString("Millseconds",
                XmlConvert.ToString(points[points.Count - 1].Time - points[0].Time));
            writer.WriteAttributeString("AppName", Assembly.GetExecutingAssembly().GetName().Name);
            writer.WriteAttributeString("AppVer", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            writer.WriteAttributeString("Date", DateTime.Now.ToLongDateString());
            writer.WriteAttributeString("TimeOfDay", DateTime.Now.ToLongTimeString());

            // write out the raw individual points
            foreach (TimePointF p in points)
            {
                writer.WriteStartElement("Point");
                writer.WriteAttributeString("X", XmlConvert.ToString(p.X));
                writer.WriteAttributeString("Y", XmlConvert.ToString(p.Y));
                writer.WriteAttributeString("T", XmlConvert.ToString(p.Time));
                writer.WriteEndElement(); // <Point />
            }

            writer.WriteEndDocument(); // </Gesture>
        }
        catch (XmlException xex)
        {
            Console.Write(xex.Message);
            success = false;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            success = false;
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }

        return success; // Xml file successfully written (or not)
    }

    public bool LoadGesture()
    {
        bool success = true;
        string[] filePaths = Directory.GetFiles(Application.dataPath + "/SpellPatterns", "shape*");

        foreach (string filePath in filePaths)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(filePath);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                reader.MoveToContent();

                Unistroke p = ReadGesture(reader);

                // Usuń gesty o tej samej nazwie i dodaj prototypowy gest
                if (_gestures.ContainsKey(p.Name))
                    _gestures.Remove(p.Name);
                _gestures.Add(p.Name, p);
            }
            catch (XmlException xex)
            {
                Debug.WriteLine("Błąd XML: " + xex.Message);
                success = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Błąd: " + ex.Message);
                success = false;
            }
        }

        return success;
    }

    // assumes the reader has been just moved to the head of the content.
    private Unistroke ReadGesture(XmlTextReader reader)
    {
        Debug.Assert(reader.LocalName == "Gesture");
        string name = reader.GetAttribute("Name");

        List<TimePointF> points = new List<TimePointF>(XmlConvert.ToInt32(reader.GetAttribute("NumPts")));

        reader.Read(); // advance to the first Point
        Debug.Assert(reader.LocalName == "Point");

        while (reader.NodeType != XmlNodeType.EndElement)
        {
            TimePointF p = TimePointF.Empty;
            p.X = XmlConvert.ToSingle(reader.GetAttribute("X"));
            p.Y = XmlConvert.ToSingle(reader.GetAttribute("Y"));
            p.Time = XmlConvert.ToInt64(reader.GetAttribute("T"));
            points.Add(p);
            reader.ReadStartElement("Point");
        }

        return new Unistroke(name, points);
    }

    #endregion

    #region Batch Processing

    /// <summary>
    /// Assemble the gesture filenames into categories that contain potentially multiple examples of the same gesture.
    /// </summary>
    /// <param name="filenames"></param>
    /// <returns>A 1-D list of category instances that each contain the same number of examples, or <b>null</b> if an
    /// error occurs.</returns>
    /// <remarks>See the comments above MainForm.TestBatch_Click.</remarks>
    public List<Category> AssembleBatch(string[] filenames)
    {
        Dictionary<string, Category> categories = new Dictionary<string, Category>();

        for (int i = 0; i < filenames.Length; i++)
        {
            string filename = filenames[i];

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(filename);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                reader.MoveToContent();

                Unistroke p = ReadGesture(reader);
                string catName = Category.ParseName(p.Name); // e.g., "circle"
                if (categories.ContainsKey(catName))
                {
                    Category cat = categories[catName];
                    cat.AddExample(p); // if the category has been made before, just add to it
                }
                else // create new category
                {
                    categories.Add(catName, new Category(catName, p));
                }
            }
            catch (XmlException xex)
            {
                Console.Write(xex.Message);
                categories.Clear();
                categories = null;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                categories.Clear();
                categories = null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        // now make sure that each category has the same number of elements in it
        List<Category> list = null;
        if (categories != null)
        {
            list = new List<Category>(categories.Values);
            int numExamples = list[0].NumExamples;
            foreach (Category c in list)
            {
                if (c.NumExamples != numExamples)
                {
                    Console.WriteLine("Different number of examples in gesture categories.");
                    list.Clear();
                    list = null;
                    break;
                }
            }
        }

        return list;
    }

    /// <summary>
    /// Tests an entire batch of files. See comments atop MainForm.TestBatch_Click().
    /// </summary>
    /// <param name="subject">Subject identification.</param>
    /// <param name="speed">"fast", "medium", or "slow"</param>
    /// <param name="categories">A list of gesture categories that each contain lists of prototypes (examples) within that gesture category.</param>
    /// <param name="dir">The directory into which to write the output files.</param>
    /// <param name="protractor">If true, uses Protractor instead of Golden Section Search.</param>
    /// <returns>The two filenames of the output file if successful; null otherwise. The main results are in string[0],
    /// while the detailed recognition results are in string[1].</returns>
    public string[] TestBatch(string subject, string speed, List<Category> categories, string dir, bool protractor)
    {
        StreamWriter mw = null; // main results writer
        StreamWriter dw = null; // detailed results writer
        string[] filenames = new string[2];
        try
        {
            // set up a main results file and detailed results file
            int start = Environment.TickCount;
            filenames[0] =
                String.Format("{0}\\$1({1})_main_{2}.txt", dir, protractor ? "protractor" : "gss",
                    start); // main results (small file)
            filenames[1] =
                String.Format("{0}\\$1({1})_nbest_{2}.txt", dir, protractor ? "protractor" : "gss",
                    start); // recognition details (large file)

            mw = new StreamWriter(filenames[0], false, Encoding.UTF8);
            mw.WriteLine("Subject = {0}, Recognizer = $1, Search = {1}, Speed = {2}, StartTime(ms) = {3}", subject,
                protractor ? "protractor" : "gss", speed, start);
            mw.WriteLine("Subject Recognizer Search Speed NumTraining GestureType RecognitionRate\n");

            dw = new StreamWriter(filenames[1], false, Encoding.UTF8);
            dw.WriteLine("Subject = {0}, Recognizer = $1, Search = {1}, Speed = {2}, StartTime(ms) = {3}", subject,
                protractor ? "protractor" : "gss", speed, start);
            dw.WriteLine("Correct? NumTrain Tested 1stCorrect Pts Ms Angle : (NBestNames) [NBestScores]\n");

            // determine the number of gesture categories and the number of examples in each one
            int numCategories = categories.Count;
            int numExamples = categories[0].NumExamples;
            double totalTests = (numExamples - 1) * NumRandomTests;

            // outermost loop: trains on N=1..9, tests on 10-N (for e.g., numExamples = 10)
            for (int n = 1; n <= numExamples - 1; n++)
            {
                // storage for the final avg results for each category for this N
                double[] results = new double[numCategories];

                // run a number of tests at this particular N number of training examples
                for (int r = 0; r < NumRandomTests; r++)
                {
                    _gestures.Clear(); // clear any (old) loaded prototypes

                    // load (train on) N randomly selected gestures in each category
                    for (int i = 0; i < numCategories; i++)
                    {
                        int[] chosen = RandomEx.Array(0, numExamples - 1, n, true); // select N unique indices
                        for (int j = 0; j < chosen.Length; j++)
                        {
                            Unistroke p = categories[i][chosen[j]]; // get the prototype from this category at chosen[j]
                            _gestures.Add(p.Name, p); // load the randomly selected test gestures into the recognizer
                        }
                    }

                    // testing loop on all unloaded gestures in each category. creates a recognition
                    // rate (%) by averaging the binary outcomes (correct, incorrect) for each test.
                    for (int i = 0; i < numCategories; i++)
                    {
                        // pick a random unloaded gesture in this category for testing.
                        // instead of dumbly picking, first find out what indices aren't
                        // loaded, and then randomly pick from those.
                        int[] notLoaded = new int[numExamples - n];
                        for (int j = 0, k = 0; j < numExamples; j++)
                        {
                            Unistroke g = categories[i][j];
                            if (!_gestures.ContainsKey(g.Name))
                                notLoaded[k++] = j; // jth gesture in categories[i] is not loaded
                        }

                        int chosen = RandomEx.Integer(0, notLoaded.Length - 1); // index
                        Unistroke p = categories[i][notLoaded[chosen]]; // gesture to test
                        Debug.Assert(!_gestures.ContainsKey(p.Name));

                        // do the recognition!
                        List<PointF> testPts = GeotrigEx.RotatePoints( // spin gesture randomly
                            TimePointF.ConvertList(p.RawPoints),
                            GeotrigEx.Degrees2Radians(RandomEx.Integer(0, 359))
                        );
                        NBestList result = this.Recognize(TimePointF.ConvertList(testPts), protractor);
                        string category = Category.ParseName(result.Name);
                        int correct = (category == categories[i].Name) ? 1 : 0;

                        dw.WriteLine("{0} {1} {2} {3} {4} {5} {6:F1}{7} : ({8}) [{9}]",
                            correct, // Correct?
                            n, // NumTrain 
                            p.Name, // Tested 
                            FirstCorrect(p.Name, result.Names), // 1stCorrect
                            p.RawPoints.Count, // Pts
                            p.Duration, // Ms 
                            Math.Round(result.Angle, 1), (char)176, // Angle tweaking : 
                            result.NamesString, // (NBestNames)
                            result.ScoresString); // [NBestScores]

                        results[i] += correct;
                    }

                    // provide feedback as to how many tests have been performed thus far.
                    double testsSoFar = ((n - 1) * NumRandomTests) + r;
                    //ProgressChangedEvent(this, new ProgressEventArgs(testsSoFar / totalTests)); // callback
                }

                //
                // now create the final results for this N and write them to a file
                //
                for (int i = 0; i < numCategories; i++)
                {
                    results[i] /= (double)NumRandomTests; // normalize by the number of tests at this N
                    Category c = (Category)categories[i];
                    // Subject Recognizer Search Speed NumTraining GestureType RecognitionRate
                    mw.WriteLine("{0} $1 {1} {2} {3} {4} {5:F3}",
                        subject,
                        protractor ? "protractor" : "gss",
                        speed,
                        n,
                        c.Name,
                        Math.Round(results[i], 3)
                    );
                }
            }

            // time-stamp the end of the processing
            int end = Environment.TickCount;
            mw.WriteLine("\nEndTime(ms) = {0}, Minutes = {1:F2}", end, Math.Round((end - start) / 60000.0, 2));
            dw.WriteLine("\nEndTime(ms) = {0}, Minutes = {1:F2}", end, Math.Round((end - start) / 60000.0, 2));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            filenames = null;
        }
        finally
        {
            if (mw != null)
                mw.Close();
            if (dw != null)
                dw.Close();
        }

        return filenames;
    }

    private int FirstCorrect(string name, string[] names)
    {
        string category = Category.ParseName(name);
        for (int i = 0; i < names.Length; i++)
        {
            string c = Category.ParseName(names[i]);
            if (category == c)
            {
                return i + 1;
            }
        }

        return -1;
    }

    #endregion


    public bool CreateRotationGraph(string file1, string file2, string dir, bool similar)
    {
        bool success = true;
        StreamWriter writer = null;
        XmlTextReader reader = null;
        try
        {
            // read gesture file #1
            reader = new XmlTextReader(file1);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();
            Unistroke g1 = ReadGesture(reader);
            reader.Close();

            // read gesture file #2
            reader = new XmlTextReader(file2);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();
            Unistroke g2 = ReadGesture(reader);

            // create output file for results
            string outfile = String.Format("{0}\\{1}({2}, {3})_{4}.txt", dir, similar ? "o" : "x", g1.Name, g2.Name,
                Environment.TickCount);
            writer = new StreamWriter(outfile, false, Encoding.UTF8);
            writer.WriteLine("Rotated: {0} --> {1}. {2}, {3}\n", g1.Name, g2.Name, DateTime.Now.ToLongDateString(),
                DateTime.Now.ToLongTimeString());

            // do the full 360 degree rotations
            double[] full = FullSearch(g1.Points, g2.Points, writer);

            // use bidirectional hill climbing to do it again
            double init = PathDistance(g1.Points, g2.Points); // initial distance
            double[] pos = HillClimbSearch(g1.Points, g2.Points, init, 1d);
            double[] neg = HillClimbSearch(g1.Points, g2.Points, init, -1d);
            double[] best = new double[3];
            best = (neg[0] < pos[0]) ? neg : pos; // min distance
            writer.WriteLine("\nHill Climb Search ({0} rotations)\n{1:F2}{2}\t{3:F3} px", pos[2] + neg[2] + 1,
                Math.Round(best[1], 2), (char)176, Math.Round(best[0], 3)); // calls, angle, distance

            // use golden section search to do it yet again
            double[] gold = GoldenSectionSearch(
                g1.Points, // to rotate
                g2.Points, // to match
                GeotrigEx.Degrees2Radians(-45.0), // lbound
                GeotrigEx.Degrees2Radians(+45.0), // ubound
                GeotrigEx.Degrees2Radians(2.0)); // threshold
            writer.WriteLine("\nGolden Section Search ({0} rotations)\n{1:F2}{2}\t{3:F3} px", gold[2],
                Math.Round(gold[1], 2), (char)176, Math.Round(gold[0], 3)); // calls, angle, distance

            // use Protractor to do it yet again
            // TODO

            // for pasting into Excel
            writer.WriteLine(
                "\n{0} {1} {2:F2} {3:F2} {4:F3} {5:F3} {6} {7:F2} {8:F2} {9:F3} {10} {11:F2} {12:F2} {13:F3} {14}",
                g1.Name, // rotated
                g2.Name, // into
                Math.Abs(Math.Round(full[1], 2)), // |angle|
                Math.Round(full[1], 2), // Full Search angle
                Math.Round(full[0], 3), // Full Search distance
                Math.Round(init, 3), // Initial distance w/o any search
                full[2], // Full Search iterations
                Math.Abs(Math.Round(best[1], 2)), // |angle|
                Math.Round(best[1], 2), // Bidirectional Hill Climb Search angle
                Math.Round(best[0], 3), // Bidirectional Hill Climb Search distance
                pos[2] + neg[2] + 1, // Bidirectional Hill Climb Search iterations
                Math.Abs(Math.Round(gold[1], 2)), // |angle|
                Math.Round(gold[1], 2), // Golden Section Search angle
                Math.Round(gold[0], 3), // Golden Section Search distance
                gold[2]); // Golden Section Search iterations
        }
        catch (XmlException xml)
        {
            Console.Write(xml.Message);
            success = false;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            success = false;
        }
        finally
        {
            if (reader != null)
                reader.Close();
            if (writer != null)
                writer.Close();
        }

        return success;
    }
}
