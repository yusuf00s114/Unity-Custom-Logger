using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Claude.
// !!! IMPORTANT !!!
// Must have a SerializableDictionaryDrawer in an Editor folder to work.

/// <summary>
///     Non-generic base class required for the CustomPropertyDrawer to target
///     open generic types. Place this file anywhere outside an Editor folder.
/// </summary>
[Serializable]
public abstract class SerializableDictionaryBase
{
}

/// <summary>
///     A serializable dictionary that is fully editable in the Unity Inspector.
///     The inspector displays one foldout per entry, labelled by the key's value.
///     Usage:
///     [SerializeField] private SerializableDictionary
///     <string, int>
///         myDict;
///         Supports all serializable key and value types (primitives, structs, UnityEngine.Object, etc.).
///         Duplicate keys entered in the Inspector are silently ignored when building the
///         runtime dictionary; a warning badge is shown in the Inspector for those rows.
/// </summary>
[Serializable]
public class SerializableDictionary<TKey, TValue>
    : SerializableDictionaryBase, ISerializationCallbackReceiver, IDictionary<TKey, TValue>
{
    // -------------------------------------------------------------------------
    // Serialized storage � this list is what Unity actually saves.
    // -------------------------------------------------------------------------
    [SerializeField] private List<SerializableKVP<TKey, TValue>> _pairs = new();

    // -------------------------------------------------------------------------
    // Runtime dictionary � rebuilt from _pairs after each deserialization.
    // -------------------------------------------------------------------------
    [NonSerialized] private Dictionary<TKey, TValue> _dict = new();

    // =========================================================================
    // IDictionary<TKey, TValue>
    // =========================================================================

    public TValue this[TKey key]
    {
        get => _dict[key];
        set
        {
            _dict[key] = value;
            // Mirror change into the serialized list.
            var idx = _pairs.FindIndex(p => EqualityComparer<TKey>.Default.Equals(p.Key, key));
            if (idx >= 0)
                _pairs[idx].Value = value;
            else
                _pairs.Add(new SerializableKVP<TKey, TValue> { Key = key, Value = value });
        }
    }

    public ICollection<TKey> Keys => _dict.Keys;
    public ICollection<TValue> Values => _dict.Values;
    public int Count => _dict.Count;
    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        _dict.Add(key, value);
        _pairs.Add(new SerializableKVP<TKey, TValue> { Key = key, Value = value });
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public bool Remove(TKey key)
    {
        if (!_dict.Remove(key)) return false;
        _pairs.RemoveAll(p => EqualityComparer<TKey>.Default.Equals(p.Key, key));
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool ContainsKey(TKey key)
    {
        return _dict.ContainsKey(key);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dict.ContainsKey(item.Key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dict.TryGetValue(key, out value);
    }

    public void Clear()
    {
        _dict.Clear();
        _pairs.Clear();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)_dict).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // =========================================================================
    // ISerializationCallbackReceiver
    // =========================================================================

    /// <summary>
    ///     Called by Unity before serializing. We push the live dictionary back into
    ///     the list so that any programmatic changes (via Add / Remove / indexer)
    ///     are persisted.  We preserve existing list entries where possible so that
    ///     Inspector foldout open/close state is not reset on every frame.
    /// </summary>
    public void OnBeforeSerialize()
    {
        // Remove list entries whose keys are no longer in the dictionary.
        _pairs.RemoveAll(p => p.Key != null && !_dict.ContainsKey(p.Key));

        // Update values for existing entries and track which keys are covered.
        var seen = new HashSet<TKey>(EqualityComparer<TKey>.Default);
        foreach (var p in _pairs)
        {
            if (p.Key == null) continue;
            if (_dict.TryGetValue(p.Key, out var val))
            {
                p.Value = val;
                seen.Add(p.Key);
            }
        }

        // Append any new keys that appeared programmatically.
        foreach (var kvp in _dict)
            if (!seen.Contains(kvp.Key))
                _pairs.Add(new SerializableKVP<TKey, TValue> { Key = kvp.Key, Value = kvp.Value });
    }

    /// <summary>
    ///     Called by Unity after deserializing. Rebuilds the runtime dictionary from
    ///     the list. Duplicate keys are skipped (first occurrence wins).
    /// </summary>
    public void OnAfterDeserialize()
    {
        _dict = new Dictionary<TKey, TValue>(EqualityComparer<TKey>.Default);
        foreach (var p in _pairs)
        {
            if (p.Key == null) continue;
            if (!_dict.ContainsKey(p.Key))
                _dict.Add(p.Key, p.Value);
        }
    }
}

// =============================================================================
// Serialized key-value pair helper
// =============================================================================

/// <summary>
///     Plain serializable container for a single dictionary entry.
///     Marked [Serializable] so Unity's serialization system can handle it,
///     and public fields so the PropertyDrawer can find "Key" and "Value" by name.
/// </summary>
[Serializable]
public class SerializableKVP<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}