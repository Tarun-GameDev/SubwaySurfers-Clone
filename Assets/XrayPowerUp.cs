using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class XrayPowerUp : MonoBehaviour
{
    [SerializeField]
    float powerUpTime = 10f;

    public float timer = 10f;
    bool pickedUp = false;

    /*Xray*/
    [SerializeField]
    GameObject powerUpModel;
    [SerializeField]
    ScriptableRendererFeature hiddenFeature;

    /*ui*/
    [SerializeField]
    GameObject uiSliderPrefab;
    powerUpSlider uiSlider;
    GameObject slider;
    [SerializeField]
    Sprite uiSprite;  //Power Up SLider handler image

    private void Update()
    {
        if (pickedUp)
        {
            timer -= Time.deltaTime;
            if (uiSlider != null)
                uiSlider.SetTime(timer);
        }
        else
            return;

        if (timer <= 0f)
        {
            disablePowerUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            hiddenFeature.SetActive(true);
            AudioManager.instance.Play("PowerUp");
            slider = Instantiate(uiSliderPrefab.gameObject, UIManager.instance.powerUpUIPar).gameObject;
            uiSlider = slider.GetComponent<powerUpSlider>();
            if(uiSprite != null)
                uiSlider.SetHandleImage(uiSprite);
            pickedUp = true;
            Physics.IgnoreLayerCollision(11, 9, true);
            PlayerController.instance.powerUpParEffe.Play();
            PlayerController.instance.powerUpsInActive = true;
            timer = powerUpTime;
            uiSlider.MaxTime(powerUpTime);
            powerUpModel.SetActive(false);
        }
    }

    void disablePowerUp()
    {
        hiddenFeature.SetActive(false);
        Physics.IgnoreLayerCollision(11, 9, false);
        PlayerController.instance.powerUpsInActive = false;
        Destroy(slider);
        Destroy(gameObject);
    }
}
