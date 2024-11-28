using UnityEngine;

public class AdjustContentSize : MonoBehaviour
{
    public RectTransform content; // The RectTransform of the Content object
    public float itemHeight = 100f; // The height of each item (adjust as needed)
    private int previousChildCount = 0; // To track the number of children to avoid redundant updates

    void Start()
    {
        UpdateContentSize();
    }

    void Update()
    {
        // If the number of children changes, update the content size
        if (content.childCount != previousChildCount)
        {
            previousChildCount = content.childCount;
            UpdateContentSize();
        }
    }

    void UpdateContentSize()
    {
        // Calculate the new height of the content based on the number of children
        float newHeight = itemHeight * content.childCount;

        // Set the size of the content RectTransform based on the new height
        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        // Optionally, force the layout to update immediately
        //LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }
}
