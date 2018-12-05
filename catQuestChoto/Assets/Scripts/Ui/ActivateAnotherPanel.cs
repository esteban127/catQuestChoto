using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnotherPanel : MonoBehaviour {

    [SerializeField] GameObject panel;
    public void OnClicked()
    {
        panel.SetActive(true);
    }
}
