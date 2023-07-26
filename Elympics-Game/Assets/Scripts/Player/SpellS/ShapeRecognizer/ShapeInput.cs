using System.Collections.Generic;
using UnityEngine;

using PDollarGestureRecognizer;

public class ShapeInput : MonoBehaviour
{
    private List<Gesture> trainingSet = new List<Gesture>();
    
    void Start()
    {
        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("SpellPatterns/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }
    
    public string GetShape(List<Point> points)
    {
        Gesture candidate = new Gesture(points.ToArray());
        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
        Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
        //if (gestureResult.Score < 0.70) return -1;
        switch (gestureResult.GestureClass)
        {
            case "triangle": return "fireBall";
            case "c": return "lightningBolt";
            default: return "empty";
        }
    } 
}
