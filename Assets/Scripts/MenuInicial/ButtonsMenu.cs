using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMenu : MonoBehaviour
{
    private static ButtonsMenu instance;

    private void Awake()
    {
        instance = this;
    }

    public static ButtonsMenu Instance {  get { return instance; } }

    public void StartGame()
    {
        FadeManager.Instance.FadeToScene("RuaBorbagato");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Credits()
    {
        FadeManager.Instance.FadeToScene("CreditsFinal");
    }

    public void BackToMenu()
    {
        FadeManager.Instance.FadeToScene("MenuInicial");
    }
}
