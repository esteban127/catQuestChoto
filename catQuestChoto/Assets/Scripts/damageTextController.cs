using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damageTextController : MonoBehaviour {

    private Text myText;
    [SerializeField] private float moveAmt;
    [SerializeField] private float moveSpeed;

    private Vector3[] moveDirs;
    private Vector3 myMoveDir;

    private bool canMove = false;

    private void Start()
    {
        moveDirs = new Vector3[]
        {
            transform.up,
            (transform.up +transform.right),
            (transform.up - transform.right)
        };
        myMoveDir = moveDirs[Random.Range(0, moveDirs.Length)];
    }

    private void Update()
    {
        if(canMove) transform.position = Vector3.MoveTowards(transform.position, transform.position + myMoveDir, moveAmt * (moveSpeed * Time.deltaTime));
    }

    public void SetTextAndMove(string damage, Color textColour)
    {
        myText = GetComponentInChildren<Text>();
        myText.color = textColour;
        myText.text = damage;
        canMove = true;
    }
}
