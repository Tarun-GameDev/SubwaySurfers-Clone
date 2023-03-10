using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    ObjectPoller objectPoller;

    [SerializeField] roomIdScript roomidscript;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] bool attachedCoin;

    
    void Start()
    {
        Invoke("spawn", .1f);
    }

    public void spawn()
    {
        objectPoller = ObjectPoller.Instance;
        
        var _coin = objectPoller.SpawnFromPool("Coin", transform.position, Quaternion.identity);
        roomidscript.coinsTrans.Enqueue(_coin.transform);
        Coin _coinscript = _coin.GetComponent<Coin>();
        _coin.transform.SetParent(this.transform);

        /*
        if (attachedCoin)
        {
            _coin.transform.SetParent(this.transform);
            _coinscript.movableCoin = true;
        }*/
    }

}
