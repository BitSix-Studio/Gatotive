using UnityEngine;
using UnityEngine.UI;

public class RumorNPCs : MonoBehaviour
{
    [SerializeField] private Rumors[] rumors;
    public int indexRumor;

    public string RumorText { get { return rumors[indexRumor].rumorText; } }
    public bool RumorTrueOrFalse { get { return rumors[indexRumor].rumorTrueOrFalse; } }
    public float ImpactOnSus { get { return rumors[indexRumor].impactOnSuspeitometro ; } }
    public GameObject Target { get { return rumors[indexRumor].targetSuspeitometro; } }
    public string InfoTrue { get { return rumors[indexRumor].receiveTrueInfo; } }
    public string InfoFalse { get { return rumors[indexRumor].receiveFalseInfo; } }

    public float valueSus;

    private static RumorNPCs instance;

    public bool canAppearRumor;

    private void Awake()
    {
        instance = this;
    }

    public static RumorNPCs Instance { get { return instance; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canAppearRumor = true;

        UpdateSuspeitometro();
        SortRumor();
    }

    public void UpdateSuspeitometro()
    {
        Suspeitometro.Instance.sliderSus.value = valueSus;
        if (Suspeitometro.Instance.sliderSus.value > 5)
        {
            Suspeitometro.Instance.handleImg.sprite = Suspeitometro.Instance.confortSuspectImg;
        }
        else
        {
            Suspeitometro.Instance.handleImg.sprite = Suspeitometro.Instance.doubtSuspectImg;
        }
    }

    public void SortRumor()
    {
        var d1 = Random.Range(0, rumors.Length);
        var d2 = Random.Range(0, rumors.Length);
        if (Suspeitometro.Instance.sliderSus.value > 5)
        {
            indexRumor = Mathf.Min(d1, d2);
        }
        else
        {
            indexRumor = Mathf.Max(d1, d2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateSuspeitometro();
            Suspeitometro.Instance.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Suspeitometro.Instance.gameObject.SetActive(false);
        }
    }
}
