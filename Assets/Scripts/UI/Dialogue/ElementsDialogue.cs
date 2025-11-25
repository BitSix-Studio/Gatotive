using UnityEngine;

[System.Serializable]
public class ElementsDialogue
{
    [SerializeField] private string nomePersonagem;
    [SerializeField] private Sprite imgPersonagem;
    [SerializeField, TextArea(1, 5)] private string txtSpeak;

    public string NomePersonagem { get { return this.nomePersonagem;  } }

    public Sprite ImagemPersonagem { get { return this.imgPersonagem; } }

    public string Falas { get { return this.txtSpeak; } }
}
