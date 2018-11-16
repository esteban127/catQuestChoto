using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropPhysics : MonoBehaviour {
    int terrainLayer;
    [SerializeField] float fallSpeed = 0.5f;
    private void Start()
    {
        terrainLayer = LayerMask.NameToLayer("Terrain");
        StartCoroutine(fall());
    }

    IEnumerator fall()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.up * -1, out hit, 20f, 1 << terrainLayer);
        Vector3 fall = new Vector3(0, -1, 0);
        while (Vector3.Magnitude(transform.position-hit.point)>0.05f)
        {
            transform.position += (fall * fallSpeed * Time.deltaTime);
            yield return null;

        }

        
    }
}
