using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveGame(StartProcessor data)
    {
        DirectoryInfo savesDir = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "saves"));
        if (!savesDir.Exists)
            savesDir.Create();
        string path = Path.Combine(Application.persistentDataPath, "saves", data.saveName + ".sym");
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            GameData actualData = new GameData(data);
            formatter.Serialize(stream, actualData);
        }
    }

    public static GameData LoadGame(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, "saves", saveName + ".sym");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                GameData data = formatter.Deserialize(stream) as GameData;
                return data;
            }

        }
        else
        {
            Debug.Log("Warning: Save file cannot be found");
            return null;
        }
    }
}