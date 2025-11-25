using UnityEngine;
using UnityEngine.SceneManagement;

public class CrimeController : MonoBehaviour
{
    public GameObject[] totalNpcs;

    public ButtonAction buttonAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonAction.contNpcs >= totalNpcs.Length)
        {
            SceneManager.LoadScene("Escritorio");
        }
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
    }
}
