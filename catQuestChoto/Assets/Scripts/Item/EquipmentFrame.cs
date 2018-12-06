using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentFrame : MonoBehaviour {


    InventoryManager iManager;    
    [SerializeField] EquipmentSlot slot;
    private bool isOver = false;
    private void Start()
    {
        iManager = InventoryManager.Instance;
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
    private void OnDisable()
    {
        isOver = false;
    }
    public void OnLeftClick()
    {
        iManager.OnLeftButtonClicked(slot);
        isOver = false;
    }
    private void OnRightClick()
    {
        iManager.OnRightButtonClicked(slot);
        isOver = false;
    }

    public void MousueEnter()
    {
        iManager.MouseOver(slot);
        isOver = true;
    }
    public void MouseExit()
    {
        iManager.HideToolTip();
        isOver = false;
    }
}
