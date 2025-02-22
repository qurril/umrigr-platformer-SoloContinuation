using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager : MonoBehaviour
{
    [Header("Slide Animation Settings")]
    public RectTransform[] buttons; // Array of buttons for the main menu
    public RectTransform[] backButtons; // Array of Back button RectTransforms
    public Button optionsButton;     // Reference to the Options button
    public Button backButtonUI;
    public Slider volume;// Reference to the Back button UI
    public float moveDistance = 500f; // Distance to move buttons off-screen
    public float moveDuration = 0.5f; // Duration of the move animation

    private float[] originalYPositions; // Store the original Y positions of main menu buttons
    private float[] backButtonOriginalYPositions; // Store the original Y positions of back buttons
    private float offScreenAboveY;      // Y position for off-screen above
    private float offScreenBelowY;      // Y position for off-screen below

    public GameObject primaryButtonsTransform; // Parent object for main menu buttons
    public GameObject secondaryButtonsTransform; // Parent object for back buttons

    private void Awake()
    {
        // Save the original Y positions of the main menu buttons
        originalYPositions = new float[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            originalYPositions[i] = buttons[i].anchoredPosition.y;
        }

        // Save the original Y positions of back buttons
        backButtonOriginalYPositions = new float[backButtons.Length];
        for (int i = 0; i < backButtons.Length; i++)
        {
            backButtonOriginalYPositions[i] = backButtons[i].anchoredPosition.y;
        }

        // Define off-screen Y positions
        offScreenAboveY = originalYPositions[0] + moveDistance; // Above screen
        offScreenBelowY = originalYPositions[0] - moveDistance; // Below screen

        // Initialize back buttons to start off-screen above
        foreach (var backButton in backButtons)
        {
            backButton.anchoredPosition = new Vector2(backButton.anchoredPosition.x, offScreenAboveY);
        }

        if (AudioManager.Instance != null) {
            volume.value = AudioManager.Instance.musicVolume;
        }
        else
        {
            volume.value = 1f;
        }

        // Add listeners for button click events
        optionsButton.onClick.AddListener(MoveToOptionsMenu);
        backButtonUI.onClick.AddListener(MoveToMainMenu);
        volume.onValueChanged.AddListener((float value) => {
            AudioManager.Instance.AdjustVolume(value);
            
        });
    }

    private void MoveToOptionsMenu()
    {
        // Activate the parent object for the buttons before they start moving
        primaryButtonsTransform.SetActive(true);

        // Move the main menu buttons off-screen to the bottom
        foreach (var button in buttons)
        {
            float currentX = button.anchoredPosition.x; // Preserve X position
            button.DOAnchorPos(new Vector2(currentX, offScreenBelowY), moveDuration)
                  .SetEase(Ease.InOutQuad)
                  .OnComplete(() =>
                  {
                      // After moving off-screen, disable the parent object for main buttons
                      primaryButtonsTransform.SetActive(false);

                      // Reset button position to above the screen
                      button.anchoredPosition = new Vector2(currentX, offScreenAboveY);
                  });
        }

        // Activate the parent object for the back buttons before they move in
        secondaryButtonsTransform.SetActive(true);

        // Move all back buttons into view from the top (using their original Y positions)
        for (int i = 0; i < backButtons.Length; i++)
        {
            RectTransform backButton = backButtons[i];
            float originalY = backButtonOriginalYPositions[i]; // Use original Y positions for back buttons

            backButton.DOAnchorPos(new Vector2(backButton.anchoredPosition.x, originalY), moveDuration) // Enter the screen
                      .SetEase(Ease.InOutQuad);
        }
    }

    private void MoveToMainMenu()
    {
        // Activate the parent object for the main buttons before they move in
        primaryButtonsTransform.SetActive(true);

        // Bring main menu buttons back into view from the top
        for (int i = 0; i < buttons.Length; i++)
        {
            float currentX = buttons[i].anchoredPosition.x; // Preserve X position
            buttons[i].anchoredPosition = new Vector2(currentX, offScreenAboveY); // Start above the screen
            buttons[i].DOAnchorPos(new Vector2(currentX, originalYPositions[i]), moveDuration) // Enter the screen
                      .SetEase(Ease.InOutQuad);
        }

        // Activate the parent object for the back buttons before they move off
        secondaryButtonsTransform.SetActive(true);

        // Move all back buttons off-screen to the bottom
        foreach (var backButton in backButtons)
        {
            backButton.DOAnchorPos(new Vector2(backButton.anchoredPosition.x, offScreenBelowY), moveDuration)
                      .SetEase(Ease.InOutQuad)
                      .OnComplete(() =>
                      {
                          // After moving off-screen, disable the parent object for back buttons
                          secondaryButtonsTransform.SetActive(false);

                          // Reset back button position to above the screen
                          backButton.anchoredPosition = new Vector2(backButton.anchoredPosition.x, offScreenAboveY);
                      });
        }
    }
}
