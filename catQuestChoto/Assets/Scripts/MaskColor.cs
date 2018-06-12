using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskColor : MonoBehaviour {

    [SerializeField] Color color;

    private void Start()
    {
        gameObject.GetComponent<SkinnedMeshRenderer>().material.color = color;
    }
}
