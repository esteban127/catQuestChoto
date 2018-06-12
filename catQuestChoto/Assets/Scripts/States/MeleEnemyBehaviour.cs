using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemyBehaviour : MonoBehaviour {

    private GameObject player;
    private PoolManager myPoolManager;
    private FSM fsm;
    private Animator enemyAnimator;
    private bool stunned = false;
    [SerializeField] float speed = 3.0f;
    [SerializeField] int killXp;
    [SerializeField] int patrollDistance;
    [SerializeField] float atackSpeed = 1f;
    [SerializeField] float damage = 2.5f;
    public void SetTransition(TransitionsID t) { fsm.PerformTransition(t); }
    public void SetPlayer(GameObject player) { this.player = player;}
    public void Stun(bool stun) { stunned = stun; }
    public float AtackSpeed {get { return atackSpeed; } }
    public float Damage { get { return damage; } }
    public float Speed { get { return speed; } }
    public bool Stunned { get { return stunned; } }
    public int KillXP { get { return killXp; } }

    private void Awake()
    {
        BuildFSM();
        enemyAnimator = GetComponentInChildren<Animator>();
        
    }
    private void Start()
    {
        myPoolManager = PoolManager.Instance;
        enemyAnimator.SetFloat("AtackSpeed", atackSpeed);
        gameObject.GetComponentInChildren<Weapon>().SetDamage(damage);
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
        chase.AddTransition(TransitionsID.Dying, StatesID.Die);
        chase.AddTransition(TransitionsID.Stun, StatesID.Stunned);
        chase.AddTransition(TransitionsID.Respawn, StatesID.Idle);

        AtackingState atacking = new AtackingState();
        atacking.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);        
        atacking.AddTransition(TransitionsID.Dying, StatesID.Die);
        atacking.AddTransition(TransitionsID.Stun, StatesID.Stunned);
        atacking.AddTransition(TransitionsID.Respawn, StatesID.Idle);

        StunnedState stunned = new StunnedState();
        stunned.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        stunned.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);
        stunned.AddTransition(TransitionsID.Dying, StatesID.Die);

        DyingState die = new DyingState();
        die.AddTransition(TransitionsID.Respawn, StatesID.Idle);

   



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

    public void Die()
    {
        enemyAnimator.ResetTrigger("Stunned");
        enemyAnimator.ResetTrigger("Atack");
        enemyAnimator.ResetTrigger("Dead");
        StopRunning();
        Stun(false);
        myPoolManager.DeleteThisFromPool(transform.parent.name, gameObject);
    }

}

public class IdleState : FSMState
{
    private CharacterController controller;
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
            //Debug.Log("Gotham me necesita");
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.StartPatrolling);
        }
        if (detection.Detect)
        {
            npc.GetComponent<MeleEnemyBehaviour>().SetPlayer(detection.Player);
            //Debug.Log("Te vi vieja");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.SawPlayer);
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.transform.GetChild(1).gameObject.SetActive(true);
            npc.transform.GetChild(2).gameObject.SetActive(false);          
            
        }        
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        if (!controller)
        {
            controller = npc.gameObject.GetComponent<CharacterController>();
        }
        if (!controller.isGrounded)
            controller.Move((npc.transform.up*-1) * npc.GetComponent<MeleEnemyBehaviour>().Speed * Time.deltaTime);
        idleTime -= Time.deltaTime;
    }
}

public class PatrollState : FSMState
{
    private CharacterController controller;      
    private float walkedDistance = 0;
    private int maxDistance;    
    private float patrollingTime = 10;

    public PatrollState(int patrollDistance)
    {
        stateID = StatesID.Patrolling;
        maxDistance = patrollDistance;
        
    }  



    public override void Rason(GameObject player, GameObject npc)
    {        
        SimpleDetection detection = npc.GetComponentInChildren<SimpleDetection>();     
        if (patrollingTime <= 0)
        {
            patrollingTime = 10;
            //Debug.Log("Uff me re canse loco");
            npc.GetComponent<MeleEnemyBehaviour>().StopRunning();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.StopPatrolling);
        }

        if (detection.Detect)
        {
            npc.GetComponent<MeleEnemyBehaviour>().SetPlayer(detection.Player);
            //Debug.Log("Te vi vieja");
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

        walkedDistance += npc.GetComponent<MeleEnemyBehaviour>().Speed * Time.deltaTime;

        if (!controller.isGrounded)
            moveDirection.y = -1;

        controller.Move(moveDirection * npc.GetComponent<MeleEnemyBehaviour>().Speed * Time.deltaTime);
        patrollingTime -= Time.deltaTime;
    }


}

public class ChasePlayerState : FSMState
{
    private CharacterController controller;    


    public ChasePlayerState()
    {
        stateID = StatesID.ChasingPlayer;
    }

    public override void Rason(GameObject player, GameObject npc)
    {
        SimpleDetection detection = npc.GetComponentInChildren<SimpleDetection>();

        if(player == null)
        {
            npc.GetComponentInChildren<SimpleDetection>().ResetDetect();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Respawn);            
        }
        else
        if (!npc.GetComponent<healthManager>().isAlive())
        {
            player.GetComponent<CharacterProgression>().GainXp(npc.GetComponent<MeleEnemyBehaviour>().KillXP);
            npc.GetComponentInChildren<Weapon>().SetState(0);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Dead");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        else
        if (npc.GetComponent<MeleEnemyBehaviour>().Stunned)
        {
            npc.GetComponentInChildren<Weapon>().SetState(0);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Stunned");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Stun);
        }
        else
        if (!detection.Detect)
        {
            //Debug.Log("Donde te fuiste loco?");
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
        if (player != null)
        {            

            Vector3 moveDirection = (player.transform.position - npc.transform.position).normalized;
            if (!controller)
            {           
                controller = npc.gameObject.GetComponent<CharacterController>();
            }

            if (!controller.isGrounded)
                moveDirection.y = -50 * Time.deltaTime;

            npc.transform.LookAt(new Vector3(player.transform.position.x, npc.transform.position.y, player.transform.position.z));
            controller.Move(moveDirection * npc.GetComponent<MeleEnemyBehaviour>().Speed * Time.deltaTime);
        }
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
        if (player == null)
        {
            npc.GetComponentInChildren<SimpleDetection>().ResetDetect();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Respawn);
        }
        else
        if (!npc.GetComponent<healthManager>().isAlive())
        {
            player.GetComponent<CharacterProgression>().GainXp(npc.GetComponent<MeleEnemyBehaviour>().KillXP);
            npc.GetComponentInChildren<Weapon>().SetState(0);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Dead");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        else
        if(npc.GetComponent<MeleEnemyBehaviour>().Stunned)
        {
            npc.GetComponentInChildren<Weapon>().SetState(0);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Stunned");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Stun);
        }
        else
        if ((new Vector2(player.transform.position.x, player.transform.position.z) - new Vector2(npc.transform.position.x, npc.transform.position.z)).magnitude > 1.4)
        {
            npc.GetComponentInChildren<Weapon>().SetState(0);
            npc.GetComponent<MeleEnemyBehaviour>().Run();
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.OutOfRange);
        }
        
    }

    public override void Behavior(GameObject player, GameObject npc)
    {
        if (player != null)
        {
            npc.transform.LookAt(new Vector3(player.transform.position.x, npc.transform.position.y, player.transform.position.z));
        if (atackCooldown <= 0)
        {
            atackCooldown = 3 / npc.GetComponent<MeleEnemyBehaviour>().AtackSpeed;
            npc.GetComponent<soundPlayer>().playSoud(Sounds.Atk);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Atack");
            npc.GetComponentInChildren<Weapon>().SetState(1);            
        }
        atackCooldown -= Time.deltaTime;


        }
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
            player.GetComponent<CharacterProgression>().GainXp(npc.GetComponent<MeleEnemyBehaviour>().KillXP);
            npc.GetComponent<MeleEnemyBehaviour>().Stun(false);
            npc.GetComponent<MeleEnemyBehaviour>().TriggerAnim("Dead");
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Dying);
        }
        else
        if (StunTime<=0)
        {
            StunTime = 4.5f;
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
        {
            despawnTime = 5;            
            npc.GetComponent<MeleEnemyBehaviour>().SetTransition(TransitionsID.Respawn);
            npc.GetComponent<healthManager>().Revive();
            npc.GetComponent<MeleEnemyBehaviour>().Die();
        }
    }   

    public override void Behavior(GameObject player, GameObject npc)
    {
        despawnTime -= Time.deltaTime;
    }

}



