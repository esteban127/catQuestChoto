using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum statesID
{


}

public enum transitionsID
{



}

public class FSM
{
    List<FSMState> stateList;

    int currentStateID;

    void AddState(FSMState state)
    {

    }
    void DeleteState(statesID stateID)
    {

    }
    void performTransition(transitionsID transition)
    {

    }
}

public abstract class FSMState{


    //dictionary<TransitionID,StateID>
    statesID myID;

    void AddTransition(transitionsID transID, statesID statID)
    {
        
    }
    void DeleteTransition(transitionsID transID)
    {

    }

    FSMState GetOuputState(transitionsID)
    {

    }
    void Razon()
    {

    }
    void Behavior()
    {

    }



}
