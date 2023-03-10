using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrain : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float speedMultiplier = 1;
    public MovingTrainTrigger trigger;

    void Update()
    {
        moveSpeed = PlayerController.instance.playerSpeed;

        if(trigger != null)
        {
            if(trigger.MoveTrain)
            {
                transform.Translate(-Vector3.forward * moveSpeed * speedMultiplier * Time.deltaTime);
            }
        }    

    }
    
}
