using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState { 
    idle,
    crouch,
    prone}

    public Transform idle_cam_pos;
    public Transform crouch_cam_pos;

    public static PlayerState playerState = PlayerState.idle;

    [Header("Stamina")]
    public float staminaDecreaseSpeed = 5;
    public float staminaIncreaseSpeed = 5;

    public static bool isMoving = false;

    [Header("Ground Checker")]
    public LayerMask groundLayer;
    public static bool landed = false;
    public static bool canGetUp = true;
    public Transform groundChecker;
    public Transform crouch_to_idle_checker;
    public float groundCheckerR = 0.05f;
    public float crouchCheckerR = 0.05f;

    [Header("Camera Objects")]
    public Transform fps_view;
    public Transform fps_camera;
    public static float defaultMouseSence = 1;
    public static float mouseSence = 1;

    [Header("RigidBody component")]
    public Rigidbody rigidbody;

    [Header("Speed vars")]
    public static float speed = 150;
    public static float speedMultiplier = 1;
    public float runSpeed = 300;
    public float walkSpeed = 150;
    public float jumpForce;

    public float downForce = 200;

    float av, ah;
    float mx, my;

    public float mouseSensitivity = 500;

    float xRotation = 0, yRotation = 0;

    float moveTime = 0;

    public static bool isRunning = false;

    public float timeToJump = 0.2f;
    bool keyUp = false;

    public float runningTime = 0.75f;

    public CapsuleCollider idleColider;
    public CapsuleCollider crouchColider;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        runningTime = Mathf.Clamp(runningTime, 0, 0.75f);

        landed = Physics.CheckSphere(groundChecker.position, groundCheckerR, groundLayer);
        canGetUp = !Physics.CheckSphere(crouch_to_idle_checker.position, crouchCheckerR, groundLayer);

        av = Input.GetAxisRaw("Vertical");
        ah = Input.GetAxisRaw("Horizontal");

        mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= my*mouseSence;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        yRotation += mx*mouseSence;

        fps_camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        fps_view.localRotation = Quaternion.Euler(0, yRotation, 0);

        PlayerStats.stamina = Mathf.Clamp(PlayerStats.stamina, 0, 100);

        if (isMoving)
        {
            moveTime += Time.deltaTime;
        }
        else
        {
            moveTime = 0;
            speed = walkSpeed;
            PlayerStats.stamina += (Time.deltaTime * staminaDecreaseSpeed);
        }

        if (Input.GetButtonDown("Jump") && PlayerStats.stamina >= 20 && playerState == PlayerState.idle)
        {
            if (keyUp)
            {
                if (landed)
                    Jump();
            }
            else if (moveTime >= 0.5f)
            {
                if (!ShootingScript.isAiming)
                {
                    isRunning = true;
                    speed = runSpeed;
                    PlayerStats.stamina -= (Time.deltaTime * staminaDecreaseSpeed);
                }
                
            }
        }
        if (Input.GetButtonUp("Jump") && playerState == PlayerState.idle)
        {
            if (runningTime < 0.01)
                keyUp = true;

            isRunning = false;
            speed = walkSpeed;
            PlayerStats.stamina += (Time.deltaTime * staminaDecreaseSpeed);
        }

        if(keyUp && timeToJump >= 0.01f)
        {
            timeToJump -= Time.deltaTime;
        }

        if(timeToJump < 0.01f)
        {
            keyUp = false;
            timeToJump = 0.2f;
        }

        if(PlayerStats.stamina < 0.1f)
        {
            isRunning = false;
            speed = walkSpeed;
        }

        if (!landed)
            isRunning = false;

        if (isRunning)
        {
            if(runningTime > 0)
            runningTime -= Time.deltaTime;
            PlayerStats.stamina -= (Time.deltaTime * staminaDecreaseSpeed);
        }
        else
        {
            runningTime = 0.75f;
            PlayerStats.stamina += (Time.deltaTime * staminaDecreaseSpeed);
        }

        if(Input.GetKeyDown(KeyCode.C) && landed)
        {
            isRunning = false;
            speed = walkSpeed;

            if(playerState != PlayerState.crouch)
            {
                StartCoroutine(Crouch());
            }
            else
            {
                if (canGetUp)
                {
                    StartCoroutine(Uncrouch());
                }
            }
        }

    }

    private void FixedUpdate()
    {
        
        if (playerState == PlayerState.idle || playerState == PlayerState.crouch)
        {
            Move();
        }
    }

    private void Move()
    {
        if (landed && (ah != 0 || av != 0))
        {
            isMoving = true;
            Vector3 direction = fps_view.forward * av + fps_view.right * ah;
            
            direction.Normalize();

            rigidbody.velocity = new Vector3(direction.x * speed * speedMultiplier * Time.deltaTime, (rigidbody.velocity.y - downForce) * Time.deltaTime, direction.z * speed * speedMultiplier * Time.deltaTime);
       
        }else if (landed && (ah == 0 && av == 0))
        {
            isMoving = false;
            Vector3 direction = fps_view.forward * av + fps_view.right * ah;

            direction.Normalize();

            rigidbody.velocity = new Vector3(rigidbody.velocity.x/(1.5f), (rigidbody.velocity.y - 0) * Time.deltaTime, rigidbody.velocity.z/1.5f);

        }
    }

    public void Jump()
    {
        keyUp = false;
        rigidbody.AddForce(0, jumpForce, 0);
        if (PlayerStats.stamina > 50)
        {
            PlayerStats.stamina -= 50;
        }
        else
            PlayerStats.stamina = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerR);
        Gizmos.DrawWireSphere(crouch_to_idle_checker.position, crouchCheckerR);
    }

    public IEnumerator Crouch()
    {
        playerState = PlayerState.crouch;

        crouchColider.isTrigger = false;
        idleColider.isTrigger = true;

        while(Mathf.Abs(crouch_cam_pos.localPosition.y - fps_camera.localPosition.y) > 0.01f && playerState == PlayerState.crouch)
        {
            fps_camera.localPosition = Vector3.Lerp(fps_camera.localPosition, crouch_cam_pos.localPosition, 11 * Time.deltaTime);

            yield return null;
        }

    }
    public IEnumerator Uncrouch()
    {
        playerState = PlayerState.idle;

        idleColider.isTrigger = false;
        crouchColider.isTrigger = true;

        while (Mathf.Abs(idle_cam_pos.localPosition.y - fps_camera.localPosition.y) > 0.01f && playerState == PlayerState.idle)
        {
            fps_camera.localPosition = Vector3.Lerp(fps_camera.localPosition, idle_cam_pos.localPosition, 11 * Time.deltaTime);

            yield return null;
        }
    }
}
