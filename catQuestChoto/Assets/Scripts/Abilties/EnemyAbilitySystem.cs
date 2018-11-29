using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class EnemyAbilitySystem : MonoBehaviour {

    IAbility ability;
    public IAbility Ability { get { return ability; } }
    [SerializeField] string abilityName;
    [SerializeField] AbilityType abilityType;

    public void Initialize()
    {
        ability = LoadAbility(abilityName, abilityType,abilityClass.Enemy);
    }
    private IAbility LoadAbility(string abilityName, AbilityType type, abilityClass AbClass)
    {
        IAbility ability;
        string path = Application.dataPath + "/Resources/Json/Ability/" + AbClass + "/" + type + "/" + abilityName + ".Json";
        switch (type)
        {
            case AbilityType.AtackAbility:
                ability = (JsonUtility.FromJson<AtackAbility>(File.ReadAllText(path)));
                break;
            case AbilityType.HealAbility:
                ability = (JsonUtility.FromJson<HealAbility>(File.ReadAllText(path)));
                break;
            default:
                ability = ((JsonUtility.FromJson<AtackAbility>(File.ReadAllText(path))));
                break;
        }
        ability.Initialize();
        return ability;
    }


}
