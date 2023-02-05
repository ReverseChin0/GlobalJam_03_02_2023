using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : SingletonAsComponent<GameManager>
{
    float _timerToDeath;
    float _score=0;
    bool _gameOverByFinish = false;
    [SerializeField]
    float _maxTimeBetweenKills = 6.0f, _maxQuickKillScore = 100;
    float _lastKillTime=0;
    bool _killedOneEnemie = false;

    #region SINGLETON
    public static GameManager Instance
    {
        get { return (GameManager) _Instance; }
        set { _Instance = value; }
    }
    #endregion

    private void Start()
    {
        SetTimerToDeath(100);
    }

    public void SetTimerToDeath(float timerToDeath)
    {
        _timerToDeath = timerToDeath;
    }

    private void Update()
    {
        _timerToDeath -= Time.deltaTime;
        if (_timerToDeath <= 0 && !_gameOverByFinish)
        {
            GameOverRoutine();
        }
    }
    void GameOverRoutine()
    {
        
    }

    [ContextMenu("FinishGame")]
    void FinishedLevelScoreCalculation()
    {
        _gameOverByFinish = true;
        _score += _timerToDeath;
    }

    
    public void AddScore(float scoretoAdd)
    {
        _score += scoretoAdd;
    }

    [ContextMenu("Do A Kill")]
    public void EnemyKilled()
    {
        
        if (_killedOneEnemie)
        {
            if (_lastKillTime > _lastKillTime+_maxTimeBetweenKills)
            {
                _killedOneEnemie = false;
                return;
            }                
            float percentageOfScore = (Time.time - _lastKillTime) / _maxTimeBetweenKills;
            float scoreToAdd = Mathf.Lerp(_maxQuickKillScore, 1, percentageOfScore);

            print("Score " + scoreToAdd);
            AddScore(scoreToAdd);
        }
        _lastKillTime = Time.time;
        _killedOneEnemie = true;
    }

    void OnDrawGizmos()
    {
        Handles.Label(transform.position, _timerToDeath.ToString());
        Handles.color = Color.red;
        Handles.Label(transform.position - Vector3.up, _score.ToString());
        /*Handles.color = Color.blue;
        Handles.Label(transform.position - Vector3.up*2, _lastKillTime.ToString());*/
    }
}
