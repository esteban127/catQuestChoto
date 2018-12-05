using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffDebuffFeed : MonoBehaviour {

    Tooltip tooltip;
    [SerializeField] Image buffdebuffIco;
    [SerializeField] Image fillIco;
    bool isABuff;
    public bool IsActive { get { return gameObject.activeInHierarchy; } }
    BuffDebuffSystem.Status actualStatus;
    public void BuffInitialize(BuffDebuffSystem.Buff buff)
    {        
        isABuff = true;
        gameObject.SetActive(true);
        GetComponent<Image>().color = Color.green;
        actualStatus = buff;
        buffdebuffIco.sprite = buff.getImage();
    }
    public void SetTooltip(Tooltip tooltipRef)
    {
        tooltip = tooltipRef;
    }
    public void DebuffInitialize(BuffDebuffSystem.Debuff debuff)
    {        
        isABuff = false;
        gameObject.SetActive(true);
        GetComponent<Image>().color = Color.red;
        actualStatus = debuff;
        buffdebuffIco.sprite = debuff.getImage();
    }
    public void Actualizate()
    {
        fillIco.fillAmount = (actualStatus.remainTime/actualStatus.StartingTime);
        if(actualStatus.potency == 0)
        {
            gameObject.SetActive(false);            
        }
    }
    public void desactivate()
    {
        gameObject.SetActive(false);
    }
    public void OnPointerEnter()
    {
        ShowTooltip();
    }
    public void onPointerExit()
    {
        tooltip.Hide();
    }

    private void ShowTooltip()
    {
        string text = "";
        if (isABuff)
        {
            text += ((BuffDebuffSystem.Buff)actualStatus).type +"\n";
        }
        else
        {
            text += ((BuffDebuffSystem.Debuff)actualStatus).type + "\n";
        }
        text += "Potency: " + actualStatus.potency+ "\n";
        text += "Remain time: " + actualStatus.remainTime + " s";
        tooltip.ShowToolTip(text);
    }
}
