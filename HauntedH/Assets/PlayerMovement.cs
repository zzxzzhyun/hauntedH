using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkingSpeed = 7.5f;
    public Camera playerCamera;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    public VariableJoystick variableJoystick;

    [HideInInspector]
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        //variableJoystick.SetMode(JoystickType.Fixed);
        characterController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        GameObject temp = GameObject.Find("Main Camera");
        playerCamera = temp.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveforward = playerCamera.transform.forward;
        Vector3 moveright = playerCamera.transform.right;

        moveforward.y = 0.0f;
        moveright.y = 0.0f;

        Vector3 direction = moveforward * variableJoystick.Vertical + moveright * variableJoystick.Horizontal;

        characterController.Move(direction * walkingSpeed);

        // 주인공 캐릭터의 애니메이션 처리
        float forward = Vector3.Dot(direction, transform.forward);
        float strafe = Vector3.Dot(direction, transform.right);

        //animator.SetFloat("Forward", forward);
        //animator.SetFloat("Strafe", strafe);
        
    }
}
