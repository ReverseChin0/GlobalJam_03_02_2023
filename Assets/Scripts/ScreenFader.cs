using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader _inst; //instancia del ScreenFader
    CanvasGroup _miCG = default; //

    private void Awake()
    {
        if (_inst != null) //si ya hay una que se auto destruya
        {
            Destroy(gameObject);
            return;
        }

        _inst = this; //sino la volvemos la instancia y conseguimos los scripts
        DontDestroyOnLoad(gameObject);
        _miCG = GetComponent<CanvasGroup>();

    }

    private void Start() //al iniciar ponemos el alfa al 1 para que aparezca en negro ...
    {
        _miCG.alpha = 1;
        _miCG.DOFade(0, 1.0f); // ... y luego lo tweeneamos a 0 durante 1 segundo
    }

    public void FadeToLevel(string _lvl) //funcion generica para cambiar de nivel
    {
        StartCoroutine(loadLevel(_lvl));
    }

    IEnumerator loadLevel(string _s) //Corrutina con la lógica para cambiar de nivel
    {
        _miCG.alpha = 0;
        _miCG.DOFade(1, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_s);//espero a que cargue
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.0f);
        _miCG.alpha = 1;
        _miCG.DOFade(0, 1.0f);
    }

    public void justFade(float nSecs = 1)
    {
        _miCG.alpha = 0;
        _miCG.DOFade(1, nSecs * 0.5f).OnComplete(() => _miCG.DOFade(0, nSecs * 0.5f));
    }
}