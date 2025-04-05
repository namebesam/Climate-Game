using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSoundScript : MonoBehaviour
{
    public AudioSource source;

    public Collider soundTrigger;

    public ParticleSystem particles;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        soundTrigger = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source.Play();
            particles.Play();
            soundTrigger.enabled = false;
        }
    }
}
