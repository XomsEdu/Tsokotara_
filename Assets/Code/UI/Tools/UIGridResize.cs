using UnityEngine;
using UnityEngine.UI;

public class UIGridResize : UILayoutResize
{
    protected override void SetLayoutSize()
        {
            GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)layoutGroup;
            float totalHeight = 0;

            int numRows = Mathf.CeilToInt((float)transform.childCount / gridLayoutGroup.constraintCount);
            
            totalHeight = (numRows * gridLayoutGroup.cellSize.y) + ((numRows - 1) * gridLayoutGroup.spacing.y);

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
        }
}
