using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemyBehavior : MonoBehaviour {

    private GameObject player;
    private FSM fsm;

    [SerializeField] int patrollDistance;

    public void SetTransition(TransitionsID t) { fsm.PerformTransition(t); }
    public void SetPlayer(GameObject player) { this.player = player;}


    private void Awake()
    {
        BuildFSM();
        
    }

    private void Update()
    {
        fsm.CurrentState.Razon(player, gameObject);
        fsm.CurrentState.Behavior(player, gameObject);
    }

    private void BuildFSM()
    {
        PatrollState follow = new PatrollState(5);
        //follow.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);
 
        /*ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);*/
 
        fsm = new FSM();
        fsm.AddState(follow);
        //fsm.AddState(chase);
    }

    
}

public class PatrollState : FSMState
{
    private CharacterController controller;

    private float speed = 5.0f;
    private Vector3 moveDirection = new Vector3 (0,0,1);
    private int walkedDistance = 0;
    private int maxDistance;

    public PatrollState(int patrollDistance)
    {
        stateID = StatesID.Patrolling;
        maxDistance = patrollDistance;
    }
    public override void Razon(GameObject player, GameObject npc)
    {
        RaycastHit hit;
        for (int i = 0; i < 35; i++)
        {
            if (Physics.Raycast(npc.transform.position, new Vector3(0,10*i,0), out hit, 15F))
            {
                if (hit.transform.gameObject.tag == "Player")
                    npc.GetComponent<MeleEnemyBehavior>().SetTransition(TransitionsID.SawPlayer);
                    npc.GetComponent<MeleEnemyBehavior>().SetPlayer(hit.transform.gameObject);
            }
        }
        
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        if (!controller)
        {
          controller = npc.gameObject.GetComponent<CharacterController>();
        }

        if (walkedDistance >= maxDistance)
        {
            moveDirection *= -1;
        }

        controller.Move(moveDirection* speed * Time.deltaTime);
    }


}
