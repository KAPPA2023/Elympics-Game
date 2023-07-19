using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using WobbrockLib;
using WobbrockLib.Extensions;
using UnityEngine;

public class Unistroke : ScriptableObject, IComparable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public string Name;
        public List<TimePointF> RawPoints; // raw points (for drawing) -- read in from XML
        public List<PointF> Points;        // pre-processed points (for matching) -- created when loaded
        public List<double> Vector;        // vector representation -- for Protractor

        /// <summary>
        /// Constructor of a unistroke gesture. A unistroke is comprised of a set of points drawn
        /// out over time in a sequence.
        /// </summary>
        /// <param name="name">The name of the unistroke gesture.</param>
        /// <param name="timepoints">The array of points supplied for this unistroke.</param>
        public Unistroke(string name, List<TimePointF> timepoints)
        {
            this.Name = name;
            this.RawPoints = new List<TimePointF>(timepoints); // copy (saved for drawing)
            double I = GeotrigEx.PathLength(timepoints) / (Recognizer2.NumPoints - 1); // interval distance between points
            this.Points = TimePointF.ConvertList(SeriesEx.ResampleInSpace(timepoints, I));
            double radians = GeotrigEx.Angle(GeotrigEx.Centroid(this.Points), this.Points[0], false);
            this.Points = GeotrigEx.RotatePoints(this.Points, -radians);
            this.Points = GeotrigEx.ScaleTo(this.Points, Recognizer2.SquareSize);
            this.Points = GeotrigEx.TranslateTo(this.Points, Recognizer2.Origin, true);
            this.Vector = Vectorize(this.Points); // vectorize resampled points (for Protractor)
        }

      
        public static List<double> Vectorize(List<PointF> points)
        {
            double sum = 0.0;
            List<double> vector = new List<double>(points.Count * 2);
            for (int i = 0; i < points.Count; i++)
            {
                vector.Add(points[i].X);
                vector.Add(points[i].Y);
                sum += points[i].X * points[i].X + points[i].Y * points[i].Y;
            }
            double magnitude = Math.Sqrt(sum);
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] /= magnitude;
            }
            return vector;
        }

        /// <summary>
        /// Gets the duration in milliseconds of this gesture.
        /// </summary>
        public long Duration
        {
            get { return (RawPoints.Count >= 2) ? RawPoints[RawPoints.Count - 1].Time - RawPoints[0].Time : 0L; }
        }

        /// <summary>
        /// Sort comparator in descending order of score.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is Unistroke)
            {
                Unistroke g = (Unistroke)obj;
                return this.Name.CompareTo(g.Name);
            }
            else throw new ArgumentException("object is not a Gesture");
        }

        /// <summary>
        /// Pulls the gesture name from the file name, e.g., "circle03" from "C:\gestures\circles\circle03.xml".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ParseName(string filename)
        {
            int start = filename.LastIndexOf('\\');
            int end = filename.LastIndexOf('.');
            return filename.Substring(start + 1, end - start - 1);
        }
}
