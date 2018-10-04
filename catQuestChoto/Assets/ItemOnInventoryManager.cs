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
        SetSize(item.Size[0] * cellSize, item.Size[1] * cellSize);
    }

    public void SetImg(Sprite img)
    {
        GetComponent<Image>().sprite = img;
    }
    public void SetSize(float iWidth, float iHeight)
    {
        width = iWidth;
        height = iHeight;
        offset = new Vector3(-width / 2, height / 2, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
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
