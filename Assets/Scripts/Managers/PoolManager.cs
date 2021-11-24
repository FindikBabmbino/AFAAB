using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //This will handle pooling of all objects to pool different specific objects we have to create a specific queue for them
    public static PoolManager instance;
    [SerializeField] private Queue<GameObject> objectPool=new Queue<GameObject>();
    //These will determine the size of the pool
    [SerializeField] private int sizeOfPool;
    //These will hold the prefabs
    [SerializeField] private GameObject prefabGameObjectPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (prefabGameObjectPool != null)
        {
            InitialisePool(objectPool, sizeOfPool, prefabGameObjectPool);
            Debug.Log("size of pool" + objectPool.Count);
        }
    }

    private void InitialisePool(Queue<GameObject> gameObjects, int poolSize,GameObject gameObjectToInstantiate)
    {
        for (int i = 0; i < sizeOfPool; i++)
        {
            GameObject newObj = Instantiate(gameObjectToInstantiate);
            newObj.SetActive(false);
            objectPool.Enqueue(newObj);
        }
    }
    //When spawning something use this
    public GameObject GetFromPool(Queue<GameObject> gameObjectsPool,Vector3 pos,Quaternion rot,GameObject prefabToLazy)
    {
        //Do a lazy instantiation
        if (gameObjectsPool.Count <= 0)
        {
            GameObject addToQueue = Instantiate(prefabToLazy);
            addToQueue.SetActive(false);
            gameObjectsPool.Enqueue(addToQueue);
        }
        GameObject objectToSpawn = gameObjectsPool.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rot;
        return objectToSpawn;
    }

    //This will return the instantiated object back to the queue
    public void ReturnToPool(Queue<GameObject> gameObjectsPool,GameObject go)
    {
        go.SetActive(false);
        gameObjectsPool.Enqueue(go);
    }

    //These will return the queues and prefab objects so we can call the functions from other scripts
    public Queue<GameObject> ReturnDefaultQueue()
    {
        return objectPool;
    }

    public GameObject ReturnDefaultPrefabObject()
    {
        return prefabGameObjectPool;
    }
}