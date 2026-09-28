using System;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float frequency = 2f;
    [SerializeField] private float frequencyOffset = 1.5f;
    [SerializeField] private float amplitude = 0.2f;
    [SerializeField] private float offset = 0.2f;
    
    private Vector3 _startPosition;

    private void Awake()
    {
       _startPosition = transform.localPosition; 
    }

    private void Update()
    {
        var height = Mathf.Sin(Time.time * frequency + frequencyOffset) * amplitude + offset;
        transform.localPosition = _startPosition + new Vector3(0, height, 0);
    }
}
