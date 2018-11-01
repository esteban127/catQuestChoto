using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentFrame : MonoBehaviour {


    InventoryManager iManager;
    int bCol, bRow;
    [SerializeField] EquipmentSlot slot;
    private void Start()
    {
        iManager = InventoryManager.Instance;
    }

    public void OnClick()
    {
        iManager.OnButtonClicked(slot);
    }
    
}
