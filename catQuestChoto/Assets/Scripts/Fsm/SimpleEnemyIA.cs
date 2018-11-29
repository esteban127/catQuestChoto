using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAbilitySystem))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(EnemyStats))]
public class SimpleEnemyIA : MonoBehaviour {

    private GameObject player;
    private PoolManager myPoolManager;
    private FSM fsm;
    private Animator enemyAnimator;
    [SerializeField] float speed = 3.0f;
    public float Speed { get { return speed; } }
    [SerializeField] int patrollDistance;
    public int PatrollDistance { get { return patrollDistance; } }
    [SerializeField] float detectionRange;
    public float DetectionRange { get { return detectionRange; } }
    Vector3 spawnPoint;
    public Vector3 SpawnPoint { get { return spawnPoint; } }
    public void SetTransition(TransitionsID t) { fsm.PerformTransition(t); }
    
    public void Initialize(Vector3 spawn, GameObject playerRef, int level)
    {
        myPoolManager = PoolManager.Instance;
        BuildFSM();
        GetComponent<EnemyAbilitySystem>().Initialize();
        GetComponent<EnemyStats>().Initialize(level);        
        player = playerRef;        
        enemyAnimator = GetComponentInChildren<Animator>();
        spawnPoint = spawn;
    }
    private void Update()
    {
        speed = speed - (speed * GetComponent<EnemyStats>().status.getDebuffPotency(DebuffType.slow));
        fsm.CurrentState.Rason(player, gameObject);
        fsm.CurrentState.Behavior(player, gameObject);
    }

    private void BuildFSM()
    {
        EnemyIdleState idle = new EnemyIdleState();
        idle.AddTransition(TransitionsID.StartPatrolling, StatesID.Patrolling);
        idle.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);

        EnemyPatrollState patroll = new EnemyPatrollState(patrollDistance);
        patroll.AddTransition(TransitionsID.SawPlayer, StatesID.ChasingPlayer);
        patroll.AddTransition(TransitionsID.StopPatrolling, StatesID.Idle);

        EnemyChasePlayerState chase = new EnemyChasePlayerState();
        chase.AddTransition(TransitionsID.LostPlayer, StatesID.GoingBack);
        chase.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);
        chase.AddTransition(TransitionsID.Dying, StatesID.Die);
        chase.AddTransition(TransitionsID.Stun, StatesID.Stunned);
        chase.AddTransition(TransitionsID.Respawn, StatesID.Idle);

        EnemyResetPosition reset = new EnemyResetPosition();
        reset.AddTransition(TransitionsID.Stun, StatesID.Stunned);
        reset.AddTransition(TransitionsID.Dying, StatesID.Die);
        reset.AddTransition(TransitionsID.PositionReseted, StatesID.Idle);

        EnemyAtackingState atacking = new EnemyAtackingState();
        atacking.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        atacking.AddTransition(TransitionsID.Dying, StatesID.Die);
        atacking.AddTransition(TransitionsID.Stun, StatesID.Stunned);
        atacking.AddTransition(TransitionsID.Respawn, StatesID.Idle);

        EnemyStunnedState stunned = new EnemyStunnedState();
        stunned.AddTransition(TransitionsID.OutOfRange, StatesID.ChasingPlayer);
        stunned.AddTransition(TransitionsID.OnRageToAtack, StatesID.AttackingPlayer);
        stunned.AddTransition(TransitionsID.Dying, StatesID.Die);

        EnemyDyingState die = new EnemyDyingState();
        die.AddTransition(TransitionsID.Respawn, StatesID.Idle);

        fsm = new FSM();
        fsm.AddState(idle);
        fsm.AddState(patroll);
        fsm.AddState(chase);
        fsm.AddState(reset);
        fsm.AddState(atacking);
        fsm.AddState(stunned);
        fsm.AddState(die);

    }
    public bool TryCast(IAbility ability)
    {
        if (ability != null)
        {
            if (ability.TryCastAbility(player, gameObject, "Player"))
            {
                enemyAnimator.SetTrigger(ability.AbilityAnimation.ToString());                
                return true;
            }
        }
        return false;
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
        myPoolManager.DeleteThisFromPool(transform.parent.name, gameObject);
    }
}
public class EnemyIdleState : FSMState
{
    private CharacterController controller;
    private float idleTime = 10;

    public EnemyIdleState()
    {
        stateID = StatesID.Idle;
    }

    public override void Rason(GameObject player, GameObject enemy)
    {        

        if (idleTime <= 0)
        {
            idleTime = 10;
            //Debug.Log("Gotham me necesita");
            enemy.GetComponent<SimpleEnemyIA>().Run();
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.StartPatrolling);
        }
        if (Vector3.Magnitude(player.transform.position - enemy.transform.position) < enemy.GetComponent<SimpleEnemyIA>().DetectionRange)
        {
            //Debug.Log("Te vi vieja");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.SawPlayer);
            enemy.GetComponent<SimpleEnemyIA>().Run();

        }
    }
    public override void Behavior(GameObject player, GameObject enemy)
    {
        if (!controller)
        {
            controller = enemy.gameObject.GetComponent<CharacterController>();
        }
        if (!controller.isGrounded)
            controller.Move((enemy.transform.up * -1) * enemy.GetComponent<SimpleEnemyIA>().Speed * Time.deltaTime);
        idleTime -= Time.deltaTime;
    }
}

public class EnemyPatrollState : FSMState
{
    private CharacterController controller;
    private float walkedDistance = 0;
    private int maxDistance;
    private float patrollingTime = 10;

    public EnemyPatrollState(int patrollDistance)
    {
        stateID = StatesID.Patrolling;
        maxDistance = patrollDistance;

    }

    public override void Rason(GameObject player, GameObject enemy)
    {
        if (patrollingTime <= 0)
        {
            patrollingTime = 10;
            //Debug.Log("Uff me re canse loco");
            enemy.GetComponent<SimpleEnemyIA>().StopRunning();
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.StopPatrolling);
        }

        if (Vector3.Magnitude(player.transform.position - enemy.transform.position) < enemy.GetComponent<SimpleEnemyIA>().DetectionRange)
        {
            //Debug.Log("Te vi vieja");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.SawPlayer);
            enemy.GetComponent<SimpleEnemyIA>().Run();

        }
    }
    public override void Behavior(GameObject player, GameObject enemy)
    {
        Vector3 moveDirection = enemy.transform.forward;
        if (!controller)
        {
            controller = enemy.gameObject.GetComponent<CharacterController>();
        }

        if (walkedDistance >= maxDistance)
        {
            enemy.transform.LookAt(enemy.transform.position - enemy.transform.forward);
            walkedDistance = 0;
        }

        walkedDistance += enemy.GetComponent<SimpleEnemyIA>().Speed * Time.deltaTime;

        if (!controller.isGrounded)
            moveDirection.y = -1;

        controller.Move(moveDirection * enemy.GetComponent<SimpleEnemyIA>().Speed * Time.deltaTime);
        patrollingTime -= Time.deltaTime;
    }


}
public class EnemyChasePlayerState : FSMState
{
    private CharacterController controller;


    public EnemyChasePlayerState()
    {
        stateID = StatesID.ChasingPlayer;
    }

    public override void Rason(GameObject player, GameObject enemy)
    {
                
        if (!enemy.GetComponent<EnemyStats>().IsAlive)
        {
            player.GetComponent<CharacterStats>().addXp(enemy.GetComponent<EnemyStats>().XpReward());
            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Dead");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Dying);
        }
        else
        if (enemy.GetComponent<EnemyStats>().status.getDebuffPotency(DebuffType.stun)>0)
        {
            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Stunned");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Stun);
        }
        else
        if ((Vector3.Magnitude(player.transform.position - enemy.transform.position) >= enemy.GetComponent<SimpleEnemyIA>().DetectionRange*1.5))
        {
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.LostPlayer);       
        }
        else
        if ((Vector3.Magnitude(player.transform.position - enemy.transform.position) < enemy.GetComponent<EnemyAbilitySystem>().Ability.Range))
        {
            enemy.GetComponent<SimpleEnemyIA>().StopRunning();
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.OnRageToAtack);
        }
    }

    public override void Behavior(GameObject player, GameObject enemy)
    {
        
        Vector3 moveDirection = (player.transform.position - enemy.transform.position).normalized;
        if (!controller)
        {
            controller = enemy.gameObject.GetComponent<CharacterController>();
        }

        if (!controller.isGrounded)
            moveDirection.y = -50 * Time.deltaTime;

        enemy.transform.LookAt(new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z));
        controller.Move(moveDirection * enemy.GetComponent<SimpleEnemyIA>().Speed * Time.deltaTime);
        
    }


}

public class EnemyResetPosition : FSMState
{
    private CharacterController controller;


    public EnemyResetPosition()
    {
        stateID = StatesID.GoingBack;
    }

    public override void Rason(GameObject player, GameObject enemy)
    {
        if (!enemy.GetComponent<EnemyStats>().IsAlive)
        {
            player.GetComponent<CharacterStats>().addXp(enemy.GetComponent<EnemyStats>().XpReward());
            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Dead");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Dying);
        }
        else
        if (enemy.GetComponent<EnemyStats>().status.getDebuffPotency(DebuffType.stun) > 0)
        {
            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Stunned");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Stun);
        }
        else
        if (Vector3.Magnitude(enemy.GetComponent<SimpleEnemyIA>().SpawnPoint - enemy.transform.position) < 0.5)
        {            
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.PositionReseted);
            enemy.GetComponent<SimpleEnemyIA>().StopRunning();
        }
    }

    public override void Behavior(GameObject player, GameObject enemy)
    {       
        Vector3 moveDirection = (enemy.GetComponent<SimpleEnemyIA>().SpawnPoint - enemy.transform.position).normalized;
        if (!controller)
        {
            controller = enemy.gameObject.GetComponent<CharacterController>();
        }

        if (!controller.isGrounded)
            moveDirection.y = -50 * Time.deltaTime;

        enemy.transform.LookAt(new Vector3(enemy.GetComponent<SimpleEnemyIA>().SpawnPoint.x, enemy.transform.position.y, enemy.GetComponent<SimpleEnemyIA>().SpawnPoint.z));
        controller.Move(moveDirection * enemy.GetComponent<SimpleEnemyIA>().Speed * Time.deltaTime * 1.5f);    
    }


}

public class EnemyAtackingState : FSMState
{

    public EnemyAtackingState()
    {
        stateID = StatesID.AttackingPlayer;
    }

    public override void Rason(GameObject player, GameObject enemy)
    {

        if (!enemy.GetComponent<EnemyStats>().IsAlive)
        {
            player.GetComponent<CharacterStats>().addXp(enemy.GetComponent<EnemyStats>().XpReward());
            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Dead");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Dying);
        }
        else
        if (enemy.GetComponent<EnemyStats>().status.getDebuffPotency(DebuffType.stun) > 0)
        {

            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Stunned");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Stun);
        }
        else
        if ((Vector3.Magnitude(player.transform.position - enemy.transform.position) >= enemy.GetComponent<EnemyAbilitySystem>().Ability.Range))
        {

            enemy.GetComponent<SimpleEnemyIA>().Run();
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.OutOfRange);
        }

    }

    public override void Behavior(GameObject player, GameObject enemy)
    {        
        enemy.transform.LookAt(new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z));
        enemy.GetComponent<SimpleEnemyIA>().TryCast(enemy.GetComponent<EnemyAbilitySystem>().Ability);
    }
}

public class EnemyStunnedState : FSMState
{
    public EnemyStunnedState()
    {
        stateID = StatesID.Stunned;
    }
    public override void Rason(GameObject player, GameObject enemy)
    {
        if (!enemy.GetComponent<EnemyStats>().IsAlive)
        {
            player.GetComponent<CharacterStats>().addXp(enemy.GetComponent<EnemyStats>().XpReward());
            enemy.GetComponent<SimpleEnemyIA>().TriggerAnim("Dead");
            enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Dying);
        }
        else
        if (enemy.GetComponent<EnemyStats>().status.getDebuffPotency(DebuffType.stun) <= 0)
        {
            
            if ((Vector3.Magnitude(player.transform.position - enemy.transform.position) >= enemy.GetComponent<EnemyAbilitySystem>().Ability.Range))
            {
                enemy.GetComponent<SimpleEnemyIA>().Run();
                enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.OutOfRange);
            }
            else
            {
                enemy.GetComponent<SimpleEnemyIA>().StopRunning();
                enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.OnRageToAtack);
            }
        }
    }
    public override void Behavior(GameObject player, GameObject npc)
    {
        
    }
}

public class EnemyDyingState : FSMState
    {
        float despawnTime;
        public EnemyDyingState()
        {
            despawnTime = 5;
            stateID = StatesID.Die;
        }

        public override void Rason(GameObject player, GameObject enemy)
        {
            if (despawnTime <= 0)
            {
                despawnTime = 5;
                enemy.GetComponent<SimpleEnemyIA>().SetTransition(TransitionsID.Respawn);                
                enemy.GetComponent<SimpleEnemyIA>().Die();
            }
        }

        public override void Behavior(GameObject player, GameObject npc)
        {
            despawnTime -= Time.deltaTime;
        }

    }

