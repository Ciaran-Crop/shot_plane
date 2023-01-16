using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab { get => prefab; }

    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;
    Queue<GameObject> queue;
    Transform parent;

    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>(size);
        this.parent = parent;
        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    GameObject Copy()
    {
        GameObject gameObjectClone = GameObject.Instantiate(prefab, parent);
        gameObjectClone.SetActive(false);
        return gameObjectClone;
    }

    GameObject ReturnOneObject()
    {
        GameObject gameObjectClone = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            gameObjectClone = queue.Dequeue();
        }
        else
        {
            gameObjectClone = Copy();
        }
        queue.Enqueue(gameObjectClone);
        return gameObjectClone;
    }

    public GameObject Get()
    {
        GameObject gameObjectClone = ReturnOneObject();
        gameObjectClone.SetActive(true);
        return gameObjectClone;
    }

    public GameObject Get(Vector3 position)
    {
        GameObject gameObjectClone = ReturnOneObject();
        gameObjectClone.SetActive(true);
        gameObjectClone.transform.position = position;
        return gameObjectClone;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject gameObjectClone = ReturnOneObject();
        gameObjectClone.SetActive(true);
        gameObjectClone.transform.position = position;
        gameObjectClone.transform.rotation = rotation;
        return gameObjectClone;
    }

    public GameObject Get(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject gameObjectClone = ReturnOneObject();
        gameObjectClone.SetActive(true);
        gameObjectClone.transform.position = position;
        gameObjectClone.transform.rotation = rotation;
        gameObjectClone.transform.localScale = localScale;
        return gameObjectClone;
    }

}
