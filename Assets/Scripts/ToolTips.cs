using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ToolTips : MonoBehaviour
{
    [SerializeField] private CanvasGroup liteModeTooltipGroup;
    [SerializeField] private CanvasGroup normalModeTooltipGroup;
    [SerializeField] private TMP_Text liteModeText;
    [SerializeField] private TMP_Text normalModeText;

    private CanvasGroup activeTooltipGroup;
    private Vector3 tooltipOffset = new Vector3(15, -15, 0);

    private void Update()
    {
        if (activeTooltipGroup != null)
        {
            Vector3 mousePosition = Input.mousePosition + tooltipOffset;
            activeTooltipGroup.transform.position = mousePosition;
        }
    }

    public void ShowLiteModeTooltip(string message)
    {
        liteModeText.text = message;
        SetTooltipVisibility(liteModeTooltipGroup, true);
        activeTooltipGroup = liteModeTooltipGroup;
    }

    public void ShowNormalModeTooltip(string message)
    {
        normalModeText.text = message;
        SetTooltipVisibility(normalModeTooltipGroup, true);
        activeTooltipGroup = normalModeTooltipGroup;
    }

    public void HideTooltip()
    {
        if (activeTooltipGroup != null)
        {
            SetTooltipVisibility(activeTooltipGroup, false);
            activeTooltipGroup = null;
        }
    }

    private void SetTooltipVisibility(CanvasGroup tooltipGroup, bool isVisible)
    {
        tooltipGroup.alpha = isVisible ? 1 : 0; // Show or hide with alpha
    }
}