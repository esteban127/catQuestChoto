using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void ButtonDelegate(int pos);
public class SkillTreeButton : MonoBehaviour {

    
    int pos = 0;
    private bool isOver = false;    
    public ButtonDelegate OnRightClick;
    public ButtonDelegate OnLeftClick;
    public ButtonDelegate OnMouseEnter;
    public ButtonDelegate OnMouseExit;


    public void SetPos(int newPos)
    {
        pos = newPos;
    }
    private void OnDisable()
    {
        isOver = false;
    }
    public void SetSprite(Sprite sprite, bool isGray)
    {
        GetComponent<Image>().sprite = sprite;
        if (isGray)
            GetComponent<Image>().color = Color.gray;
        else
            GetComponent<Image>().color = Color.white;
    }
    public void TogleSprite(bool isGray)
    {
        if (isGray)
            GetComponent<Image>().color = Color.gray;
        else
            GetComponent<Image>().color = Color.white;
    }
    private void Update()
    {
        if (isOver)
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnRightClick(pos);
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick(pos);
            }
        }
    }

    public void MousueEnter()
    {
        OnMouseEnter(pos);
        isOver = true;
    }
    public void MouseExit()
    {
        OnMouseExit(pos);
        isOver = false;
    }
    
}
