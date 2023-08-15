using System.Collections.Generic;
using UnityEngine;
using System;
using PDollarGestureRecognizer;

public class ShapeInput : MonoBehaviour
{
    private List<Gesture> trainingSet = new List<Gesture>();
    public static event Action OnBadSpell = null;

    void Start()
    {
        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("SpellPatterns/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }
    
    public Spells GetShape(List<Point> points)
    {
        Gesture candidate = new Gesture(points.ToArray());
        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
        Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
        if (gestureResult.Score < 0.70)
        {
            OnBadSpell?.Invoke();
            return Spells.Empty;
        }

        switch (gestureResult.GestureClass)
        {
            case "Fire": return Spells.Fireball;
            case "Light": return Spells.Lightbolt;
            case "Water": return Spells.WaterBlast;
            case "Ground": return Spells.SandGranade;
            case "Wind": return Spells.Tornado;
            case "Ice": return Spells.IceSpike;
            default: OnBadSpell?.Invoke(); return Spells.Empty;
        }
    }


}
