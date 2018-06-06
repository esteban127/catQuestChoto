using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesID
{
    NullState = 0,
    Patrolling,
    ChasingPlayer,
    AttackingPlayer,
    Hurt,
    Die,
    Stunt,
    Stunned,
    Idle,

}

public enum TransitionsID
{
    NullTransition = 0,
    SawPlayer,
    LostPlayer,
    OnRageToAtack,
    OutOfRange,
    GettingHit,
    Dying,
    Stun,
    StealthAttack,    
    StopPatrolling,
    StartPatrolling,
    NoLongerHurt,



}

public class FSM
{
    List<FSMState> stateList;

    private StatesID currentStateID;
    public StatesID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSM()
    {
        stateList = new List<FSMState>();
    }


    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }
        if (stateList.Count == 0)
        {
            stateList.Add(state);
            currentState = state;
            currentStateID = state.ID;
            return;
        }

        // Add the state to the List if it's not inside it
        foreach (FSMState listedState in stateList)
        {
            if (listedState.ID == state.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + state.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        stateList.Add(state);
    }
    public void DeleteState(StatesID stateID)
    {        
        if (stateID == StatesID.NullState)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }
               
        foreach (FSMState listedState in stateList)
        {
            if (listedState.ID == stateID)
            {
                stateList.Remove(listedState);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + stateID.ToString() +
                       ". It was not on the list of states");

    }
    public void PerformTransition(TransitionsID transition)
    {
        if (transition == TransitionsID.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        StatesID id = currentState.GetOuputState(transition);
        if (id == StatesID.NullState)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + transition.ToString());
            return;
        }
        currentStateID = id;
        foreach (FSMState listedState in stateList)
        {
            if (listedState.ID == currentStateID)
            {
                // Do the post processing of the state before setting the new one
                currentState.DoBeforeLeaving();

                currentState = listedState;

                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering();
                break;
            }
        }

    }
}

public abstract class FSMState{


    Dictionary<TransitionsID, StatesID> map = new Dictionary<TransitionsID, StatesID>();
    protected StatesID stateID;
    public StatesID ID { get { return stateID; } }

    public void AddTransition(TransitionsID transID, StatesID statID)
    {
        if (transID == TransitionsID.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }
 
        if (statID == StatesID.NullState)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }
        if (map.ContainsKey(transID))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + transID.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        map.Add(transID, statID);
    }
    void DeleteTransition(TransitionsID transID)
    {
        // Check for NullTransition
        if (transID == TransitionsID.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(transID))
        {
            map.Remove(transID);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + transID.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    public StatesID GetOuputState(TransitionsID transition)
    {
        if (map.ContainsKey(transition))
        {
            return map[transition];
        }
        return StatesID.NullState;
    }

    public virtual void DoBeforeEntering() { }
    public virtual void DoBeforeLeaving() { }


    public abstract void Rason(GameObject player, GameObject npc);
    public abstract void Behavior(GameObject player, GameObject npc);



}
