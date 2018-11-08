using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInventoryDetection : MonoBehaviour {
    InventoryManager iManager;    
    private bool isOver = false;

    private void Start()
    {
        iManager = InventoryManager.Instance;
    }

    private void Update()
    {
        if (!isOver)
        {            
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick();
            }
        }
    }
    public void OnLeftClick()
    {
        Debug.Log("Bai");
        iManager.ClickedOutOfInventory();        
    }
    

    public void MousueEnter()
    {
        
        isOver = true;
    }
    public void MouseExit()
    {
        
        isOver = false;
    }
}
