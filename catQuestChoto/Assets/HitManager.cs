using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour {

    private void OnTriggerStay(Collider collide)
    {
        if(collide.tag == "Enemy")
        {

            Debug.Log("holiWacho");

        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
