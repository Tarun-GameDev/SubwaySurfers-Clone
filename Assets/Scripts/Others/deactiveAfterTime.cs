using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactiveAfterTime : MonoBehaviour
{
    [SerializeField]
    float deactiveTime = 20f;

    void Awake()
    {
        Invoke("DeactiveObj", deactiveTime);
    }

    private void OnEnable()
    {
        Invoke("DeactiveObj", deactiveTime);
    }

    void DeactiveObj()
    {
        gameObject.SetActive(false);
    }

}
