using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrame : MonoBehaviour {

    [SerializeField] GameObject playerRef;
    CharacterStats playerStats;
    Clock timer;
    [SerializeField]HealtManaBars healtBar;
    [SerializeField] HealtManaBars manaBar;
    [SerializeField] Text toolTipSizeText;
    [SerializeField] Text toolTipVisualText;
    [SerializeField] GameObject toolTip;

    private void Start()
    {
        playerStats = playerRef.GetComponent<CharacterStats>();
        timer = Clock.Instance;
        timer.OnTick += actualizeHealthAndMana;
    }
    public void actualizeHealthAndMana(float time)
    {
        healtBar.UpdateFillBar(playerStats.CurrentHealth / playerStats.MaxHealth());
        manaBar.UpdateFillBar(playerStats.CurrentMana / playerStats.MaxMana());
    }
    
    public void ShowToolTip(barType bar)
    {
        string text = "<b>";
        toolTip.SetActive(true);
        float xPos = Input.mousePosition.x - 3;
        float yPos = Input.mousePosition.y;

        float tooltipWidth = toolTipSizeText.GetComponent<RectTransform>().rect.width;
        float tooltipHeight = toolTipSizeText.GetComponent<RectTransform>().rect.height;

        xPos -= (tooltipWidth / 2 + 5);
        if (yPos + tooltipHeight + 10 < Screen.height)
        {
            yPos = yPos + tooltipHeight / 2 + 10;
        }
        else
        {
            yPos = yPos - tooltipHeight / 2 - 5;
        }
        if (xPos + tooltipWidth + 10 < Screen.width)
        {
            xPos = xPos + tooltipWidth / 2 + 10;
        }
        else
        {
            xPos = xPos - tooltipWidth / 2 - 5;
        }

        switch (bar)
        {
            case barType.Health:
                text += (int)playerStats.CurrentHealth + " / " + (int)playerStats.MaxHealth();
                break;
            case barType.Mana:
                text += (int)playerStats.CurrentMana + " / " + (int)playerStats.MaxMana();
                break;
        }
        text += "</b>";
        toolTipSizeText.text = text;      
        toolTipVisualText.text = toolTipSizeText.text;
        toolTip.transform.position = new Vector2(xPos, yPos);
    }


    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

}
