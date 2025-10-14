using UnityEngine;

public class InteractionsNPC : MonoBehaviour
{
    [SerializeField] private Transform iconInteract;

    private void Awake()
    {
        
    }

    private void Start()
    {
        iconInteract.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iconInteract.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iconInteract.gameObject.SetActive(false);
        }
    }
}
