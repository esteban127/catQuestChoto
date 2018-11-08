using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnInventoryManager : MonoBehaviour {

    bool picked = false;
    float width;
    float height;
    Vector3 offset;
    Iitem itemRef;
    float cellSize;
    private void Update()
    {
        if (picked)
        {
            transform.position = Input.mousePosition+offset;
        }
    }

    public void Initialize(Iitem item, float cell)
    {
        itemRef = item;
        cellSize = cell;
        SetImg(item.getImage());
        SetSize(item.Size);
    }

    public Iitem getItem()
    {
        return itemRef;
    }

    public void SetImg(Sprite img)
    {
        GetComponent<Image>().sprite = img;
    }
    public void SetSize(int[]size)
    {
        width = size[0]*cellSize;
        height = size[1]*cellSize;
        offset = new Vector3(-width + (cellSize/2) , -cellSize/2, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GetComponent<RectTransform>().localPosition = offset;
    }

    public void Draw()
    {
        picked = true;
        GetComponent<Image>().raycastTarget = false;
    }

    public void Release(GameObject parent)
    {
        picked = false;
        transform.SetParent(parent.transform);
        offset = new Vector3(-width + (cellSize / 2), -cellSize / 2, 0);
        GetComponent<RectTransform>().localPosition = offset;
        GetComponent<Image>().raycastTarget = true;
    }
    public void Equip (GameObject parent)
    {
        picked = false;
        transform.SetParent(parent.transform);
        Vector3 EquipedOffset = new Vector3(-width/2, -height/2, 0);
        GetComponent<RectTransform>().localPosition = EquipedOffset;
        GetComponent<Image>().raycastTarget = true;
    }
   
    
}
