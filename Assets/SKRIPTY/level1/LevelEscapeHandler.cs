using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEscapeHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MENU");
        }
    }
}