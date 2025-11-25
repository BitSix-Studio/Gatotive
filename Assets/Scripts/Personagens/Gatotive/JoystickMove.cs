using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickMove : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("References")]
    [SerializeField] private RectTransform background;   // o círculo de fundo
    [SerializeField] private RectTransform handle;       // a bolinha que o usuário arrasta

    [Header("Settings")]
    [SerializeField] private float handleLimit = 80f;    // distância máxima (em pixels) que o handle pode se afastar do centro
    [SerializeField, Range(0f, 1f)] private float deadZone = 0.15f; // zona morta (em fração da handleLimit)
    [SerializeField, Range(0f, 1f)] private float leftScreenLimit = 0.5f; // limite para aceitar toques (dentro do painel "JoystickMovement")

    // resultado de input normalizado (-1..1)
    private Vector2 inputVector = Vector2.zero;

    // referências
    private Canvas parentCanvas;
    private Camera uiCamera;

    // posição inicial do background quando nasceu (em tela) — não usaremos para mover background
    private Vector2 startScreenPos;

    // cor do joystick
    private Image bgImage;
    private Image handImage;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    private void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
            Debug.LogError("Joystick deve estar dentro de um Canvas.");

        if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            uiCamera = parentCanvas.worldCamera;

        if (background != null)
        {
            startScreenPos = background.position;

            bgImage = background.GetComponent<Image>();
            handImage = handle.GetComponent<Image>();
            SetAlpha(bgImage, 0.5f);
            SetAlpha(handImage, 0.2f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Aceita toque apenas dentro do limite especificado
        if (eventData.position.x > Screen.width * leftScreenLimit)
            return;

        SetAlpha(bgImage, 1f);
        SetAlpha(handImage, 1f);
        // Posiciona o background onde o jogador tocou (em tela)
        background.position = eventData.position;

        // Centraliza o handle
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convertendo a posição do toque para coordenadas locais do background
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, uiCamera, out localPoint);

        // localPoint está em pixels relativos ao centro do background
        // ClampMagnitude para garantir que o handle nunca "salte" para borda de forma brusca

        // POSIÇÃO VISUAL DO HANDLE: segue o dedo, mas é limitada ao handleLimit
        Vector2 clampedHandlePos = Vector2.ClampMagnitude(localPoint, handleLimit);
        handle.anchoredPosition = clampedHandlePos;

        // INPUT EFETIVO: baseado na distância do handle ao centro (normalizado 0..1), aplicando deadzone sem causar teleporte visual.
        float currentMagnitude = clampedHandlePos.magnitude;            // pixels
        float magnitudeNormalized = currentMagnitude / handleLimit;     // 0..1

        if (magnitudeNormalized <= deadZone)
        {
            // Dentro da deadzone: input = zero (mesmo que o handle esteja deslocado visualmente)
            inputVector = Vector2.zero;
        }
        else
        {
            // Fora da deadzone: remapear magnitude de forma suave (0..1)
            float adjustedMagnitude = (magnitudeNormalized - deadZone) / (1f - deadZone);
            Vector2 direction = (currentMagnitude > 0f) ? (clampedHandlePos / currentMagnitude) : Vector2.zero;
            inputVector = direction * Mathf.Clamp01(adjustedMagnitude);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        background.position = startScreenPos;

        SetAlpha(bgImage, 0.5f);
        SetAlpha(handImage, 0.2f);
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color targetColor = img.color;
        targetColor.a = alpha;

        img.color = targetColor;
    }
}