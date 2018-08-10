using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDispaly : MonoBehaviour {

    private GameObject target;

    public void NewTarget(GameObject newTarget)
    {
        this.gameObject.SetActive(true);
        if (target != null)
            target.transform.GetChild(target.transform.childCount-1).gameObject.SetActive(false);
        this.target = newTarget;
        target.transform.GetChild(target.transform.childCount-1).gameObject.SetActive(true);
        transform.GetChild(0).GetComponentInChildren<Image>().sprite = target.GetComponent<CharacterImage>().ImageToDisplay;
        this.Actualizate();
    }

    public void Actualizate()
    {
        this.GetComponentInChildren<ProgressionBar>().SetProgression((float)target.GetComponent<healthManager>().CurrentHealth / (float)target.GetComponent<healthManager>().MaxHealth);
        this.GetComponentInChildren<ProgressionBar>().SetText(target.GetComponent<healthManager>().CurrentHealth.ToString() + " / " + target.GetComponent<healthManager>().MaxHealth.ToString());
    }
}
