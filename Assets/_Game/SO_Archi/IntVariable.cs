using System;
using UnityEngine;

[CreateAssetMenu(fileName="IntVariable", menuName="ScriptableObject/IntVariable")]
public class IntVariable :  ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int initVal;
    private int _runtimeVal;
    
    public int Value => _runtimeVal;

    public event Action<int> Changed;

    public void SetValue(int v)
    {
        if (_runtimeVal == v) return;
        _runtimeVal = v;
        Changed?.Invoke(_runtimeVal);
    }

    public void Add(int v)
    {
        SetValue(_runtimeVal + v);
    }

    public void ResetValue()
    {
        SetValue(initVal);
    }

    public void OnAfterDeserialize()
    {
        _runtimeVal = initVal;
    }

    public void OnBeforeSerialize() { }
}