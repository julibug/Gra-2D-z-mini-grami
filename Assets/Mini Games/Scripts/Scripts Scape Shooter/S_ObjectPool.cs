using UnityEngine;
using System.Collections.Generic;

/**
 * Author: Aleksandra Rusek
 * 
 * Manages an object pool for a specific prefab.
 */
public class S_ObjectPool
{

    private GameObject prefab; /* The prefab to be used for creating instances in the pool. */
    private List<GameObject> pool; /* The list containing instances of the prefab. */
    private GameObject poolParent; /* The parent object that holds all instances in the hierarchy. */

    /**
     * Constructor for S_ObjectPool.
     * @param prefab The prefab used to create instances in the pool.
     * @param initialSize The initial size of the object pool.
     * @param poolParentName The name of the parent object in the hierarchy.
     */
    public S_ObjectPool(GameObject prefab, int initialSize, string poolParentName)
    {
        this.prefab = prefab;
        poolParent = new GameObject(poolParentName);
        poolParent.transform.position = Vector3.zero;
        poolParent.transform.rotation = Quaternion.identity;
        this.pool = new List<GameObject>();
        for (int i = 0; i < initialSize; i++)
        {
            AllocateInstance();
        }
    }

    /**
     * Retrieves an inactive instance from the object pool.
     * If the pool is empty, a new instance is allocated.
     * @return The retrieved or newly allocated instance.
     */
    public GameObject GetInstance()
    {
        if (pool.Count == 0)
        {
            AllocateInstance();
        }

        int lastIndex = pool.Count - 1;
        GameObject instance = pool[lastIndex];
        pool.RemoveAt(lastIndex);

        instance.SetActive(true);
        return instance;
    }

    /**
     * Returns an instance to the object pool, deactivating it.
     * @param instance The instance to be returned to the pool.
     */
    public void ReturnInstance(GameObject instance)
    {
        instance.SetActive(false);
        pool.Add(instance);
    }

    /**
     * Allocates a new instance of the prefab and adds it to the pool.
     * @return The newly allocated instance.
     */
    protected virtual GameObject AllocateInstance()
    {
        if (poolParent == null)
            return null;

        GameObject instance = (GameObject)GameObject.Instantiate(prefab, poolParent.transform);
        instance.SetActive(false);
        pool.Add(instance);

        return instance;
    }

}
