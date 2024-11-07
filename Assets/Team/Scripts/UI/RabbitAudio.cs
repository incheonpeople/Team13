using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAudio : MonoBehaviour
{
    public AudioClip[] RabbitAudioClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float RabbitstepRate;
    public float RabbitstepThreashold;
    private float RabbitStepTime;

    Vector3 LastPosition;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        LastPosition = _rigidbody.position;
    }
    private void Update()
    {
        Update(RabbitStepTime);
    }
    void Update(float rabbitStepTime)
    {
        Vector3 dir = _rigidbody.position - LastPosition;
        dir.y = 0;
        {
            if (dir.magnitude > 0.1f)
            {
                if (Time.time - RabbitStepTime > RabbitstepRate)
                {
                    RabbitStepTime = Time.time;
                    LastPosition = _rigidbody.position;
                    audioSource.PlayOneShot(RabbitAudioClips[Random.Range(0, RabbitAudioClips.Length)]);
                }
            }
        }

    }
}
