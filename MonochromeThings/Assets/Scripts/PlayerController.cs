using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    Vector3 moveDir;

    float originFov;

    [Header("플레이어 이동 스탯")]
    [SerializeField] private float speed = 3;
    private GameObject playerCam;
    [SerializeField] private float rotSpeed = 200f;
    private GroundDetector groundDetector;
    [SerializeField] private float JumpForce = 10.0f;    


    [Header("Dash Status")]
    [SerializeField] private float dashCoolDown = 1.0f;
    [SerializeField] private float dashSpeed = 2.0f;    
    private float dashTimer = 0;


    private Rigidbody rb;

    float mx, my = 0;

    [Header("공격 관련 스탯")]


    [SerializeField] GameObject[] weaponGameObjs = new GameObject[2];
    [SerializeField] Weapon[] weapons = new Weapon[2];
    [SerializeField] int selectedWeapon = 0;


    [Header("상호작용 관련 스탯")]
    [SerializeField] private float InteractiveDistance = 4.0f;
    [SerializeField] private LayerMask weaponLayer;

    Weapon tmpWeapon = new Weapon();

    public GameObject pannel;

    private bool isDashed = false;

    [Header("Key Binds")]
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode DashKey = KeyCode.LeftShift;
    public KeyCode PauseKey = KeyCode.Escape;
    public KeyCode InteractiveKey = KeyCode.F;
    public KeyCode[] WeaponKeys = new KeyCode[2]{KeyCode.Alpha1, KeyCode.Alpha2};    
    

    private void Awake()
    { 
    }


    private void Start()
    {
        if (playerCam == null)
            playerCam = Camera.main.gameObject;
        if (groundDetector == null)
            TryGetComponent<GroundDetector>(out groundDetector);

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        
        pannel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < weaponGameObjs.Length; i++)
        {
            if (weaponGameObjs[i] != null && weapons[i] == null)
            {
                weapons[i] = weaponGameObjs[i].GetComponent<Weapon>();
                weapons[i].Equip();
            }
        }
        weaponGameObjs[-selectedWeapon + 1].SetActive(false);
       

    }

    private void FixedUpdate()
    {
        transform.Translate(moveDir* Time.fixedDeltaTime,Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        //move

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDir.x = horizontal * speed;
        moveDir.z = vertical * speed;
        

        //camRotate

        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        mx +=mX* rotSpeed * Time.deltaTime;
        my +=mY* rotSpeed * Time.deltaTime;


        my = Mathf.Clamp(my, -90, 90);
        
        

        playerCam.transform.localEulerAngles = new Vector3(-my,mx,0);
        
        //플레이어 돌리기
        transform.localEulerAngles = new Vector3(0,mx,0);
        //이동 벡터 보정
        moveDir = Quaternion.Euler(0,mx,0) * moveDir;

        //Dash
        Dash();
        //Jump
        Jump();
        //shoot
        Shoot();

        //Pause
        Pause();
        // Weapon Swap
        WeaponSwap();

        // Interactive (Pick Up Weapon)
       Interactive();

        UpdateTimer();

    }
 
    private void ResetDash()
    {
        PlayerCamera.Instance.ResetFov();
    }

    //

    private void Dash()
    {
        if (Input.GetKeyDown(DashKey) && dashTimer <= 0)
        {
            dashTimer = dashCoolDown;

            Vector3 DashForce = new Vector3(moveDir.x, 0.4f, moveDir.z) * dashSpeed;

            PlayerCamera.Instance.playerCam.DOFieldOfView(65, 0.25f);

            rb.AddRelativeForce(DashForce, ForceMode.Impulse);

            Invoke(nameof(ResetDash), 0.25f);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(JumpKey))
        {
            //normal jump
            if (groundDetector.DetectGround())
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }//wall jump
            //else if ()
            {

            }

        }
    }
    private void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            weapons[selectedWeapon].Shoot();
        }
    }
    private void Pause()
    {
        if (Input.GetKeyDown(PauseKey))
        {
            pannel.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void WeaponSwap()
    {
        for (int i = 0; i < WeaponKeys.Length; i++)
        {
            if (Input.GetKeyDown(WeaponKeys[i]))
            {
                if (i != selectedWeapon)
                {
                    SwapWeapon(i);
                }
            }

        }
    }
    private void Interactive()
    {
        if (Input.GetKeyDown(InteractiveKey))
        {
            Ray tRay = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(tRay, out hitInfo, InteractiveDistance))
            {
                //Debug.Log("Interactive!");
                if (hitInfo.transform.parent != null &&
                    hitInfo.transform.parent.TryGetComponent<Weapon>(out tmpWeapon))
                {
                    //Debug.Log("Weapon!");
                    PickUpWeapon(tmpWeapon);
                    tmpWeapon = null;
                }

            }

        }
    }

    //




    public void ContinueGame(GameObject obj)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        obj.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SwapWeapon(int index)
    {
        if (index < weaponGameObjs.Length && index > -1)
        {
            if (selectedWeapon != index)
            {
                weaponGameObjs[selectedWeapon].SetActive(false);
                selectedWeapon = -selectedWeapon + 1;

                weaponGameObjs[selectedWeapon].SetActive(true);

            }

        }
    }

    public void PickUpWeapon(Weapon tWeapon)
    {
        if (weaponGameObjs[selectedWeapon] != null)
        {
            GameObject go = weaponGameObjs[selectedWeapon];
            go.transform.SetParent(null);
            go.GetComponent<Weapon>().Dequip();                        
        }

        weaponGameObjs[selectedWeapon] = tWeapon.transform.gameObject;
        weapons[selectedWeapon] = tWeapon;

        weaponGameObjs[selectedWeapon].transform.SetParent(PlayerCamera.Instance.Hand);
        weaponGameObjs[selectedWeapon].transform.localPosition = Vector3.zero;
        weapons[selectedWeapon].Equip();
    }
        
    private void UpdateTimer()
    {
        if(dashTimer > 0)
        {
            dashTimer-= Time.deltaTime;
        }
    }

}
