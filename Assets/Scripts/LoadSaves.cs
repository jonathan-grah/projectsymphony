using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using TMPro;

public class LoadSaves : MonoBehaviour
{
    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "saves");
        string[] saves = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        GameObject SaveList = GameObject.Find("SaveList");
        for (int i = 0; i < saves.Length; i++)
        {
            SaveData save = SaveManager.QuerySave(Path.GetFileNameWithoutExtension(saves[i]));
            GameObject saveListing = Instantiate(Resources.Load("Prefabs/saveListing", typeof(GameObject)) as GameObject);
            saveListing.transform.SetParent(SaveList.transform);
            saveListing.transform.GetChild(2).GetComponent<SaveListing>().saveName = save.saveName;
            saveListing.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = save.saveName; // save name
            saveListing.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = save.createdDate; // date created
        }
    }
}
