using UnityEngine;
using UnityEngine.SceneManagement;

namespace App
{
    public class Bootstrap : MonoBehaviour
    {
        const string SceneName = "Game";
        
        private void Start()
        {
            SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        }
    }
}
