using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyTipe
{
    mele,
    ranged
}

abstract class EnemyActor : IACTOR {

    protected FSMState myFsm;
    //item[] drop;

    protected void drop()
    {

    }


	

}
