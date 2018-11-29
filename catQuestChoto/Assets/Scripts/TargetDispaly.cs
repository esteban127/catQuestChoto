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
    Clock timer;
    ActorStats targetStats;
    private void Start()
    {
        timer = Clock.Instance;
        timer.OnTick += actualziateLife;
    }
    public void NewTarget(GameObject newTarget)
    {
        gameObject.SetActive(true);
        targetStats = newTarget.GetComponent<ActorStats>();        
        targetSprite.sprite = targetStats.getActor().getImage();
        targetName.text = targetStats.getActor().Name;
        targetLevel.text = targetStats.getActor().Level.ToString();
    }

    private void actualziateLife(float time)
    {
        targetHealtBar.fillAmount = (targetStats.CurrentHealth / targetStats.MaxHealth());
        targetHealtText.text = (int)targetStats.CurrentHealth + " / " + (int)targetStats.MaxHealth();
    }
    
}
