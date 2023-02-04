using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonAsComponent<GameManager>
{
    #region SINGLETON
    public static GameManager Instance
    {
        get { return (GameManager)_Instance; }
        set { _Instance = value; }
    }
    #endregion
}
