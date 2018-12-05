using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffType
{
    
    shield,
    hpRegen,
    manaRegen,    
    damageBuff,     
    defenseBuff,
    critChanceBuff,
    precisionBuff,
    dodgeBuff,
}
public enum DebuffType
{
    stun,
    damageOverTime,
    slow,
    damageReduction,
    defenseReduction,
}

[System.Serializable]
public class BuffDebuffSystem  {

    Clock timer;
    List<Buff> activeBuff;
    public List<Buff> ActiveBuff { get { return activeBuff; } }
    List<Debuff> activeDebuff;
    public List<Debuff> ActiveDebuff { get { return activeDebuff; } }
    List<int> posToRemove;
    public delegate void StatusDelegate();
    public StatusDelegate onStatusChange;


    public BuffDebuffSystem()
    {
        timer = Clock.Instance;
        activeBuff = new List<Buff>();
        activeDebuff = new List<Debuff>();
        posToRemove = new List<int>();  
        if(timer!= null)
        {
            timer.OnTick += reduceBuffDebuffTime;
        }
    }

    public void addBuff(BuffType type, float duration, float potency)
    {
        bool added = false;
        Buff status = new Buff(type,duration,potency);       
        for (int i = 0; i < activeBuff.Count; i++)
        {
            if(activeBuff[i].type == status.type)
            {
                added = true;
                if (activeBuff[i].potency <= status.potency)
                {
                    activeBuff[i] = status;
                    if(onStatusChange!= null)
                        onStatusChange();
                }
            }                
        }
        if (!added)
        {
            activeBuff.Add(status);
            if (onStatusChange != null)
                onStatusChange();
        }
    }
    public void addBuff(Buff buff)
    {
        bool added = false;        
        for (int i = 0; i < activeBuff.Count; i++)
        {
            if (activeBuff[i].type == buff.type)
            {
                added = true;
                if (activeBuff[i].potency <= buff.potency)
                {
                    activeBuff[i] = buff;
                    if (onStatusChange != null)
                        onStatusChange();
                }
            }
        }
        if (!added)
        {
            activeBuff.Add(buff);
            if (onStatusChange != null)
                onStatusChange();
        }
    }


    public float getBuffPotency(BuffType type)
    {
        for (int i = 0; i < activeBuff.Count; i++)
        {
            if (activeBuff[i].type == type)
            {
                return (activeBuff[i].potency*0.01f);
            }
        }
        return 0;
    }
    public void ReduceBuffPotency(float potency, BuffType type)
    {
        for (int i = 0; i < activeBuff.Count; i++)
        {
            if (activeBuff[i].type == type)
            {
                activeBuff[i].potency-= potency;
                if (activeBuff[i].potency < 0)
                    activeBuff[i].potency = 0;
                if (onStatusChange != null)
                    onStatusChange();
            }
        }        
    }

    public void addDebuff(DebuffType type, float duration, float potency)
    {
        bool added = false;
        Debuff status = new Debuff(type, duration, potency);
        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i].type == status.type)
            {
                added = true;
                if (activeDebuff[i].potency <= status.potency)
                {
                    activeDebuff[i] = status;
                    if (onStatusChange != null)
                        onStatusChange();
                }
            }
        }
        if (!added)
        {
            activeDebuff.Add(status);
            if (onStatusChange != null)
                onStatusChange();
        }
    }
    public void addDebuff(Debuff debuff)
    {
        bool added = false;
        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i].type == debuff.type)
            {
                added = true;
                if (activeDebuff[i].potency <= debuff.potency)
                {
                    activeDebuff[i] = debuff;
                    if (onStatusChange != null)
                        onStatusChange();
                }
            }
        }
        if (!added)
        {
            activeDebuff.Add(debuff);
            if (onStatusChange != null)
                onStatusChange();
        }
    }

    public float getDebuffPotency(DebuffType type)
    {
        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i].type == type)
            {
               return (activeDebuff[i].potency*0.01f);
            }
        }
        return 0;
    }
    public void ReduceDebuffPotency(float potency, DebuffType type)
    {
        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i].type == type)
            {
                activeDebuff[i].potency -= potency;
                if (activeDebuff[i].potency < 0)
                    activeDebuff[i].potency = 0;
                if (onStatusChange != null)
                    onStatusChange();
            }
        }
    }
    public void RemoveAllBuffAndDebuff()
    {
        activeBuff.Clear();
        activeDebuff.Clear();
        if (onStatusChange != null)
            onStatusChange();
    }

    private void reduceBuffDebuffTime(float time)
    {
        
        for (int i = 0; i < activeBuff.Count; i++)
        {            
            activeBuff[i].remainTime -= time;
            if(activeBuff[i].remainTime <= 0)
            {
                posToRemove.Add(i);
            }
        }
        for (int i = 0; i < posToRemove.Count; i++)
        {
            activeBuff.RemoveAt(posToRemove[posToRemove.Count-1- i]);
            if (onStatusChange != null)
                onStatusChange();
        }
        posToRemove.Clear();
        for (int i = 0; i < activeDebuff.Count; i++)
        {
            activeDebuff[i].remainTime -= time;
            if (activeDebuff[i].remainTime <= 0)
            {
                posToRemove.Add(i);
            }
        }
        for (int i = 0; i < posToRemove.Count; i++)
        {
            activeDebuff.RemoveAt(posToRemove[posToRemove.Count-1- i]);
            if (onStatusChange != null)
                onStatusChange();
        }
        posToRemove.Clear();
    }    
  

    [System.Serializable]
    public class Status
    {
        protected float startingTime;
        public float StartingTime { get { return startingTime; } }
        public float potency;
        public float remainTime;
    }

    [System.Serializable]
    public class Buff : Status
    {
        
        public BuffType type;        

        public void reduceTime(float amount)
        {
            remainTime = remainTime - amount;
        }
        public Buff(BuffType ntype, float npotency, float nremainTime)
        {
            type = ntype;
            potency = npotency;
            remainTime = nremainTime;
            startingTime = nremainTime;
        }
        public Sprite getImage()
        {

            Sprite itemImage = null;
            itemImage = Resources.Load<Sprite>("Art/BuffDebuffSprite/" + type);
            return itemImage;
        }
    }
    [System.Serializable]
    public class Debuff : Status
    {        
        public DebuffType type;        

        public void reduceTime(float amount)
        {
            remainTime = remainTime - amount;
        }
        public Debuff(DebuffType ntype, float npotency, float nremainTime)
        {
            type = ntype;
            potency = npotency;
            remainTime = nremainTime;
            startingTime = nremainTime;
        }
        public Sprite getImage()
        {
            Sprite itemImage = null;
            itemImage = Resources.Load<Sprite>("Art/BuffDebuffSprite/" + type);
            return itemImage;
        }
    }

}
