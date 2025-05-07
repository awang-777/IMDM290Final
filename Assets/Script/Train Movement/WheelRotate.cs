using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public float rotationSpeed = 360f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime); 
        // Use Vector3.forward or Vector3.left if the axis is wrong
    }
}
