using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(CharacterController))]

public class PhantomController : MonoBehaviour
{
    public static PhantomController Instance;

    public float walkingSpeed = 90.0f;
    public Camera playerCamera;
    public float lookSpeed = 0.03f;
    public float lookXLimit = 20.0f;

    public Rigidbody RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NicknameText;

    InventoryManager invenManager;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    GameObject phantom;
    GameObject arm_left;
    GameObject arm_right;
    GameObject leg_left;
    GameObject leg_right;
    GameObject bat;

    [HideInInspector]
    public bool canMove = true;
    public bool canHit = false;




    private void Awake()
    {
        Instance = this;
        arm_left = GameObject.Find("arm_left");
        arm_right = GameObject.Find("arm_right");
        leg_left = GameObject.Find("leg_left");
        leg_right = GameObject.Find("leg_right");
        bat = GameObject.Find("Bat");
    }


    void Start()
    {
        if (PV.IsMine)
        {
            GameObject.FindGameObjectWithTag("phantomcam").SetActive(true);
            playerCamera = GameObject.FindGameObjectWithTag("phantomcam").GetComponent<Camera>();
        }
        characterController = GetComponent<CharacterController>();
        invenManager = GetComponentInChildren<InventoryManager>();
        phantom = GameObject.Find("Phantom");
        phantom.GetComponent<MeshRenderer>().enabled = false;

        arm_left.SetActive(false);
        arm_right.SetActive(false);
        leg_left.SetActive(false);
        leg_right.SetActive(false);
        bat.SetActive(false);

    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        if (PV.IsMine)
        {
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

        }
    }

    void ProcessCollision(GameObject human)
    {
        if (bat.GetComponent<Collider>().CompareTag("Human"))
        {
            SC_HumanController.Instance.stamina = 0;
        }
    }

    public void HitCorn()
    {
        bat.SetActive(true);
        canHit = true;
    }

    public void ShowBody(Item item)
    {
        switch (item.itemName)
        {
            case "Left Leg":
                leg_left.SetActive(true);
                break;
            case "Right Leg":
                leg_right.SetActive(true);
                break;
            case "Left Arm":
                arm_left.SetActive(true);
                break;
            case "Right Arm":
                arm_right.SetActive(true);
                break;
            case "Body":
                phantom.GetComponent<MeshRenderer>().enabled = true;
                break;
        }

    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }



}