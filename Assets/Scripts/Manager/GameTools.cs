using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTools : MonoBehaviour
{
    private void Start()
    {
        //ResetPlayerPrefs();
    }

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
