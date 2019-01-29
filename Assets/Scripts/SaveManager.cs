using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private SaveData saveData = null;

    private string currentSaveName = "";

    public void EditSaveName(string newSaveName)
    {
        currentSaveName = newSaveName;
    }

    public static Vector3 StringToVector(string text)
    {
        text = text.Substring(1, text.Length - 2);
        string[] textArray = text.Split(',');

        return new Vector3(
            float.Parse(textArray[0]),
            float.Parse(textArray[1]),
            float.Parse(textArray[2])
        );
    }

    public static Quaternion StringToQuaternion(string text)
    {
        text = text.Substring(1, text.Length - 2);
        string[] textArray = text.Split(',');

        return new Quaternion(
            float.Parse(textArray[0]),
            float.Parse(textArray[1]),
            float.Parse(textArray[2]),
            float.Parse(textArray[3])
        );
    }

    public void SaveGame()
    {
        DirectoryInfo savesDir = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "saves"));
        if (!savesDir.Exists)
            savesDir.Create();
        if (currentSaveName.Length <= 0) Debug.Log("You have not entered an appropriate save name");
        string path = Path.Combine(Application.persistentDataPath, Path.Combine("saves", currentSaveName + ".sym"));
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            saveData = new SaveData(currentSaveName);
            // if user is not creating a new game:
            if (SceneManager.GetActiveScene().name != "GameConfigurator")
                saveData.AddUnits();
            formatter.Serialize(stream, saveData);
        }
    }

    public void Load()
    {
        saveData = LoadGame("gun");
        Debug.Log(saveData.Units[0].id);
    }

    public SaveData LoadGame(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, Path.Combine("saves", saveName + ".sym"));
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                saveData = formatter.Deserialize(stream) as SaveData;
                foreach (UnitDetails unit in saveData.GetUnits())
                {
                    GameObject prefab = Instantiate(Resources.Load(
                        String.Concat("Prefabs/", String.Format(
                            unit.id.Substring(0, unit.id.IndexOf("_")))
                        ), typeof(GameObject)) as GameObject);
                    prefab.transform.parent = GameObject.Find("Units").transform;
                    prefab.GetComponent<UnitController>().Load(unit);
                }
                return saveData;
            }

        }
        else
        {
            Debug.Log("Warning: Save file cannot be found");
            return null;
        }
    }
}