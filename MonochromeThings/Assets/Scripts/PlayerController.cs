using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 moveDir;

    [Header("플레이어 이동 스탯")]
    [SerializeField]private float speed = 3;
    private GameObject playerCam;
    [SerializeField] private float rotSpeed = 200f;
    private GroundDetector groundDetector;
    [SerializeField] private float JumpForce = 10.0f;

    private Rigidbody rb;

    float mx, my = 0;

    [Header("공격 관련 스탯")]
    [SerializeField]
    public GameObject bullet;
    public float bulletSpeed = 0;
    public Transform bulletPoint;
    [SerializeField] float shootCoolDown = 0.3f;

    float shootTimer; // 무기구현 시 코드 이전할 것

    public GameObject pannel;

    private bool isDashed = false;


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

        //Jump

        if (groundDetector.IsDeteted == true &&
            Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }


        //shoot
        if (Input.GetMouseButton(0))
        {
            if (shootTimer <= 0)
            {
                shootTimer = shootCoolDown;

                Quaternion axis = Camera.main.transform.rotation;
                Vector3 shootDir = Vector3.forward;

                GameObject go = GameObject.Instantiate(bullet);
                go.transform.position = bulletPoint.position;
                go.transform.rotation = axis;
                go.GetComponent<Bullet>().Shoot(shootDir,20.0f);   
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pannel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }


        shootTimer -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

        }


    }


    public void ContinueGame(GameObject obj)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        obj.SetActive(false);

    }


}
