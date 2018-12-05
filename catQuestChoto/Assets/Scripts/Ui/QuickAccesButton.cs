using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAccesButton : MonoBehaviour {

    [SerializeField] GameObject windowsToToggle;
    [SerializeField] string textForTheTooltip;
    [SerializeField] Tooltip tooltip;

	public void OnClick()
    {
        windowsToToggle.SetActive(!windowsToToggle.activeInHierarchy);
    }
    public void OnPointerEnter()
    {
        tooltip.ShowToolTip(textForTheTooltip);
    }
    public void OnPonterExit()
    {
        tooltip.Hide();
    }
}
