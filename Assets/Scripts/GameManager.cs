using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonAsComponent<GameManager>
{
    float _timerToDeath;
    float _score=0;
    bool _gameOverByFinish = false;
    [SerializeField]
    float _maxTimeBetweenKills = 6.0f, _maxQuickKillScore = 100;
    float _lastKillTime=-1;
    bool _killedOneEnemie = false;
    private byte _rankSkill = 0;

    [SerializeField]
    TextMeshProUGUI _rankText,_scoreText;
    [SerializeField]
    Image _rankImage,_rankStar;
    [SerializeField]
    GameObject _endOverScreen,_victoryScreen;
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
        _rankImage.fillAmount = 0;
    }

    public void SetTimerToDeath(float timerToDeath)
    {
        _timerToDeath = timerToDeath;
        _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0);
    }

    private void Update()
    {
        _timerToDeath -= Time.deltaTime;
        if (_timerToDeath <= 0 && !_gameOverByFinish)
        {
            GameOverRoutine();
        }

        float percentageOfScore = (Time.time - _lastKillTime) / _maxTimeBetweenKills;
        _rankImage.fillAmount = 1 - percentageOfScore;
        if (1 - percentageOfScore <= 0)
        {
            _rankSkill = 0;
            _killedOneEnemie = false;
            StyleFeedback();
        }

        _rankStar.rectTransform.Rotate(0, 0, Time.deltaTime * _rankSkill*2);
    }

    [ContextMenu("FinishedDeath")]
    public void GameOverRoutine()
    {
        _endOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    [ContextMenu("FinishGame")]
    public void FinishedLevelScoreCalculation()
    {
        _victoryScreen.SetActive(true);
        _gameOverByFinish = true;
        _score += _timerToDeath;
        _scoreText.text = "Your score is :" + _score.ToString();
        Time.timeScale = 0;
    }

    public void LoadLevel(string _s)
    {
        ScreenFader._inst.FadeToLevel(_s);
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
                _rankSkill = 0;
                StyleFeedback();
                return;
            }                
            float percentageOfScore = (Time.time - _lastKillTime) / _maxTimeBetweenKills;
            float scoreToAdd = Mathf.Lerp(_maxQuickKillScore, 1, percentageOfScore);

            if (_rankSkill<7) _rankSkill++;
            print("Score " + scoreToAdd);
            AddScore(scoreToAdd);
            StyleFeedback();
        }
        _lastKillTime = Time.time;
        _killedOneEnemie = true;
    }

    public void StyleFeedback()
    {
        switch (_rankSkill)
        {
            case 1: _rankText.text = "D"; break;
            case 2: _rankText.text = "C";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0.2f); break;
            case 3: _rankText.text = "B";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0.3f); break;
            case 4: _rankText.text = "A";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0.5f); break;
            case 5: _rankText.text = "S";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0.7f); break;
            case 6: _rankText.text = "SS";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0.8f); break;
            case 7: _rankText.text = "SSS";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0.9f); break;
            default: _rankText.text = "";
                _rankStar.color = new Color(0.8f, 0.6f, 0.1f, 0f); break;
        }
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
