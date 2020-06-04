using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class OptionsBinary
{
    public static void SaveOptionsData(OptionsMenu options)
    {
        //Reference a Binary Formatter
        BinaryFormatter formatter = new BinaryFormatter();
        //Location to Save
        string path = Application.persistentDataPath + "/" + "Kittens" + ".jpeg";
        //Create File at file path
        FileStream stream = new FileStream(path, FileMode.Create);
        //What Data to write to the file
        OptionsData data = new OptionsData(options);
        //write it and convert to bytes for writing to binary
        formatter.Serialize(stream, data);
        //and we are done
        stream.Close();
    }
    public static OptionsData LoadOptionsData(OptionsMenu options)
    {
        //Location to Save
        string path = Application.persistentDataPath + "/" + "Kittens" + ".jpeg";
        //if we have the file at that path
        if (File.Exists(path))
        {
            //get our binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            //and read the data from the path
            FileStream stream = new FileStream(path, FileMode.Open);
            //set the data from what it is back to usable variables
            OptionsData data = formatter.Deserialize(stream) as OptionsData;
            //and we are done
            stream.Close();
            //Oh Wait...send usable data back to the PlayerDataToSave Script
            return data;
        }
        else
        {
            return null;
        }
    }
}
