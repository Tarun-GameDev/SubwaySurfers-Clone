using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayAudioAnimTrigger : MonoBehaviour
{
    [SerializeField]
    AudioSource audioClip;


    public void PlayAudioTrigger()
    {
        audioClip.Play();
    }
}
