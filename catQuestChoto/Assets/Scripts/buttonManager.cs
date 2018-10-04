using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class buttonManager:MonoBehaviour {

    Button button;
    InventoryManager iManager;
    int bCol, bRow;

    private void Start()
    {
        button = GetComponent<Button>();
        iManager = GetComponentInParent<InventoryManager>();
    }

    public void OnClick()
    {
        iManager.OnButtonClicked(bCol, bRow);
    }

    public void SetPos(int col, int row)
    {
        bCol = col;
        bRow = row;
    }
}
