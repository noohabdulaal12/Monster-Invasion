using UnityEngine;
using System.Collections;

public class ThunderSystem : MonoBehaviour
{
    [Header("Thunder Settings")]
    public float minThunderInterval = 5f;
    public float maxThunderInterval = 20f;
    public AudioClip[] thunderSounds;

    [Header("Lightning Flash Settings")]
    public Light lightningLight;
    public float flashIntensity = 5f;
    public float flashDuration = 0.15f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 2D sound (ambient)

        // Auto-create a directional light if none assigned
        if (lightningLight == null)
        {
            GameObject lightObj = new GameObject("LightningLight");
            lightningLight = lightObj.AddComponent<Light>();
            lightningLight.type = LightType.Directional;
            lightningLight.intensity = 0f;
            lightningLight.color = new Color(0.9f, 0.95f, 1f);
        }

        StartCoroutine(ThunderLoop());
    }

    IEnumerator ThunderLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minThunderInterval, maxThunderInterval);
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(LightningFlash());

            // Play thunder sound slightly after the flash
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            PlayThunderSound();
        }
    }

    IEnumerator LightningFlash()
    {
        // Quick double-flash for realism
        lightningLight.intensity = flashIntensity;
        yield return new WaitForSeconds(flashDuration * 0.3f);
        lightningLight.intensity = 0f;
        yield return new WaitForSeconds(0.05f);
        lightningLight.intensity = flashIntensity * 0.8f;
        yield return new WaitForSeconds(flashDuration);
        lightningLight.intensity = 0f;
    }

    void PlayThunderSound()
    {
        if (thunderSounds != null && thunderSounds.Length > 0)
        {
            AudioClip clip = thunderSounds[Random.Range(0, thunderSounds.Length)];
            audioSource.PlayOneShot(clip, Random.Range(0.7f, 1f));
        }
    }
}