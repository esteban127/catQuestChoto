using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonManager : MonoBehaviour {

    bool isOver = false;
    AbilitySystem aSystem;
    int pos = 0;
    IAbility ability;
    public IAbility Ability { get { return ability; } }
    [SerializeField] Image fillBar;
    Clock timer;
    private void Start()
    {
        aSystem = GetComponentInParent<AbilitySystem>();      
        timer = Clock.Instance;
        timer.OnTick += CDFill;

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
    
    public void SetAbility(IAbility newAbility)
    {
        ability = newAbility;
        GetComponent<Image>().sprite = ability.AbilitySprite;
        if (ability.Loked)
        {
            GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
    
    public void CDFill(float time)
    {
        if (ability!= null && ability.OnCooldow)
        {
            fillBar.fillAmount = (ability.RemainCooldown>0.2? ability.RemainCooldown / ability.Cooldown:0);
        }
    }

    private void OnLeftClick()
    {
        aSystem.TryCast(pos);
    }
    private void OnRightClick()
    {
        aSystem.TryCast(pos);
    }


   
    public void MousueEnter()
    {
        aSystem.ShowToolTip(ability);
        isOver = true;
    }
    public void MouseExit()
    {
        aSystem.HideToolTip();
        isOver = false;
    }
    public void SetPos (int aPos)
    {
        pos = aPos;
    }
}
