using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditionBehaviour : MonoBehaviour {
    private GameObject player;
    private bool detect = false;
    private float fieldOfView = 30.0f;
    public void SetFieldOfView(float newView){ fieldOfView = newView; } 
    public GameObject Player {get{ return player; } }
    public bool Detect {get { return detect; } }

    //private Vector3 soundSource;
    //public Vector3 SoundSource{get{return soundSource;}}



    private void OnTriggerStay(Collider collide)
    {
        RaycastHit hit;
        if (collide.tag == "Player")
        {
            //soundSource = collide.transform.position;
            player = collide.gameObject;
            Vector3 direction =  player.transform.position - transform.position ;
            if (Vector3.Angle(transform.forward, direction ) < fieldOfView/2)
            {
                if(Physics.Raycast(transform.position, direction, out hit)){
                    if(hit.transform.gameObject.tag == "Player")
                    {
                        detect = true;
                    }
                    else
                    {
                        detect = false;
                    }
                }
                else
                {
                    detect = false;
                }
            }
            else
            {
                detect = false;
            }
        }
        else
        {
            detect = false;
        }
    }
}
