using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeFunc : MonoBehaviour
{
    bool pickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            pickedUp = true;
            AudioManager.instance.Play("PowerUp");
            if(!GameManager.instance.androidContro)
                PlayerController.instance.bombsPicked++;
            else
                PlayerController.instance.StartExplode();
            PlayerController.instance.powerUpParEffe.Play();
            Destroy(gameObject);
        }
    }
}
