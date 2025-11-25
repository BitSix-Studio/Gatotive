using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("RuaBorbagato");
    }
}
