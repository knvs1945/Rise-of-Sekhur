using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum platformMoveType
{
    Reverse,
    Rotate,
    ControlledRotate,
    ControlledReverse
}

public class TriggerPlatform : LevelObject
{
    public platformMoveType moveBehavior = platformMoveType.Reverse;
    public float platformSpd = 1;

    [SerializeField]
    protected Vector2[] positions;

    protected Vector2 startPos, nextPos;
    protected int positionCount, currentPosCount, countFactor = 1;
    protected bool positionReached = false, playerRiding = false;
    protected Transform playerParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        positionCount = positions.Length - 1;
        doOnResetObject();
    }

    // Update is called once per frame
    void Update()
    {
        moveToNextPosition();
    }

    // moves the platform. Does not move if not ACTIVE or has no other positions to move towards
    protected void moveToNextPosition() {
        if (state != ObjectStates.ACTIVE) return;
        if (positionCount < 0) return;


        if (!positionReached) {
            transform.position = Vector2.MoveTowards(transform.position, nextPos, platformSpd * Time.deltaTime);
            if (Vector2.Distance(transform.position, nextPos) == 0) positionReached = true;
        }
        else {
            if (moveBehavior == platformMoveType.Reverse) {
                // start counting positions backwards when it's time to reverse
                // Debug.Log("Switching Movement Direction");
                if (currentPosCount >= positionCount) countFactor = -1;
                else if (currentPosCount <= 0) countFactor = 1;
                currentPosCount += countFactor;
            }
            else {
                if (currentPosCount >= positionCount) currentPosCount = 0;
                else currentPosCount++;
            }
            nextPos = positions[currentPosCount];
            // Debug.Log("Current Pos Count: " + currentPosCount + " Next Pos: " + nextPos);
            positionReached = false;
        }

    }

    // Collision Events

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // store the parent temporarily and move player to be part of this platform
            playerParent = collision.transform.parent;
            collision.transform.SetParent(transform);
            playerRiding = true;    
        }
    }

    protected void OnCollisionExit2D(Collision2D collision) {
        if (!playerRiding) return;
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(playerParent);
            playerRiding = false;
        }
    }

    // Overrideables
    protected override void doOnTriggerObject(bool enable) {
        state = enable ? ObjectStates.ACTIVE : ObjectStates.DISABLED;
    }

    protected override void doOnResetObject() {
        positionReached = false;
        playerRiding = false;
        currentPosCount = 0;
        countFactor = 1;
        if (positions.Length > 0) {
            startPos = positions[0];
            nextPos = positions[0];
        }
        if (positions.Length > 1) nextPos = positions[1];
        transform.position = startPos;

    }
}
