using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonMovement : MonoBehaviour, IPointerEnterHandler
{
    private RectTransform rectTransform;
    private Vector3 initialPositionOnHover; // Store the position when hover starts
    private bool isShaking = false;  // Flag to check if the button is already shaking

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Prevent shaking if it's already shaking or moving
        if (isShaking || DOTween.IsTweening(rectTransform))
            return;

        // Save the current position when the hover starts
        initialPositionOnHover = rectTransform.anchoredPosition;

        // Ensure any existing tweens are completed or killed to avoid conflicts
        rectTransform.DOKill();

        // Start shaking the button
        isShaking = true;
        
        rectTransform.DOShakePosition(
            duration: 0.5f, // Duration of the shake
            strength: new Vector3(10f, 10f, 0f), // Strength of the shake in x, y, and z directions
            vibrato: 10, // Vibrations
            randomness: 90f // Randomness of the shake
        ).SetEase(Ease.OutQuad).OnKill(() =>
        {
            // Reset to the position where the hover started, not the original position
            rectTransform.anchoredPosition = initialPositionOnHover;
            isShaking = false; // Reset the shaking flag
        });
    }
}
