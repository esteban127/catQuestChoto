using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour {

    private healthManager healtM;
    private Weapon weapon;  

    private float damage;

    void Start()
    {
        healtM = gameObject.GetComponent<healthManager>();
    }
    
    private void OnTriggerEnter(Collider collide)
    {

        if (healtM.isAlive())
        {
            if (collide.tag == "EnemyWeapon")
            {
                weapon = collide.GetComponent<Weapon>();
                Hit();
            }
        }
    }



    private void Hit()
    {

        if (weapon.CurrentState != 0)
        {
            switch (weapon.CurrentState)
            {
                case 1:
                    damage = weapon.Damage;
                    weapon.SetState(0);
                    break;
                case 2:
                    damage = weapon.Damage * 1.25f;
                    weapon.SetState(0);
                    break;
                case 3:                   
                    damage = weapon.Damage * 1.5f;
                    weapon.SetState(0);
                    break;
                case 4:
                    damage = weapon.Damage * 2.5f;
                    weapon.SetState(0);
                    break;

            }
            damage += Mathf.Round(damage * Random.Range(-0.2f, 0.2f));
            healtM.getDamage((int)damage);
                        
        }
    }
}
