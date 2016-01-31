using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.25f;
    public float maxIntensity = 0.5f;

    public Vector2 timeBetweenFlickers = new Vector2(0.3f, 5f);

    public Vector2 timeToFlick = new Vector2(0.1f, 0.2f);

    public Vector2 lightIntervalRange = new Vector2(0.04f, 0.25f);

    protected float lightTimer;

    protected float betweenFlickTimer;

    protected float flickerTimer;

    protected Light myLight;

    float random;

    protected bool flicker;

    void Start()
    {
        myLight = GetComponent<Light>();
        random = Random.Range(0.0f, 65535.0f);
    }

    protected void Update()
    {
        if (!flicker)
        {
            myLight.intensity = 0;
            betweenFlickTimer -= Time.deltaTime;

            if (betweenFlickTimer < 0)
            {
                flicker = true;
                flickerTimer = Random.Range(timeToFlick.x, timeToFlick.y);
                lastIntensity = 0;
            }
        }
        else
        {
            flickerTimer -= Time.deltaTime;

            if (flickerTimer < 0)
            {
                flicker = false;
                betweenFlickTimer = Random.Range(timeBetweenFlickers.x, timeBetweenFlickers.y);
            }
            else
            {
                Flicker();
            }
        }
    }

    protected float lastIntensity;

    protected void Flicker()
    {
        lightTimer -= Time.deltaTime;

        if (lightTimer < 0)
        {
            //            var noise = Mathf.PerlinNoise(random, Time.time);

            var newIntensity = Random.value;

            if(newIntensity > lastIntensity) newIntensity = Random.value;

            myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, newIntensity);

            lightTimer = Random.Range(lightIntervalRange.x, lightIntervalRange.y);

            lastIntensity = newIntensity;
        }

    }
}