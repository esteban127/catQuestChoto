using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(PoolManager))]
public class PoolManagerInspector : Editor {


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PoolManager myScript = (PoolManager)target;

        GUILayout.Label(myScript.ArrayVerify());
    }
    
}
#endif
