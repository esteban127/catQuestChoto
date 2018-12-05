using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxComponent : MonoBehaviour {


    [SerializeField] Text dialogText;
    [SerializeField] Image profilePict;
    bool mouseOver;
    string[] dialog;
    int currentDialog;
    public delegate void DialogBoxDelegate();
    public DialogBoxDelegate onDialogEnd;
    public void Initialize(string[] newDialog, Sprite pict)
    {
        dialog = newDialog;
        profilePict.sprite = pict;
        currentDialog = 0;
        dialogText.text = dialog[currentDialog];
    }

    public void pointerEnter()
    {
        mouseOver = true;
    }
    public void pointerExit()
    {
        mouseOver = false;
    }
    private void Update()
    {
        if (mouseOver)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                NextDialog();
            }
        }
    }

    private void NextDialog()
    {
        currentDialog++;
        if (currentDialog < dialog.Length)
        {
            dialogText.text = dialog[currentDialog];
        }
        else
        {
            if (onDialogEnd != null)
                onDialogEnd();
            mouseOver = false;
            gameObject.SetActive(false);
        }
    }
}
