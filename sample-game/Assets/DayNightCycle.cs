using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayLengthInMinutes = 1f;
    private float timeOfDay = 0f;

    public Light sun;
    public Material skyboxMaterial;

    void Update()
    {
        timeOfDay += Time.deltaTime / (dayLengthInMinutes * 60f);
        timeOfDay %= 1f;

        UpdateSun();
        UpdateSkybox();
    }

    void UpdateSun()
    {
        sun.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 170f, 0);

        float intensity = Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.PI));
        sun.intensity = intensity;
    }

    void UpdateSkybox()
    {
        float exposure = Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.PI));
        skyboxMaterial.SetFloat("_Exposure", exposure * 1.3f);

        DynamicGI.UpdateEnvironment();
    }
}