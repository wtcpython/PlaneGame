using System;
using UnityEngine;
using UnityEngine.UI;

public class GridCellResizer : MonoBehaviour
{
    private GridLayoutGroup grid;
    private RectTransform rect;
    
    void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        rect = grid.GetComponent<RectTransform>();
        
        ResizeCells();
    }

    private void OnRectTransformDimensionsChange()
    {
        ResizeCells();
    }

    private void ResizeCells()
    {
        float totalWidth = rect.rect.width;
        float spacing = grid.spacing.x;
        int columns = grid.constraintCount;

        float cellWidth = (totalWidth - spacing * (columns - 1)) / columns;
        float cellHeight = grid.cellSize.y;
        
        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
