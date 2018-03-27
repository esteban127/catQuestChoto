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
            GameObject instancie = Instantiate(objectToPool);
            PreInitializeObject(instancie, deletedPos);
            myArray[i] = instancie;            
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
        while (arrayPos<myArray.Length && myArray[arrayPos].activeInHierarchy)
        {
            arrayPos++;
        }

        if(arrayPos < myArray.Length)
        {
            Debug.Log("entre, array pos=" + arrayPos);
            objectToReturn = myArray[arrayPos];
            InitializeObject(objectToReturn, pos, rotation, scale);
        }
        else
        {
            if (!forceExpand)
                Debug.LogError("Pool is full");
            else
            {
                ExpandPool();
                objectToReturn = myArray[arrayPos];
                InitializeObject(objectToReturn, pos, rotation, scale);
            }
        }
                
        return objectToReturn;
    }

    private void ExpandPool()
    {
        GameObject[]auxArray = new GameObject[myArray.Length + 1];
        for (int i = 0; i < myArray.Length; i++)
        {
            auxArray[i] = myArray[i];
        }
        GameObject instancie = Instantiate(objectToPool);
        PreInitializeObject(instancie, deletedPos);
        auxArray[auxArray.Length - 1] = instancie;
        myArray = new GameObject[0];
        myArray = auxArray;
    }

    private void InitializeObject(GameObject objectToInitialize, Vector3 pos, Vector3 rotation, Vector3 scale)
    {
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
    public void poolObjectDelete(GameObject objectToDelete)
    {
        objectToDelete.transform.position = deletedPos;
        objectToDelete.SetActive(false);
    }

    public void deleteFirstObject()
    {
        int arrayPos = 0;

        while (arrayPos < myArray.Length && !myArray[arrayPos].activeInHierarchy )
        {
            arrayPos++;
        }
        if(arrayPos < myArray.Length)
        {
            poolObjectDelete(myArray[arrayPos]);
        }
        else
        {
            Debug.LogError("The pool is empty");
        }        
    }

    public void deleteLastObject()
    {
        int arrayPos = myArray.Length-1;

        while (arrayPos >= 0 &&!myArray[arrayPos].activeInHierarchy )
        {
            arrayPos--;
        }
        if (arrayPos >= 0)
        {
            poolObjectDelete(myArray[arrayPos]);
        }
        else
        {
            Debug.LogError("The pool is empty");
        }
    }


}
