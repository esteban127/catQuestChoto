using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class draginSpell : MonoBehaviour {

    AbilityInTree ability;
    [SerializeField] Vector3 offset;
    bool picked = false;
    public bool Picked { get { return picked; } }
    private void Update()
    {        
        transform.position = Input.mousePosition + offset;        
    }    
    public void Drag(Sprite newSprite, AbilityInTree treeAbility)
    {
        transform.position = Input.mousePosition + offset;
        ability = treeAbility;
        GetComponent<Image>().sprite = newSprite;
        picked = true;
        gameObject.SetActive(true);
    }
    public AbilityInTree Release()
    {
        picked = false;
        gameObject.SetActive(false);
        return ability;
    }
}
