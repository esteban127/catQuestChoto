using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour {

    [SerializeField] bool hasText = true;
	public void SetProgression(float progression)
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Slider>().value = progression;
    }

    public void SetText( string text)
    {
        if(hasText)
            gameObject.GetComponentInChildren<Text>().text = text;
    }
}
