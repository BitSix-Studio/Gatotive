using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractionObjects : MonoBehaviour
{
    [SerializeField] private Light2D lightObject;
    [SerializeField] private GameObject panelButtonAction;

    [HideInInspector] public static Action<string, bool, Dialogue> objAtivo;

    [SerializeField] private float timeLight, intensityLightPerSecond, intensityLightFinal;

    [SerializeField] private bool hasDialogue;
    public Dialogue dialogue;

    private void Awake()
    {
        if (panelButtonAction == null)
        {
            panelButtonAction = GameObject.Find("ButtonAction");
        }
        lightObject = GetComponent<Light2D>();
    }

    private void Start()
    {
        lightObject.enabled = false;
        panelButtonAction.SetActive(false);
    }

    IEnumerator AtivarLuzGradiente()
    {
        lightObject.enabled = true;
        lightObject.intensity = 0f;
        while (lightObject.intensity < intensityLightFinal) 
        { 
            lightObject.intensity += intensityLightPerSecond;
            if (lightObject.intensity >= intensityLightFinal)
            {
                lightObject.intensity = intensityLightFinal;
            }
            yield return new WaitForSeconds(timeLight);
        }
    }

    IEnumerator DesativarLuzGradiente()
    {
        lightObject.intensity = intensityLightFinal;
        while (lightObject.intensity > 0f)
        {
            lightObject.intensity -= intensityLightPerSecond;
            if (lightObject.intensity <= 0f)
            {
                lightObject.intensity = 0f;
            }
            yield return new WaitForSeconds(timeLight);
        }
        lightObject.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(AtivarLuzGradiente());
            panelButtonAction.SetActive(true);
            objAtivo?.Invoke(gameObject.tag, hasDialogue, dialogue);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DesativarLuzGradiente());
            panelButtonAction.SetActive(false);
        }
    }
}
