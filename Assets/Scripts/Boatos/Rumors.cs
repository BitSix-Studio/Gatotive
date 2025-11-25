using UnityEngine;

[System.Serializable]
public class Rumors
{
    [TextArea(2,5)]
    public string rumorText;
    public bool rumorTrueOrFalse;

    [Range(1,10)]
    public float impactOnSuspeitometro;

    [Header("Suspeitometro do Personagem Impactado")]
    public GameObject targetSuspeitometro;

    [TextArea(2, 5)]
    public string receiveTrueInfo, receiveFalseInfo;
}
