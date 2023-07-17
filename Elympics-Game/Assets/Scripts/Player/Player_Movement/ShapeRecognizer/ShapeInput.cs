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
    
    // public void UpdateShape()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         _points.Clear();
    //         _position = Input.mousePosition;
    //         _points.Add(new TimePointF(_position.x, _position.y, TimeEx.NowMs));
    //     }
    //     else if (Input.GetMouseButton(0))
    //              {
    //                  _position = Input.mousePosition;
    //                  _points.Add(new TimePointF(_position.x, _position.y, TimeEx.NowMs));
    //              }
    //     else if (Input.GetMouseButtonUp(0))
    //     {
    //         NBestList result = _rec.Recognize(_points, _protractor);
    //         Debug.Log(result.Name);
    //         _points.Clear();
    //         if (result.Name == "triangle")
    //         {
    //             Instantiate(CubePrefab, transform.position, transform.rotation);
    //             Debug.Log(GetComponent<Inventory>().FireballUse);
    //             GetComponent<Inventory>().addSpell();
    //             Debug.Log(GetComponent<Inventory>().FireballUse);
    //
    //         }
    //     }
    // }

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
                this.gameObject.GetComponent<SpellManager>().setRemainingUses(FireSpell.getMaxUse());
                return 0;
            case "c": 
                this.gameObject.GetComponent<SpellManager>().setRemainingUses(LightSpell.getMaxUse());
                return 1;
            default: 
                return -1;
        }
    } 
}
