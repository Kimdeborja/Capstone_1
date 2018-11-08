using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool attack;

    private bool facingRight;

    [SerializeField]
    private Transform[] GroundPoints;

    [SerializeField]
    private float GroundRadius;

    [SerializeField]
    private LayerMask WhatIsGround;

    private bool IsGrounded;

    private bool jump;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;



	void Start ()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

	}

    void Update()
    {
        HandleInput();
    }

   
    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        IsGrounded = Isgrounded();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleAttacks();

        HandleLayers();

        ResetValues();

    
	}

    private void HandleMovement(float horizontal)
    {
        if (myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("Land", true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }

        if (IsGrounded && jump)
        {
            IsGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("Jump");
        }

        myAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
    }
   
    private void HandleAttacks()
    {
        if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("Attack");
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
        }
    }   

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
        
    }
    private void ResetValues()
    {
        attack = false;
        jump = false;
    }

    private bool Isgrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in GroundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, GroundRadius, WhatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("Jumnp");
                        myAnimator.SetBool("Land", false);
                        return true;
                    }
                }

            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!IsGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}

