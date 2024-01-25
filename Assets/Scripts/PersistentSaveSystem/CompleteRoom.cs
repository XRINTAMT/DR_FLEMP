using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PersistentSaveSystem
{
    public class CompleteRoom : MonoBehaviour
    {
        [SerializeField] string Tag;

        public void Complete()
        {
            if(SceneManager.GetActiveScene().name != "Lobby")
                Tag = SceneManager.GetActiveScene().name;
            Debug.Log("Complete");
            PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + Tag, 1);
            WelcomeScreenController _screen = FindObjectOfType<WelcomeScreenController>();
            if(_screen)
                _screen.OpenEndPanel();
        }
    }

}
