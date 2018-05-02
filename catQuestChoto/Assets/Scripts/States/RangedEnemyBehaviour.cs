using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBehaviour : MonoBehaviour {


    private GameObject player;
    private FSM fsm;

    [SerializeField] int patrollDistance;

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

    private void BuildFSM()
    {
        RangedIdleState idle = new RangedIdleState();
        idle.AddTransition(TransitionsID.StealthAttack, StatesID.Stunt);
        idle.AddTransition(TransitionsID.StartPatrolling, StatesID.Patrolling);
        idle.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);

        RangedPatrollState patroll = new RangedPatrollState(patrollDistance);
        patroll.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);
        patroll.AddTransition(TransitionsID.StealthAttack, StatesID.Stunt);
        patroll.AddTransition(TransitionsID.StopPatrolling, StatesID.Idle);

        RangedChasePlayerState chase = new RangedChasePlayerState();
        chase.AddTransition(TransitionsID.LostPlayer, StatesID.Patrolling);
        chase.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);

        RangedAtackingState atacking = new RangedAtackingState();
        atacking.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        atacking.AddTransition(TransitionsID.GettingHit, StatesID.Hurt);

        RangedStuntState stunting = new RangedStuntState();
        stunting.AddTransition(TransitionsID.GettingHit, StatesID.Hurt);

        RangedHurtState hurting = new RangedHurtState();
        hurting.AddTransition(TransitionsID.Dying, StatesID.Die);
        hurting.AddTransition(TransitionsID.NoLongerHurt, StatesID.ChasingPlayer);

        RangedDyingState die = new RangedDyingState();





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

public class RangedIdleState : FSMState
{
    private float idleTime = 10;

    public RangedIdleState()
    {

        stateID = StatesID.Idle;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(1, 0, 0);



        if (idleTime <= 0)
        {
            idleTime = 10;
            Debug.Log("Gotham me necesita");
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.StartPatrolling);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.StealthAttack);
        }


        for (int i = 0; i < 35; i++)
        {
            direction.x = Mathf.Cos(10 * i) * direction.x - Mathf.Sin(10 * i) * direction.z;
            direction.z = Mathf.Sin(10 * i) * direction.x - Mathf.Cos(10 * i) * direction.z;
            if (Physics.Raycast(npc.transform.position, direction, out hit, 15))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    Debug.Log("Te vi vieja");
                    npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);
                    npc.GetComponent<RangedEnemyBehaviour>().SetPlayer(hit.transform.gameObject);
                    npc.transform.GetChild(1).gameObject.SetActive(true);
                    npc.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                }
            }
        }

    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        idleTime -= Time.deltaTime;
    }
}

public class RangedPatrollState : FSMState
{
    private CharacterController controller;
    private float speed = 5.0f;
    private float walkedDistance = 0;
    private int maxDistance;
    private float facingDirection = 1;
    private float patrollingTime = 10;

    public RangedPatrollState(int patrollDistance)
    {
        stateID = StatesID.Patrolling;
        maxDistance = patrollDistance;

    }



    public override void Rason(GameObject player, GameObject npc)
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(1, 0, 0);


        if (Input.GetKeyDown(KeyCode.O))
        {
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.StealthAttack);
        }

        if (patrollingTime <= 0)
        {
            patrollingTime = 10;
            Debug.Log("Uff me re canse loco");
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.StopPatrolling);
        }

        
        for (int i = 0; i < 35; i++)
        {
            


            direction.x = Mathf.Cos(10 * i) * direction.x - Mathf.Sin(10 * i) * direction.z;
            direction.z = Mathf.Sin(10 * i) * direction.x - Mathf.Cos(10 * i) * direction.z;
            if (Physics.Raycast(npc.transform.position, direction, out hit, 15))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    Debug.Log("Te vi vieja");
                    npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);
                    npc.GetComponent<RangedEnemyBehaviour>().SetPlayer(hit.transform.gameObject);
                    npc.GetComponentInChildren<EvilRotation>().SetEvilness(900);
                    npc.transform.GetChild(1).gameObject.SetActive(true);
                    npc.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                }
            }
        }

    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Vector3 moveDirection = facingDirection * npc.transform.forward;
        if (!controller)
        {
            controller = npc.gameObject.GetComponent<CharacterController>();
        }

        if (walkedDistance >= maxDistance)
        {
            facingDirection *= -1;
            walkedDistance = 0;
        }

        walkedDistance += speed * Time.deltaTime;

        if (!controller.isGrounded)
            moveDirection.y = -50 * Time.deltaTime;

        controller.Move(moveDirection * speed * Time.deltaTime);
        patrollingTime -= Time.deltaTime;
    }


}

public class RangedChasePlayerState : FSMState
{
    private CharacterController controller;
    private float speed = 5.0f;


    public RangedChasePlayerState()
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
                npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.LostPlayer);
                npc.GetComponent<RangedEnemyBehaviour>().SetPlayer(null);
                npc.GetComponentInChildren<EvilRotation>().SetEvilness(150);
                npc.transform.GetChild(2).gameObject.SetActive(true);
                npc.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            if ((player.transform.position - npc.transform.position).magnitude < 2)
            {
                Debug.Log("Omae wa mou shindeiru");
                npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.OnRageToAtack);
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

        controller.Move(moveDirection * speed * Time.deltaTime);
    }

}

public class RangedAtackingState : FSMState
{

    public RangedAtackingState()
    {
        stateID = StatesID.AttackingPlayer;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        if (!player.GetComponent<healthManager>().isAlive())
        {
            Debug.Log("Harakiri");
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.GettingHit);

        }
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        Debug.Log("Shine");
        player.GetComponent<healthManager>().Death();
    }

}

public class RangedStuntState : FSMState
{
    public RangedStuntState()
    {
        stateID = StatesID.Stunt;
    }
    public override void Rason(GameObject player, GameObject npc)
    {
        npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.GettingHit);
    }
    public override void Behavior(GameObject player, GameObject npc)
    {
        //nothing
    }
}

public class RangedHurtState : FSMState
{
    float hurtTime = 1;
    public RangedHurtState()
    {
        stateID = StatesID.Hurt;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        if (!npc.GetComponent<healthManager>().isAlive())
        {
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        else
        if (hurtTime <= 0)
        {
            npc.GetComponent<RangedEnemyBehaviour>().SetTransition(TransitionsID.NoLongerHurt);
        }
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        hurtTime -= Time.deltaTime;
        Debug.Log("Ouch");
        npc.GetComponent<healthManager>().getDamage(127);
    }


}

public class RangedDyingState : FSMState
{
    public RangedDyingState()
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
