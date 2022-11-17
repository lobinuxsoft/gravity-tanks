using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab = default;

    private Queue<GameObject> pool = default;
    
    public void SetObjectToPool(GameObject prefab)
    {
        this.prefab = prefab;
    }

    // Start is called before the first frame update
    void Awake()
    {
        pool = new Queue<GameObject>();
        //GrowPool();
    }

    /// <summary>
    /// Devuelve un objeto de la pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetFromPool(bool activated = true)
    {
        if(pool.Count <= 0)
            GrowPool();
        
        var nextObj = pool.Dequeue();
        nextObj.SetActive(activated);
        return nextObj;
    }

    /// <summary>
    /// Retorna un objeto a la pool
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    /// <summary>
    /// Resetea la pool
    /// </summary>
    public void ResetPool()
    {
        pool.Clear();
        
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ReturnToPool(transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Hace crecer la pool si fuera necesario
    /// </summary>
    private void GrowPool()
    {
        for (int i = 0; i < 1; i++)
        {
            var newObj = Instantiate(prefab, transform);
            newObj.name = prefab.name;
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }
    }

    /// <summary>
    /// Devuelve la cantidad de objetos activos en la pool
    /// </summary>
    /// <returns></returns>
    public int GetAmountActiveObjects()
    {
        int counter = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            counter = (transform.GetChild(i).gameObject.activeInHierarchy) ? counter + 1 : counter;
        }
        
        return counter;
    }
}
