using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : LevelObject
{

    protected Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // when player bumps into the gate, show

    // Switch and Trigger Behaviors here
    protected override void doOnTriggerObject(bool enable) {
        state = ObjectStates.ACTIVE;
        anim.SetTrigger("open");
    }

    protected override void doOnResetObject() {
        anim.SetTrigger("close");
    }

}
