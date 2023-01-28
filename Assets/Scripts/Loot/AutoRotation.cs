using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    # region Rotate

    [SerializeField] float speed = 360f;
    [SerializeField] Vector3 rotateDirection;

    # endregion

    # region Base

    void Update()
    {
        transform.Rotate(rotateDirection * Time.deltaTime * speed);
    }

    # endregion
}
