using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RumorPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textRumor, infoText;
    [SerializeField] GameObject infoReceive;

    private RumorNPCs currentNPC;

    private string buttonChoose;

    public int contRumors;

    private static RumorPanel instance;

    private void Awake()
    {
        instance = this;
    }

    public static RumorPanel Instance { get { return instance; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textRumor.text = "";
        this.gameObject.SetActive(false);
        infoReceive.SetActive(false);
        contRumors = 0;
    }

    public void ExibitionRumor(RumorNPCs npc)
    {
        if (npc.canAppearRumor) {
            currentNPC = npc;
            UpdateRumorPanel();
            this.gameObject.SetActive(true);
        }
    }

    public void HideRumor()
    {
        this.gameObject.SetActive(false);
        currentNPC.UpdateSuspeitometro();
        contRumors++;
    }

    private void UpdateRumorPanel()
    {
        textRumor.text = currentNPC.RumorText;
    }


    public void InfoReceive(string buttonChoose)
    {
        if (currentNPC.RumorTrueOrFalse)
        {
            if (buttonChoose == "CONFIAR")
            {
                infoText.text = currentNPC.InfoTrue;
                currentNPC.Target.GetComponent<RumorNPCs>().valueSus += currentNPC.ImpactOnSus;
            }
            else if (buttonChoose == "DUVIDAR")
            {
                infoText.text = currentNPC.InfoFalse;
                currentNPC.Target.GetComponent<RumorNPCs>().valueSus -= currentNPC.ImpactOnSus;
            }
        }
        else
        {
            if (buttonChoose == "CONFIAR")
            {
                infoText.text = currentNPC.InfoFalse;
                currentNPC.Target.GetComponent<RumorNPCs>().valueSus -= currentNPC.ImpactOnSus;
            }
            else if (buttonChoose == "DUVIDAR")
            {
                infoText.text = currentNPC.InfoTrue;
                currentNPC.Target.GetComponent<RumorNPCs>().valueSus += currentNPC.ImpactOnSus;
            }
        }
    }

    public void TrustButton()
    {
        buttonChoose = "CONFIAR";
        InfoReceive(buttonChoose);

        HideRumor();
        infoReceive.SetActive(true);
    }

    public void DoubtButton()
    {
        buttonChoose = "DUVIDAR";
        InfoReceive(buttonChoose);

        HideRumor();
        infoReceive.SetActive(true);
    }

    public void ResumeBoato()
    {
        infoReceive.SetActive(false);
        currentNPC.canAppearRumor = false;
    }
}
