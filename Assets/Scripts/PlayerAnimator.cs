using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Player player;

    private const string IS_WALKING = "IsWalking";
    private Animator animator;

    private void Start()
    {
        if (!player.IsOwner)
            return;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!player.IsOwner)
            return;

        animator.SetBool(IS_WALKING, player.IsWalking);
    }
}
