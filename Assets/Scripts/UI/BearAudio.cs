using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearAudio : MonoBehaviour
{
    public AudioClip[] BearAudioClips;
    public AudioClip[] BearAttackClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float BearstepThreashold;
    public float BearstepRate;
    public float BearAttackRate;
    private float BearStepTime;
    private float BearAttackTime;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        GetComponent<State>();

    }

    private void Update()
    {
        Update(BearAttackTime);
    }

    void Update(float bearAttackTime)
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            if (_rigidbody.velocity.magnitude > BearstepRate)
            {
                if (Time.time - BearStepTime > BearstepRate)
                {
                    BearStepTime = Time.time;
                    audioSource.PlayOneShot(BearAudioClips[Random.Range(0, BearAudioClips.Length)]);
                }
                if (Bear.State.Idle == Bear.State.Attacking)
                {
                    BearAttackTime = Time.time;
                    audioSource.PlayOneShot(BearAttackClips[Random.Range(0, BearAttackClips.Length)]);
                }
            }
        }
    }
}
