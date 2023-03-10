using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrainTrigger : MonoBehaviour
{
    public bool MoveTrain = false;

    public bool isTriggerd = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isTriggerd == false && other.CompareTag("Player"))
        {
            MoveTrain = true;
            RoomsSpawner.instance.DeleteAndSpawn();

            isTriggerd = true;
        }
    }
}
