using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRune : LevelObject
{

    public delegate void onRuneCollected(bool enable);
    public delegate void onRuneReset();

    public event onRuneCollected doOnRuneCollected;
    public event onRuneReset doOnRuneReset;

    private Animator anim;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Access functions here
    public Sprite getRuneIcon() {
        Sprite icon = null;
        SpriteRenderer rnd = GetComponent<SpriteRenderer>();
        if (rnd != null ) icon = rnd.sprite;
        return icon;
    }

    // Switch and Trigger Behaviors here
    protected override void doOnActivateObject(bool enable) {
        state = enable ? ObjectStates.ACTIVE : ObjectStates.DISABLED;
    }

    protected override void doOnTriggerObject(bool enable) {
        state = ObjectStates.TRIGGERED;
        anim.SetTrigger("trigger");
        for (int i = 0; i < enableTargets.Length; i++) {
            if (enableTargets[i]) enableTargets[i].triggerObject(enable);
        }
    }

    // collision triggers here
    // Compared to button switches, runes should only trigger once they are active
    protected void OnTriggerEnter2D(Collider2D collider) {
        for (int i = 0; i < users.Length; i++) {
            if (collider.CompareTag(users[i]) && state == ObjectStates.ACTIVE) {
                triggerObject(true);
                if (doOnRuneCollected != null) doOnRuneCollected(true);
                break;
            }
        }
    }
}
