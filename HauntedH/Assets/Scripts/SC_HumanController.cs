using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(CharacterController))]

public class SC_HumanController : MonoBehaviour
{
    public static SC_HumanController Instance;

    public float walkingSpeed = 140.0f;
    public Camera playerCamera;
    public float lookSpeed = 6.0f;
    public float lookXLimit = 40.0f;
    public int heartRate = 80;
    public int numCorn = 0;
    public int stamina = 100;

    public TextMeshProUGUI bpmtext;

    InventoryManager invenManager;
    CharacterController characterController;
    Animator animator;
    Vector3 moveDirection = Vector3.zero;
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
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        invenManager = GetComponentInChildren<InventoryManager>();
    }

    void Update()
    {
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
        IncreaseBPM();
    }


    public void IncreaseBPM()
    {
        // calculate distance between human and phantom
        //heartRate += value;
        float distance = Vector3.Distance(this.transform.position, GameObject.Find("Phantom").transform.position);
        if (distance < 10)
        {
            heartRate +=1;
        }
        bpmtext.text = $"BPM: {heartRate}";
    }

    public void DetectPhantom()
    {
        seeGhost = !seeGhost;

        if (seeGhost)
        {
            playerCamera.cullingMask = 1 << LayerMask.NameToLayer("Phantom");
            playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ground");

        } else {
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
 
}