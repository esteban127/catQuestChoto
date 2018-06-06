using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDetection : MonoBehaviour {

    [SerializeField] float detectionRadius;
    private  GameObject player;
    private bool detect = false;   
    public bool Detect { get { return detect; } }
    public GameObject Player { get { return player; } }

    private void Start()
    {
        gameObject.GetComponent<SphereCollider>().radius = detectionRadius;
    }

    private void OnTriggerStay(Collider collide)
    {
     
            if (collide.tag == "Player")
            {
                detect = true;
                player = collide.gameObject;

            }
            else
            {
            if(player!=null && (gameObject.transform.position-player.transform.position).magnitude > detectionRadius)
                detect = false;
            }
     
    }
}
