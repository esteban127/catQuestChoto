using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour{

    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] Color textColor;
    [SerializeField] float texTime;
    private healthManager healtM;
    private Weapon weapon;
    private float cFrames = 0;

    private float damage;

    void Start () {
        healtM = gameObject.GetComponent<healthManager>();
	}
    private void Update()
    {
        cFrames -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collide)
    {
        
        if(healtM.isAlive())
        {
            if (collide.tag == "Sword")
            {
                if(cFrames<=0)
                {
                    cFrames = 0.8f;

                    Debug.Log("Auch!");
                    weapon = collide.GetComponent<Weapon>();

                    Hit();
                    
                        
                }
            }
        }
    }
        
   

    private void Hit()
    {

        if (weapon.CurrentState != 0)
        {
            switch (weapon.CurrentState)
            {
                case 1:
                    damage = weapon.Damage;
                    break;
                case 2:
                    damage = weapon.Damage * 1.25f;
                    break;
                case 3:
                    cFrames += 1f;
                    damage = weapon.Damage * 1.5f;
                    break;
                case 4:
                    damage = weapon.Damage * 2.5f;
                    gameObject.GetComponent<MeleEnemyBehaviour>().Stun(true);
                    break;

            }
            damage += (damage * Random.Range(-0.2f, 0.2f));
            healtM.getDamage(damage);

    
            GameObject newText = Instantiate(damageTextPrefab, gameObject.transform.position, gameObject.transform.rotation);
            newText.transform.SetParent(gameObject.transform);
            newText.transform.localPosition = new Vector3(0, 0, 0);
            newText.GetComponent<damageTextController>().SetTextAndMove((Mathf.Round(damage * 100f)).ToString(), textColor);
            newText.SetActive(true);
            Destroy(newText.gameObject, texTime);
        }
    }
}
