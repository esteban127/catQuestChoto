using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilRotation : MonoBehaviour {
    
    public float evilness = 1.0f;
    
    void Update () {        
        transform.RotateAround(transform.position , new Vector3(0, 0, 1),evilness*Time.deltaTime);
  	}

    public void SetEvilness(int evilness)
    {
        this.evilness = evilness;
    }
}
