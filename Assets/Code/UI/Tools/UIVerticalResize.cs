using UnityEngine;
using UnityEngine.UI;

public class UIVerticalResize : UILayoutResize
{
    protected override void SetLayoutSize()
        {
            float totalHeight = 0;

            foreach (Transform child in transform)
                {
                    RectTransform childRectTransform = child.GetComponent<RectTransform>();

                    if (childRectTransform != null) totalHeight += childRectTransform.rect.height;
                }

            VerticalLayoutGroup verticalLayoutGroup = (VerticalLayoutGroup)layoutGroup;
            totalHeight += verticalLayoutGroup.spacing * transform.childCount;

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);       
        }
}
