using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErroPanel : MonoBehaviour {
    [SerializeField] Text errorLog;
    public void Error(string log)
    {
        gameObject.SetActive(true);
        errorLog.text = log;
    }
}
