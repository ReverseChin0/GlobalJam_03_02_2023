using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() //al iniciar ponemos el alfa al 1 para que aparezca en negro ...
    {
    }

    public void LoadLevel(string _lvl) //funcion generica para cambiar de nivel
    {
        StartCoroutine(RoutineLoadLevel(_lvl));
    }

    IEnumerator RoutineLoadLevel(string _s) //Corrutina con la lógica para cambiar de nivel
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_s);//espero a que cargue
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.0f);
    }

}

