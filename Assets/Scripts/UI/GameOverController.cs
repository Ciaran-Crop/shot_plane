using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] ShootUIController shootUIController;
    public Animator Animator => animator;
    [SerializeField] Animator animator;
    void SwitchToGameOverInput() => shootUIController.SwitchToGameOverInput();

}
