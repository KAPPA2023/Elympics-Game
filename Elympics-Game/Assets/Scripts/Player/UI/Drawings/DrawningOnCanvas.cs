using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawningOnCanvas : MonoBehaviour
{
    public Canvas canvas;
    public LineRenderer lineRenderer;
    private Vector3[] linePoints;
    private int pointCount = 0;
    private bool isDrawing = false;

    void Start()
    {
        // Inicjalizacja Line Renderer
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.useWorldSpace = false;

        linePoints = new Vector3[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDrawing = true;
            pointCount = 0;
            lineRenderer.positionCount = 0;
        }

        if (isDrawing)
        {
            if (Input.GetMouseButton(1))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = canvas.planeDistance;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                pointCount++;
                System.Array.Resize(ref linePoints, pointCount);
                linePoints[pointCount - 1] = worldPosition;

                lineRenderer.positionCount = pointCount;
                lineRenderer.SetPositions(linePoints);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isDrawing = false;
                pointCount = 0;
                lineRenderer.positionCount = 0;
            }
        }
    }

}
