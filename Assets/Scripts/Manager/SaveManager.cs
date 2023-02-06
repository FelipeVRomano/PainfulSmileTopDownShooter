using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public List<Save> valueToSave;
    public List<string> _keyToSave;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }

        valueToSave = new List<Save>();
        _keyToSave = new List<string>();
    }

    public void AddChangeKeyToSave(string keySave, int valueSave)
    {
        if(_keyToSave.Contains(keySave))
        {
            int indexChangeSave = keySave.IndexOf(keySave);

            valueToSave[indexChangeSave].keyValue = valueSave;
            return;
        }

        Debug.Log("Adding new value to save");
        Save newValueToSave = new Save();

        newValueToSave.keySave = keySave;
        newValueToSave.keyValue = valueSave;

        valueToSave.Add(newValueToSave);
        _keyToSave.Add(keySave);
    }

    public void SaveGame()
    {
        if(valueToSave.Count > 0)
        {
            for(int i = 0; i < valueToSave.Count; i++)
            {
                SetSave(valueToSave[i].keySave, valueToSave[i].keyValue);
            }

            valueToSave.Clear();
        }
    }
    public void SetSave(string keySave, int valueSave)
    {
        PlayerPrefs.SetInt(keySave, valueSave);
    }
}
