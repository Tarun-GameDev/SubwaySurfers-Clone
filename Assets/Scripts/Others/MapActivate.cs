using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivate : MonoBehaviour
{
    [SerializeField]
    GameObject[] enviroArray;

    [SerializeField]
    int randomNum;

    void Start()
    {
        randomNum = Random.Range(0, enviroArray.Length);

        for (int i = 0; i < enviroArray.Length; i++)
        {
            if(i!=randomNum)
            {
                Destroy(enviroArray[i]);
            }
            else
                enviroArray[randomNum].SetActive(true);
        }

    }
}
