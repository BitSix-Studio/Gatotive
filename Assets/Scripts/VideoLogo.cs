using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoLogo : MonoBehaviour
{
    public VideoPlayer video;
    public string nextSceneName;

    void Start()
    {
        video.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
