using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {
    
    private PoolManager myPoolManager;

    [SerializeField] string poolNameToUse;

    private void Awake()
    {
        myPoolManager = PoolManager.Instance;
    }


    void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            myPoolManager.RequestToPool(poolNameToUse,transform.position, new Vector3(0, 0, 0), transform.localScale);

        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            myPoolManager.DeleteFirstFromPool("cubePool");
        }
	}
}
