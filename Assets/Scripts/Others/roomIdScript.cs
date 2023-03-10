using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomIdScript : MonoBehaviour
{
    public int roomId;
    public Queue<Transform> coinsTrans = new Queue<Transform>();

    private void Start()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(.2f);
        //RoomsSpawner.instance.coinsDict.Add(roomId, coinsTrans);
    }

    public void dequeueAllCoins()
    {
        foreach (Transform _coin in coinsTrans)
        {
            _coin.GetComponent<Coin>().Deactive();
        }
    }
}
