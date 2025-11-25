using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class InteractionsNPC : MonoBehaviour
{
    [SerializeField] private Transform iconInteract;
    [SerializeField] private float moveSpeedIcon, maxIconMove;
    private Vector2 initialPos;

    [SerializeField] private GameObject panelButtonAction;

    private InteractionsNPC NPCAtivoAtual;
    [HideInInspector] public static Action<GameObject, bool, bool, Dialogue> npcAtivo;

    public bool hasDialogue;
    public Dialogue dialogue;

    public Dialogue[] extraDialogues;
    [HideInInspector] public int indexDialogue;

    public bool hasRumor;

    private void Awake()
    {
        if (panelButtonAction == null)
        {
            panelButtonAction = GameObject.Find("ButtonAction");
        }
        if (iconInteract == null)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Contains("Icons"))
                {
                    iconInteract = child;
                    break;
                }
            }
        }
    }

    private void Start()
    {
        initialPos = iconInteract.position;
        iconInteract.gameObject.SetActive(false);

        panelButtonAction.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (TelaDialogo.Instance.endDialogue && NPCAtivoAtual == this)
        {
            ChooseDialogue();
            TelaDialogo.Instance.endDialogue = false;
        }
    }

    private IEnumerator MoveIcon()
    {
        while (true)
        {
            float iconY = Mathf.Sin(Time.time * moveSpeedIcon) * maxIconMove + initialPos.y;
            iconInteract.position = new Vector2(iconInteract.position.x, iconY);
            yield return null;
        }
    }

    private void ChooseDialogue()
    {
        if (extraDialogues == null || extraDialogues.Length == 0) return; 

        if (indexDialogue > extraDialogues.Length -1)
        {
            indexDialogue = extraDialogues.Length;
            return;
        }

        dialogue = extraDialogues[indexDialogue];

        if (indexDialogue < extraDialogues.Length) {
            indexDialogue++;
        }

        SendNewDialogue();
    }

    private void SendNewDialogue()
    {
        if (NPCAtivoAtual == this)
            npcAtivo?.Invoke(gameObject, hasDialogue, hasRumor, dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NPCAtivoAtual = this;

            iconInteract.gameObject.SetActive(true);
            StartCoroutine(MoveIcon());

            panelButtonAction.SetActive(true);
            SendNewDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (NPCAtivoAtual == this)
                NPCAtivoAtual = null;

            iconInteract.gameObject.SetActive(false);
            if (panelButtonAction != null) {
                panelButtonAction.SetActive(false);
            }
        }
    }
}
