using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {


    [SerializeField] pool[] poolArray;      

    static private PoolManager instance = null;
    static public PoolManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
        
        for (int i = 0; i < poolArray.Length; i++)
        {
            poolArray[i].InitializePool(gameObject);
        }        
    }

    [System.Serializable]
    class pool

    {  

        [SerializeField] private string poolName;

        [SerializeField] private int poolSize = 5;

        [SerializeField] private GameObject objectToPool;

        [SerializeField] private Vector3 deletedPos;

        [SerializeField] private bool forceExpand = false;

        private GameObject poolContainer;
        private GameObject[] myArray;

        public string PoolName { get { return poolName; } }
        public int PoolSize { get{ return poolSize; } }
        public GameObject ObjectToPool { get{ return objectToPool; } }



	    public void InitializePool (GameObject poolParent)
        {
            InitializePoolContainer(poolParent);
            myArray = new GameObject[poolSize];
            for (int i = 0; i < myArray.Length; i++)
            {
                GameObject instancie = Instantiate(objectToPool);
                PreInitializeObject(instancie);
                myArray[i] = instancie;            
            }
        }
    

        private void InitializePoolContainer(GameObject poolParent)
        {
            if(poolContainer == null)
            {
                poolContainer = new GameObject();
                poolContainer.transform.SetParent(poolParent.transform);
                poolContainer.name = poolName;

            }

        }

        public GameObject PoolRequest(Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            GameObject objectToReturn = null;            
            for (int i = 0; i < myArray.Length; i++)
            {
                if (!myArray[i].activeInHierarchy)
                {
                    objectToReturn = myArray[i];
                    InitializeObject(objectToReturn, pos, rotation, scale);
                    return objectToReturn;
                }
                
                if(i==myArray.Length-1)
                {
                    if (!forceExpand)
                        Debug.LogError("Pool is full");
                    else
                    {
                        ExpandPool();
                        objectToReturn = myArray[i];
                        InitializeObject(objectToReturn, pos, rotation, scale);
                    }
                }                               
            }
            return objectToReturn;

        }
        
        public void SetForceExpand(bool setActive) {
            forceExpand = setActive;
        }

        private void ExpandPool()
        {
            GameObject[]auxArray = new GameObject[myArray.Length + 1];
            for (int i = 0; i < myArray.Length; i++)
            {
                auxArray[i] = myArray[i];
            }
            GameObject instancie = Instantiate(objectToPool);
            PreInitializeObject(instancie);
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

        private void PreInitializeObject(GameObject objectToInitialize)
        {
            objectToInitialize.transform.SetParent(poolContainer.transform);
            objectToInitialize.transform.position = deletedPos;
            objectToInitialize.transform.eulerAngles = new Vector3(0,0,0);
            objectToInitialize.transform.localScale = new Vector3 (1,1,1);
            objectToInitialize.SetActive(false);
        }
        public void PoolObjectDelete(GameObject objectToDelete)
        {
            objectToDelete.transform.position = deletedPos;
            objectToDelete.SetActive(false);
        }

        public void DeleteFirstObject()
        {       
            for (int i = 0; i < myArray.Length; i++)
            {
                if (myArray[i].activeInHierarchy)
                {
                    PoolObjectDelete(myArray[i]);
                    return;
                }
                if(i==myArray.Length-1)
                {
                    Debug.LogError("The pool is empty");
                }
            }
                
        }

        public void DeleteLastObject()
        {
            for (int i = 1; i <= myArray.Length; i++)
            {
                if (myArray[myArray.Length-i].activeInHierarchy)
                {
                    PoolObjectDelete(myArray[myArray.Length - i]);
                    return;
                }
                if (i == myArray.Length)
                {
                    Debug.LogError("The pool is empty");
                }
            }
        }

    }

    private pool SearchPoolForName(string poolName)
    {
        for (int i = 0; i < poolArray.Length; i++)
        {
            if (poolArray[i].PoolName == poolName)                
                return poolArray[i];
        }
        Debug.LogError("Isnt a pool with that name");
        return null;
    }
    public GameObject RequestToPool(string poolName,Vector3 pos, Vector3 rotation,Vector3 scale)
    {

        pool poolToRequest = SearchPoolForName(poolName);      
                 
        return poolToRequest.PoolRequest(pos, rotation, scale);
        
    }       

    public void DeleteFirstFromPool(string poolName) {

        SearchPoolForName(poolName).DeleteFirstObject();

    }
    public void DeleteLastFromPool(string poolName)
    {

        SearchPoolForName(poolName).DeleteLastObject();

    }

    public string ArrayVerify()
    {
        string mensage;
        int count = 0;
        foreach(pool p in poolArray)
        {
            count++;
            if (string.IsNullOrEmpty(p.PoolName))
            {
                mensage = "WARNING The slot " + count + " Name is empty";
                return mensage;
            }
            if(p.ObjectToPool == null)
            {
                mensage = "WARNING " + p.PoolName + " has no object to pool";
                return mensage;
            }
            if(p.PoolSize == 0)
            {
                mensage = "WARNING The pool " + p.PoolName + " has no size";
                return mensage;
            }
            for (int i = 0; i < poolArray.Length; i++)
            {
                if (p.PoolName == poolArray[i].PoolName)
                {
                    if(p != poolArray[i])
                    {
                        mensage = "WARNING The name " + p.PoolName + " is repeated ";
                        return mensage;
                    }                    
                }
            }
            
        }
        mensage = "All is fine";
        return mensage;
    }



}
