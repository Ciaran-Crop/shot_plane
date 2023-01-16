using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] bool isAutoDestroy;
    [SerializeField] float lifeTime = 3f;
    WaitForSeconds lifeTimeWait;
    IEnumerator AutoDestroyCoroutine()
    {
        yield return lifeTimeWait;
        if (isAutoDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Awake()
    {
        lifeTimeWait = new WaitForSeconds(lifeTime);
    }

    void OnEnable() 
    {
        StartCoroutine(nameof(AutoDestroyCoroutine));
    }

    void Start()
    {
    }
}
