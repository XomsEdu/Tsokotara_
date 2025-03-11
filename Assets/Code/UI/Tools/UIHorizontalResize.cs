using UnityEngine;
using UnityEngine.UI;

public class UIHorizontalResize : UILayoutResize
{
    protected override void SetLayoutSize()
        {
            float totalWidth = 0;

            foreach (Transform child in transform)
                {
                    RectTransform childRectTransform = child.GetComponent<RectTransform>();

                    if (childRectTransform != null) totalWidth += childRectTransform.rect.width;
                }

            HorizontalLayoutGroup horizontalLayoutGroup = (HorizontalLayoutGroup)layoutGroup;
            totalWidth += horizontalLayoutGroup.spacing * transform.childCount;

            rectTransform.sizeDelta = new Vector2(totalWidth, rectTransform.sizeDelta.y);
        }
}

