using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject where T : UnityEngine.Object
{
    private readonly List<T> _items = new();
    public IReadOnlyList<T> Items => _items;

    public void Add(T item)
    {
        if (!_items.Contains(item)) _items.Add(item);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }
}