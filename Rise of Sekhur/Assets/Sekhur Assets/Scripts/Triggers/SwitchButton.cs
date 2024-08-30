using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwitchButton : LevelObject
{
    public delegate void onButtonPressed();
    public delegate void onButtonReset();
    public event onButtonPressed doOnButtonPressed;
    public event onButtonReset doOnButtonReset;
    
    public Sprite spritePressed, spriteDefault;
    public Vector3Int tilePos;
    
    private SpriteRenderer buttonSprite;

    void Awake() {
        buttonSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Switch and Trigger Behaviors here
    protected override void doOnTriggerObject(bool enable) {
        state = ObjectStates.TRIGGERED;
        buttonSprite.sprite = spritePressed;

        // enable/disable anything here
        for (int i = 0; i < enableTargets.Length; i++) {
            if (enableTargets[i]) enableTargets[i].triggerObject(enable);
        }
    }

    // collision triggers here
    protected void OnTriggerEnter2D(Collider2D collider) {
        for (int i = 0; i < users.Length; i++) {
            if (collider.CompareTag(users[i]) && state == ObjectStates.INITIAL) {
                triggerObject(true);
                if (doOnButtonPressed != null) doOnButtonPressed();
                break;
            }
        } 
    }
}
