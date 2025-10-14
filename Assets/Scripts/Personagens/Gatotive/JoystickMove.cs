using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMove : MonoBehaviour
{
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject joystickBg;
    public Vector2 joystickVector;
    private Vector2 joystickTouchPosition;
    private Vector2 joystickOriginalPosition;
    private float joystickRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joystickOriginalPosition = joystickBg.transform.position;
        joystickRadius = joystickBg.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBg.transform.position = Input.mousePosition;
        joystickTouchPosition = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        joystickVector = (dragPos - joystickTouchPosition).normalized;

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPosition);

        if (joystickDist < joystickRadius)
        {
            joystick.transform.position = joystickTouchPosition + joystickVector * joystickDist;
        }
        else
        {
            joystick.transform.position = joystickTouchPosition + joystickVector * joystickRadius;
        }
    }

    public void PointerUp() 
    {
        joystickVector = Vector2.zero;
        joystick.transform.position = joystickOriginalPosition;
        joystickBg.transform.position = joystickOriginalPosition;
    }
}
