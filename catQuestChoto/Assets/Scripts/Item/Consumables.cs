using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumables : Iitem
{
    [SerializeField]float duration = 5.0f;
    [SerializeField] bool restoration = true;
    public float Duration { get { return duration; } }
    public bool Restoration { get { return restoration; } }

}
