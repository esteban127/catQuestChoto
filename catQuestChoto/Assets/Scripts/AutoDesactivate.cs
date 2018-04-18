using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDesactivate : MonoBehaviour {

    [SerializeField] float desactivateTime = 5;
    float currentTime = 0;
    private void Start()
    {
       
    }
    


    void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= desactivateTime)
        {
            gameObject.SetActive(false);
            currentTime = 0;
        }
            
    }
}
