using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTrainSpawner : MonoBehaviour
{
    public GameObject[] obstuclesArray;
    public GameObject trainCol;
    public Transform movTrainParentTransf;
    public Transform collParentTransf;
    public Transform trigger;
    public float speedMultiplier = 1;
    //public int trainStartsFromPos = 0;
    //public float colStartsFromPos = 0;
    public int limit = 1;
    [InspectorName("If Random Nedded")]
    public int maxLimit = 1;

    [HideInInspector]
    public int trainsLength;

    void Start()
    {
        trainsLength = Random.Range(0, obstuclesArray.Length);

        //For Visual Moving Trains
        for (int i = 0; i < Random.Range(limit, maxLimit + 1); i++)
        {
            GameObject trainsObj = Instantiate(obstuclesArray[trainsLength], transform.position, Quaternion.identity);
            trainsObj.transform.parent = movTrainParentTransf;
            trainsObj.transform.localPosition = new Vector3(0, 0, ((i * 10) + (transform.localPosition.z) - (trigger.localPosition.z))* speedMultiplier);

            //Constant COlliders Spawning
            /*
            GameObject colliders = Instantiate(trainCol, transform.position, Quaternion.identity);
            colliders.transform.parent = collParentTransf;
            colliders.transform.localPosition = new Vector3(0, 0, (i * 10));*/
        }
    }
}
