using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private GameObject[] myArray;
    
    [SerializeField]
    private int poolSize = 5;

    [SerializeField]
    private GameObject objectToPool;

    [SerializeField]
    private string poolName = "pool";

    [SerializeField]
    private Vector3 deletedPos;

    private GameObject poolContainer;
    

	void Awake () {
        InitializePoolContainer();
        myArray = new GameObject[poolSize];
        for (int i = 0; i < myArray.Length; i++)
        {
            Instantiate(objectToPool);
            myArray[i] = (objectToPool);
            PreInitializeObject(objectToPool, deletedPos);
        }        
	}
        
   private void InitializePoolContainer()
    {
        if(poolContainer == null)
        {
            poolContainer = new GameObject();
            poolContainer.name = poolName;

        }

    }

    public GameObject PoolRequest(Vector3 pos, Vector3 rotation, Vector3 scale, bool forceExpand = false)
    {
        GameObject objectToReturn = null;
        int arrayPos = 0;

        while (myArray[arrayPos].activeInHierarchy && arrayPos<myArray.Length)
        {
            arrayPos++;
        }
        if(arrayPos>= myArray.Length)
        {
            if (!forceExpand)
                Debug.LogError("Pool is full");
            else
            {
                ExandPool();
                objectToReturn = (GameObject)Instantiate(objectToPool);
                myArray.Add(objectToReturn);
                InitializeObject(objectToReturn, pos, rotation, scale);
            }
        }



        if( myArray.Length < poolSize)
        {
            for (int i = 0; i < myArray.Length; i++)
            {
                objectToReturn = (GameObject)Instantiate(objectToPool);
                myArray.Add(objectToReturn);
                InitializeObject(objectToReturn, pos, rotation, scale);

            }
            
        }
        else {
            if (myArray.Find(x => !x.activeInHierarchy))
            {
                objectToReturn = myArray.Find(x => !x.activeInHierarchy);
                InitializeObject(objectToReturn, pos, rotation, scale);
            }
            else
            {
                if (!forceExpand)
                    Debug.LogError("Pool is full");
                else
                {
                    objectToReturn = (GameObject)Instantiate(objectToPool);
                    myArray.Add(objectToReturn);
                    InitializeObject(objectToReturn, pos, rotation, scale);
                }
            }
        }     

        return objectToReturn;
    }

    private void ExandPool()
    {
        GameObject[]auxPool = new GameObject[myArray.Length + 1];
        for (int i = 0; i < myArray.Length; i++)
        {
            auxPool[i] = myArray[i];
        }
    }

    private void InitializeObject(GameObject objectToInitialize, Vector3 pos, Vector3 rotation, Vector3 scale)
    {
        objectToInitialize.transform.SetParent(poolContainer.transform);
        objectToInitialize.transform.position = pos;       
        objectToInitialize.transform.eulerAngles = rotation;
        objectToInitialize.transform.localScale = scale;
        objectToInitialize.SetActive(true);
    }

    private void PreInitializeObject(GameObject objectToInitialize, Vector3 deletedPos)
    {
        objectToInitialize.transform.SetParent(poolContainer.transform);
        objectToInitialize.transform.position = deletedPos;
        objectToInitialize.transform.eulerAngles = new Vector3(0,0,0);
        objectToInitialize.transform.localScale = new Vector3 (1,1,1);
        objectToInitialize.SetActive(false);
    }
    public void poolObjectDelete(GameObject objectToDelete, Vector3 deletedPos)
    {
        objectToDelete.transform.position = deletedPos;
        objectToDelete.SetActive(false);
    }

    public void deleteFirstObject()
    {
        if (myArray.Find(x => x.activeInHierarchy))
            poolObjectDelete(myArray.Find(x => x.activeInHierarchy));
        else
            Debug.LogError("The pool is empty");
    }
    
	
}
