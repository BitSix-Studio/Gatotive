using UnityEngine;
using UnityEngine.UI;

public class Suspeitometro : MonoBehaviour
{
    public Image handleImg;
    public Sprite confortSuspectImg, doubtSuspectImg;

    public Slider sliderSus;

    private static Suspeitometro instance;

    private void Awake()
    {
        instance = this;
        sliderSus = GetComponentInChildren<Slider>();
    }

    public static Suspeitometro Instance { get { return instance; } }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}
