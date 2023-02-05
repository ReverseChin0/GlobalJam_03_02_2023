using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackComponent : MonoBehaviour
{
    public JicamaAnimManager m_Animations;
    [SerializeField]                                                                                                                                                                                                                                                                                                                                            
    float[] _attackForwardMovement, _nextAttackDelayTime, _attackStrength, _attackKnockback;
    Rigidbody2D _rigibody;
    int _lightAttackIndex, _heavyAttackIndex;

    [SerializeField]
    float _resetLightAttackIndex, _resetHeavyAttackIndex;

    [SerializeField]
    LayerMask _layerEnemies;
    private PlayerJump m_Jump;
    private PlayerMovement _Movement;

    bool _activeLightHitBox = false, _activeHeavyHitBox = false;

    float _currentLightAttackDuration = 0, _currentHeavyAttDuration = 0, _timeSinceLasLighttAttack = 0, _timeSinceLastHeavyAttack=0;

    private void Awake()
    {
        m_Jump = GetComponent<PlayerJump>();
        _Movement = GetComponent<PlayerMovement>();
        m_Animations = GetComponentInChildren<JicamaAnimManager>();
        _rigibody = GetComponent<Rigidbody2D>();
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
            DetectAndDamage(_lightAttackIndex);
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
            DetectAndDamage(_heavyAttackIndex + 3);
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
            {
                if (!m_Jump.landed && _lightAttackIndex == 0) _rigibody.simulated = false;
                if (_lightAttackIndex == 0) StartCoroutine(ResetRigi());
                m_Animations.LightAttack();
                LightAttack();
            }
        }
    }

    public void DoHeavyAttack(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
            {
                if (!m_Jump.landed && _lightAttackIndex == 0) _rigibody.simulated = false;
                if (_lightAttackIndex == 0) StartCoroutine(ResetRigi());
                m_Animations.HeavyAttack();
                HeavyAttack();
            }
        }
    }

    IEnumerator ResetRigi()
    {
        yield return new WaitForSeconds(0.6f);
        _rigibody.simulated = true;
    }

    public void DetectAndDamage(int attIndex)
    {
        print("punch");
        Vector3 direction = _Movement.IsFacingRight ? transform.right : -transform.right;
        direction *= 1.5f;
        Debug.DrawLine(transform.position + direction, transform.position + direction + Vector3.up*1.5f, Color.red, 1);
        Collider2D[] colliders =  Physics2D.OverlapCircleAll(transform.position + direction, 1.5f , _layerEnemies);
        print(colliders.Length);
        foreach(Collider2D col in colliders)
        {
            print(col+"Golpee un enemigo e hice" + _attackStrength[attIndex] + "de daño");
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
