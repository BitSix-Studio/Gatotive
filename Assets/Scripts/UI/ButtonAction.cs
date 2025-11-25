using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private Button button;

    private Dialogue dialogueNpc;
    private Dialogue dialogueObj;

    private RumorNPCs npcAtual;

    public int contNpcs;

    private void OnEnable()
    {
        InteractionsNPC.npcAtivo += AdicionarChamada;
        InteractionObjects.objAtivo += AdicionarChamada;
    }

    private void OnDisable()
    {
        InteractionsNPC.npcAtivo -= AdicionarChamada;
        InteractionObjects.objAtivo -= AdicionarChamada;
    }
    private void Start()
    {
        contNpcs = 0;
    }

    void AdicionarChamada(string action, bool hasDialogue, Dialogue dialogue)
    {
        button.onClick.RemoveAllListeners();

        if (action == "SuspectFrame")
        {
            dialogueObj = dialogue;
            button.onClick.AddListener(() => QuadroSuspeitos(hasDialogue));
        }
        if (action == "RG")
        {
            button.onClick.AddListener(() => CrimeScene(action));
        }
        if (action == "Vitima")
        {
            button.onClick.AddListener(() => CrimeScene(action));
        }
    }

    void AdicionarChamada(GameObject actor, bool hasDialogue, bool rumor, Dialogue dialogue)
    {
        button.onClick.RemoveAllListeners();

        if (actor.CompareTag("Npc"))
        {
            contNpcs++;

            dialogueNpc = dialogue;
            npcAtual = actor.GetComponent<RumorNPCs>();
            button.onClick.AddListener(() => AcionarNPC(hasDialogue, rumor));
        }
    }

    void AcionarNPC(bool hasDialogue, bool rumor)
    {
        if (hasDialogue && !rumor)
        {
            TelaDialogo.Instance.DialogueExibition(this.dialogueNpc);
        }
        else if (!hasDialogue && rumor)
        {
            RumorPanel.Instance.ExibitionRumor(npcAtual);
        }
        else if (hasDialogue && rumor)
        {
            StartCoroutine(ShowDialogueAfterRumor());
        }
    }

    private IEnumerator ShowDialogueAfterRumor()
    {
        // Começa o diálogo
        TelaDialogo.Instance.DialogueExibition(this.dialogueNpc);

        // Espera até o diálogo terminar
        yield return new WaitUntil(() => TelaDialogo.Instance.endDialogue);

        // Mostra o rumor
        RumorPanel.Instance.ExibitionRumor(npcAtual);
    }

    void QuadroSuspeitos(bool hasDilaogue)
    {
        if (hasDilaogue)
        {
            TelaDialogo.Instance.DialogueExibition(this.dialogueObj);
            SuspectFrame.Instance.FrameExibition();
        }
    }

    void CrimeScene(string tag)
    {
        ItensCrimePanel.Instance.ItemExibition(tag);
    }
}
