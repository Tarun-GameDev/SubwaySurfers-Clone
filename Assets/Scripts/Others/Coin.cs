using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField]
    float durationTime;
    [SerializeField]
    Vector3 rotationatlAxis;
    [SerializeField]
    float smooth;
    [SerializeField]
    Renderer coinprefabRenderer;
    [SerializeField]
    ParticleSystem coinCollectParticle;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.coinCollecting++;
            UIManager.instance.coinUIIncrement();
            audioManager.Play("collectCoin");
            coinCollected();
            coinprefabRenderer.enabled = false;
            Invoke("Deactive", 1f);
        }
    }

    private void Update()
    {
        smooth = Time.deltaTime * durationTime * 200f;
        transform.Rotate(rotationatlAxis * smooth);
    }

    public void Deactive()
    {
        transform.parent = null;
        gameObject.SetActive(false);
        coinprefabRenderer.enabled = true;
    }

    void coinCollected()
    {
        //particle effect
        coinCollectParticle.Play();
    }
}


