using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioSourceController : MonoBehaviour
{
    public AudioSource audioData;
    public AudioClip audioClip;

    void Awake()
    {
        audioData = GetComponent<AudioSource>();
        // audioData.Play();
        // audioData.PlayOneShot(audioData.clip, 1f);
    }

    private void Slash()
    {
        audioData.PlayOneShot(audioClip);
    }
}
