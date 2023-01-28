using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    [SerializeField] float speed;
    Animator animator;
    [SerializeField] AudioData pickData;
    
    GameObject player;
    Vector3 direction;
    int pickAnimationId = Animator.StringToHash("PickUp");

    void Awake()
    {
        player = EnemyManager.Instance.GetPlayer();
        animator = GetComponent<Animator>();
        direction = Vector3.left;
    }

    void OnEnable()
    {
        StartCoroutine(nameof(MoveToPlayer));
    }

    IEnumerator MoveToPlayer()
    {
        while(true)
        {
            direction = player.activeSelf ? (player.transform.position - transform.position).normalized : Vector3.left;
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }

    # region Pick Up

    void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp(collision);
    }

    protected virtual void PickUp(Collider2D collision)
    {
        StopAllCoroutines();
        animator.Play(pickAnimationId);
        AudioManager.Instance.PlayRandomPitch(pickData);
    }

    public void PickUpEnd()
    {
        gameObject.SetActive(false);
    }

    # endregion
}
