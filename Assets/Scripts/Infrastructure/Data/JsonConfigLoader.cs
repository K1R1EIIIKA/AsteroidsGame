using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Infrastructure.Data
{
    public static class JsonConfigLoader
    {
        private const string ConfigsPath = "Configs";

        public static T Load<T>(string fileName)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
    var path    = Path.Combine(Application.streamingAssetsPath, ConfigsPath, fileName + ".json");
    var request = UnityWebRequest.Get(path);
    var op      = request.SendWebRequest();

    var timeout = 5f;
    var elapsed = 0f;
    while (!op.isDone && elapsed < timeout)
    {
        elapsed += 0.01f;
        System.Threading.Thread.Sleep(10);
    }

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError($"Config load failed: {fileName} — {request.error}");
        return default;
    }

    var json = request.downloadHandler.text.TrimStart('\uFEFF', '\u200B');
    return JsonUtility.FromJson<T>(json);
#else
            var path = Path.Combine(Application.streamingAssetsPath, ConfigsPath, fileName + ".json");
            var json = File.ReadAllText(path).TrimStart('\uFEFF', '\u200B');
            return JsonUtility.FromJson<T>(json);
#endif
        }

        public static async UniTask<T> LoadAsync<T>(string fileName)
        {
            var path    = Path.Combine(Application.streamingAssetsPath, ConfigsPath, fileName + ".json");

#if UNITY_ANDROID && !UNITY_EDITOR
        var request = UnityWebRequest.Get(path);
        await request.SendWebRequest().ToUniTask();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load: {fileName}");
            return default;
        }

        return JsonUtility.FromJson<T>(request.downloadHandler.text);
#else
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
#endif
        }
    }
}
