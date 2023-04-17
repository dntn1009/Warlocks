using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioClip BGMClip;
    [SerializeField] AudioMixerGroup BGMGroup;

    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioClip[] SFXClip;
    [SerializeField] AudioMixerGroup SFXGroup;

    void Start()
    {
        if (BGMSource != null)
        {
            BGMSource.clip = BGMClip;
            BGMSource.outputAudioMixerGroup = BGMGroup;
            BGMSource.Play();
        }
        if(SFXSource != null)
        {
            SFXSource.outputAudioMixerGroup = SFXGroup;
        }
    }

    public void SetSFX(int bullet)
    {
        if(SFXSource != null)
          SFXSource.clip = SFXClip[bullet];
    }

    public void SFXPlay()
    {
        SFXSource.Play();
    }

}
