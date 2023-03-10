using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longJumpPowerUp : MonoBehaviour
{
    [SerializeField]
    float powerUpTime = 10f;

    public float timer = 10f;
    bool pickedUp = false;

    /*shoes*/
    [SerializeField]
    GameObject shoesRende;


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
            if(uiSlider != null)
                uiSlider.SetTime(timer);
        }
        else 
            return;

        if(timer <= 0f)
        {
            disablePowerUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !pickedUp)
        {
            AudioManager.instance.Play("PowerUp");
            slider = Instantiate(uiSliderPrefab.gameObject,UIManager.instance.powerUpUIPar).gameObject;
            uiSlider = slider.GetComponent<powerUpSlider>();
            if (uiSprite != null)
                uiSlider.SetHandleImage(uiSprite);
            pickedUp = true;
            PlayerController.instance.LongJumpCtrl(true);
            PlayerController.instance.powerUpParEffe.Play();
            timer = powerUpTime;
            uiSlider.MaxTime(powerUpTime);
            shoesRende.SetActive(false);
        }
    }

    void disablePowerUp()
    {
        PlayerController.instance.LongJumpCtrl(false);
        Destroy(slider);
        pickedUp = false;
        Destroy(gameObject);
    }
    
}
