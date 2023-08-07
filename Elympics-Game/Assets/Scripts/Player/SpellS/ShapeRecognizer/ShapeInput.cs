using System.Collections.Generic;
using UnityEngine;

using PDollarGestureRecognizer;

public class ShapeInput : MonoBehaviour
{
    private List<Gesture> trainingSet = new List<Gesture>();
    public delegate void MyEventHandler();
    public static event MyEventHandler OnBadSpell;
    public static event MyEventHandler OnGoodSpell;

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
            case "Fire": Invoke("GoodSpellInvoke", 0.1f); return "fireBall";
            case "Light": Invoke("GoodSpellInvoke", 0.1f); return "lightningBolt";
            default: Invoke("BadSpellInvoke", 0.1f); return "empty";
        }
    }

    private void GoodSpellInvoke()
    {
        OnGoodSpell();
    }
    private void BadSpellInvoke()
    {
        OnBadSpell();
    }
}
