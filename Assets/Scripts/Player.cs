using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    // Config 
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider2D;

    // Message then methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        ClimbLadder();
        FlipSprite();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is betweeen -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
        myAnimator.SetBool("Climbing", !playerHasHorizontalSpeed);

    }

    private void Jump()
    {
        myAnimator.SetBool("Running", false);
        myAnimator.SetBool("Climbing", false);

        int layer = LayerMask.GetMask("Ground");
        bool isTouchingLayer = myCollider2D.IsTouchingLayers(layer);
        if (CrossPlatformInputManager.GetButtonDown("Jump") && isTouchingLayer)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        int layer = LayerMask.GetMask("Climbing");
        bool isTouchingLadder = myCollider2D.IsTouchingLayers(layer);
        if (!isTouchingLadder)
        {
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        myAnimator.SetBool("Running", !playerHasVerticalSpeed);

    }


    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

}
