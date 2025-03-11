using UnityEngine;
using UnityEngine.UI;

public class UILayoutResize : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected LayoutGroup layoutGroup;
    protected bool awaken = false;

    protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            layoutGroup = GetComponent<LayoutGroup>();
            awaken = true;
        }

    private void OnTransformChildrenChanged()
        {   
            if (!awaken) Awake();
            SetLayoutSize();
            gameObject.SetActive(transform.childCount != 0);   
        }
    
    protected virtual void SetLayoutSize()  {}  // Method to be overridden in child classes

    private void OnDisable()
    {   transform.parent?.GetComponent<UILayoutResize>()?.SetLayoutSize();  }
}
