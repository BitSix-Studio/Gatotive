using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/New Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private ElementsDialogue[] dialogues;
    private int indexCurrentSpeak;

    public void StartDialogue()
    {
        indexCurrentSpeak = 0;
    }

    public ElementsDialogue CurrentDialogue
    {
        get {
            if (indexCurrentSpeak < dialogues.Length)
            {
                return dialogues[indexCurrentSpeak];
            }
            return null;
        }
    }

    public void NextDialogue()
    {
        if (HasNextDialogue())
        {
            indexCurrentSpeak++;
        }
    }

    public bool HasNextDialogue()
    {
        if (indexCurrentSpeak < (dialogues.Length - 1))
        {
            return true;
        }

        return false;
    }
}
