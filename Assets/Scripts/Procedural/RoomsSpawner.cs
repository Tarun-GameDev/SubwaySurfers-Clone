using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsSpawner : MonoBehaviour
{
    public static RoomsSpawner instance;

    [SerializeField] GameObject staringRoom;
    [SerializeField] GameObject[] roomsPrefab;
    [SerializeField] int noOfRooms = 5;
    [SerializeField] Vector3 nextRoomSpawnPos = new Vector3(0,0,60);

    public int touchedTriggers = 0;

    /*
    #region coins Dictionary Setup
    [System.Serializable]
    public class coinPool
    {
        public int id;
        public Transform prefab;
    }

    public List<coinPool> coinPools;
    public Dictionary<int, Queue<Transform>> coinsDict;
    [SerializeField]
    int id = 0;
    #endregion
    */


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        //coinsDict = new Dictionary<int, Queue<Transform>>();

        for (int i = 0; i < noOfRooms; i++)
        {
            SpawnRoom();
        }
    }

    public void DeleteAndSpawn()
    {
        touchedTriggers ++;

        if(touchedTriggers >= 3)
        {
            DeleteRoom();

            //add new spawn
            SpawnRoom();

            touchedTriggers--;
        }

    }

    void SpawnRoom()
    {
        GameObject room = Instantiate(roomsPrefab[Random.Range(0, roomsPrefab.Length)], nextRoomSpawnPos, Quaternion.identity);
        //room.GetComponent<roomIdScript>().roomId = id; // room id for coins
        room.transform.parent = transform;
        Vector3 previousRoomPos = room.transform.position;
        nextRoomSpawnPos = room.transform.GetChild(0).transform.position;

        //id++;
    }


    void DeleteRoom()
    {
        Transform firstRoom = transform.GetChild(0);
        roomIdScript _roomidscript = firstRoom.GetComponent<roomIdScript>();
        _roomidscript.dequeueAllCoins();
        //coinsDict.Remove(_roomidscript.roomId);

        //delete first child
        Destroy(firstRoom.gameObject,4f);
    }

    public void GameRestart()
    {
        for(var i = transform.childCount - 1;i >= 0;i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        nextRoomSpawnPos = new Vector3(0, 0, 0);
        for (int i = 0; i < noOfRooms; i++)
        {
            SpawnRoom();
        }
        if (staringRoom != null)
            staringRoom.SetActive(true);
    }

}
