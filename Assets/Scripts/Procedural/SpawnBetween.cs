using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBetween : MonoBehaviour
{
    public GameObject[] obstuclesArray;
    public int startsFromPos = 0;
    public int limit = 1;
    [InspectorName("If Random Nedded")]
    public int maxLimit = 1;

    [HideInInspector]
    public int trainsLength;

    void Start()
    {
        for (int i = 0; i < Random.Range(limit,maxLimit + 1); i++)
        {
            trainsLength = Random.Range(0, obstuclesArray.Length);
            GameObject obstucle = Instantiate(obstuclesArray[trainsLength], transform.position, Quaternion.identity);
            obstucle.transform.parent = transform;
            obstucle.transform.localPosition = new Vector3(0, 0, (i * 10) + startsFromPos);
        }
    }

}
