using UnityEngine;
using UnityEngine.UI;

public class ItensCrimePanel : MonoBehaviour
{
    [SerializeField] private Image imgItem;
    [SerializeField] private Sprite spriteRG, spriteCorpoCoberto, spriteCorpo;

    [SerializeField] private Button btnCorpo;

    float rotateZ;

    private Vector2 initialAnchorMin;
    private Vector2 initialAnchorMax;
    private Vector2 initialOffsetMin;
    private Vector2 initialOffsetMax;
    private Vector3 initialRotation;

    private static ItensCrimePanel instance;

    private void Awake()
    {
        instance = this;

        RectTransform rt = imgItem.rectTransform;

        initialAnchorMin = rt.anchorMin;
        initialAnchorMax = rt.anchorMax;
        initialOffsetMin = rt.offsetMin;
        initialOffsetMax = rt.offsetMax;
        initialRotation = rt.localEulerAngles;
    }

    public static ItensCrimePanel Instance { get { return instance; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgItem.rectTransform.Rotate(0f, 0f, 0f);
        gameObject.SetActive(false);
    }

    public void ItemExibition(string tag)
    {
        btnCorpo.onClick.RemoveAllListeners();
        if (tag == "RG")
        {
            imgItem.sprite = spriteRG;
            rotateZ = 10;
            imgItem.rectTransform.Rotate(0f, 0f, rotateZ);
            btnCorpo.onClick.AddListener(() => ResumeItem());
        }
        else if (tag == "Vitima")
        {
            imgItem.sprite = spriteCorpoCoberto;

            // STRETCH FULLSCREEN
            RectTransform rt = imgItem.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localEulerAngles = Vector3.zero;

            btnCorpo.onClick.AddListener(() => DescobrirCorpo());
        }

        gameObject.SetActive(true);
    }

    public void DescobrirCorpo()
    {
        imgItem.sprite = spriteCorpo;

        btnCorpo.onClick.AddListener(() => ResumeItem());
    }

    public void ResumeItem()
    {
        gameObject.SetActive(false);
        ResetToInitial();
    }

    private void ResetToInitial()
    {
        RectTransform rt = imgItem.rectTransform;

        rt.anchorMin = initialAnchorMin;
        rt.anchorMax = initialAnchorMax;
        rt.offsetMin = initialOffsetMin;
        rt.offsetMax = initialOffsetMax;
        rt.localEulerAngles = initialRotation;
    }
}
