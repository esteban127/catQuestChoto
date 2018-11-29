using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPause : MonoBehaviour {   

    void Start()
    {
        Pause("Eh buachin, esos goblins se andan haciendo los piolas, mata a un par hasta que seas lvl 20 para que sepan quien manda");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            UnPause();
        }

    }

    public void Pause(string text)
    {        
        transform.parent.gameObject.SetActive(true);
        gameObject.GetComponentInChildren<Text>().text = text;
        Time.timeScale = 0;        
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        transform.parent.gameObject.SetActive(false);
    }

}
