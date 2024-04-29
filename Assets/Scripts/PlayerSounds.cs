using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;
    private float footstepVolume = 1.0f;

    private void Start()
    {
        player = GetComponent<Player>();
        footstepTimer = footstepTimerMax;
    }

    private void Update()
    {
        if(player.IsWalking)
        {
            footstepTimer += Time.deltaTime;
            if(footstepTimer > footstepTimerMax)
            {
                footstepTimer = 0;
                SoundManager.Instance.PlayFootStepsSound(transform.position, footstepVolume);
            }
        }
    }
}
