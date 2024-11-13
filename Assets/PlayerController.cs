using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    private bool isDashing = false;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the Player GameObject.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from the Player GameObject.");
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }

                UpdateAnimatorParameters(movementInput);
            }
            else
            {
                ResetAnimatorParameters();
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );

        if (count == 0)
        {
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnDash()
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        animator.SetBool("isDashing", true);

        rb.velocity = Vector2.right * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;
        animator.SetBool("isDashing", false);
    }

    private void UpdateAnimatorParameters(Vector2 input)
    {
        animator.SetBool("isMoving", true);

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x > 0)
            {
                SetDirection("isMovingRight", true);
                SetDirection("isMovingLeft", false);
                SetDirection("isMovingUp", false);
                SetDirection("isMovingDown", false);
            }
            else if (input.x < 0)
            {
                SetDirection("isMovingLeft", true);
                SetDirection("isMovingRight", false);
                SetDirection("isMovingUp", false);
                SetDirection("isMovingDown", false);
            }
        }
        else if (Mathf.Abs(input.y) > Mathf.Abs(input.x))
        {
            if (input.y > 0)
            {
                SetDirection("isMovingUp", true);
                SetDirection("isMovingDown", false);
                SetDirection("isMovingRight", false);
                SetDirection("isMovingLeft", false);
            }
            else if (input.y < 0)
            {
                SetDirection("isMovingDown", true);
                SetDirection("isMovingUp", false);
                SetDirection("isMovingRight", false);
                SetDirection("isMovingLeft", false);
            }
        }
        else
        {
            ResetAnimatorParameters();
        }
    }

    private void SetDirection(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    private void ResetAnimatorParameters()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isMovingRight", false);
        animator.SetBool("isMovingLeft", false);
        animator.SetBool("isMovingUp", false);
        animator.SetBool("isMovingDown", false);
        animator.SetBool("isDashing", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Game Over");
            GameObject.FindObjectOfType<GameController>().GameOver();
            gameObject.SetActive(false);
            
        }
        else if (other.CompareTag("Pumpkin"))
    {
        GameObject.FindObjectOfType<GameController>().IncrementScore();
        Destroy(other.gameObject); 
    }
    }
}
