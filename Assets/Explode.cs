using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField]
    GameObject nonFracObj;
    [SerializeField]
    GameObject FracPrefab;

    bool exploded = false;

    public void ObstExplode()
    {
        if(!exploded)
        {
            if(nonFracObj != null)
                Destroy(nonFracObj);
            if(FracPrefab != null)
                Instantiate(FracPrefab, transform.position, Quaternion.identity);
            exploded = true;
        }
        
    }
}
