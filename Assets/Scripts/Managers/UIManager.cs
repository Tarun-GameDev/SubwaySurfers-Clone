using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Score UI
    [SerializeField]
    int score = 0;
    [SerializeField]
    int coinscollected = 0;
    [SerializeField]
    int totalCoins = 0;
    [SerializeField]
    Transform playerTransform;
    Vector3 startingPos;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI coinsCollectingText;
    [SerializeField]
    int scoreMultiplyer = 10;
    [SerializeField]
    TextMeshProUGUI scoreMultiplyerText;
    [SerializeField]
    TextMeshProUGUI scoreInDeadMenu;
    [SerializeField]
    TextMeshProUGUI coinsInDeadMenu;
    [SerializeField]
    TextMeshProUGUI[] allCoinsInBankText;
    

    //Pause Menu
    [SerializeField]
    GameObject pauseUI;
    [SerializeField]
    GameObject playingMenuUI;
    [SerializeField]
    GameObject deadUI;
    [SerializeField]
    GameObject MainMenuUI;
    [SerializeField]
    GameObject settingsMenuUI;

    public Transform powerUpUIPar;
    public static bool gamePaused = false;

    /*Audio*/
    [SerializeField]
    AudioManager audioManager;
    

    //Settings
    public AudioMixer audioMixer;
    [SerializeField] GameObject postProcessObj;
    [SerializeField] Toggle postprocessToggle;
    [SerializeField] Toggle tiltControllersToggle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        #region ScoreUI
        if (playerTransform == null)
            playerTransform = PlayerController.instance.transform;

        if (playerTransform != null)
            startingPos = playerTransform.position;

        if (scoreMultiplyerText != null)
            scoreMultiplyerText.text = "x" + scoreMultiplyer;

        if (audioManager == null)
            audioManager = AudioManager.instance;
        #endregion

        #region SettingsUI
        if (postprocessToggle != null)
            postprocessToggle.isOn = GameManager.instance.postprocessToggle;
        if (tiltControllersToggle != null)
            tiltControllersToggle.isOn = GameManager.instance.tiltControllersToggle;

        #endregion

        MainMenuUI.SetActive(true);

        //All COins Display at every UI
        foreach (TextMeshProUGUI _text in allCoinsInBankText)
        {
            _text.text = PlayerPrefs.GetInt("Coins", 0).ToString("000");
        }
    }

    private void Update()
    {
        #region Score
        if (scoreText != null)
        {
            score = (int)(playerTransform.position.z - startingPos.z) * scoreMultiplyer;
            scoreText.text = "Score:" + score.ToString("000000");
        }
        #endregion

        #region PauseGame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerController.instance.playerDead == false && PlayerController.instance.gameStarted == true)
            {
                if (gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }

        }
        #endregion

    }

    public void coinUIIncrement()
    {
        coinscollected++;
        coinsCollectingText.text = coinscollected.ToString("000");
    }

    #region Pause Menu Funtions
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        audioManager.Play("UISwitch");
        pauseUI.SetActive(true);
        playingMenuUI.SetActive(false);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void ResumeGame()
    {
        audioManager.Play("UISwitch");
        pauseUI.SetActive(false);
        playingMenuUI.SetActive(true);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        settingsMenuUI.SetActive(true);
    }

    public void SettingsCancelButton()
    {
        settingsMenuUI.SetActive(false);
    }

    public void PostProcessToggle(bool postProcess)
    {
        postProcessObj.SetActive(postProcess);
        GameManager.instance.postprocessToggle = postProcess;
    }

    public void TiltControllersToggle(bool tiltControllers)
    {
        PlayerController.instance.ToggleTiltControllers(tiltControllers);
        GameManager.instance.tiltControllersToggle = tiltControllers;
    }
    #endregion


    #region Player Caught UI
    public void PlayerCaughtUI()
    {
        scoreInDeadMenu.text = score.ToString();

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + coinscollected);  // saving coins internally

        totalCoins = PlayerPrefs.GetInt("Coins",0);
        audioManager.Play("UISwitch");
        deadUI.SetActive(true);
        coinsInDeadMenu.text = coinscollected.ToString();

        //All COins Display at every UI
        foreach (TextMeshProUGUI _text in allCoinsInBankText)
        {
            _text.text = PlayerPrefs.GetInt("Coins", 0).ToString("000");
        }
    }

    public void PlayButton()
    {
        audioManager.Play("UISwitch");
        deadUI.SetActive(false);
        PlayerController.instance.StartTheGame();
    }

    public void HomeButton()
    {
        audioManager.Play("UISwitch");
        deadUI.SetActive(false);
        MainMenuUI.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region MainMenu UI
    public void MainPlayButton()
    {
        MainMenuUI.SetActive(false);
        PlayerController.instance.StartTheGame();
        playingMenuUI.SetActive(true);
    }
    #endregion

    #region Settings
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume",volume);
    }

    public  void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    #endregion

}
