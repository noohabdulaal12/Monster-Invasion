using UnityEngine;

public class RainSystem : MonoBehaviour
{
    [Header("Rain Settings")]
    public int maxParticles = 5000;
    public float rainRate = 1000f;
    public float rainSpeed = 15f;
    public float rainSpread = 50f;
    public float rainHeight = 20f;

    private ParticleSystem rainParticles;

    void Start()
    {
        SetupRainParticles();
    }

    void SetupRainParticles()
    {
        rainParticles = gameObject.AddComponent<ParticleSystem>();

        // Main module
        var main = rainParticles.main;
        main.maxParticles = maxParticles;
        main.startLifetime = rainHeight / rainSpeed;
        main.startSpeed = rainSpeed;
        main.startSize = new ParticleSystem.MinMaxCurve(0.02f, 0.05f);
        main.startColor = new Color(0.7f, 0.8f, 1f, 0.6f);
        main.gravityModifier = 1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        // Emission module
        var emission = rainParticles.emission;
        emission.rateOverTime = rainRate;

        // Shape module — rain falls from above
        var shape = rainParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(rainSpread, 1f, rainSpread);
        shape.rotation = new Vector3(90f, 0f, 0f); // point downward

        // Renderer
        var renderer = rainParticles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        renderer.velocityScale = 0.05f;
        renderer.lengthScale = 2f;
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        rainParticles.Play();
    }
}