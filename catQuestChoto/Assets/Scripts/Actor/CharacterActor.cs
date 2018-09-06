using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum c_class
{
    warrior,
    archer,
    mage
}



public class CharacterActor : FriendlyActor {
    
    [SerializeField] c_class characterClass;
    [SerializeField] float experience;
    //[SerializeField] skilltree skilltree?
    //[SerializeField] inventory ?
    //[SerializeField] questLog
}
