using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEntryLevelRoomSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] LevelsArray;
    [SerializeField] GameObject runningTrain;
    [SerializeField] int startsFromPos = 0;
    [SerializeField] float trainYPos = 70f;

    [SerializeField] int selectedLevel;

    void Start()
    {
        //Spawn Entry Level Room 
        selectedLevel = Random.Range(0, LevelsArray.Length);
        GameObject obstucle = Instantiate(LevelsArray[selectedLevel], transform.position, Quaternion.identity); 
        obstucle.transform.parent = transform;
        obstucle.transform.localPosition = new Vector3(0, 0,startsFromPos);

        //Spawn Moving Train
        switch (selectedLevel)
        {
            case 0:
                runningTrain.transform.localPosition = new Vector3(-8, runningTrain.transform.position.y, trainYPos);
                break;
            case 1:
                runningTrain.transform.localPosition = new Vector3(-4, runningTrain.transform.position.y, trainYPos);
                break;
            case 2:
                runningTrain.transform.localPosition = new Vector3(0, runningTrain.transform.position.y, trainYPos);
                break;
            case 3:
                runningTrain.transform.localPosition = new Vector3(4, runningTrain.transform.position.y, trainYPos);
                break;
            case 4:
                runningTrain.transform.localPosition = new Vector3(8, runningTrain.transform.position.y, trainYPos);
                break;
            case 5:
                runningTrain.transform.localPosition = new Vector3(4, runningTrain.transform.position.y, trainYPos);
                break;
            case 6:
                runningTrain.transform.localPosition = new Vector3(-8, runningTrain.transform.position.y, trainYPos);
                break;
            default:
                break;
        }
    }

}
