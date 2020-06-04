using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveOptions(OptionsMenu optionsMenu)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "C:/Users/ascle/Desktop/Unity Stuffs/Unity Projects/TAFE" +
            "/Cert IV/TAFE Assesment Projects/TAFE-UI-Menu-Systems-Assignment" +
            "/Resources/optionsData.bin";

        FileStream stream = new FileStream(path, FileMode.Create);

        OptionsData data = new OptionsData(optionsMenu);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save Succesful");
    }

    public static OptionsData LoadOptions()
    {
        string path = "C:/Users/ascle/Desktop/Unity Stuffs/Unity Projects/TAFE" +
            "/Cert IV/TAFE Assesment Projects/TAFE-UI-Menu-Systems-Assignment" +
            "/Resources/optionsData.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionsData data = formatter.Deserialize(stream) as OptionsData;
            stream.Close();

            Debug.Log("Load Succesful");
            return data;
        }
        else
        {
            Debug.LogError("Save FIle not found in " + path);
            return null;
        }
    }
}
