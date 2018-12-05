using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

    float CurrentTime = 0;
    [SerializeField] float tickTime = 0.1f;
    static private Clock instance = null;
    static public Clock Instance { get { return instance; } }

    public delegate void ClockDelegate(float clockTime);
    public ClockDelegate OnTick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
 
	// Update is called once per frame
	void Update () {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= tickTime)
        {
            CurrentTime = 0;
            OnTick(tickTime);
        }
	}
    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
