using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PapelSuspect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Canvas canvas; // Canvas pai
    private CanvasGroup canvasGroup;

    public bool canDrag;
    public bool canLink;
    public bool isLocked;

    private Vector2 dragOffset;

    [TextArea(3, 5)] public string infoPaper;
    [SerializeField] private TextMeshProUGUI textInfo;
    [SerializeField] private GameObject papelInfos;
    public GameObject pinInstance;

    public static System.Action<PapelSuspect> OnPaperClicked;

    public RectTransform rectTransform { get; private set; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        papelInfos.SetActive(false);

        canDrag = true;
        canLink = false;
        isLocked = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        papelInfos.SetActive(true);
        textInfo.text = infoPaper;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        papelInfos.SetActive(false);
        textInfo.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!canDrag) return;

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false; // evita conflito com outros elementos da UI

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvas.transform as RectTransform,
        eventData.position,
        canvas.worldCamera,
        out Vector2 localPoint);

        dragOffset = rectTransform.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag && !canLink)
        {
            // Converte posição do toque para posição no Canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint);

            rectTransform.anchoredPosition = localPoint + dragOffset;
        }
        else if(canLink)
        {
            LineManager.Instance.UpdateTempLine(eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return; // impede novas conexões
        if (canLink)
            LineManager.Instance.StartLine(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (canLink)
        {
            PapelSuspect target = null;

            var hit = eventData.pointerCurrentRaycast.gameObject;
            if (hit != null)
                target = hit.GetComponent<PapelSuspect>();

            LineManager.Instance.FinalizeLine(target);
        }
    }

    public Vector2 GetCenterScreen()
    {
        return RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
    }

    public Vector2 GetTopScreen()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 worldTop = rect.TransformPoint(new Vector3(0, rect.rect.height * 0.5f, 0));
        return RectTransformUtility.WorldToScreenPoint(null, worldTop);
    }
}
