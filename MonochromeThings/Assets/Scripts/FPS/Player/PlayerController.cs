using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ClassTemp;

namespace FPS
{
   
    public class PlayerController : MonoBehaviour
    {
        Vector3 moveDir;
        float originFov;
        PlayerFSMEnum playerState;

        [Header("플레이어 이동 스탯")]
        [SerializeField] private float originSpeed = 3;
        [SerializeField] private float JumpForce = 10.0f;
        private GroundDetector groundDetector;
        private float speed;

        private float horizontalInput;
        private float verticalInput;

        [Header("Dash Status")]
        [SerializeField] private float dashCoolDown = 1.0f;
        [SerializeField] private float dashTime = 0.75f;
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
        public KeyCode[] WeaponKeys = new KeyCode[2] { KeyCode.Alpha1, KeyCode.Alpha2 };



        [Header("Others")]
        public TMPro.TextMeshProUGUI SpeedIndicater;

        [Header("State_Nodes")]
        public State_Node now_Node;


        private WallJumpDetector wallJumper;

        private void Awake()
        {
            
        }

        private void Start()
        {
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
            speed = originSpeed;

            playerState = PlayerFSMEnum.IDLE;

            

            wallJumper = GameObject.FindObjectOfType<WallJumpDetector>();

            now_Node = FSM_Booter.BootFSM();

        }

        private void FixedUpdate()
        {
            //transform.Translate(moveDir * Time.fixedDeltaTime, Space.Self);
            MovePlayer();
        }

        private void MovePlayer()
        {
            CalcMoveDir();
            rb.AddForce(moveDir.normalized * speed *10f, ForceMode.Force);
        }

        private Vector3 CalcMoveDir()
        {

            moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
            //moveDir = Quaternion.Euler(0, PlayerCamera.Instance.get_mx, 0) * moveDir;

            return moveDir;
        }



        // Update is called once per frame
        void Update()
        {
            //move
            SpeedControl();

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            //플레이어 돌리기
            transform.localEulerAngles = new Vector3(0, PlayerCamera.Instance.get_mx, 0);

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

            float nowSpeed = new Vector3(rb.velocity.x, 0 ,rb.velocity.z).magnitude;
            SpeedIndicater.SetText(((int)nowSpeed).ToString() + " : " + speed.ToString());
        }


        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude >speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                
            }


        }
        //

        private void Dash()
        {
            if (Input.GetKeyDown(DashKey) && dashTimer <= 0)
            {
                dashTimer = dashCoolDown;
                

                PrepareDash();
                PlayerCamera.Instance.playerCam.DOFieldOfView(65, 0.25f);

                Vector3 DashForce =  CalcMoveDir();
                DashForce = DashForce.normalized;
                speed = dashSpeed;
                rb.AddForce(DashForce * 20f, ForceMode.Impulse);

                Invoke(nameof(ResetDash), dashTime);
            }
        }
        private void PrepareDash()
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
            playerState = PlayerFSMEnum.DASH; 
        }
        private void ResetDash()
        {
            PlayerCamera.Instance.ResetFov();
            speed = originSpeed;
            playerState = PlayerFSMEnum.IDLE;
        }
        private void Jump()
        {
            if (Input.GetKeyDown(JumpKey))
            {
                //normal jump
                if (groundDetector.DetectGround())
                {
                    rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                }
                else if (wallJumper.IsWallExist())
                {
                    Debug.Log("Walljump !");
                    wallJumper.WallJump();
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
                Ray tRay = new Ray(PlayerCamera.Instance.transform.position, PlayerCamera.Instance.transform.forward);
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

            weaponGameObjs[selectedWeapon].transform.SetParent(PlayerCamera.Instance.hand);
            weaponGameObjs[selectedWeapon].transform.localPosition = Vector3.zero;
            weapons[selectedWeapon].Equip();
        }

        private void UpdateTimer()
        {
            if (dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
            }
        }
        
        

    }

}