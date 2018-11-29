using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffType
{
    
    shield,
    hpRegen,
    manaRegen,    
    damageMultpy,     
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
}
[System.Serializable]
public class BuffDebuffSystem  {

    Clock timer;
    List<Buff> activeBuff;
    List<Debuff> activeDebuff;
    List<int> posToRemove;

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
                    
                }
            }                
        }
        if (!added)
        {
            activeBuff.Add(status);
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

                }
            }
        }
        if (!added)
        {
            activeBuff.Add(buff);
        }
    }


    public float getBuffPotency(BuffType type)
    {
        for (int i = 0; i < activeBuff.Count; i++)
        {
            if (activeBuff[i].type == type)
            {
                return activeBuff[i].potency;
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

                }
            }
        }
        if (!added)
        {
            activeDebuff.Add(status);
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

                }
            }
        }
        if (!added)
        {
            activeDebuff.Add(debuff);
        }
    }

    public float getDebuffPotency(DebuffType type)
    {
        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i].type == type)
            {
                return activeDebuff[i].potency;
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
            }
        }
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
        }
        posToRemove.Clear();
    }
    [System.Serializable]
    public class Buff
    {
        public BuffType type;
        public float potency;
        public float remainTime;

        public void reduceTime(float amount)
        {
            remainTime = remainTime - amount;
        }
        public Buff(BuffType ntype, float npotency, float nremainTime)
        {
            type = ntype;
            potency = npotency;
            nremainTime = remainTime;
        }        
    }
    [System.Serializable]
    public class Debuff
    {
        public DebuffType type;
        public float potency;
        public float remainTime;

        public void reduceTime(float amount)
        {
            remainTime = remainTime - amount;
        }
        public Debuff(DebuffType ntype, float npotency, float nremainTime)
        {
            type = ntype;
            potency = npotency;
            nremainTime = remainTime;
        }
    }

}
