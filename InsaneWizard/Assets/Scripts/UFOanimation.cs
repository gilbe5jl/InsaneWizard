using UnityEngine;

public class SpinForever : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationSpeed = new Vector3(0f, 45f, 0f); // degrees per second

    [Header("Bobbing")]
    public float bobAmplitude = 0.2f;    // height of the bob
    public float bobFrequency = 1.5f;    // speed of the bob

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
        
    }

    void Update()
    {
        // Rotate in place
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);

        // Bob up and down
        float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.localPosition = startPosition + new Vector3(0f, bobOffset, 0f);
    }
}