using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpSkillButton : MonoBehaviour {

    [SerializeField] AbilityInTree ability;

    public ButtonDelegate OnClik;

    public void Clicked()
    {
        OnClik((int)ability);
    }
    public void Interactable(bool set)
    {
        gameObject.GetComponent<Button>().interactable=set;
    }
}
