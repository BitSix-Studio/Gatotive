using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuspectFrame : MonoBehaviour
{
    private static SuspectFrame instance;
    [SerializeField] private Dialogue[] dialoguesContinue, dialoguesVoltar, dialoguePins; // Serve para organizar os diálogos no inspector

    [Header ("Diálogos de Alteração")]
    [SerializeField] private Dialogue dialogueChangeAkemi, afterCompleteFrameDialogue;

    private int clickOnContinue, clickOnVoltar, pinAppear, buttonsClick; // Controla a disposição dos diálogos entre acontecimentos do quadro

    [Header ("Botões da Cena")]
    [SerializeField] private Button backButton, continueButton, cleanButton;

    private bool completeFrame;

    private void Awake()
    {
        instance = this;
        clickOnContinue = 0;
        clickOnVoltar = 0;
        pinAppear = 0;
        buttonsClick = 0;
        completeFrame = false;
    }

    public static SuspectFrame Instance { get { return instance; } }

    private void FixedUpdate()
    {
        if (LineManager.Instance.lineContainer.Find("Pin(Clone)") &&
            pinAppear == 0)
        {   
            TelaDialogo.Instance.DialogueExibition(dialoguePins[pinAppear]);
            pinAppear++;
        }
        else if (LineManager.Instance.HasAllCorrectConnections() &&
            pinAppear == 1)
        {
            TelaDialogo.Instance.DialogueExibition(dialoguePins[pinAppear]);
            pinAppear++;
            backButton.gameObject.SetActive(false);
            cleanButton.gameObject.SetActive(false);
            buttonsClick = 4;
        }
    }

    // Esconde o quadro
    public void HideFrame()
    {
        gameObject.SetActive(false);
        LineManager.Instance.lineContainer.gameObject.SetActive(false);
    }

    //Faz o quadro aparecer
    public void FrameExibition()
    {
        gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        cleanButton.gameObject.SetActive(false);
        if (completeFrame)
        {
            TelaDialogo.Instance.DialogueExibition(afterCompleteFrameDialogue);
            LineManager.Instance.lineContainer.gameObject.SetActive(true);
        } 
    }

    // Botão de Continuar
    public void Continuar()
    {
        backButton.gameObject.SetActive(true);
        cleanButton.gameObject.SetActive(true);

        buttonsClick++;
        if (buttonsClick < 3)
        {
            if (clickOnContinue <= 0)
            {
                TelaDialogo.Instance.DialogueExibition(dialoguesContinue[clickOnContinue]); // Aparece o diálogo das linhas
            }
            else if (clickOnContinue == 1)
            {
                TelaDialogo.Instance.DialogueExibition(dialoguesContinue[clickOnContinue]); // Aparece o diálogo das linhas resumido
            }

            clickOnContinue = 1; // Para controlar o comportamento dos diálogos e não voltar o clickContinue a 0 novamente

            if (clickOnContinue != 0)
            {
                clickOnContinue++; // clickContinue = 2
            }
            if (clickOnVoltar > 0)
            {
                clickOnVoltar--; // clickVoltar = 0
            }
        }

        // Pega todos os scripts dos papeis dentro deste objeto
        PapelSuspect[] papeis = GetComponentsInChildren<PapelSuspect>(includeInactive: true);

        // Ativa comportamento das linhas vermelhas no quadro e desativa a possibilidade de arrastar os itens
        foreach (var papel in papeis)
        {
            papel.canDrag = false; // Desativa o comportamento de arraste
            papel.canLink = true; // Ativa o modo de conectar linhas
            LineManager.Instance.lineContainer.gameObject.SetActive(true);
        }

        if (LineManager.Instance.HasAllCorrectConnections())
        {
            clickOnContinue = 2;
            TelaDialogo.Instance.DialogueExibition(dialoguesContinue[clickOnContinue]);
            HideFrame();

            var akemi = GameObject.Find("Akemi_NPC");
            akemi.GetComponent<InteractionsNPC>().dialogue = dialogueChangeAkemi;

            completeFrame = true;

            var frame = GameObject.FindWithTag("SuspectFrame");
            frame.GetComponent<InteractionObjects>().dialogue = afterCompleteFrameDialogue;
        }
    }

    // Botão de Voltar
    public void Voltar()
    {
        backButton.gameObject.SetActive(false);
        cleanButton.gameObject.SetActive(false);

        if (clickOnVoltar <= 0)
        {
            TelaDialogo.Instance.DialogueExibition(dialoguesVoltar[clickOnVoltar]); // Aparece diálogo de voltar ao arraste dos itens
        }

        // Pega todos os scripts dos papeis dentro deste objeto
        PapelSuspect[] papeis = GetComponentsInChildren<PapelSuspect>(includeInactive: true);

        // Ativa a possibilidade de arrastar os itens do quadro novamente
        foreach (var papel in papeis)
        {
            papel.canDrag = true; // Ativa o comportamento de arraste
            papel.canLink = false; // Desativa o modo de conectar linhas
            LineManager.Instance.lineContainer.gameObject.SetActive(false);
        }

        if (buttonsClick < 3)
        {
            clickOnContinue--; // clickContinue = 1
            clickOnVoltar++; // clickVoltar = 1
        }
    }
}
