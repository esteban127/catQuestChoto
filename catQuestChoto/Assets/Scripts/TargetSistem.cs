using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetSistem : MonoBehaviour
{
    int terrainLayer;
    private GameObject currentTarget;
    [SerializeField] GameObject targetBase;
    [SerializeField] GameObject targetBar;
    InventoryManager iManager;
    PoolManager myPoolManager;

    [SerializeField] Tooltip toolTip;

    private void Awake()
    {       
       terrainLayer = LayerMask.NameToLayer("Terrain");
    }
    private void Start()
    {
        iManager = InventoryManager.Instance;
        myPoolManager = PoolManager.Instance;
    }
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {                       
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit))
            {
                
                if (hit.transform.tag == "Enemy"|| hit.transform.tag == "Player" || hit.transform.tag == "NPC")
                {
                    toolTip.Hide();
                    if (Input.GetMouseButtonDown(0))
                    {
                        currentTarget = hit.transform.gameObject;
                        Physics.Raycast(currentTarget.transform.position, Vector3.up * -1, out hit,20f, 1<<terrainLayer );
                        targetBase.transform.position = hit.point;
                        targetBase.transform.SetParent(currentTarget.transform);
                        targetBase.gameObject.SetActive(true);
                        targetBar.GetComponent<TargetDispaly>().NewTarget(currentTarget);
                        if (currentTarget.tag == "NPC")
                        {
                            currentTarget.GetComponent<NpcIneraction>().Interact();
                        }
                    }
                }
                else
                {

                    if (hit.transform.tag == "Drop")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (iManager.PickItem(hit.transform.gameObject))
                            {
                                toolTip.Hide();
                                myPoolManager.DeleteThisFromPool(hit.transform.parent.name,hit.transform.gameObject);
                            }
                        }
                        else
                        {
                            ShowLootName(hit.transform.gameObject.GetComponent<ItemComponent>().GiveStats());
                        }
                    }
                    else
                    {
                        toolTip.Hide();
                        if (Input.GetMouseButtonDown(0))
                        {
                            
                            targetBase.SetActive(false);
                            currentTarget = null;
                            targetBar.SetActive(false);
                           
                        }
                    }
                }
                
            }
            else
            {
                toolTip.Hide();
            }
        }
        if (currentTarget != null)
        {
            if (!currentTarget.GetComponent<ActorStats>().Alive)
            {
                targetBase.SetActive(false);
                currentTarget = null;
                targetBar.SetActive(false);
            }
        }
    }

    private void ShowLootName(Iitem item)
    {
        string text = "";
        
        switch (item.Tier)
        {
            case ItemTier.Tier0:
                text += "<color=white>";
                break;
            case ItemTier.Tier1:
                text += "<color=blue>";
                break;
            case ItemTier.Tier2:
                text += "<color=yellow>";
                break;
            case ItemTier.Tier3:
                text += "<color=orange>";
                break;
        }
        text += ("<b>" + "<size=18>" + item.Name + "</size>" + "</b>" + "</color>");
        toolTip.ShowToolTip(text);     
    }

    public GameObject GetTarget()
    {
        return currentTarget;
    }



}
