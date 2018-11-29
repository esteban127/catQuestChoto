using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySystem : MonoBehaviour {
    BuffDebuffSystem.Buff[] buffs;
    BuffDebuffSystem.Debuff[] debuff;
    [SerializeField] GameObject playerRef;
    public CharacterStats PlayerStats { get { return playerRef.GetComponent<CharacterStats>(); } }
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] int numberOfSpellsInBar;
    public int NumOfSpells { get { return numberOfSpellsInBar; } }
    [SerializeField] float separationBetweenButtons;
    AbilityInterface aInterface;
    [SerializeField] Text toolTipSizeText;
    [SerializeField] Text toolTipVisualText;
    [SerializeField] GameObject toolTip;
    Animator playerAnimator;
    

    static private AbilitySystem instance = null;
    static public AbilitySystem Instance { get { return instance; } }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            aInterface = new AbilityInterface(gameObject);
            aInterface.CreateAbilities(numberOfSpellsInBar, buttonPrefab, separationBetweenButtons);       
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerAnimator = playerRef.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            TryCast(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            TryCast(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            TryCast(2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            TryCast(3);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            TryCast(4);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            TryCast(5);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            TryCast(6);
        }
    }

    public bool TryCast(int pos)
    {
        if (aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability != null)
        {            
            if( (aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability.TryCastAbility(playerRef.GetComponent<TargetSistem>().GetTarget(), playerRef, "Enemy")))
            {
                playerAnimator.SetTrigger(aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability.AbilityAnimation.ToString());
                TriggerGlobalColdown();
                return true;
            }
        }
        
        return false;
    }

    private void TriggerGlobalColdown()
    {
        for (int i = 0; i < numberOfSpellsInBar; i++)
        {
            aInterface.GetSlot(i).GetComponent<AbilityButtonManager>().Ability.PutInGlobarColdown();
        }
    }
    public void SetAbilityCDR(IAbility ability)
    {
        ability.SetCDR(playerRef.GetComponent<CharacterStats>().ColdownReduction());
    }
    public void SetAbility(int slot, IAbility ability)
    {        
        aInterface.GetSlot(slot).GetComponent<AbilityButtonManager>().SetAbility(ability);
    }

    public void ShowToolTip(IAbility ability)
    {
        if (ability != null&& ability.Name != "Empty")
        {
            toolTip.SetActive(true);
            float xPos = Input.mousePosition.x - 3;
            float yPos = Input.mousePosition.y;

            float tooltipWidth = toolTipSizeText.GetComponent<RectTransform>().rect.width;
            float tooltipHeight = toolTipSizeText.GetComponent<RectTransform>().rect.height;

            xPos -= (tooltipWidth / 2 + 5);
            if (yPos + tooltipHeight + 10 < Screen.height)
            {
                yPos = yPos + tooltipHeight / 2 + 10;
            }
            else
            {
                yPos = yPos - tooltipHeight / 2 - 10;
            }
            if (xPos + tooltipWidth + 10 < Screen.width)
            {
                xPos = xPos + tooltipWidth / 2 + 10;
            }
            else
            {
                xPos = xPos - tooltipWidth / 2 - 10;
            }

            toolTipSizeText.text = GenerateAbilityText(ability);
            toolTipVisualText.text = toolTipSizeText.text;
            toolTip.transform.position = new Vector2(xPos, yPos);
        }        
    }

    private string GenerateAbilityText(IAbility ability)
    {
        string text = "";
        text += "<b>" + ability.Name + "</b>      lvl: " + ability.Level + "\n";
        text += ability.Description + "\n";
        if(ability.GetType() == typeof(AtackAbility))
        {
            text += "Damage: " + (playerRef.GetComponent<CharacterStats>().MinDamage() * ((AtackAbility)ability).DamageMultiplier).ToString("F1") + " - " + (playerRef.GetComponent<CharacterStats>().MaxDamage() * ((AtackAbility)ability).DamageMultiplier).ToString("F1") + " \n";
        }
        if(ability.GetType() == typeof(HealAbility))
        {
            text += "Healing: " + ((HealAbility)ability).Heal + "\n";
        }
        text += "Mana cost: " + ability.ManaCost + "\n";
        text += "Cooldown: " + ability.Cooldown.ToString("F1")+ "\n";

        if (ability.Loked)
            text += "<color=red>";
        for (int i = 0; i < ability.WeaponReq.Length; i++)
        {
            if (i == 0)
                 text += "Weapon set required: \n";
            text += ability.WeaponReq[i];
            if (i < ability.WeaponReq.Length - 1)
                text += ", ";            
        }
        if (ability.Loked)
            text += "</color>";
        return text; 
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }



    class AbilityInterface
    {
        GameObject[] abilitiesSlot;
        GridLayoutGroup grid;
        GameObject parent;

        public AbilityInterface(GameObject reference)
        {
            parent = reference;
            grid = reference.GetComponent<GridLayoutGroup>();
        }

        public void CreateAbilities(int col, GameObject prefab, float separation)
        {

            Vector2 cellSize = new Vector2((parent.GetComponent<RectTransform>().rect.width - (separation * (col-1))) / col, parent.GetComponent<RectTransform>().rect.height);
            grid.cellSize = cellSize;
            abilitiesSlot = new GameObject[col];

            for (int i = 0; i < col; i++)
            {
                GameObject instance = Instantiate(prefab);
                instance.transform.SetParent(parent.transform);
                instance.GetComponent<AbilityButtonManager>().SetPos(i);
                abilitiesSlot[i] = instance;                
            }
        }
        public GameObject GetSlot(int slot)
        {
            return abilitiesSlot[slot];
        }

    }

}
