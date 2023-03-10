using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float coinCollecting;
    public bool androidContro = false;
    public bool tiltControllers = false;
    
    //UI Settings
    public bool postprocessToggle = true;
    public bool tiltControllersToggle = false;

    [Header("Power Ups-Settings")]
    [SerializeField]
    ScriptableRendererFeature hiddenPlayerFeature;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);

        AudioManager.instance.Play("theme");
        hiddenPlayerFeature.SetActive(false);
    }
}
