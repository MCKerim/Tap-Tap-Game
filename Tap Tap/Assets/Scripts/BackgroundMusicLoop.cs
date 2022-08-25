using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusicLoop : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip loopStart;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(loopStart);
        audioSource.PlayScheduled(AudioSettings.dspTime + loopStart.length);
    }
}
//00:07.00  // 05:05.00
//04:21.36 // 00:32.29
