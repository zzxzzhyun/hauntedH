using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(CharacterController))]

public class PhantomController : MonoBehaviour
{
    public float walkingSpeed = 90.0f;
    public Camera playerCamera;
    public float lookSpeed = 0.03f;
    public float lookXLimit = 20.0f;

    InventoryManager invenManager;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;



    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
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
        
        invenManager.ListItems();

    }




}