using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;  // Make sure to include the TMPro namespace

public class ButtonTextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text targetText;           // The TextMeshPro component whose color will change
    public Color hoverColor = Color.red;  // Color when hovered
    public Color clickColor = Color.green; // Color when clicked
    private Color originalColor;          // The original color of the text

    void Start()
    {
        // Store the original color of the TMP_Text
        if (targetText != null)
        {
            originalColor = targetText.color;
        }
    }

    // Called when the pointer enters the button (hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = hoverColor;
        }
    }

    // Called when the pointer exits the button (no longer hovering)
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = originalColor;  // Reset to original color
        }
    }

    // Called when the button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = clickColor;
        }
    }
}
