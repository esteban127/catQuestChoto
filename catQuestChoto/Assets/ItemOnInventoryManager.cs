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
    private void Update()
    {
        if (picked)
        {
            transform.position = Input.mousePosition+offset;
        }
    }

    public void Initialize(Iitem item, float cellSize)
    {
        itemRef = item;
        SetImg(item.Image);
        SetSize(item.Size, cellSize);
    }

    public void SetImg(Sprite img)
    {
        GetComponent<Image>().sprite = img;
    }
    public void SetSize(int[]size, float cellSize)
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
        GetComponent<RectTransform>().position = new Vector3(-width, height / 2, 0);
        GetComponent<Image>().raycastTarget = true;
    }
   
    
}
