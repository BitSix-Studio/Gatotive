using UnityEngine;
using UnityEngine.SceneManagement;

public class CrimeController : MonoBehaviour
{
    [SerializeField] private GameObject[] totalNpcsWithRumor;
    public Dialogue changeDialogueKenai;

    public bool completeCrimeScene;

    [SerializeField] private ButtonAction buttonAction;

    private static CrimeController instance;

    private void Awake()
    {
        instance = this;
    }

    public static CrimeController Instance { get { return instance; } }

    // Update is called once per frame
    void Update()
    {
        if (RumorPanel.Instance.contRumors >= totalNpcsWithRumor.Length)
        {
            var kenai = GameObject.Find("Kenai");
            kenai.GetComponent<InteractionsNPC>().dialogue = changeDialogueKenai;
        }

        if (completeCrimeScene)
        {
            completeCrimeScene = false;
            FadeManager.Instance.FadeToScene("Escritorio");
        }
    }
}
