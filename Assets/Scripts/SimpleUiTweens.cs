using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*
 * Libreria de Tween para UI Extensible
 * Ponselo a cualquiere elemento de Ui que desees que este animado al iniciar o desactivar
 */

public enum TweenType
{
    Move,
    Scale,
    Fade,
    Jump,
    Swing
}
public class SimpleUiTweens : MonoBehaviour
{
    [SerializeField] private bool _playOnStart = false;
    [SerializeField] private bool _playOnEnable = false;
    [SerializeField] private bool _useInitialPosition = false;
    [SerializeField] private bool _unscaleTime = false;
    [SerializeField] private bool _loops = false;
    [SerializeField] private float _delay = 0.0f, _duration = 0.5f;
    [SerializeField] private TweenType _tweenType = TweenType.Move;
    [SerializeField] private Ease _easeType = default;
    [SerializeField] Vector2 _initialPos = default;
    [SerializeField] private Vector2 _endPos = default;
    RectTransform _rectTransform = default;
    CanvasGroup _cGroup = default;
    bool _isDeactivating = false;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (_tweenType == TweenType.Fade)
        {
            _cGroup = GetComponent<CanvasGroup>();
            if (_cGroup == null)
                _cGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        if (!_useInitialPosition)
            _initialPos = _rectTransform.anchoredPosition;

        if (_playOnStart)
            PlayTween();
    }

    private void OnEnable()
    {
        if (_playOnEnable)
            PlayTween();
    }

    public void PlayTween()
    {
        switch (_tweenType)
        {
            case TweenType.Move:
                if (_useInitialPosition)
                    _rectTransform.anchoredPosition = _initialPos;

                _rectTransform.
                    DOAnchorPos(_endPos, _duration).
                    SetDelay(_delay).
                    SetEase(_easeType).SetLoops(_loops ? -1 : 0).
                    SetUpdate(_unscaleTime);
                break;

            case TweenType.Scale:
                if (_useInitialPosition)
                    _rectTransform.localScale = new Vector3(_initialPos.x, _initialPos.y, 0);

                _rectTransform.
                    DOScale(new Vector3(_endPos.x, _endPos.y, 0), _duration).
                    SetDelay(_delay).
                    SetEase(_easeType).
                    SetLoops(_loops ? -1 : 0).
                    SetUpdate(_unscaleTime);
                break;

            case TweenType.Fade:
                if (_useInitialPosition)
                    _cGroup.alpha = _initialPos.x;

                _cGroup.
                    DOFade(_endPos.x, _duration).
                    SetDelay(_delay).
                    SetEase(_easeType).
                    SetLoops(_loops ? -1 : 0, LoopType.Yoyo).
                    SetUpdate(_unscaleTime);
                break;

            case TweenType.Jump:
                if (_useInitialPosition)
                    _rectTransform.anchoredPosition = _initialPos;

                _rectTransform.
                    DOJumpAnchorPos(_endPos, 100.0f, 0, _duration).
                    SetDelay(_delay).
                    SetLoops(_loops ? -1 : 0);
                break;

            case TweenType.Swing:
                if (_useInitialPosition)
                    _rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, _initialPos.x));

                _rectTransform.
                    DOLocalRotate(new Vector3(0, 0, _endPos.x), _duration).
                    SetEase(_easeType).
                    SetLoops(_loops ? -1 : 0, LoopType.Yoyo).
                    SetUpdate(_unscaleTime);
                break;

            default:
                break;
        }
    }

    public void Switch()
    {
        Vector2 temp = _initialPos;
        _initialPos = _endPos;
        _endPos = temp;
    }

    void InvertAndPlay()
    {
        Switch();
        PlayTween();
    }

    public void Deactivate() //llama esta funcion para setactive false un objeto
    {
        if (gameObject.activeSelf)
            StartCoroutine(deactivate());
    }

    IEnumerator deactivate()
    {
        _isDeactivating = true;
        InvertAndPlay();
        yield return new WaitForSeconds(_duration);
        Switch();
        gameObject.SetActive(false);
        _isDeactivating = false;
    }

    public void Activate() //llama esta funcion para setactive true un objeto
    {
        if (!_isDeactivating)
        {
            gameObject.SetActive(true);
        }
        else
        {
            StopAllCoroutines();
            _isDeactivating = false;
            Switch();
            PlayTween();
        }
    }
}