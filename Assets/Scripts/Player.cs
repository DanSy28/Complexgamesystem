﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cam;
    public GameObject weapon;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;
    public Rigidbody rigid;



    // public void Move(inputH, inputV)
    public void Move(float inputH, float inputV, bool isJumping)
    {
        // step 1). Make a "Move" function
        // step 2). Replace some of update's logic for movement in the "Move" function
        if (controller.isGrounded)
        {
            Vector3 eular = cam.transform.eulerAngles;
            transform.rotation = Quaternion.AngleAxis(eular.y, Vector3.up);

            moveDirection = new Vector3(inputH, 0, inputV);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (isJumping)
            {
                moveDirection.y = jumpSpeed;
            }

        }

    }


    // Update is called once per frame
    void Update()
    {
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            weapon.SetActive(true);
        }
    }
}
