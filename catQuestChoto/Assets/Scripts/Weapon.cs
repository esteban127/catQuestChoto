using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private int currentState;
    [SerializeField] private float damage;
    public int CurrentState{ get{ return currentState; } }
    public float Damage { get { return damage; } }
       

    public void SetState(int state)
    {
        currentState = state;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

}
