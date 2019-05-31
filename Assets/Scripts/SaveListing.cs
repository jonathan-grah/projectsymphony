using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using TMPro;

public class SaveListing : MonoBehaviour
{
    public string saveName;

    public void LoadScene()
    {
        SceneManager.LoadScene("TestScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveManager.LoadGame(saveName);
    }
}
