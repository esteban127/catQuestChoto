using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeButton : MonoBehaviour {

    SaveLoad sLManager;
    private void Start()
    {
        sLManager = SaveLoad.Instance;
    }

    public void goToStartMenu()
    {
        sLManager.ChangeScene("StartMenu");
    }
    public void Quit()
    {
        sLManager.QuitApplication();
    }
    public void ReturnToTown()
    {
        sLManager.ChangeScene("Town");
    }

}
