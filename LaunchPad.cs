using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] AudioClip backgroundMusic;

    AudioSource audioSource;


    // Start is called before the first frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.PlayOneShot(backgroundMusic);
    }
}
