using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSistem : MonoBehaviour
{
    int terrainLayer;
    private GameObject currentTarget;
    [SerializeField] GameObject targetBase;
    [SerializeField] GameObject targetBar;

    private void Awake()
    {
       terrainLayer = LayerMask.NameToLayer("Terrain");
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Transform select = GameObject.FindWithTag("Enemy").transform;
            if (Physics.Raycast(ray,out hit))
            {
                if (hit.transform.tag == "Enemy"|| hit.transform.tag == "Player" || hit.transform.tag == "NPC")
                {                    
                    currentTarget = hit.transform.gameObject;
                    Physics.Raycast(currentTarget.transform.position, Vector3.up * -1, out hit,20f, 1<<terrainLayer );
                    targetBase.transform.position = hit.point;
                    targetBase.transform.SetParent(currentTarget.transform);
                    targetBase.gameObject.SetActive(true);
                    //targetBar.GetComponent<TargetDispaly>().NewTarget(currentTarget);
                    if (currentTarget.tag == "NPC")
                    {
                        currentTarget.GetComponent<NpcIneraction>().Interact();
                    }

                }
            }

        }
    }

    public GameObject GetTarget()
    {
        return currentTarget;
    }



}
