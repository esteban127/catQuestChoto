using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    [SerializeField] Slider slider;

    public void Activate()
    {
        gameObject.SetActive(true);
        setSliderProgress(0);
    }
    public void setSliderProgress(float progress)
    {        
        slider.value = progress;
    }
    public void Desactivate()
    {
        gameObject.SetActive(false);        
    }
}
