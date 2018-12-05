using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDispaly : MonoBehaviour {

    [SerializeField] Image targetSprite;
    [SerializeField] Text targetName;
    [SerializeField] Image targetHealtBar;
    [SerializeField] Text targetHealtText;
    [SerializeField] Text targetLevel;
    [SerializeField] ActiveBuffDebuff activeBnD;
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] Transform healtBarTransform;
    float lastHealt;
    Clock timer;
    ActorStats targetStats;
    private void Start()
    {
        timer = Clock.Instance;        
        timer.OnTick += actualziateLife;
        LoadSystem.OnMidleLoading += MiddleLoad;
    }
    private void MiddleLoad()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        LoadSystem.OnMidleLoading -= MiddleLoad;
    }

    public void NewTarget(GameObject newTarget)
    {
        gameObject.SetActive(true);
        targetStats = newTarget.GetComponent<ActorStats>();
        lastHealt = targetStats.CurrentHealth;
        targetSprite.sprite = targetStats.getActor().getImage();
        targetName.text = targetStats.getActor().Name;
        activeBnD.setStatus(targetStats.status);
        targetLevel.text = targetStats.getActor().Level.ToString();
    }    

    private void actualziateLife(float time)
    {
        if(targetStats!= null)
        {
            if (targetStats.CurrentHealth - lastHealt > 1)
                GenerateDamageText((int)(targetStats.CurrentHealth - lastHealt), Color.green);
            else
           if (targetStats.CurrentHealth - lastHealt < -1)
                GenerateDamageText((int)(targetStats.CurrentHealth - lastHealt), Color.red);
            lastHealt = targetStats.CurrentHealth;
            targetHealtBar.fillAmount = (targetStats.CurrentHealth / targetStats.MaxHealth());
            targetHealtText.text = (int)targetStats.CurrentHealth + " / " + (int)targetStats.MaxHealth();

        }        
    }

    private void GenerateDamageText(int amount, Color color)
    {
        string text = "";
        if (amount > 0)
            text += "+";
        text += amount;
        DamageText dmgText = Instantiate(damageTextPrefab, healtBarTransform).GetComponent<DamageText>();
        dmgText.Create(text, false, color);
    }

}
