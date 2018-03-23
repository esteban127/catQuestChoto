using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {
    
    private PoolManager myPoolManager;

    private void Awake()
    {
        myPoolManager = GetComponent<PoolManager>();
    }


    void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            myPoolManager.PoolRequest(transform.position, new Vector3(0, 0, 0), transform.localScale);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            myPoolManager.deleteFirstObject();
        }
	}
}
