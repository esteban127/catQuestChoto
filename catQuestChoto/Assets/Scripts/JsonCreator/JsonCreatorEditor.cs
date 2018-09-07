using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(JsonCreator)),CanEditMultipleObjects]
public class ObjectBuilderEditor : Editor
{
        public SerializedProperty
        file_tipe,
        item_tipe,
        baseStrength,
        baseConstitution,
        baseDextery,
        baseInteligence,
        baseLuck,
        itemTipe,
        size,
        generalStats,
        randomProperty,
        isConsumible,
        duration,
        a_tipe,
        defense,
        w_tipe, 
        minDmg,
        maxDmg,
        critDmg,
        baseCritChance;
    
    
      

    private void OnEnable()
    {
        file_tipe = serializedObject.FindProperty("fileTipe");
        item_tipe = serializedObject.FindProperty("itemTipe");
        baseStrength = serializedObject.FindProperty("baseStrength");
        baseConstitution = serializedObject.FindProperty("baseConstitution");
        baseDextery = serializedObject.FindProperty("baseDextery");
        baseInteligence = serializedObject.FindProperty("baseInteligence");
        baseLuck = serializedObject.FindProperty("baseLuck");
        itemTipe = serializedObject.FindProperty("itemTipe");
        size = serializedObject.FindProperty("size");
        generalStats = serializedObject.FindProperty("generalStats");
        randomProperty = serializedObject.FindProperty("randomProperty");
        isConsumible = serializedObject.FindProperty("isConsumible");
        duration = serializedObject.FindProperty("duration");
        a_tipe = serializedObject.FindProperty("a_tipe");
        defense = serializedObject.FindProperty("defense");
        w_tipe = serializedObject.FindProperty("w_tipe");
        minDmg = serializedObject.FindProperty("minDmg");
        maxDmg = serializedObject.FindProperty("maxDmg");
        critDmg = serializedObject.FindProperty("critDmg");
        baseCritChance = serializedObject.FindProperty("baseCritChance");
    }


    public override void OnInspectorGUI()
    {             
        
        EditorGUILayout.PropertyField(file_tipe);
        
        fileTipe fileT = (fileTipe)file_tipe.enumValueIndex;

        switch (fileT)
        {
            case fileTipe.character:

            
                EditorGUILayout.PropertyField(baseStrength);                
                EditorGUILayout.PropertyField(baseConstitution);
                EditorGUILayout.PropertyField(baseDextery);
                EditorGUILayout.PropertyField(baseInteligence);
                EditorGUILayout.PropertyField(baseLuck);             
                break;
            case fileTipe.item:
                EditorGUILayout.PropertyField(item_tipe);
                itemTipe itemT = (itemTipe)item_tipe.enumValueIndex;
                switch (itemT)
                {
                }

           break;
        }

        serializedObject.ApplyModifiedProperties();

        JsonCreator myScript = (JsonCreator)target;
        if (GUILayout.Button("Generate File"))
        {
            myScript.GenerateFile();
        }

    }
}
