using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventsystem;


    public void setActiveButton()
    {
        GameObject selected = GameObject.FindGameObjectWithTag("EditorOnly");
        print(selected);
        _eventsystem.SetSelectedGameObject(selected);
    }

    public void ToggleLanguage()
    {
        changeLocale(LocalizationSettings.SelectedLocale.LocaleName == "Spanish (es)" ? 0 : 1);
    }

    public void changeLocale(int localeID)
    {
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
