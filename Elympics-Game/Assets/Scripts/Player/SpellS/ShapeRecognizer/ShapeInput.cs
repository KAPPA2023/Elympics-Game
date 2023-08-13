using System.Collections.Generic;
using UnityEngine;

using PDollarGestureRecognizer;

public class ShapeInput : MonoBehaviour
{
    private List<Gesture> trainingSet = new List<Gesture>();
    public delegate void MyEventHandler();
    public static event MyEventHandler OnBadSpell;

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
            Invoke("BadSpellInvoke", 0.1f);
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
            default: Invoke("BadSpellInvoke", 0.1f); return Spells.Empty;
        }
    }

    private void BadSpellInvoke()
    {
        OnBadSpell();
    }
}
