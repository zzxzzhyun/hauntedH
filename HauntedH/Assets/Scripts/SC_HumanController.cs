using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
[RequireComponent(typeof(CharacterController))]

public class SC_HumanController : MonoBehaviour
{
    public static SC_HumanController Instance;

    public Rigidbody RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NicknameText;
    public Animator animator;

    public float walkingSpeed = 90.0f;
    public Camera playerCamera;
    public float lookSpeed = 0.03f;
    public float lookXLimit = 20.0f;
    public int heartRate = 80;
    public int numCorn = 0;
    public int stamina = 100;
    private Vector3 moveDirection;

    public TextMeshProUGUI bpmtext;
    InventoryManager invenManager;
    CharacterController characterController;
    private Transform tr;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
    public bool seeGhost = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PV.IsMine)
        {
            GameObject.FindGameObjectWithTag("humancam").SetActive(true);
            playerCamera = GameObject.FindGameObjectWithTag("humancam").GetComponent<Camera>();
        }
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        invenManager = GetComponentInChildren<InventoryManager>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            float translation = Input.GetAxis("Vertical");
            float rotation = Input.GetAxis("Horizontal");
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = (walkingSpeed) * (SC_MobileControls.instance.GetJoystick("JoystickLeft").y);
            float curSpeedY = (walkingSpeed) * (SC_MobileControls.instance.GetJoystick("JoystickLeft").x);

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            moveDirection.y = 0;

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
            rotationX += -(SC_MobileControls.instance.GetJoystick("JoystickRight").y) * lookSpeed;
#else
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
#endif

            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
            transform.rotation *= Quaternion.Euler(0, SC_MobileControls.instance.GetJoystick("JoystickRight").x * lookSpeed, 0);
#else
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
#endif

            // Set animation
            animator.SetFloat("speed", moveDirection.magnitude);
            animator.SetFloat("stamina", stamina);
            if(moveDirection.magnitude > 50)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
            else if(moveDirection.magnitude <=50 && moveDirection.magnitude>=20)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }
    }


    public void IncreaseBPM()
    {
        // calculate distance between human and phantom
        //heartRate += value;
        bpmtext.text = $"BPM: {heartRate}";
    }

    public void DetectPhantom()
    {
        seeGhost = !seeGhost;

        if (seeGhost)
        {
            playerCamera.cullingMask = 1 << LayerMask.NameToLayer("Phantom");
            playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ground");
            playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ground");

        }
        else
        {
            playerCamera.cullingMask = -1;
            playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Phantom"));
            playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PhantomItem"));

        }

    }

    public void ShowFlashlight()
    {

    }

    public void MakeKey()
    {
        numCorn += 1;
        if (numCorn == 4)
        {

        }



    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
        }
        else
        {
            moveDirection = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

}