using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabComponent : MonoBehaviour
{
    //public GameObject Mano;
    //GameObject enemigoAgarrado;
    //Animator AnimatorHand;
    //public Collider2D triggermano;
    //public Transform reticula;
    //SpringJoint2D miSpring;

    //Vector2 currdirection, currentpos, posiAim, lastpos;
    //float cangrabTime = 0.0f;
    //bool cangrab = true;
    //[HideInInspector]
    //public bool Isgrabbing = false;
    //private void Start()
    //{
    //    triggermano.enabled = false;
    //}
    //void GrabHandler()
    //{
    //    if (Input.GetAxis("GrabTrigg") == 1 || Input.GetAxis("Fire3") == 1)
    //    {
    //        triggermano.enabled = true;
    //        Mano.transform.position = new Vector3(reticula.position.x, reticula.position.y, Mano.transform.position.z);
    //        Mano.transform.rotation = Quaternion.LookRotation(posiAim, Vector3.forward);
    //        Mano.transform.Rotate(90, 0, 0);


    //        if (cangrab)
    //        {
    //            cangrabTime = Time.time;
    //            cangrab = false;
    //            AnimatorHand.SetBool("brazoBool", true);

    //            if (Isgrabbing)
    //            {
    //                Mano.transform.position = new Vector3(reticula.position.x, reticula.position.y, Mano.transform.position.z);
    //                Mano.transform.rotation = Quaternion.LookRotation(posiAim, Vector3.forward);
    //                Mano.transform.Rotate(90, 0, 0);
    //                cangrab = false;
    //                ThrowSomething();
    //            }
    //        }

    //    }


    //    if (!cangrab)
    //    {
    //        if (Time.time - cangrabTime > 0.2f)
    //        {
    //            triggermano.enabled = false;
    //            AnimatorHand.SetBool("brazoBool", false);
    //            cangrab = true;
    //        }
    //    }
    //}

    //public void grabbedSomething(Collider2D elagarrado)
    //{
    //    enemigoAgarrado = elagarrado.gameObject;
    //    miSpring.connectedBody = elagarrado.attachedRigidbody;
    //    Enemie enemigo = elagarrado.gameObject.GetComponent<Enemie>();
    //    enemigo.mienemimanager.disminuirenemigos();
    //    enemigo.enabled = false;
    //    //elagarrado.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    //    elagarrado.isTrigger = true;
    //    enemigoAgarrado.tag = "Escudo";
    //    miSpring.enabled = true;
    //    Isgrabbing = true;
    //}

    //public void ThrowSomething()
    //{
    //    enemigoAgarrado.tag = "Lanzado";
    //    miSpring.connectedBody = null;//spring joint suelta al objeto
    //    CircleCollider2D micircle = enemigoAgarrado.GetComponent<CircleCollider2D>();//obtenemos el collider
    //    micircle.radius = 0.5f;
    //    //micircle.isTrigger = true;  //lo convierte en trigger
    //    micircle.enabled = true;    //lo activa
    //    miSpring.enabled = false;   //apaga el spring joint
    //    Rigidbody2D enemigeishon = enemigoAgarrado.GetComponent<Rigidbody2D>();//obtenemos rigidbody del obj
    //    enemigeishon.velocity = new Vector2(0, 0); //igualamos su velocidad a 0
    //    enemigeishon.AddForce(posiAim * 1000f); //lo disparamos
    //    Isgrabbing = false; //ya no esta agarrando
    //    StartCoroutine(DestruirEnemigo(enemigoAgarrado));
    //}

    //public IEnumerator DestruirEnemigo(GameObject enemigo)
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    enemigo.SetActive(false);
    //}

    bool _isGrabbing = false;
    float _grabCoolDown = 1, _timeSinceGrabTry=0;

    [SerializeField]
    LayerMask _layerEnemies;
    [SerializeField]
    Transform _rootHook;
    [SerializeField]
    float _grabDistance = 5.0f, _grabTryDuration=0.8f;

    private PlayerMovement _Movement;

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
