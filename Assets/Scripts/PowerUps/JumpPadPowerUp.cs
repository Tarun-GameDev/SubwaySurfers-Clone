using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadPowerUp : MonoBehaviour
{

    bool collided = false;
    [SerializeField]
    Animator animator;

    [SerializeField]
    float pushStren = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collided)
        {
            collided = true;
            AudioManager.instance.Play("bouncing");
            PlayerController.instance.JumpPad(pushStren);
            PlayerController.instance.powerUpParEffe.Play();
            animator.SetTrigger("push");
        }
    }
}
