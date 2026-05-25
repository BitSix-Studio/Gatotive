using UnityEngine;

public class OfficeController : MonoBehaviour
{
    private static OfficeController instance;

    public bool completeOfficeScene;

    private void Awake()
    {
        instance = this;
    }

    public static OfficeController Instance { get { return instance; } }
}
