using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabComponent : MonoBehaviour
{
    bool _isGrabbing = false;
    float _grabCoolDown = 1, _timeSinceGrabTry=0;

    [SerializeField]
    LayerMask _layerEnemies;
    [SerializeField]
    Transform _rootHook;
    [SerializeField]
    float _grabDistance = 5.0f, _grabTryDuration=0.8f;

    private PlayerMovement _Movement;
    GameObject _enemieGrabbed = default;

    private void Awake()
    {
        _Movement = GetComponent<PlayerMovement>();
    }


    public void Grab() {
        
        if (_isGrabbing)
        {
            ThrowWhatIsGrabbed();
        }
        else
        {
            if (Time.time > _timeSinceGrabTry + _grabCoolDown)
            {
                _timeSinceGrabTry = Time.time;
               StartCoroutine( TryGrabWhatCanBeThrowned() );
            }
            
        }
    }

    public void DoGrab(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
                Grab();
        }
    }

    void ThrowWhatIsGrabbed()
    {
        Vector3 grabOrientation = _Movement.IsFacingRight ? Vector2.right : Vector2.left;
        //_enemieGrabbed.transform
    }

    IEnumerator TryGrabWhatCanBeThrowned()
    {
        Vector3 grabOrientation = _Movement.IsFacingRight ? Vector2.right : Vector2.left;
        float t = 0, elapsedTime=0;
        
        while (t <= 1)
        { 
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _grabTryDuration;
            _rootHook.position = Vector3.Lerp(transform.position,
                                    transform.position + grabOrientation * _grabDistance,
                                    t);
            if (!_isGrabbing)
            {
                Collider2D col = Physics2D.OverlapCircle(_rootHook.position, 2, _layerEnemies);
                if (col != null)
                {
                    col.transform.parent = _rootHook.transform;
                    _isGrabbing = true;
                    _enemieGrabbed = col.gameObject;
                    //col.transform.localScale *= 0.85f;
                    col.enabled = false;
                }
            }                
            yield return null;
        }
         elapsedTime = t = 0;
        while (t <= 1)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _grabTryDuration;
            _rootHook.position = Vector3.Lerp(transform.position,
                                    transform.position + grabOrientation * _grabDistance,
                                    1-t);
            yield return null;
        }

        print("tryingGrab");
    }
}
