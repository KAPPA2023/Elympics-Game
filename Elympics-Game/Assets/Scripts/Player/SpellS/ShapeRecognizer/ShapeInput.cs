using System.Collections.Generic;
using UnityEngine;
using WobbrockLib;
using WobbrockLib.Extensions;


public class ShapeInput : MonoBehaviour
{
    private Recognizer2 _rec;
    private bool _protractor = false;
    
    void Start()
    {
        _rec = ScriptableObject.CreateInstance<Recognizer2>();
        _rec.LoadGesture();
    }
    
    public int GetShape(List<TimePointF> points)
    {
        NBestList result = _rec.Recognize(points, _protractor);
        if (result.Score < 0.75 )
        {
            return -1;
        }
        
        switch (result.Name)
        { 
            case "triangle":
                return 0;
            case "c":
                return 1;
            default: 
                return -1;
        }
    } 
}
