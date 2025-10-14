using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class NewspaperPages : MonoBehaviour
{
    [SerializeField] private float pageSpeed;
    [SerializeField] private List<Transform> pages;

    private Animator animatorPataDireita;
    private Animator animatorPataEsquerda;
    private AnimatorStateInfo stateInfoAnimPataDireita;
    private AnimatorStateInfo stateInfoAnimPataEsquerda;

    private int index = 0;
    private int maxPages, minPages;
    private bool isRotate = false;

    private void Start()
    {
        maxPages = pages.Count - 1;
        minPages = 0;
        animatorPataDireita = GameObject.Find("SecondPawCat").GetComponent<Animator>();
        animatorPataEsquerda = GameObject.Find("FirstPawCat").GetComponent<Animator>();
    }

    private void Update()
    {
        stateInfoAnimPataDireita = animatorPataDireita.GetCurrentAnimatorStateInfo(0);
        stateInfoAnimPataEsquerda = animatorPataEsquerda.GetCurrentAnimatorStateInfo(0);
    }

    public void RotateForwardPage()
    {
        if (isRotate) { return; }

        if (index < maxPages) {
            animatorPataEsquerda.SetTrigger("ClickDirButton");

            StartCoroutine(WaitForAnimEnd("PataEsquerdaMovimento", () =>
            {
                animatorPataEsquerda.SetBool("VirarPag", true);

                index++;
                float angleY = 180f;
                pages[index].SetAsLastSibling();
                
                StartCoroutine(Rotate(angleY, true));
                StartCoroutine(WaitForAnimEnd("PataEsquerdaMudarPagina", () => { animatorPataEsquerda.SetBool("VirarPag", false); }, true));
            }, true));
        }
    }

    public void RotateBackPage()
    {
        if (isRotate) { return; }
        if (index > minPages) {
            animatorPataDireita.SetTrigger("ClickEsqButton");

            StartCoroutine(WaitForAnimEnd("PataDireitaMovimento", () =>
            {
                animatorPataDireita.SetBool("VirarPag", true);

                float angleY = 0f;
                pages[index].SetAsLastSibling();
                StartCoroutine(Rotate(angleY, false));
                StartCoroutine(WaitForAnimEnd("PataDireitaMudarPagina", () => { animatorPataDireita.SetBool("VirarPag", false); }, false));
            }, false));
        }
    }

    IEnumerator Rotate(float angleY, bool forward)
    {
        float value = 0f;
        while (true)
        {
            isRotate = true;
            Quaternion targetRotation = Quaternion.Euler(0, angleY, 0);
            value += Time.deltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);
            float angleSmooth = Quaternion.Angle(pages[index].rotation, targetRotation);
            if (angleSmooth < 0.1f)
            {
                if (forward==false)
                {
                    index--;
                }
                isRotate = false;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAnimEnd(string animName, System.Action onEnd, bool pataEsquerda)
    {
        if (pataEsquerda)
        {
            yield return new WaitUntil(() => stateInfoAnimPataEsquerda.IsName(animName));
            
            yield return new WaitUntil(() => stateInfoAnimPataEsquerda.normalizedTime >= 1f);

            animatorPataEsquerda.ResetTrigger("ClickDirButton");
        }
        else
        {
            yield return new WaitUntil(() => stateInfoAnimPataDireita.IsName(animName));

            yield return new WaitUntil(() => stateInfoAnimPataDireita.normalizedTime >= 1f);

            animatorPataDireita.ResetTrigger("ClickEsqButton");
        }
        onEnd?.Invoke();
    }
}
