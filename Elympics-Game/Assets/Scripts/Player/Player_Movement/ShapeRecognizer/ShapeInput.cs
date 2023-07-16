using System.Collections.Generic;
using UnityEngine;
using WobbrockLib;
using WobbrockLib.Extensions;


public class ShapeInput : MonoBehaviour
{
    private List<TimePointF> _points;
    [SerializeField] private Vector2 _position;
    [SerializeField] private GameObject CubePrefab;
    private Recognizer2 _rec;
    private bool _protractor = false;

    // Start is called before the first frame update

    void Start()
    {
        _rec = ScriptableObject.CreateInstance<Recognizer2>();
        Debug.Log(_rec.LoadGesture("C:/Users/antol/Desktop/Gestures/slope"));
        Debug.Log(_rec.NumGestures);
        _points = new List<TimePointF>(255);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _points.Clear();
            _position = Input.mousePosition;
            _points.Add(new TimePointF(_position.x, _position.y, TimeEx.NowMs));
        }
        else if (Input.GetMouseButton(0))
                 {
                     _position = Input.mousePosition;
                     _points.Add(new TimePointF(_position.x, _position.y, TimeEx.NowMs));
                 }
        else if (Input.GetMouseButtonUp(0))
        {
            NBestList result = _rec.Recognize(_points, _protractor);
            Debug.Log(result.Name);
            _points.Clear();
            if (result.Name == "triangle")
            {
                Instantiate(CubePrefab, transform.position, transform.rotation);
                Debug.Log(GetComponent<Inventory>().FireballUse);
                GetComponent<Inventory>().addSpell();
                Debug.Log(GetComponent<Inventory>().FireballUse);

            }
                

        }
    }
}
