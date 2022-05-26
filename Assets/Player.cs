using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Controls controls;
    Rigidbody2D rb;
    Animator anim;
    public GameObject pause;

    bool isMoving = false;

    Vector2 rawMoveDir;
    Vector2 moveDir;
    Vector2 velocity;

    float currentSpeed;
    public float accel;
    public float deaccel;
    public float maxSpeed;
    public Transform wallCheckPoint;
    public Transform wallCheckPointLeft;
    public Vector2 wallCheckSize;
    public LayerMask wallCheckLayerMask;

    public bool isJumping = false;
    public bool isFalling = false;
    public bool isGrounded = false;
    public float jumpRate;
    public float maxYVel;
    public float jumpGrav;
    public float fallGrav;
    public Transform headCheckPoint;
    public Transform groundedCheckPoint;
    public float groundedCheckRadius;
    public LayerMask headCheckLayerMask;
    public LayerMask groundedCheckLayerMask;

    public AudioSource jump;

    private void Awake() {
        controls = new Controls();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += _ => StopMove();
        controls.Player.Jump.performed += _ => Jump(true);
        controls.Player.Jump.canceled += _ => Jump(false);
        controls.Player.Pause.performed += _ => Pause();

        Cursor.visible = false;
    }

    void Move(Vector2 dir) {
        isMoving = true;
        rawMoveDir = dir;
    }

    void StopMove() {
        isMoving = false;
        rawMoveDir = Vector2.zero;
    }

    void Jump(bool pressed) {
        if (pressed && isGrounded) {
            isJumping = true;
            jump.Play();
        } else {
            isJumping = false;
            isFalling = true;
        }
    }

    void Pause() {
        pause.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    private void FixedUpdate() {
        CalcMovement();
        CalcJump();

        anim.SetFloat("playerxvel", rb.velocity.x);
        anim.SetBool("isgrounded", isGrounded);
    }

    void CalcJump() {
        Collider2D coll = Physics2D.OverlapCircle(groundedCheckPoint.position, groundedCheckRadius, groundedCheckLayerMask);
        if (coll != null) {
            isGrounded = true;
            isFalling = false;
        } else {
            isGrounded = false;
        }

        Collider2D coll2 = Physics2D.OverlapCircle(headCheckPoint.position, groundedCheckRadius, headCheckLayerMask);
        if (coll2 != null) {
            isJumping = false;
        }

        if (isJumping) {
            if (rb.velocity.y < maxYVel) {
                rb.AddForce(new Vector2(0, jumpRate), ForceMode2D.Impulse);
            } else {
                isJumping = false;
            }
        }

        if (isFalling)
        {
            rb.gravityScale = fallGrav;
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                rb.gravityScale = jumpGrav;
            }
            else
            {
                rb.gravityScale = fallGrav;
            }
        }
    }

    void CalcMovement() {
        moveDir = Vector2.Lerp(moveDir, rawMoveDir, Time.deltaTime * 4f);

        velocity.y = rb.velocity.y;

        if (isMoving) {
            if (currentSpeed > maxSpeed) {
                currentSpeed = maxSpeed;
            }
            else {
                currentSpeed += accel * Time.fixedDeltaTime;
            }
        }
        else {
            if (currentSpeed > 0) {
                currentSpeed -= deaccel * Time.fixedDeltaTime;
            }
            else {
                currentSpeed = 0;
            }
        }

        if (moveDir.x > 0) {
            Collider2D coll = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0f, wallCheckLayerMask);
            if (coll != null) {
                moveDir.x = 0;
            }
        } else if (moveDir.x < 0) {
            Collider2D coll = Physics2D.OverlapBox(wallCheckPointLeft.position, wallCheckSize, 0f, wallCheckLayerMask);
            if (coll != null) {
                moveDir.x = 0;
            }
        }

        velocity.x = currentSpeed * moveDir.x;

        rb.velocity = velocity;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
