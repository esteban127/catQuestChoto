using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemyBehaviour : MonoBehaviour {

    private GameObject player;
    private FSM fsm;
    private Animator enemyAnimator;
    [SerializeField] int patrollDistance;

    public void SetTransition(TransitionsID t) { fsm.PerformTransition(t); }
    public void SetPlayer(GameObject player) { this.player = player;}

    

    private void Awake()
    {
        BuildFSM();
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        fsm.CurrentState.Rason(player, gameObject);
        fsm.CurrentState.Behavior(player, gameObject);        
    }

    private void BuildFSM()
    {
        IdleState idle = new IdleState();
        idle.AddTransition(TransitionsID.StealthAttack, StatesID.Stunt);
        idle.AddTransition(TransitionsID.StartPatrolling, StatesID.Patrolling);
        idle.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);

        PatrollState patroll = new PatrollState(patrollDistance);
        patroll.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);
        patroll.AddTransition(TransitionsID.StealthAttack, StatesID.Stunt);
        patroll.AddTransition(TransitionsID.StopPatrolling, StatesID.Idle);
 
        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(TransitionsID.LostPlayer, StatesID.Patrolling);
        chase.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);

        AtackingState atacking = new AtackingState();
        atacking.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        atacking.AddTransition(TransitionsID.GettingHit, StatesID.Hurt);

        StuntState stunting = new StuntState();
        stunting.AddTransition(TransitionsID.GettingHit, StatesID.Hurt);

        HurtState hurting = new HurtState();
        hurting.AddTransition(TransitionsID.Dying, StatesID.Die);
        hurting.AddTransition(TransitionsID.NoLongerHurt, StatesID.ChasingPlayer);

        DyingState die = new DyingState();

   



        fsm = new FSM();
        fsm.AddState(idle); 
        fsm.AddState(patroll);
        fsm.AddState(chase);
        fsm.AddState(atacking);
        fsm.AddState(stunting);
        fsm.AddState(hurting);
        fsm.AddState(die);
        
    }

    public void TriggerAnim(string triggerName)
    {
        enemyAnimator.SetTrigger(triggerName);
    }

    
}

public class IdleState : FSMState
{
    private float idleTime = 10;

    public IdleState()
    {
        
        stateID = StatesID.Idle;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        RaycastHit hit;      
        
        if (idleTime <= 0)
        {
            idleTime = 10;
            Debug.Log("Gotham me necesita");
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Run");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.StartPatrolling);
        }
        if (Physics.Raycast(npc.transform.position, player.transform.position - npc.transform.position, out hit, 15))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("Te vi vieja");
                npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);                    
                npc.transform.GetChild(1).gameObject.SetActive(true);
                npc.transform.GetChild(2).gameObject.SetActive(false);
                npc.transform.LookAt(player.transform.position);
                npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Run");

            }

        }        
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        idleTime -= Time.deltaTime;
    }
}

public class PatrollState : FSMState
{
    private CharacterController controller;
    private float speed = 3.0f;    
    private float walkedDistance = 0;
    private int maxDistance;
    private float facingDirection = 1;
    private float patrollingTime = 10;

    public PatrollState(int patrollDistance)
    {
        stateID = StatesID.Patrolling;
        maxDistance = patrollDistance;
        
    }  



    public override void Rason(GameObject player, GameObject npc)
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(1, 0, 0);
                       
        
        if (patrollingTime <= 0)
        {
            patrollingTime = 10;
            Debug.Log("Uff me re canse loco");
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Run");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.StopPatrolling);
        }

        if (Physics.Raycast(npc.transform.position, player.transform.position - npc.transform.position, out hit, 15))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
            Debug.Log("Te vi vieja");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);
            npc.transform.GetChild(1).gameObject.SetActive(true);
            npc.transform.GetChild(2).gameObject.SetActive(false);
            npc.transform.LookAt(player.transform.position);
            }
        }     
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Vector3 moveDirection = npc.transform.forward;
        if (!controller)
        {
            controller = npc.gameObject.GetComponent<CharacterController>();
        }

        if (walkedDistance >= maxDistance)
        {
            npc.transform.LookAt(npc.transform.position - npc.transform.forward);
            walkedDistance = 0;
        }

        walkedDistance += speed * Time.deltaTime;

        if (!controller.isGrounded)
            moveDirection.y = -50 * Time.deltaTime;

        controller.Move(moveDirection * speed * Time.deltaTime);
        patrollingTime -= Time.deltaTime;
    }


}

public class ChasePlayerState : FSMState
{
    private CharacterController controller;
    private float speed = 3.0f;


    public ChasePlayerState()
    {
        stateID = StatesID.ChasingPlayer;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        RaycastHit hit;

        if (Physics.Raycast(npc.transform.position, player.transform.position - npc.transform.position, out hit, 15))
        {
            if (hit.transform.gameObject.tag != "Player")
            {
                Debug.Log("Donde te fuiste loco?");
                npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.LostPlayer);
                npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Run");                
                npc.transform.GetChild(2).gameObject.SetActive(true);
                npc.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            if ((player.transform.position - npc.transform.position).magnitude < 2)
            {
                npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Atack");
                npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OnRageToAtack);
            }
        }

    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Vector3 moveDirection = (player.transform.position - npc.transform.position).normalized;
        if (!controller)
        {
            controller = npc.gameObject.GetComponent<CharacterController>();
        }

        if (!controller.isGrounded)
            moveDirection.y = -50 * Time.deltaTime;

        npc.transform.LookAt(player.transform.position);
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

}

public class AtackingState : FSMState
{

    public AtackingState()
    {
        stateID = StatesID.AttackingPlayer;
    }

    public override void Rason (GameObject player, GameObject npc)
    {
        npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OutOfRange);
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Debug.Log("Ataque");
        //player.GetComponent<healthManager>().Death();
    }
    
}

public class StuntState : FSMState
{
    public StuntState()
    {
        stateID = StatesID.Stunt;
    }
    public override void Rason(GameObject player, GameObject npc)
    {
        npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.GettingHit);
    }
    public override void Behavior(GameObject player, GameObject npc)
    {
        //nothing
    }
}

public class HurtState : FSMState
{
    float hurtTime = 1;
    public HurtState()
    {
        stateID = StatesID.Hurt;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        if (!npc.GetComponent<healthManager>().isAlive())
        {
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        else
        if(hurtTime<= 0)
        {
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.NoLongerHurt);
        }
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        hurtTime -= Time.deltaTime;
        Debug.Log("Ouch");
        npc.GetComponent<healthManager>().getDamage(127);
    }


}

public class DyingState : FSMState
{
    public DyingState()
    {
        stateID = StatesID.Die;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        //Im dead :c
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Debug.Log("Yea, noise? Then I'll be brief. O happy dagger! This is thy sheath; there rust, and let me die");
        npc.SetActive(false);
    }

}



