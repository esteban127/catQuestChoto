using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadSystem : MonoBehaviour {

    [SerializeField] LoadingScreen LoadingScreen;

    static public LoadSystem instance = null;
    static public LoadSystem Instance { get { return instance; } }

    public delegate void LoadDelegate();
    public static event LoadDelegate OnMidleLoading;
    public static event LoadDelegate OnEndLoading;
      
    private void Awake()
    {
    
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }       
        LoadingScreen.Activate();
        StartCoroutine(Loading(2.0f));        
    }
    IEnumerator Loading(float time)
    {
        float CurrentTime = time;
        while (CurrentTime > time/2 )
        {            
            LoadingScreen.setSliderProgress((time - CurrentTime)/time );
            CurrentTime -= Time.deltaTime;
            yield return null;
        }
        OnMidleLoading();
        while (CurrentTime > 0)
        {            
            LoadingScreen.setSliderProgress((time - CurrentTime) / time);
            CurrentTime -= Time.deltaTime;
            yield return null;
        }
        OnEndLoading();
        LoadingScreen.Desactivate();
    }   
}
