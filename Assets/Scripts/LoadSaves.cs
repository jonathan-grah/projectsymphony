using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using UnityEngine;
using TMPro;

public class LoadSaves : MonoBehaviour
{
    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "saves");
        string[] savePaths = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);

        List<SaveData> savesList = new List<SaveData>();
        for (int i = 0; i < savePaths.Length; i++)
        {
            savesList.Add(SaveManager.QuerySave(Path.GetFileNameWithoutExtension(savePaths[i])));
        }
        SaveData[] saves = savesList.ToArray();

        GameObject SaveListContainer = GameObject.Find("SaveList");
        foreach (SaveData save in saves.OrderBy(sort => sort.modifiedDate).ToArray().Reverse())
        {
            GameObject saveListing = Instantiate(Resources.Load("Prefabs/saveListing", typeof(GameObject)) as GameObject);
            saveListing.transform.SetParent(SaveListContainer.transform);
            saveListing.transform.GetChild(2).GetComponent<SaveListing>().saveName = save.saveName;
            saveListing.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = save.saveName; // save name
            saveListing.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = save.modifiedDate.ToString("h:mm tt, dddd dd MMMM yyyy"); // date created
        }
    }
}
