using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour {

    [SerializeField] GameObject pauseDialog;
    private PoolManager myPoolManager;

    private void Start()
    {
        myPoolManager = PoolManager.Instance;
    }

    public void DeadEnd()
    {
        StartCoroutine(Dead());
    }
    public void WinEnd()
    {
        StartCoroutine(Win());
    }

    IEnumerator Dead()
    {
        pauseDialog.GetComponent<PopUpPause>().Pause("Que paso ameo? te mataron, mal ahi vieja");
        yield return new WaitForSeconds(0.1f);
        myPoolManager.DeleteAll();
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    IEnumerator Win()
    {
        pauseDialog.GetComponent<PopUpPause>().Pause("Bien ahi buasho, sos re copado");
        yield return new WaitForSeconds(0.1f);  
        myPoolManager.DeleteAll();
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }



}
