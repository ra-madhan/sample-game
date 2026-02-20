using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    public float dayLengthInMinutes = 1f;
    private float timeOfDay = 0f;

    [Header("Lights")]
    public Light sun;
    public Light moon;

    [Header("Skyboxes")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Transition")]
    [Range(0f, 1f)] public float sunriseTime = 0.23f;
    [Range(0f, 1f)] public float sunsetTime = 0.73f;
    public float transitionDuration = 0.04f;

    void Update()
    {
        timeOfDay += Time.deltaTime / (dayLengthInMinutes * 60f);
        timeOfDay %= 1f;

        UpdateCelestials();
        UpdateSkybox();
    }

    void UpdateCelestials()
    {
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0);
        moon.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0);

        float sunIntensity = Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.PI));
        sun.intensity = sunIntensity;

        moon.intensity = 1f - sunIntensity;
    }

    void UpdateSkybox()
    {
        float sunriseStart = sunriseTime;
        float sunriseEnd = sunriseTime + transitionDuration;

        float sunsetStart = sunsetTime - transitionDuration;
        float sunsetEnd = sunsetTime;

        // 🌅 Sunrise blend (Night → Day)
        if (timeOfDay >= sunriseStart && timeOfDay <= sunriseEnd)
        {
            float t = (timeOfDay - sunriseStart) / transitionDuration;
            BlendSkyboxes(nightSkybox, daySkybox, t);
        }
        // 🌇 Sunset blend (Day → Night)
        else if (timeOfDay >= sunsetStart && timeOfDay <= sunsetEnd)
        {
            float t = (timeOfDay - sunsetStart) / transitionDuration;
            BlendSkyboxes(daySkybox, nightSkybox, t);
        }
        // ☀ Day
        else if (timeOfDay > sunriseEnd && timeOfDay < sunsetStart)
        {
            RenderSettings.skybox = daySkybox;
        }
        // 🌙 Night
        else
        {
            RenderSettings.skybox = nightSkybox;
        }

        DynamicGI.UpdateEnvironment();
    }

    void BlendSkyboxes(Material from, Material to, float t)
    {
        RenderSettings.skybox = to;

        if (to.HasProperty("_Exposure") && from.HasProperty("_Exposure"))
        {
            float fromExp = from.GetFloat("_Exposure");
            float toExp = to.GetFloat("_Exposure");

            float blended = Mathf.Lerp(fromExp, toExp, t);
            to.SetFloat("_Exposure", blended);
        }
    }
}