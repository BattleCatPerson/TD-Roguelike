using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    public void Play() => particle.Play();
}
