using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField]                                                                                                                                                                                                                                                                                                                                            
    float[] _attackForwardMovement, _nextAttackDelayTime, _attackStrength, _attackKnockback;

    int _lightAttackIndex, _heavyAttackIndex;

    [SerializeField]
    float _resetLightAttackIndex, _resetHeavyAttackIndex;

    private PlayerMovement _Movement;

    bool _activeLightHitBox = false, _activeHeavyHitBox = false;

    float _currentLightAttackDuration = 0, _currentHeavyAttDuration = 0, _timeSinceLasLighttAttack = 0, _timeSinceLastHeavyAttack=0;

    private void Awake()
    {
        _Movement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        _currentLightAttackDuration -= Time.deltaTime;        
        _timeSinceLasLighttAttack += Time.deltaTime;
        if(_timeSinceLasLighttAttack > _resetLightAttackIndex) {
            _lightAttackIndex = 0;
            _activeLightHitBox = false;
        }

        _currentHeavyAttDuration -= Time.deltaTime;
        _timeSinceLastHeavyAttack += Time.deltaTime;
        if(_timeSinceLastHeavyAttack > _resetHeavyAttackIndex)
        {
            _heavyAttackIndex = 0;
            _activeHeavyHitBox = false;
        }
    }

    public void InterruptEverything()
    {
        _lightAttackIndex = 0;
        _heavyAttackIndex = 0;
        _timeSinceLasLighttAttack = _nextAttackDelayTime[2];
        _timeSinceLastHeavyAttack = _nextAttackDelayTime[4];
    }

    void LightAttack()
    {        
        if (_lightAttackIndex < 3 && _currentLightAttackDuration <= 0 )
        {            
            _currentLightAttackDuration = _nextAttackDelayTime[_lightAttackIndex];
            _timeSinceLasLighttAttack = 0;
            _activeLightHitBox = true;            
            _lightAttackIndex++;            
        }
        else
        {
            if (_lightAttackIndex == 3)
            {
                _lightAttackIndex = 0;
            }
        }        
    }

    void HeavyAttack()
    {
        if (_heavyAttackIndex < 2 && _currentHeavyAttDuration <= 0)
        {
            _currentHeavyAttDuration = _nextAttackDelayTime[_heavyAttackIndex+3];            
            _timeSinceLastHeavyAttack = 0;
            _activeHeavyHitBox = true;
            _heavyAttackIndex++;
        }
        else
        {
            if (_heavyAttackIndex == 2)
            {
                _heavyAttackIndex = 0;
            }
        }
    }

    public void DoLightAttack(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
                LightAttack();
        }
    }

    public void DoHeavyAttack(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
                HeavyAttack();
        }
    }


    /*private void OnDrawGizmos()
    {
        /*if (_activeHeavyHitBox)
        {
            Gizmos.color = _heavyAttackIndex - 1 == 0 ? Color.green :
                           _heavyAttackIndex - 1 == 1 ? Color.yellow : Color.red;

            Vector3 facing = _isFacingRight ? Vector3.right : -Vector3.right;
            Gizmos.DrawSphere(transform.position + facing, 0.5f);
        }
        if (_activeHeavyHitBox)
        {
            Gizmos.color = _heavyAttackIndex - 1 == 0 ? Color.green :
                           _heavyAttackIndex - 1 == 1 ? Color.yellow : Color.red;

            Vector3 facing = _isFacingRight ? Vector3.right : -Vector3.right;
            Gizmos.DrawSphere(transform.position + facing, 0.5f);
        }
    }*/

}
