using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeTime = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        fadeCanvas.alpha = 1;
        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        fadeCanvas.alpha = 0;

        while (fadeCanvas.alpha < 1)
        {
            fadeCanvas.alpha += Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

        yield return SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(FadeIn());
    }
}