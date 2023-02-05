using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 * Pool para spawnear efectos y otras cosas
 */
public class ObjPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string _etiqueta;
        public GameObject _prefab;
        public int _cantidadInicial;
        [Tooltip("Marca si el Manager instanciara mas objetos cuando exceda la cantidad maxima")]
        public bool _debeCrecer = false;
        [Tooltip("Marca si el Pool reutilizara objetos aunque esten activos")]
        public bool _debeReutilizar = true;
    }

    #region Singleton
    public static ObjPooler _instance;

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public List<Pool> _pools;
    public Dictionary<string, Queue<GameObject>> _diccionarioPool;

    private void Start()
    {

        _diccionarioPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool p in _pools)
        {
            Queue<GameObject> poolDeObjeto = new Queue<GameObject>();

            for (int i = 0; i < p._cantidadInicial; i++)
            {
                GameObject objectToPool = Instantiate(p._prefab);
                objectToPool.SetActive(false);
                poolDeObjeto.Enqueue(objectToPool);
            }

            _diccionarioPool.Add(p._etiqueta, poolDeObjeto);
        }
    }

    public GameObject Spawn(string etiqueta, Vector3 position, Quaternion rotation)
    {
        if (!_diccionarioPool.ContainsKey(etiqueta))
        {
            Debug.LogWarning("El pool " + etiqueta + " no existe.");
            return null;
        }

        Pool currentPool = _pools.Find(p => p._etiqueta == etiqueta);//obtengo el pool actual

        if (currentPool._debeReutilizar)
        {
            //Si debe reutilizar me debe de valer y solo saco del queue lo que siga y lo activo
            GameObject spawneable = _diccionarioPool[etiqueta].Dequeue();

            spawneable.SetActive(true);
            spawneable.transform.position = position;
            spawneable.transform.rotation = rotation;

            _diccionarioPool[etiqueta].Enqueue(spawneable); //Lo volvemos a meter en el queue
            return spawneable;
        }
        else
        {
            //sino checo si debe crecer
            if (currentPool._debeCrecer)
            {
                GameObject spawneable = null;
                foreach (GameObject obj in _diccionarioPool[etiqueta])//checo si hay alguno apagado
                {
                    if (!obj.activeSelf)
                        spawneable = obj;
                }

                if (spawneable == null) //si no hay objeto hacemos uno nuevo, lo colocamos y lo metemos al queue
                {
                    spawneable = Instantiate(currentPool._prefab);
                    spawneable.SetActive(true);
                    spawneable.transform.position = position;
                    spawneable.transform.rotation = rotation;

                    _diccionarioPool[etiqueta].Enqueue(spawneable);
                    return spawneable;
                }
                else
                {
                    spawneable.SetActive(true);
                    spawneable.transform.position = position;
                    spawneable.transform.rotation = rotation;

                    _diccionarioPool[etiqueta].Enqueue(spawneable); //Lo volvemos a meter en el queue
                    return spawneable;
                }
            }
            //si no debe crecer y no hay objetos inactivos pues no hacemos ni mais
            return null;
        }

    } //usa este metodo en vez de instantiate

}