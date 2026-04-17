using UnityEngine;
using UnityEngine.SceneManagement;

namespace App
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        }
    }
}
