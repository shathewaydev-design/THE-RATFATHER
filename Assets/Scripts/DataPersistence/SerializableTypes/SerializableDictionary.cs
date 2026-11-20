using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    //Save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    //Load the dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();
        if(keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SeriazableDictionary but the amount of keys ("
             + keys.Count + ") does not match the amount of values (" + values.Count + ")" +
              "which indicates that soemthing went wrong");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
