using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TelaDialogo : MonoBehaviour
{
    [SerializeField] private Image photoCharacter;
    [SerializeField] private TextMeshProUGUI textNameCharacter;
    [SerializeField] private TextMeshProUGUI textSpeakDialogue;

    [SerializeField] private GameObject backgroundDialogue;

    private Dialogue dialogue;

    public bool endDialogue;

    private static TelaDialogo instance;

    private void Awake()
    {
        instance = this;
        DialogueHide();
        endDialogue = false;
    }

    public static TelaDialogo Instance { get { return instance; } }

    public void DialogueExibition(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        this.dialogue.StartDialogue();

        endDialogue = false;

        CurrentDialogueExibition();
        gameObject.SetActive(true);
    }

    public void Next()
    {
        if (dialogue.HasNextDialogue()) {
            dialogue.NextDialogue();
            CurrentDialogueExibition();
        }
        else
        {
            DialogueHide();
            endDialogue = true;
        }
    }

    private void CurrentDialogueExibition()
    {
        ElementsDialogue currentSpeak = dialogue.CurrentDialogue;

        if (currentSpeak.ImagemPersonagem != null)
            photoCharacter.sprite = currentSpeak.ImagemPersonagem;

        
        textNameCharacter.text = currentSpeak.NomePersonagem;
        VerificarPersonagemAtivo();

        textSpeakDialogue.text = currentSpeak.Falas;
    }

    private void DialogueHide()
    {
        gameObject.SetActive(false);
    }

    private void VerificarPersonagemAtivo()
    {
        Vector3 mirror = new Vector3(-1, 1, 1);
        Vector3 normal = Vector3.one;

        if (textNameCharacter.text == "KURO" || textNameCharacter.text == "GATOTIVE")
        {
            backgroundDialogue.transform.localScale = normal;
            textNameCharacter.rectTransform.localScale = normal;
            textNameCharacter.alignment = TextAlignmentOptions.MidlineJustified;

            textSpeakDialogue.rectTransform.localScale = normal;
            textSpeakDialogue.alignment = TextAlignmentOptions.TopJustified;

            photoCharacter.rectTransform.pivot = new Vector2(0, 0);
            photoCharacter.rectTransform.anchorMax = new Vector2(0, 0);
            photoCharacter.rectTransform.anchorMin = new Vector2(0, 0);
        }
        else
        {
            backgroundDialogue.transform.localScale = mirror;
            textNameCharacter.rectTransform.localScale = mirror;

            textSpeakDialogue.rectTransform.localScale = mirror;

            photoCharacter.rectTransform.pivot = new Vector2(1, 0);
            photoCharacter.rectTransform.anchorMax = new Vector2(1, 0);
            photoCharacter.rectTransform.anchorMin = new Vector2(1, 0);
        }

        
    }
}
