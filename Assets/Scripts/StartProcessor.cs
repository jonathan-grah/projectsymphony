using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartProcessor : MonoBehaviour
{
    public string saveName;

    public void UpdateSaveName(string newSaveName)
    {
        saveName = newSaveName;
    }

    public void SaveGame()
    {
        SaveManager.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData data = SaveManager.LoadGame(saveName);

        saveName = data.saveName;
        // need to change UI so it knows its loading a current save (user wont be able to adjust config. settings)
    }
}
