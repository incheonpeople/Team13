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
    public Bear.State _currentState;

    Vector3 LastPosition;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        LastPosition = _rigidbody.position;
        _currentState = GetComponent<Bear>()._currentState;
    }

    private void Update()
    {

        Update(BearAttackTime);
    }

    void Update(float bearStepTime)
    {
        Vector3 dir = _rigidbody.position - LastPosition;
        dir.y = 0;
        {
            if (dir.magnitude > 0.1f)
            {
                if (Time.time - BearStepTime > BearstepRate)
                {
                    BearStepTime = Time.time;
                    LastPosition = _rigidbody.position;
                    audioSource.PlayOneShot(BearAudioClips[Random.Range(0, BearAudioClips.Length)]);
                }
            }
        }

    }
    public void bearaudio()
    {
        BearAttackTime = Time.deltaTime;
        audioSource.PlayOneShot(BearAttackClips[Random.Range(0, BearAttackClips.Length)]);
    }
}
