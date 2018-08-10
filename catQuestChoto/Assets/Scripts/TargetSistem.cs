using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSistem : MonoBehaviour
{
    private GameObject currentTarget;
    [SerializeField] GameObject targetBar;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Transform select = GameObject.FindWithTag("Enemy").transform;
            if (Physics.Raycast(ray,out hit))
            {              
                if (hit.transform.tag == "Enemy")
                {
                    currentTarget = hit.transform.gameObject;
                    targetBar.GetComponent<TargetDispaly>().NewTarget(currentTarget);
                }
            }

        }
    }

    public GameObject GetTarget()
    {
        return currentTarget;
    }



}
