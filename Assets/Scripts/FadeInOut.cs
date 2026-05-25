using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public Animator logoAnimator;
    public string nextScene;
    public float waitAfterAnimation = 1f;

    private void Start()
    {
        StartCoroutine(PlaySplash());
    }
    
    private IEnumerator PlaySplash()
    {
        // espera o fim da animação atual
        var length = logoAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length + waitAfterAnimation);

        // carrega a próxima cena
        SceneManager.LoadScene(nextScene);
    }
}
