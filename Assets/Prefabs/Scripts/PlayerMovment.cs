using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public CharacterController controller;

    public Camera cam;

    [SerializeField]
    private float speed = 10f;

    public float gravity = -9.81f;
    private float oldGravity = 0f;
    public float jumpHight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    Vector3 velocity = Vector3.zero;
    bool isGrounded;

    Vector3 move;

    //wall run
    public bool isWallR = false;
    public bool isWallL = false;

    public Transform leftCheck;
    public Transform rightCheck;

    public LayerMask wallMask;
    public float wallDistance = 1.5f;
    public float wallDistanceSphere = 0.99f;
    private float slowGravity;

    //dash
    Vector3 Drag;
    public float DashDistance = 6f;

    [SerializeField]
    private float dashUSE = 100f;
    [SerializeField]
    private float dashMax = 1f;
    [SerializeField]
    private float dashRegen = 0.08333f;

    public float GetDashRegen()
    {
        return dashMax;
    }

    //crouch
    float crchSpeed;
    Transform tr;
    float dis;
    bool isCrouch = false;

    //network

    private PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
        oldGravity = gravity;
        slowGravity = gravity / 2;
        Drag.Set(2, 0, 2);
        crchSpeed = speed / 2;
        tr = transform;
        dis = controller.height /2;
    }

    void Update()
    {

        if (PouseMenue.isGamePoused)
        {
            return;
        }

        moveCommand();
        jumpCommand();
        wallRun();

    }

    void moveCommand()
    {
        if (isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask)) { isGrounded = true; }
        else if (isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, wallMask)) { isGrounded = true; }

        if (isGrounded && (velocity.y < 0))
        {
            velocity.y = -0.5f;
        }


        //get input
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        //move
        move = transform.right * xMove + transform.forward * zMove;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Dash") && dashMax == 1f)
        {
            dashMax -= dashUSE * Time.deltaTime;

            velocity += Vector3.Scale(transform.forward,
                                       DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime),
                                                                  (Mathf.Log(1f / (Time.deltaTime * Drag.y + 1)) / -Time.deltaTime),
                                                                  (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));
        }
        else
        {
            dashMax += dashRegen * Time.deltaTime;
        }

        dashMax = Mathf.Clamp(dashMax, 0f, 1f);

        velocity.x /= 1 + Drag.x * Time.deltaTime;
        velocity.y /= 1 + Drag.y * Time.deltaTime;
        velocity.z /= 1 + Drag.z * Time.deltaTime;

        float vScale = 1f;
        float speedCourant = speed;

        if (Input.GetButton("Croutch") && isGrounded)
        {
            vScale = 0.5f;
            speedCourant = crchSpeed;
            isCrouch = true;
        }
        else
        {
            isCrouch = false;
        }

        float ultScale = tr.localScale.y;

        Vector3 tempScale = tr.localScale;
        Vector3 tempPosition = tr.position;

        tempScale.y = Mathf.Lerp(tr.localScale.y, vScale, 5 * Time.deltaTime);
        tr.localScale = tempScale;

        tempPosition.y += dis * (tr.localScale.y - ultScale);
        tr.position = tempPosition;
    }

    int allowJump = 1;
    public int jumpCounter = 0;


    void jumpCommand()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && !isCrouch){
                velocity.y = Mathf.Sqrt(jumpHight * -2f * gravity);
                jumpCounter = 0;
            }
            if(!isGrounded && jumpCounter < allowJump && !isWallL && !isWallR)
            {
                velocity.y = Mathf.Sqrt(jumpHight * -2f * gravity);
                jumpTrue = false;
                jumpCounter++;
            }
        }

        //gravitiy
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void wallJump()
    {
        velocity.y = Mathf.Sqrt(jumpHight * -3f * gravity);
        jumpCounter = 0;

        isWallL = false;
        isWallR = false;

        //gravitiy
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public bool wallRUN = false;

    private float runTime = 5f;
    private bool jumpTrue = false;

    private void wallRun()
    {
        if (isGrounded)
        {
            jumpTrue = false;
            isWallL = false;
            isWallR = false;
            wallRUN = false;
            gravity = oldGravity;
        }

        if (!isGrounded)
        {
            wallRUN = true;

            if (Physics.Raycast(leftCheck.transform.position, leftCheck.transform.forward, wallDistance, wallMask)) { isWallL = true; }
            else if (Physics.Raycast(leftCheck.transform.position, leftCheck.transform.forward, wallDistance, groundMask)) { isWallL = true; }
            else if (Physics.CheckSphere(leftCheck.transform.position, wallDistanceSphere, wallMask)) { isWallL = true; }
            else if (Physics.CheckSphere(leftCheck.transform.position, wallDistanceSphere, groundMask)) { isWallL = true; }

            if (Physics.Raycast(rightCheck.transform.position, rightCheck.transform.forward, wallDistance, wallMask)) { isWallR = true; }
            else if (Physics.Raycast(rightCheck.transform.position, rightCheck.transform.forward, wallDistance, groundMask)) { isWallR = true; }
            else if (Physics.CheckSphere(rightCheck.transform.position, wallDistanceSphere, wallMask)) { isWallR = true; }
            else if (Physics.CheckSphere(rightCheck.transform.position, wallDistanceSphere, groundMask)) { isWallR = true; }


            if (isWallL)
            {
                gravity = slowGravity;
                StartCoroutine(afterRun());
                if (jumpTrue == false && Input.GetButtonDown("Jump"))
                {
                    wallJump();
                    jumpTrue = true;
                }
            } else if (isWallR)
            {
                gravity = slowGravity;
                StartCoroutine(afterRun());
                if (jumpTrue == false && Input.GetButtonDown("Jump"))
                {
                    wallJump();
                    jumpTrue = true;
                }
            }
        }
    }

    IEnumerator afterRun()
    {
        yield return new WaitForSeconds(runTime);
        isWallL = false;
        isWallR = false;
        wallRUN = false;
        gravity = oldGravity;
    }
}
