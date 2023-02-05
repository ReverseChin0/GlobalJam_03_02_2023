using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    float _changeDuration = 0.5f;

    bool _targetIsRight = false, _transitioning = false, _isVisible = true;
    Vector3 _initialPose, _endPose;
    float _elapsedTime;
    private void Awake()
    {

    }
    private void Update()
    {
        if (!_transitioning)
        {
            _targetIsRight = _target.position.x > transform.position.x + 10;            
            if (_target.position.x > transform.position.x + 10 || _target.position.x < transform.position.x - 10)
            {
                _transitioning = true;
                _elapsedTime = 0;
                _initialPose = transform.position;

                _endPose = _targetIsRight ? transform.position + Vector3.right * 15 : transform.position + Vector3.right * -15;
            }
        }
        else
        {
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _changeDuration;
            transform.position = Vector3.Lerp(_initialPose, _endPose, Mathf.SmoothStep(0, 1,t));
            if (t >= 1)
                _transitioning = false;
        }
    }

}
