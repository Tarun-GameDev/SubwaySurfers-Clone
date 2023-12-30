using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using System.Linq;
//using UnityEngine.Windows.Speech;
using Random = UnityEngine.Random;

[System.Serializable]
public enum SIDE { LeftCorner = -8, Left = -4, Mid = 0, Right = 4, RightCorner = 8 }
public enum HitX { Left,Mid,Right,None}
public enum HitY { UP, Mid, Down,Low, None }
public enum HitZ { Forward, Mid, Backward, None }

public class PlayerController : MonoBehaviour
{
    // Accelertaion Settings
    [SerializeField]
    TextMeshProUGUI accelerationUIText;
    float accelerationSensitivity = .4f;
    bool inXAcceleration = false;
    bool inYAccelertaion = false;
    public bool tiltControllerson = false;

    //speech Input Settings
    //KeywordRecognizer keywordRecognizer;
    //Dictionary<string, bool> actions = new Dictionary<string, bool>();

    public float velcity;
    float jumpCounter;
    public TextMeshProUGUI diaplayTap;
    public static PlayerController instance;
    [SerializeField]
    ParticleSystem dodgeParEffe;
    public ParticleSystem powerUpParEffe;
    [SerializeField]
    ParticleSystem landingparEffe;
    [SerializeField]
    SkinnedMeshRenderer shoesMeshRende;
    public float playerSpeed = 10f;
    [SerializeField] float initialSpeed = 10f;
    public float maxSpeed = 20f;
    public float speedMultiplier = 1f;
    public SIDE m_side = SIDE.Mid;
    public bool swipeRight, swipeLeft,swipeUp,swipeDown;
    float x;
    public float doudgeSpeed = 10f;
    public float jumpForce = 15f;
    public float longJumpForce = 25f;
    float y;
    public bool inJump;
    public bool inRoll;
    float colHeight;
    float colCenterY;
    [SerializeField] CapsuleCollider other_colli;
    float othercolHeight;
    float othercolCenterY;
    SIDE lastSide;

    [SerializeField]
    Animator charanimator;

    CharacterController charContr;

    public HitX hitX = HitX.None;
    public HitY hitY = HitY.None;
    public HitZ hitZ = HitZ.None;

    //Touch Controllers
    public Vector2 startTouchPos;
    public Vector2 currentTouchPos;
    public Vector2 endTouchPos;
    public bool stopTouch = false;

    public float swipeRange;
    public float tapRange;

    public bool gameStarted = false;
    public bool playerDead = false;
    public bool longjump = false;

    bool velocityRecheck = false;

    float initialZpos = -130f;
    AudioManager audioManager;
    [SerializeField]
    LayerMask fracObsLayerMask;

    float destroyInfrontTimer = 0f;
    bool checkingInfront = false;

    /*Explode PowerUps*/
    [SerializeField]
    Transform boxTransf;
    [SerializeField]
    Vector3 scale;
    [SerializeField]
    LayerMask colliderLayerMask;
    [SerializeField]
    ParticleSystem explodeParEffe;
    public int bombsPicked = 0;
    public bool powerUpsInActive = false;
    public bool powerUpSpawned = false;
    [SerializeField]
    float powerUptimer = 10f;

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
        transform.position = new Vector3(0, 0, -130);
    }


    void Start()
    {
        gameStarted = false;
        charContr = GetComponent<CharacterController>();
        colHeight = charContr.height;
        colCenterY = charContr.center.y;
        othercolHeight = other_colli.height;
        othercolCenterY = other_colli.center.y;
        transform.position = new Vector3(0,0,initialZpos);
        audioManager = AudioManager.instance;
        shoesMeshRende.enabled = false;
        tiltControllerson = GameManager.instance.tiltControllers;

        //Speech Input
        /*
        actions.Add("right", swipeRight = true);
        actions.Add("left", swipeLeft = true);
        actions.Add("up", swipeUp = true);
        actions.Add("down", swipeDown = true);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();*/
    }

    /*
    void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
    }*/

    void SwipeRight()
    {
        swipeRight = true;
    }
    void SwipeLeft()
    {
        swipeLeft = true;
    }
    void SwipeUp()
    {
        swipeUp = true;
    }
    void SwipeDown()
    {
        swipeDown = true;
    }


    void Update()
    {
        accelerationUIText.text = "Y:" + Input.acceleration.y;


        if (playerDead || !gameStarted)
            return;

        charContr.detectCollisions = false;


        bool isgroundedcheck = charContr.isGrounded;

        playerSpeed = Mathf.Clamp(playerSpeed, 15f, maxSpeed);
        playerSpeed += speedMultiplier / 100 * Time.deltaTime;
        charanimator.SetFloat("PlayerSpeed", playerSpeed);

        MyInput();

        if (charContr.isGrounded != isgroundedcheck)
            landingparEffe.Play();

        PowerUpsUpdate();

        velcity = charContr.velocity.magnitude;
        if (velocityRecheck)
        {
            if (velcity <= 0)
            {
                StartCoroutine(PlayerCaught());
            }
        }
        else
        {
            StartCoroutine(playerVelocity());
        }

    }

    void PowerUpsUpdate()
    {
        /*for Xray Power Up*/
        if (checkingInfront)
            DestroyInfront();

        /*for Exploding PowerUp*/
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (bombsPicked >= 1)
            {
                StartExplode();
            }
        }

        /*for powerUp Spawing Delay*/
        if (powerUptimer <= 0f)
            powerUpSpawned = false;
        else
            powerUptimer -= Time.deltaTime;
    }

    public void ResetPowerUpSpawn()
    {
        powerUpSpawned = true;
        powerUptimer = 10f; // max time putting
    }

    IEnumerator playerVelocity()
    {
        yield return new WaitForSeconds(2f);
        velocityRecheck = true;
    }

    public void ToggleTiltControllers(bool _tilt)
    {
        GameManager.instance.tiltControllers = _tilt;
        tiltControllerson = _tilt;
    }    

    void MyInput()
    {

        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        swipeDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

        #region Mobile Input
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = currentTouchPos - startTouchPos;
            if(!stopTouch)
            {
                float xval = Mathf.Abs(Distance.x);
                float yval = Mathf.Abs(Distance.y);
                if (xval > yval)
                {
                    if(Distance.x > swipeRange)
                    {
                        swipeRight = true;
                        stopTouch = true;
                    }
                    else if(Distance.x < -swipeRange)
                    {
                        swipeLeft = true;
                        stopTouch = true;
                    }
                }    
                else
                {
                    if (Distance.y > swipeRange)
                    {
                        swipeUp = true;
                        stopTouch = true;
                    }
                    else if (Distance.y < -swipeRange)
                    {
                        swipeDown = true;
                        stopTouch = true;
                    }
                }
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;
            endTouchPos = Input.GetTouch(0).position;
            Vector2 distance = endTouchPos - startTouchPos;
            if(Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange)
            {
                //tapped
            }
        }


        //Tilt Controllers
        if (tiltControllerson)
        {
            if (Input.acceleration.x >= accelerationSensitivity && inXAcceleration == false)
            {
                swipeRight = true;
                inXAcceleration = true;

            }
            else if (Input.acceleration.x <= -accelerationSensitivity && inXAcceleration == false)
            {
                swipeLeft = true;
                inXAcceleration = true;
            }
            else if (Input.acceleration.x >= -accelerationSensitivity && Input.acceleration.x <= accelerationSensitivity && inXAcceleration == true)
            {
                inXAcceleration = false;
            }

            if (Input.acceleration.y >= accelerationSensitivity && inYAccelertaion == false)
            {
                swipeDown = true;
                inYAccelertaion = true;
            }
            else if (Input.acceleration.y <= -accelerationSensitivity && inYAccelertaion == false)
            {
                swipeUp = true;
                inYAccelertaion = true;
            }
            else if (Input.acceleration.y >= -accelerationSensitivity && Input.acceleration.y <= accelerationSensitivity && inYAccelertaion == true)
            {
                inYAccelertaion = false;
            }
        }

        #endregion

        //voice Input


        //swipe left and right movements
        if (swipeLeft)
        {
            randomswipeAudio();
            charanimator.Play("side_move_L");
            if(charContr.isGrounded)
                dodgeParEffe.Play();
            if (m_side == SIDE.Mid)
            {
                lastSide = m_side;
                m_side = SIDE.Left;
            }
            else if (m_side == SIDE.Right)
            {
                lastSide = m_side;
                m_side = SIDE.Mid;
            }
            else if (m_side == SIDE.RightCorner)
            {
                lastSide = m_side;
                m_side = SIDE.Right;
            }
            else if (m_side == SIDE.Left)
            {
                lastSide = m_side;
                m_side = SIDE.LeftCorner;
            }
            else
            {
                lastSide = m_side;
                //play stumble off left
            }

        }
        else if (swipeRight)
        {
            charanimator.Play("side_move_R");
            randomswipeAudio();
            if (charContr.isGrounded)
                dodgeParEffe.Play();
            if (m_side == SIDE.Mid)
            {
                lastSide = m_side;
                m_side = SIDE.Right;
            }
            else if (m_side == SIDE.Left)
            {
                lastSide = m_side;
                m_side = SIDE.Mid;
            }
            else if (m_side == SIDE.LeftCorner)
            {
                lastSide = m_side;
                m_side = SIDE.Left;
            }
            else if (m_side == SIDE.Right)
            {
                lastSide = m_side;
                m_side = SIDE.RightCorner;
            }
            else
            {
                lastSide = m_side;
                //play stumble off right
            }

        }

        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, playerSpeed * Time.deltaTime);
        x = Mathf.Lerp(x, (int)m_side, Time.deltaTime * doudgeSpeed);
        charContr.Move(moveVector);
        if (longjump)
            Jump(longJumpForce);
        else
            Jump(jumpForce);
        Roll();
    }

    void randomswipeAudio()
    {
        int rand = Random.Range(0, 1);
        switch (rand)
        {
            case 0:
                audioManager.Play("swipe01");
                break;
            case 1:
                audioManager.Play("swipe02");
                break;
            default:
                audioManager.Play("swipe01");
                break;
        }
    }

    void Jump(float _jumpForce)
    {
        jumpCounter -= Time.deltaTime;
        
        if(charContr.isGrounded)
        {
            if(charanimator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            {
                charanimator.Play("Landing");
                inJump = false;
            }
            if (swipeUp)
            {
                y = _jumpForce;
                charanimator.CrossFadeInFixedTime("Jump", 0.1f);
                inJump = true;
                jumpCounter = 0f;
                audioManager.Play("jump");
            }
        }
        else
        {
            //falling Anim //or if player is in air
            y -= 15f * 2.5f * Time.deltaTime;
            if(charContr.velocity.y < -0.1f)
            {

            }
            if(!inRoll)
                charanimator.Play("Falling");
        }        

    }

    public void LongJumpCtrl(bool _longjump)
    {
        longjump = _longjump;
        powerUpParEffe.Play();
        shoesMeshRende.enabled = _longjump;
        powerUpsInActive = _longjump;
    }
    
    public void JumpPad(float _jumpForce)
    {
        y = _jumpForce;
    }

    internal float rollCounter;
    void Roll()
    {
        rollCounter -= Time.deltaTime;
        if(rollCounter <= 0f)
        {
            rollCounter = 0f;
            charContr.height = colHeight;
            charContr.center = new Vector3(0, colCenterY, 0);
            other_colli.height = othercolHeight;
            other_colli.center = new Vector3(0, othercolCenterY, 0);
            inRoll = false;
        }
        if(swipeDown)
        {
            randomswipeAudio();
            rollCounter = 1f;
            y = -10f;
            charContr.height = colHeight / 2;
            charContr.center = new Vector3(0, colCenterY / 2, 0);
            other_colli.height = 0.6f;
            other_colli.center = new Vector3(0, -0.3f, 0);
        
            charanimator.CrossFadeInFixedTime("Roll", 0.1f);
            inRoll = true;
            inJump = false;

        }
    }

    void DestroyInfront()
    {
        if (destroyInfrontTimer >= 0f)
        {
            destroyInfrontTimer -= Time.deltaTime;
            Collider[] collider = Physics.OverlapBox(transform.position, Vector3.one * 5f, Quaternion.identity, fracObsLayerMask);

            foreach (Collider nearbyObj in collider)
            {
                Destroy(nearbyObj);
            }
        }
        else
        {
            checkingInfront = false;
        }
    }

    public void StartExplode()
    {
        explodeParEffe.Play();
        powerUpParEffe.Play();
        audioManager.Play("explosion");
        PlayerController.instance.StartDestroyInfront();
        
        //better collision
        Collider[] collider = Physics.OverlapBox(boxTransf.position, scale / 2, Quaternion.identity, colliderLayerMask);
        foreach (Collider nearbyObject in collider)
        {
            Explode _explode = nearbyObject.GetComponent<Explode>();
            if (_explode != null)
                _explode.ObstExplode();
        }

        CinemachineShake.instance.CameraShake(10f, 2f);

        bombsPicked--;
    }

    public void StartDestroyInfront()
    {
        destroyInfrontTimer = 7f;
        checkingInfront = true;
    }

    public void StartTheGame()
    {
        gameStarted = true;
        playerDead = false;
        RoomsSpawner.instance.GameRestart();
    }

    public IEnumerator PlayerCaught()
    {
        charanimator.SetTrigger("DeadByHit");
        gameStarted = false;
        playerDead = true;
        yield return new WaitForSeconds(1f);
        UIManager.instance.PlayerCaughtUI();
        transform.position = new Vector3(0, 0, initialZpos);
        playerSpeed = initialSpeed;
    }

    public void OnCharacterColliderHit(Collider col)
    {

        hitX = GetHitX(col);
        hitY = GetHitY(col);
        hitZ = GetHitZ(col);

        if(hitZ == HitZ.Forward && hitX == HitX.Mid)//Death
        {
            StartCoroutine(PlayerCaught());
            if(hitY == HitY.Low)
            {
                //play  stumble low anim
                ResetCollision();
            }
            else if(hitY == HitY.Down)
            {
                //play death lower
                ResetCollision();
            }
            else if(hitY == HitY.Mid)
            {
                if(col.tag == "Moving Train")
                {
                    //play death moving train anim
                    ResetCollision();
                }
                else if(col.tag != "Ramp")
                {
                    //play death bounce anim
                    ResetCollision();
                }

            }
            else if(hitY == HitY.UP && !inRoll)
            {
                //play death Upper anim
                ResetCollision();
            }
        }
        else if(hitZ == HitZ.Mid)
        {
            if (hitX == HitX.Right)
            {
                //play stumble side right
                m_side = lastSide;
                ResetCollision();
            }
            else if(hitX == HitX.Left)
            {
                //play stumble side left
                m_side = lastSide;
                ResetCollision();
            }
        }
        else
        {
            if (hitX == HitX.Right)
            {
                //play stumble Corner right
                ResetCollision();
            }
            else if (hitX == HitX.Left)
            {
                //play stumble corner left
                ResetCollision();
            }
        }
    }

    void ResetCollision()
    {
        hitX = HitX.None;
        hitY = HitY.None;
        hitZ = HitZ.None;

    }

    public HitX GetHitX(Collider col)
    {
        Bounds charBounds = charContr.bounds;
        Bounds col_bounds = col.bounds;
        float min_x = Mathf.Max(col_bounds.min.x, charBounds.min.x);
        float max_x = Mathf.Min(col_bounds.max.x, charBounds.max.x);
        float average = (min_x + max_x)/2f - col.bounds.min.x;

        HitX hit;
        if (average > col_bounds.size.x - 0.33f)
        {
            hit = HitX.Left;
        }
        else if (average < 0.33f)
            hit = HitX.Right;
        else
            hit = HitX.Mid;
        return hit;
    }

    public HitY GetHitY(Collider col)
    {
        Bounds charBounds = charContr.bounds;
        Bounds col_bounds = col.bounds;
        float min_y = Mathf.Max(col_bounds.min.y, charBounds.min.y);
        float max_y = Mathf.Min(col_bounds.max.y, charBounds.max.y);
        float average = ((min_y + max_y) / 2f - charBounds.min.y)/charBounds.size.y;

        HitY hit;
        if (average < 0.17f)
        {
            hit = HitY.Low;
        }
        else if (average < 0.33f)
        {
            hit = HitY.Down;
        }
        else if (average < 0.66f)
            hit = HitY.Mid;
        else
            hit = HitY.UP;
        return hit;
    }

    public HitZ GetHitZ(Collider col)
    {
        Bounds charBounds = charContr.bounds;
        Bounds col_bounds = col.bounds;
        float min_z = Mathf.Max(col_bounds.min.z, charBounds.min.z);
        float max_z = Mathf.Min(col_bounds.max.z, charBounds.max.z);
        float average = ((min_z + max_z) / 2f - charBounds.min.z) / charBounds.size.z;

        HitZ hit;
        if (average < 0.33f)
        {
            hit = HitZ.Backward;
        }
        else if (average < 0.66f)
            hit = HitZ.Mid;
        else
            hit = HitZ.Forward;
        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxTransf.position, scale);
    }

}
