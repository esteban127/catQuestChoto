using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ActiveBuffDebuff : MonoBehaviour {

    [SerializeField] int numberOfStatusInBar = 7;
    [SerializeField] float separation;
    [SerializeField] GameObject BuffDebuffFeedPrefab;
    [SerializeField] Tooltip tooltip;
    BuffDebuffSystem status;
    BuffDebuffFeed[] feeds;    
    Clock timer;
    private void Start()
    {
        timer = Clock.Instance;
        timer.OnTick+= ActualizateFeeds;
        CreateFeeds();
    }

    public void setStatus(BuffDebuffSystem newStatus)
    {
        if(status!= null)
        {
            status.onStatusChange -= setFeeds;
        }
        status = newStatus;
        status.onStatusChange += setFeeds;
        setFeeds();
    }

    private void setFeeds()
    {
        for (int i = 0; i < status.ActiveBuff.Count; i++)
        {
            feeds[i].BuffInitialize(status.ActiveBuff[i]);
        }
        for (int i = 0; i < status.ActiveDebuff.Count; i++)
        {
            if(i+ status.ActiveBuff.Count <= numberOfStatusInBar)
                feeds[i+ status.ActiveBuff.Count].DebuffInitialize(status.ActiveDebuff[i]);
        }
        for (int i = (status.ActiveBuff.Count + status.ActiveDebuff.Count); i < numberOfStatusInBar; i++)
        {
            feeds[i].desactivate();
        }
    }

    private void CreateFeeds()
    {  
        Vector2 cellSize = new Vector2((GetComponent<RectTransform>().rect.width - (separation * (numberOfStatusInBar - 1))) / numberOfStatusInBar, GetComponent<RectTransform>().rect.height);
        GetComponent<GridLayoutGroup>().cellSize = cellSize;
        feeds = new BuffDebuffFeed[numberOfStatusInBar];
        for (int i = 0; i < numberOfStatusInBar; i++)
        {
            GameObject instance = Instantiate(BuffDebuffFeedPrefab);
            instance.transform.SetParent(transform);
            feeds[i] = instance.GetComponent<BuffDebuffFeed>();
            feeds[i].SetTooltip(tooltip);
            instance.SetActive(false);
        }
    }      

    public void ActualizateFeeds(float time)
    {
        for (int i = 0; i < feeds.Length; i++)
        {
            if (feeds[i].IsActive)
            {
                feeds[i].Actualizate();
            }
        }
    }

}
