using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoAudio : MonoBehaviour
{
    public AudioClip[] DinoAudioClips;
    public AudioClip[] DinoAttackClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float DinostepThreashold;
    public float DinostepRate;
    public float DinoAttackRate;
    private float DinoStepTime;
    private float DinoAttackTime;
    public Dinosaur.State _currentState;

    Vector3 LastPosition;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        LastPosition = _rigidbody.position;
        _currentState = GetComponent<Dinosaur>()._currentState;
    }

    private void Update()
    {

        Update(DinoAttackTime);
    }

    void Update(float dinoStepTime)
    {
        Vector3 dir = _rigidbody.position - LastPosition;
        dir.y = 0;
        {
            if (dir.magnitude > 0.1f)
            {
                if (Time.time - DinoStepTime > DinostepRate)
                {
                    DinoStepTime = Time.time;
                    LastPosition = _rigidbody.position;
                    audioSource.PlayOneShot(DinoAudioClips[Random.Range(0, DinoAudioClips.Length)]);
                }
            }
        }

    }
    public void dinoaudio()
    {
        DinoAttackTime = Time.deltaTime;
        audioSource.PlayOneShot(DinoAudioClips[Random.Range(0, DinoAudioClips.Length)]);
    }
}