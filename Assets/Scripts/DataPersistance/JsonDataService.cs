using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public interface IDataService
{
    bool SaveData<T>(string RelativePath, T Data, bool Encrypted);
    T LoadData<T>(string RelativePath, bool Encrypted);
}

public class JsonDataService : IDataService
{
    
    public T LoadData<T>(string RelativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;
        if (!File.Exists(path))
        {
            Debug.LogError("Cannot load file at "+path+". File does not exist.");
            throw new FileNotFoundException("File "+path+" does not exist!");
        }
        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Cannot load file due to " + e.Message+""+e.StackTrace);
            throw e;
        }
    }

    public bool SaveData<T>(string RelativePath, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;
        try
        {
            Debug.Log("Data exists, delete old one");
            if (File.Exists(path)) File.Delete(path);
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("Unable to save data: "+e.Message+" "+e.StackTrace);
            return false;
        }
    }
}
