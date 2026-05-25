using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float stormIntensity = 1f;  // 0 = clear, 1 = heavy storm

    private RainSystem rain;
    private ThunderSystem thunder;
    private Light sunLight;

    void Start()
    {
        // Attach systems to this GameObject
        rain = gameObject.AddComponent<RainSystem>();
        thunder = gameObject.AddComponent<ThunderSystem>();

        // Dim the sun during storm
        sunLight = FindObjectOfType<Light>();
        ApplyStormIntensity();
    }

    void ApplyStormIntensity()
    {
        if (sunLight) sunLight.intensity = Mathf.Lerp(1f, 0.2f, stormIntensity);

        // Adjust fog
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.5f, 0.5f, 0.55f);
        RenderSettings.fogDensity = Mathf.Lerp(0f, 0.03f, stormIntensity);
    }
}