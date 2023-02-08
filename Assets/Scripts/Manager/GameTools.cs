using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTools : MonoBehaviour
{
#if UNITY_EDITOR
    //USE ONLY TO RESET SAVE
    //private void Start() => ResetPlayerPrefs();
#endif

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void LoadScene(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame();
    }
}
