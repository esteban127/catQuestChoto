using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Sounds
{
    Spot,
    Die,  
    Dmg,
    Atk,
    Sl0,
    Sl1,
    Sl2,
    LvlUp
}

public class soundPlayer : MonoBehaviour {
    
    [SerializeField] AudioClip spotSfx;
    [SerializeField] AudioClip dieSfx;
    [SerializeField] AudioClip atackSfx;
    [SerializeField] AudioClip getDmgSfx;
    [SerializeField] AudioClip slash0;
    [SerializeField] AudioClip slash1;
    [SerializeField] AudioClip slash2;
    [SerializeField] AudioClip lvlUp;
    float vol = 0.3f;
    float atkVol = 0.3f;
    private AudioSource source;

    void Awake()
    {

        source = GetComponent<AudioSource>();

    }


    public void playSoud(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.Spot:
                source.PlayOneShot(spotSfx, vol);
                break;
            case Sounds.Die:
                source.PlayOneShot(dieSfx, vol);
                break;
            case Sounds.Atk:
                source.PlayOneShot(atackSfx, atkVol);
                break;
            case Sounds.Dmg:
                source.PlayOneShot(getDmgSfx, vol);
                break;
            case Sounds.Sl0:
                source.PlayOneShot(slash0, atkVol);
                break;
            case Sounds.Sl1:
                source.PlayOneShot(slash1, atkVol);
                break;
            case Sounds.Sl2:
                source.PlayOneShot(slash2, atkVol);
                break;
            case Sounds.LvlUp:
                source.PlayOneShot(lvlUp, vol);
                break;
        } 
    }
}
