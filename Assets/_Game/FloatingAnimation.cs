using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float frequency = 5f;
    [SerializeField] private float frequencyOffset = 1.5f;
    [SerializeField] private float amplitude = 0.2f;
    [SerializeField] private float offset = 1f;
    
    private void Update()
    {
        var height = Mathf.Sin(Time.time * frequency + frequencyOffset) * amplitude + offset;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
