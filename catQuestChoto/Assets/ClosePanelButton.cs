using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanelButton : MonoBehaviour {

	public void OnClicked()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
