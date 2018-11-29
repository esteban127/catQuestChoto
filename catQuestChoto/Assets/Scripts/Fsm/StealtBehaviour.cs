using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealtBehaviour : MonoBehaviour {
    [SerializeField] float detectionRadius;
    [SerializeField] float fieldOfView;
    [SerializeField] int patrollDistance;

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
        idle.AddTransition(TransitionsID.StealthAttack, StatesID.Stunt);
        idle.AddTransition(TransitionsID.StartPatrolling, StatesID.Patrolling);
        idle.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);

        StealthPatrollState patroll = new StealthPatrollState(patrollDistance);
        patroll.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);
        patroll.AddTransition(TransitionsID.StealthAttack, StatesID.Stunt);
        patroll.AddTransition(TransitionsID.StopPatrolling, StatesID.Idle);

        StealthChasePlayerState chase = new StealthChasePlayerState();
        chase.AddTransition(TransitionsID.LostPlayer, StatesID.Patrolling);
        chase.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);

        StealthAtackingState atacking = new StealthAtackingState();
        atacking.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        atacking.AddTransition(TransitionsID.GettingHit, StatesID.Hurt);

        StealthStuntState stunting = new StealthStuntState();
        stunting.AddTransition(TransitionsID.GettingHit, StatesID.Hurt);

        StealthHurtState hurting = new StealthHurtState();
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
}


public class StealthIddle : FSMState
{
    private float idleTime = 10;

    public StealthIddle()
    {

        stateID = StatesID.Idle;
    }

    public override void Rason(GameObject player, GameObject npc)
    {        
        AuditionBehaviour audio = npc.GetComponent<AuditionBehaviour>();

        if (idleTime <= 0)
        {
            idleTime = 10;
            Debug.Log("Gotham me necesita");
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.StartPatrolling);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.StealthAttack);
        }
                       
                if (audio.Detect)
                {
                    Debug.Log("Te vi vieja");
                    npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.SawPlayer);
                    npc.GetComponent<StealtBehaviour>().SetPlayer(audio.Player);
                    npc.transform.GetChild(1).gameObject.SetActive(true);
                    npc.transform.GetChild(2).gameObject.SetActive(false);
                    npc.transform.LookAt(audio.Player.transform.position);

                }
            
        

    }
    public override void Behavior(GameObject player, GameObject npc)
    {
        
        idleTime -= Time.deltaTime;
    }
}


public class StealthPatrollState : FSMState
{
    private CharacterController controller;
    private float speed = 5.0f;
    private float walkedDistance = 0;
    private int maxDistance;    
    private float patrollingTime = 10;

    public StealthPatrollState(int patrollDistance)
    {
        stateID = StatesID.Patrolling;
        maxDistance = patrollDistance;

    }



    public override void Rason(GameObject player, GameObject npc)
    {
        AuditionBehaviour audio = npc.GetComponent<AuditionBehaviour>();       


        if (Input.GetKeyDown(KeyCode.O))
        {
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.StealthAttack);
        }

        if (patrollingTime <= 0)
        {
            patrollingTime = 10;
            Debug.Log("Uff me re canse loco");
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.StopPatrolling);
        }


        if (audio.Detect)
        {
            Debug.Log("Te vi vieja");
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.SawPlayer);
            npc.GetComponent<StealtBehaviour>().SetPlayer(audio.Player);
            npc.transform.GetChild(1).gameObject.SetActive(true);
            npc.transform.GetChild(2).gameObject.SetActive(false);
            npc.transform.LookAt(audio.Player.transform.position);
        }

    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Vector3 moveDirection =  npc.transform.forward;
        if (!controller)
        {
            controller = npc.gameObject.GetComponent<CharacterController>();
        }

        if (walkedDistance >= maxDistance)
        {
            npc.transform.LookAt(npc.transform.position - npc.transform.forward );
            walkedDistance = 0;
        }

        walkedDistance += speed * Time.deltaTime;

        if (!controller.isGrounded)
            moveDirection.y = -50 * Time.deltaTime;

        controller.Move(moveDirection * speed * Time.deltaTime);
        patrollingTime -= Time.deltaTime;
    }


}

public class StealthChasePlayerState : FSMState
{
    private CharacterController controller;
    private float speed = 5.0f;


    public StealthChasePlayerState()
    {
        stateID = StatesID.ChasingPlayer;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        AuditionBehaviour audio = npc.GetComponent<AuditionBehaviour>();

        
            if (!audio.Detect)
            {
                Debug.Log("Donde te fuiste loco?");
                npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.LostPlayer);
                npc.GetComponent<StealtBehaviour>().SetPlayer(null);                
                npc.transform.GetChild(2).gameObject.SetActive(true);
                npc.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            if ((player.transform.position - npc.transform.position).magnitude < 2)
            {
                Debug.Log("Omae wa mou shindeiru");
                npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.OnRageToAtack);
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

public class StealthAtackingState : FSMState
{

    public StealthAtackingState()
    {
        stateID = StatesID.AttackingPlayer;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        if (!player.GetComponent<healthManager>().isAlive())
        {
            Debug.Log("Harakiri");
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.GettingHit);

        }
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Debug.Log("Shine");
        player.GetComponent<healthManager>().Death();
    }

}

public class StealthStuntState : FSMState
{
    public StealthStuntState()
    {
        stateID = StatesID.Stunt;
    }
    public override void Rason(GameObject player, GameObject npc)
    {
        npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.GettingHit);
    }
    public override void Behavior(GameObject player, GameObject npc)
    {
        //nothing
    }
}

public class StealthHurtState : FSMState
{
    float hurtTime = 1;
    public StealthHurtState()
    {
        stateID = StatesID.Hurt;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        if (!npc.GetComponent<healthManager>().isAlive())
        {
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.Dying);
        }
        else
        if (hurtTime <= 0)
        {
            npc.GetComponent<StealtBehaviour>().SetTransition(TransitionsID.NoLongerHurt);
        }
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        hurtTime -= Time.deltaTime;
        Debug.Log("Ouch");
        npc.GetComponent<healthManager>().getDamage(127);
    }


}

public class StealthDyingState : FSMState
{
    public StealthDyingState()
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


