using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamstem : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] steps = new AudioClip[4];

    [SerializeField] private ParticleSystem whatAreYouDoingStepParticle;

    void onStep()
    {
        source.PlayOneShot(steps[Random.Range(0, 3)]);
        whatAreYouDoingStepParticle.Play();
    }
}
