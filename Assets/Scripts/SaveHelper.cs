using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public static class SaveHelper
{
    public static async UniTask SaveAsync<T>(T obj, string key)
    {
        if(!typeof(T).IsSerializable)
        {
            throw new Exception($"Attempt to save a non-serializable " +
                $"object: {typeof(T).ToString()}");
        }

        string json = JsonConvert.SerializeObject(obj);

        Directory.CreateDirectory(Application.streamingAssetsPath);
        var path = Path.Combine(Application.streamingAssetsPath, key);
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            await outputFile.WriteAsync(json);
        }
    }

    public static async UniTask<T> LoadAsync<T>(string key)
    {
        var path = Path.Combine(Application.streamingAssetsPath, key);
        if(!File.Exists(path))
        {
            return default(T);
        }
        using StreamReader reader = new(path);
        string json = await reader.ReadToEndAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }
}
