using UnityEngine;

public class TrainMover : MonoBehaviour
{
    public float speed = 5f;
    public float resetDistance = 40f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move in the object's local forward direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Reset position if too far from start
        if (Vector3.Distance(startPosition, transform.position) > resetDistance)
        {
            transform.position = startPosition;
        }
    }
}
