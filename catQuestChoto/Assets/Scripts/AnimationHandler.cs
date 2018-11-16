using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {

    InventoryManager iManager;
    Animator pAnimator;
    int currentWeaponSet = 0;
    [SerializeField] GameObject RSword;
    [SerializeField] GameObject LSword;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject RDagger;
    [SerializeField] GameObject LDagger;
    [SerializeField] GameObject Bow;
    [SerializeField] GameObject Staff;
    [SerializeField] GameObject TwoHandedSword;
    List<GameObject> activeWeapons;
    // Use this for initialization
    void Start () {
        iManager = InventoryManager.Instance;
        iManager.OnWeaponChange += CheckAnimationSet;
        pAnimator = GetComponentInChildren<Animator>();
        activeWeapons = new List<GameObject>();
	}



    private void CheckAnimationSet()
    {
        if(currentWeaponSet != (int)iManager.CurrentWeaponSet)
        {
            pAnimator.SetInteger("CurrentWeaponSet", (int)iManager.CurrentWeaponSet);
            pAnimator.SetTrigger("ChangeWeapon");
            for (int i = 0; i < activeWeapons.Count; i++)
            {
                activeWeapons[i].SetActive(false);
            }
            activeWeapons.Clear();
            switch (iManager.CurrentWeaponSet)
            {
                case WeaponSet.Fist:
                    break;
                case WeaponSet.OneHanded:
                    RSword.SetActive(true);
                    activeWeapons.Add(RSword);
                    break;
                case WeaponSet.SwordAndShield:
                    RSword.SetActive(true);
                    Shield.SetActive(true);
                    activeWeapons.Add(RSword);
                    activeWeapons.Add(Shield);
                    break;
                case WeaponSet.Bow:
                    Bow.SetActive(true);
                    activeWeapons.Add(Bow);
                    break;
                case WeaponSet.TwoHandedSword:
                    TwoHandedSword.SetActive(true);
                    activeWeapons.Add(TwoHandedSword);
                    break;
                case WeaponSet.Staff:
                    Staff.SetActive(true);
                    activeWeapons.Add(Staff);
                    break;
                case WeaponSet.DualBlades:
                    RSword.SetActive(true);
                    LSword.SetActive(true);
                    activeWeapons.Add(RSword);
                    activeWeapons.Add(LSword);
                    break;
            }

            currentWeaponSet = (int)iManager.CurrentWeaponSet;
        }
    }
}
