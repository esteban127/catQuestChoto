using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPanel : MonoBehaviour {

    string confirmResult = "Null";
	public string ConfirmResult { get { return confirmResult; } }
    public void Confirm(bool confirmed)
    {
        if (confirmed)
            confirmResult = "Yes";
        else
            confirmResult = "No";

        gameObject.SetActive(false);
    }

    public void Ask()
    {
        gameObject.SetActive(true);
        confirmResult = "Null";
    }

}
