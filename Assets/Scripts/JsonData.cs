using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class JsonData : MonoBehaviour, ISerializationCallbackReceiver
{
    public List<Item> itemsValue = new List<Item>();
    public Dictionary<int, Item> items = new Dictionary<int, Item>();

    private static JsonData instance;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        itemsValue.Clear();

        foreach (var kvp in items)
        {
            itemsValue.Add(kvp.Value);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    private void Start()
    {
        if (instance == null)
        {
            string path = Application.streamingAssetsPath + "/items.json";
            string jsonString = File.ReadAllText(path);
            Items tempItems = JsonUtility.FromJson<Items>(jsonString);
            tempItems.temp.ForEach(item =>
            {
                items.Add(item.id, tempItems.ItemFactory(item));
                //items.Add(tempItems.ItemFactory(item));
            });
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}