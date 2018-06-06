﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemyBehaviour : MonoBehaviour {

    private GameObject player;
    private FSM fsm;
    private Animator enemyAnimator;
    private bool stunned = false; 
    [SerializeField] int patrollDistance;
    [SerializeField] float atackSpeed = 2.5f;
    [SerializeField] float damage = 2.5f;
    public void SetTransition(TransitionsID t) { fsm.PerformTransition(t); }
    public void SetPlayer(GameObject player) { this.player = player;}
    public void Stun(bool stun) { stunned = stun; }
    public float AtackSpeed {get { return atackSpeed; } }
    public float Damage { get { return damage; } }
    public bool Stunned { get { return stunned; } }


    private void Awake()
    {
        BuildFSM();
        enemyAnimator = GetComponentInChildren<Animator>();
    }
    


    private void Update()
    {
        fsm.CurrentState.Rason(player, gameObject);
        fsm.CurrentState.Behavior(player, gameObject);        
    }

    private void BuildFSM()
    {
        IdleState idle = new IdleState();       
        idle.AddTransition(TransitionsID.StartPatrolling, StatesID.Patrolling);
        idle.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);

        PatrollState patroll = new PatrollState(patrollDistance);
        patroll.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);        
        patroll.AddTransition(TransitionsID.StopPatrolling, StatesID.Idle);
 
        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(TransitionsID.LostPlayer, StatesID.Patrolling);
        chase.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);

        AtackingState atacking = new AtackingState();
        atacking.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);        
        atacking.AddTransition(TransitionsID.Dying, StatesID.Die);
        atacking.AddTransition(TransitionsID.Stun, StatesID.Stunned);

        StunnedState stunned = new StunnedState();
        stunned.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        stunned.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);
        stunned.AddTransition(TransitionsID.Dying, StatesID.Die);

        DyingState die = new DyingState();

   



        fsm = new FSM();
        fsm.AddState(idle); 
        fsm.AddState(patroll);
        fsm.AddState(chase);
        fsm.AddState(atacking);
        fsm.AddState(stunned);        
        fsm.AddState(die);
        
    }

    public void TriggerAnim(string triggerName)
    {
        enemyAnimator.SetTrigger(triggerName);
    }

    public void Run()
    {
        enemyAnimator.SetBool("Run", true);
    }
    public void StopRunning()
    {
        enemyAnimator.SetBool("Run", false);
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
        SimpleDetection detection = npc.GetComponentInChildren<SimpleDetection>();             
        
        if (idleTime <= 0)
        {
            idleTime = 10;
            Debug.Log("Gotham me necesita");
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.StartPatrolling);
        }
        if (detection.Detect)
        {
            npc.GetComponent<MeleEnemyBehaviour>().SetPlayer(detection.Player);
            Debug.Log("Te vi vieja");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.transform.GetChild(1).gameObject.SetActive(true);
            npc.transform.GetChild(2).gameObject.SetActive(false);           
            
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
        SimpleDetection detection = npc.GetComponentInChildren<SimpleDetection>();
        Vector3 direction = new Vector3(1, 0, 0);
                       
        
        if (patrollingTime <= 0)
        {
            patrollingTime = 10;
            Debug.Log("Uff me re canse loco");
            npc.GetComponent<MeleEnemyBehaviour>().StopRunning();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.StopPatrolling);
        }

        if (detection.Detect)
        {
            npc.GetComponent<MeleEnemyBehaviour>().SetPlayer(detection.Player);
            Debug.Log("Te vi vieja");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.transform.GetChild(1).gameObject.SetActive(true);
            npc.transform.GetChild(2).gameObject.SetActive(false);          
            
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
        SimpleDetection detection = npc.GetComponentInChildren<SimpleDetection>();
        
        
        if (!detection.Detect)
        {
            Debug.Log("Donde te fuiste loco?");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.LostPlayer);                           
            npc.transform.GetChild(2).gameObject.SetActive(true);
            npc.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        if (( new Vector2(player.transform.position.x, player.transform.position.z) - new Vector2(npc.transform.position.x, npc.transform.position.z)).magnitude < 1.2)
        {
            npc.GetComponent<MeleEnemyBehaviour>().StopRunning();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OnRageToAtack);
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

        npc.transform.LookAt(new Vector3(player.transform.position.x, npc.transform.position.y, player.transform.position.z));
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

}

public class AtackingState : FSMState
{
    float atackCooldown = 1.0f; 
    public AtackingState()
    {
        stateID = StatesID.AttackingPlayer;        
    }

    public override void Rason (GameObject player, GameObject npc)
    {

        if(!npc.GetComponent<healthManager>().isAlive())
        {
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Dead");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        if(npc.GetComponent<MeleEnemyBehaviour>().Stunned)
        {
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Stunned");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Stun);
        }
        if ((new Vector2(player.transform.position.x, player.transform.position.z) - new Vector2(npc.transform.position.x, npc.transform.position.z)).magnitude > 1.8)
        {
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OutOfRange);
        }
        
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        npc.transform.LookAt(new Vector3(player.transform.position.x, npc.transform.position.y, player.transform.position.z));
        if (atackCooldown <= 0)
        {
            atackCooldown = npc.GetComponent<MeleEnemyBehaviour>().AtackSpeed;
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Atack");
            player.GetComponent<healthManager>().getDamage(npc.GetComponent<MeleEnemyBehaviour>().Damage);
        }
        atackCooldown -= Time.deltaTime;
        
        
       
    }
    
}

public class StunnedState : FSMState
{
    float StunTime;
    public StunnedState()
    {
        StunTime = 4.5f;
        stateID = StatesID.Stunned;
    }
    public override void Rason(GameObject player, GameObject npc)
    {
        if (!npc.GetComponent<healthManager>().isAlive())
        {
            npc.GetComponent<MeleEnemyBehaviour>().Stun(false);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Dead");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        if (StunTime<=0)
        {
            npc.GetComponent<MeleEnemyBehaviour>().Stun(false);
            if ((new Vector2(player.transform.position.x, player.transform.position.z) - new Vector2(npc.transform.position.x, npc.transform.position.z)).magnitude > 1.8)
            {
                npc.GetComponent<MeleEnemyBehaviour>().Run();
                npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OutOfRange);
            }
            else
            {
                npc.GetComponent<MeleEnemyBehaviour>().StopRunning();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OnRageToAtack);
            }
        }
    }
    public override void Behavior(GameObject player, GameObject npc)
    {      
        StunTime -= Time.deltaTime;
    }
}


public class DyingState : FSMState
{
    float despawnTime;
    public DyingState()
    {
        despawnTime = 5;
        stateID = StatesID.Die;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        if (despawnTime <= 0)
            npc.SetActive(false);
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        despawnTime -= Time.deltaTime;
    }

}



