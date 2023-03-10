using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerUp : MonoBehaviour
{
    [SerializeField]
    bool noRestrict = false;

    [SerializeField]
    GameObject[] powerUps;

    private void Start()
    {
        if(!noRestrict)
        {
            if (!PlayerController.instance.powerUpsInActive && !PlayerController.instance.powerUpSpawned)
            {
                GameObject _power = Instantiate(powerUps[Random.Range(0, powerUps.Length)]);
                _power.transform.position = this.transform.position;
                PlayerController.instance.ResetPowerUpSpawn();
            }
        }
        else
        {
            GameObject _power = Instantiate(powerUps[Random.Range(0, powerUps.Length)]);
            _power.transform.position = this.transform.position;
            PlayerController.instance.ResetPowerUpSpawn();
        }            
    }
}