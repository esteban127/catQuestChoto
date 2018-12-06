using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class buttonManager:MonoBehaviour {


    InventoryManager iManager;
    int bCol, bRow;
    private bool isOver = false;

    private void Start()
    {
        
        iManager = GetComponentInParent<InventoryManager>();
    }
    private void Update()
    {
        if (isOver)
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnRightClick();
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick();
            }
        }
    }

    private void OnLeftClick()
    {
        iManager.OnLeftButtonClicked(bCol, bRow);        
    }
    private void OnRightClick()
    {        
        iManager.OnRightButtonClicked(bCol, bRow);        
    }
    private void OnDisable()
    {
        isOver = false;
    }


    public void SetPos(int col, int row)
    {
        bCol = col;
        bRow = row;
    }
    public void MousueEnter()
    {
        iManager.MouseOver(bCol, bRow);
        isOver = true;
    }
    public void MouseExit()
    {
        iManager.HideToolTip();
        isOver = false;
    }
}