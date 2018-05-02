using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealtBehaviour : MonoBehaviour {
    [SerializeField] float detectionRadius;
    [SerializeField] float fieldOfView;

    private GameObject player;
    private FSM fsm;
    public void SetTransition(TransitionsID t) { fsm.PerformTransition(t); }
    public void SetPlayer(GameObject player) { this.player = player; }


    private void Awake()
    {
        BuildFSM();

    }

    private void Update()
    {
        fsm.CurrentState.Rason(player, gameObject);
        fsm.CurrentState.Behavior(player, gameObject);
    }

    private void Start()
    {
        gameObject.GetComponent<SphereCollider>().radius = detectionRadius;
    }


    private void BuildFSM()
    {
        StealthIddle idle = new StealthIddle();
        fsm = new FSM();
        fsm.AddState(idle);
    }
}


public class StealthIddle : FSMState
{
    //private float idleTime = 10;

    public StealthIddle()
    {

        stateID = StatesID.Idle;
    }

    public override void Rason(GameObject player, GameObject npc)
    {        
        AuditionBehaviour audio = npc.GetComponent<AuditionBehaviour>();

        /*if (idleTime <= 0)
        {
            idleTime = 10;
            Debug.Log("Gotham me necesita");
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.StartPatrolling);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.StealthAttack);
        }*/
                       
                if (audio.Detect)
                {
                    Debug.Log("Te vi vieja");
                    npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.SawPlayer);
                    npc.GetComponent<StealtBehaviour>().SetPlayer(audio.Player);
                    npc.transform.GetChild(1).gameObject.SetActive(true);
                    npc.transform.GetChild(2).gameObject.SetActive(false);                    
                }
            
        

    }
    public override void Behavior(GameObject player, GameObject npc)
    {
        //idleTime -= Time.deltaTime;
    }
}
