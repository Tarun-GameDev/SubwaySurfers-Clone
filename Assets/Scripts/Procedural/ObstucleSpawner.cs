using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstucleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstuclesPrefabs;
    [SerializeField] int roomLength = 6;

    void Awake()
    {
        for (int i = 0; i < roomLength; i++)
        {
            if (obstuclesPrefabs.Length > 0)
            {
                GameObject obstucle = Instantiate(obstuclesPrefabs[Random.Range(0, obstuclesPrefabs.Length)], transform.position, Quaternion.identity);
                obstucle.transform.parent = transform;
                obstucle.transform.localPosition = new Vector3(Random.Range(-2, 2) * 4, 0, i * 10);
            }
            else
                Debug.Log("Obstucle Prefabs not assigned");
        }

    }

}
