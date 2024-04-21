using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public Image greenKey;
    public GameObject greenKeyUI;

    public List<string> keysHeld = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickUpKey(string key)
    {
        switch (key)
        {
            case "green":
                if (!keysHeld.Contains(key))
                {
                    keysHeld.Add(key);
                    Image keySlot = greenKeyUI.GetComponent<Image>();
                    keySlot.color = new Color(1f, 1f, 1f, 1f);
                    keySlot.sprite = greenKey.sprite;
                }
                break;
        }
    }
    
    public string Keys()
    {
        string keys = string.Empty;
        foreach(string keyCarried in keysHeld)
        {
            if(keys.Length > 1)
            {
                keys += ", ";
            }
            keys += keyCarried;
        }
        return keys;
    }
}
