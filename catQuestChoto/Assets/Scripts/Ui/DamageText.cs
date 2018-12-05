using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    
    [SerializeField] Text damageText;
    [SerializeField] float basetextSpeed;
    float textSpeed;
    [SerializeField] float duration;

    private void Update()
    {    
        GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y +textSpeed*Time.deltaTime);
    }

    public void Create(string amount,bool goesUp, Color color)
    {   
        if (goesUp)
        {
            textSpeed = basetextSpeed;
        }
        else
        {
            textSpeed = basetextSpeed * -1;
        }
        damageText.color = color;
        damageText.text = amount;
        Destroy(gameObject, duration);
    }
}
