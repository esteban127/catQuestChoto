using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

    [SerializeField] Text toolTipSizeText;
    [SerializeField] Text toolTipVisualText;
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void ShowToolTip(string text)
    {
        gameObject.SetActive(true);
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
            yPos = yPos - tooltipHeight / 2 - 10;
        }
        if (xPos + tooltipWidth + 10 < Screen.width)
        {
            xPos = xPos + tooltipWidth / 2 + 10;
        }
        else
        {
            xPos = xPos - tooltipWidth / 2 - 10;
        }
        toolTipSizeText.text = text;
        toolTipVisualText.text = toolTipSizeText.text;
        transform.position = new Vector2(xPos, yPos);
    }    
}
