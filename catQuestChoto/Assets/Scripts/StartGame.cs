using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private PoolManager myPoolManager;

    private void Start()
    {
        myPoolManager = PoolManager.Instance;
    }


    private void Update()
    {
        if (Input.GetAxis("Submit") != 0)
        {
            myPoolManager.DeleteAll();
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        if (Input.GetKey("escape"))
            Application.Quit();


    }
}
