using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1f;
    private float rotation = 0f;

    void Update()
    {
        rotation += rotationSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}