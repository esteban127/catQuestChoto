using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStatButton : MonoBehaviour {

    [SerializeField] attribute attribute;
    [SerializeField] CharacterStats playerStats;
    public void onClick()
    {
        playerStats.addAttribute(attribute);
    }
}
