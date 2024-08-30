using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sekhur : PlayerUnit
{
    protected const string TAG_PLATFORM = "Platform", TAG_TRAPS = "Traps", TAG_LADDERS = "Ladders";

    public delegate void onSekhurDied();

    public event onSekhurDied doOnSekhurDied;

    private Animator anim;
    private int prevDir = 1;
    private float fallThreshold = 0.1f, baseGravity = 0;
    
    protected bool isJumping = false, isGrounded = true, isClimbing = false;

    [SerializeField]
    protected Rigidbody2D rbBody;

    [SerializeField]
    protected float jumpPower = 1;

    // Start is called before the first frame update
    void Start(){}

    protected void Awake() {
        anim = GetComponent<Animator>();
        rbBody = GetComponent<Rigidbody2D>();
        baseGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    // Restart sequences here
    public void resetPlayerStats() {
        isAlive = true;
        isControlDisabled = false;
        isJumping = false;
        isClimbing = false;
        anim.Play("idle");
    }

    // Update is called once per frame
    protected void Update()
    {
        detectInput();
    }


    // Event Listeners here
    protected void detectInput() {

        if ( isGamePaused
          || !isAlive
          || isControlDisabled
        ) return;

        // get input via GetAxis and determine direction
        // player can only move up or down while on a ladder
        float axisVector = Input.GetAxis("Horizontal");
        if (axisVector == 0) {
            xDirection = 0;
            anim.SetBool("isMoving", false);
        }
        else {
            xDirection = axisVector < 0 ? -1 : 1;
            flipSprite(xDirection);
            anim.SetBool("isMoving", true);
            moveHorizontal();
        }

        if (!isJumping && isGrounded) {
            axisVector = Input.GetAxis("Vertical");
            if (axisVector > 0 ) {
                if (!isClimbing) {
                    moveJump();
                }
                else {
                    yDirection = axisVector < 0 ? -1 : 1;
                    moveVertical();
                }
            }
        }
    }

    // Actions and Animation sequences here
    protected void moveHorizontal() {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    // move vertical when on ladders
    protected void moveVertical() {
        if (!isClimbing) return;
        transform.Translate(Vector3.up * yDirection * moveSpeed * Time.deltaTime);
    }

    protected void moveJump() {
        if (isJumping || isClimbing) return;
        rbBody.velocity = new Vector2(rbBody.velocity.x, jumpPower);
        isGrounded = false;
        isJumping = true;
    }

    protected void flipSprite(int newDir) {
        if (prevDir != newDir) {
            transform.Rotate(new Vector3(0, 180, 0));
            prevDir = newDir;
        }
    }

    // do death animation and actions here
    protected void doPlayDeath() {
        isControlDisabled = true;
        isAlive = false;
        anim.Play("death");
    }

    protected void startRestartLevelEvent() {
        if (doOnSekhurDied != null) doOnSekhurDied();
    }

    // Collision detections here
    protected void OnCollisionEnter2D(Collision2D collision) {
        checkTraps(collision);
    }

    protected void OnCollisionStay2D(Collision2D collision) {
        checkPlatforms(collision);
    }

    protected void OnTriggerStay2D(Collider2D collision) {
        checkLadders(collision, true);
    }

    protected void OnTriggerExit2D(Collider2D collision) {
        checkLadders(collision, false);
    }

    // Collision checks here
    protected void checkPlatforms(Collision2D collision) {
        if (collision.collider.CompareTag(TAG_PLATFORM)) {
            // detect which side of the platform we hit. Ideally it should just be the top
            foreach (ContactPoint2D contact in collision.contacts) {
                if (contact.normal.y > 0.5f) {
                    if (isJumping && !isGrounded) {
                        isJumping = false;
                        isGrounded = true;
                    }
                    break; // 
                }
            }
        }
    }

    protected void checkTraps(Collision2D collision) {
        if (collision.collider.CompareTag(TAG_TRAPS)) {
            // send SekhurDied event to subscribers
            doPlayDeath();
        }
    }

    protected void checkLadders(Collider2D collision, bool climb) {
        
        if (collision.CompareTag(TAG_LADDERS)) {
            isClimbing = climb;
            // temporarily disable gravity when climbing mode
            if (isClimbing) GetComponent<Rigidbody2D>().gravityScale = 0;
            else GetComponent<Rigidbody2D>().gravityScale = baseGravity;
        }
    }

}
