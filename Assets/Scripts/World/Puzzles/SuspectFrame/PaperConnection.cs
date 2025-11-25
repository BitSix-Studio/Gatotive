using UnityEngine;

[System.Serializable]
public class PaperConnection
{
    public RectTransform line;
    public PapelSuspect paperA;
    public PapelSuspect paperB;

    public PaperConnection(RectTransform line, PapelSuspect a, PapelSuspect b)
    {
        this.line = line;
        paperA = a;
        paperB = b;
    }
}
