using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] protected Vector2 direction;

    void OnEnable()
    {
        StartCoroutine(ProjectileMoveCoroutine());
    }
    
    IEnumerator ProjectileMoveCoroutine()
    {
        while(gameObject.activeSelf)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
