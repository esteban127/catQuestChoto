using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum barType
{
    Mana,
    Health
}
public class HealtManaBars : MonoBehaviour {
    CharacterFrame cFrame;
    [SerializeField] barType bar;
    [SerializeField] Image fillBar;

	private void Start () {
        cFrame = GetComponentInParent<CharacterFrame>();
	}
    public void MousueEnter()
    {
        cFrame.ShowToolTip(bar);
    }
    public void MouseExit()
    {
       cFrame.HideToolTip();
    }
    public void UpdateFillBar(float porcentage)
    {
        if (porcentage > 0.99)
            porcentage = 1;
        if (porcentage < 0.01)
            porcentage = 0;
        fillBar.fillAmount = porcentage;
    }
}
