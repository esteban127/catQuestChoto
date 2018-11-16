using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCComponent : MonoBehaviour {

    [SerializeField] private NPCActor actor;

    public IACTOR getActor()
    {
        return actor;
    }
    
    public void setActor(NPCActor newActor)
    {
        actor = newActor;
    }
}
