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
    [SerializeField] Tooltip toolTip;
    Animator playerAnimator;
    Clock timer;    
    [SerializeField] float castTime = 0.6f;
    float currentCastTime = 0;
    IAbility castingAbility;
    GameObject castTarget;
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
        LoadSystem.OnEndLoading += EndOfLoad;
        timer = Clock.Instance;
        timer.OnTick += CastingTime;
    }
    private void OnDisable()
    {
        LoadSystem.OnEndLoading -= EndOfLoad;
    }
    private void EndOfLoad()
    {
        playerAnimator = playerRef.GetComponentInChildren<Animator>();
    }
    private void CastingTime(float time)
    {
        if (currentCastTime>0)
        {
            currentCastTime -= time;
            if (currentCastTime <= 0)
                castingAbility.CastEffect(castTarget.GetComponent<ActorStats>(), playerRef.GetComponent<ActorStats>());
                playerRef.GetComponent<InputController>().Casting = false;
        }
    }

    void Update()
    {
        if (playerRef.GetComponent<CharacterStats>().Alive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TryCast(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TryCast(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TryCast(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                TryCast(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                TryCast(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                TryCast(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                TryCast(6);
            }
        }        
    }

    public bool TryCast(int pos)
    {
        if (aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability != null)
        {
            if ((aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability.TryCastAbility(playerRef.GetComponent<TargetSistem>().GetTarget(), playerRef, "Enemy")))
            {
                playerAnimator.SetTrigger(aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability.AbilityAnimation.ToString());
                TriggerGlobalColdown();
                castingAbility = aInterface.GetSlot(pos).GetComponent<AbilityButtonManager>().Ability;
                castTarget = playerRef.GetComponent<TargetSistem>().GetTarget();
                currentCastTime = castTime;
                playerRef.GetComponent<InputController>().Casting = true;
                return true;
            }
        }

        return false;
    }

    private void TriggerGlobalColdown()
    {
        for (int i = 0; i < numberOfSpellsInBar; i++)
        {
            if (aInterface.GetSlot(i).GetComponent<AbilityButtonManager>().Ability != null)
            {
                aInterface.GetSlot(i).GetComponent<AbilityButtonManager>().Ability.PutInGlobarColdown();
            }
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
        if (ability != null && ability.Name != "Empty")
        {       
            toolTip.ShowToolTip(GenerateAbilityText(ability));
        }
    }

    private string GenerateAbilityText(IAbility ability)
    {
        string text = "";
        text += "<b>" + ability.Name + "</b>      lvl: " + ability.Level + "\n";
        text += ability.Description + "\n";
        if (ability.GetType() == typeof(AtackAbility))
        {
            text += "Damage: " + (playerRef.GetComponent<CharacterStats>().MinDamage() * ((AtackAbility)ability).DamageMultiplier).ToString("F1") + " - " + (playerRef.GetComponent<CharacterStats>().MaxDamage() * ((AtackAbility)ability).DamageMultiplier).ToString("F1") + " \n";
        }
        if (ability.GetType() == typeof(HealAbility))
        {
            if(((HealAbility)ability).Heal> 0)
                text += "Healing: " + ((HealAbility)ability).Heal + "\n";
            if (((HealAbility)ability).Heal < 0)
                text += "Self damage: " + ((HealAbility)ability).Heal + "\n";
        }
        text += "Mana cost: " + ability.ManaCost + "\n";
        text += "Cooldown: " + ability.Cooldown.ToString("F1")+ "\n";
        if (ability.CurrentCharges > 0)
        {
            text += "Charges: " + ability.CurrentCharges + "\n";
        }
        for (int i = 0; i < ability.Buff.Length; i++)
        {
            text += "Apply the effect: " + ability.Buff[i].type + " with a potency of " + ((ability.Buff[i].potency + ability.BuffPotencyPerLevel[i] * ability.Level )).ToString("F1") + " for " + ability.Buff[i].remainTime + " seconds \n";
        }
        for (int i = 0; i < ability.Debuff.Length; i++)
        {
            text += "Apply the effect: " + ability.Debuff[i].type + " with a potency of " + ((ability.Debuff[i].potency + ability.DebuffPotencyPerLevel[i] * ability.Level)).ToString("F1") + " for " + ability.Debuff[i].remainTime + " seconds \n";
        }

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
        toolTip.Hide();
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
