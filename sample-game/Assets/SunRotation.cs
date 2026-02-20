using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public float daySpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.right * daySpeed * Time.deltaTime);
    }
}