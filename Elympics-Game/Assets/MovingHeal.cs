using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHeal : MonoBehaviour
{
    public float startY;        // Pozycja Y początkowa
    public float endY;          // Pozycja Y końcowa
    public float oscillationSpeed = 1.0f;   // Szybkość oscylacji
    public float rotationSpeed = 0.5f;       // Szybkość rotacji

    private float timeElapsed = 0.0f;
    private Transform startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y-0.5f;
        endY = transform.position.y+0.5f;
    }
    
    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime * oscillationSpeed;
        float newY = Mathf.Lerp(startY, endY, (Mathf.Sin(timeElapsed) + 1.0f) / 2.0f);
        
        // Zastosowanie nowej pozycji Y
        Vector3 newPosition = transform.position;
        newPosition.y = newY;
        transform.position = newPosition;

        // Rotacja obiektu wokół osi Y
        transform.Rotate(Vector3.forward, rotationSpeed);
    }
}
