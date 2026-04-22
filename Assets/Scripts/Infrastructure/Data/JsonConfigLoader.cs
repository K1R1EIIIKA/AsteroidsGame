using System.IO;
using System.Reflection;
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
#else
            var path = Path.Combine(Application.streamingAssetsPath, ConfigsPath, fileName + ".json");
            var json = File.ReadAllText(path).TrimStart('\uFEFF', '\u200B');
#endif
            var result = JsonUtility.FromJson<T>(json);
            ConfigValidator.Validate(result, fileName);
            return result;
        }

        public static async UniTask<T> LoadAsync<T>(string fileName)
        {
            var path = Path.Combine(Application.streamingAssetsPath, ConfigsPath, fileName + ".json");

#if UNITY_ANDROID && !UNITY_EDITOR
            var request = UnityWebRequest.Get(path);
            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load: {fileName} — {request.error}");
                return default;
            }

            var json = request.downloadHandler.text.TrimStart('\uFEFF', '\u200B');
#else
            var json = File.ReadAllText(path).TrimStart('\uFEFF', '\u200B');
#endif
            var result = JsonUtility.FromJson<T>(json);
            ConfigValidator.Validate(result, fileName);
            return result;
        }
    }
}