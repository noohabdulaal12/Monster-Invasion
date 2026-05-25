using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;

    void Start()
    {
        audioSource.clip = musicClip;
        audioSource.loop = true;  
        audioSource.Play();
    }
}