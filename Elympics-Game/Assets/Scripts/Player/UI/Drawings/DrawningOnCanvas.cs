using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawningOnCanvas : MonoBehaviour
{
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameManager gameManager;
    private Vector3[] linePoints;
    private int pointCount = 0;
    private bool isDrawing = false;
    public Vector2 mousePadding;

    [SerializeField] private Image canvasImage;
    public Color drawingBackgroundColor;
    public Color originalBackgroundColor;

    public Color goodSpellColor;
    public Color badSpellColor;

    void Start()
    {
        // Inicjalizacja Line Renderer
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.useWorldSpace = false;
        linePoints = new Vector3[0];
        ActionHandler.OnBadSpell += BadSpell;
        ActionHandler.OnGoodSpell += GoodSpell;
        ShapeInput.OnBadSpell += BadSpell;
    }

    private void OnDestroy()
    {
        ActionHandler.OnBadSpell -= BadSpell;
        ActionHandler.OnGoodSpell -= GoodSpell;
        ShapeInput.OnBadSpell -= BadSpell;
    }

    void Update()
    {
        if (!gameManager.matchTime.Value) return;
            
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            isDrawing = true;
            pointCount = 0;
            lineRenderer.positionCount = 0;
            
        }

        if (isDrawing)
        {
            if (Input.GetMouseButton(1))
            {

                Vector3 mousePosition = Input.mousePosition ;
                mousePosition.z = canvas.planeDistance;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                pointCount++;
                System.Array.Resize(ref linePoints, pointCount);
                linePoints[pointCount - 1] = worldPosition + new Vector3(mousePadding.x, mousePadding.y, 0);

                lineRenderer.positionCount = pointCount;
                lineRenderer.SetPositions(linePoints);
                canvasImage.color = drawingBackgroundColor;

            }
            else if (Input.GetMouseButtonUp(1))
            {
                isDrawing = false;
                pointCount = 0;
                lineRenderer.positionCount = 0;
                canvasImage.color = originalBackgroundColor;

            }
        }
    }

    private void ResetCanvasColor()
    {
        canvasImage.color = originalBackgroundColor;
    }

    private void BadSpell()
    {
        canvasImage.color = badSpellColor;
        Invoke("ResetCanvasColor", 0.5f); // Przywracanie koloru po 0.5 sekundy
    }
    private void GoodSpell()
    {
        canvasImage.color = goodSpellColor;
        Invoke("ResetCanvasColor", 0.5f); // Przywracanie koloru po 0.5 sekundy
    }
}
