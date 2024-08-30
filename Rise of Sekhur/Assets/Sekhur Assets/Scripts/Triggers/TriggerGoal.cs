using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGoal : LevelObject
{

    [SerializeField]
    protected GameObject[] switchRunes;

    protected List<Sprite> runeIcons;
    protected SwitchRune currentRune;
    protected Animator anim;
    protected int runeCtr = 0, runeLimit = 0;


    void Awake()
    {
        anim = GetComponent<Animator>();
        resetGoalData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision detection here
    // When the player bumps or collides with the gate, show the player the needed rune order to unlock it
    protected void OnCollisionEnter2D(Collision2D collider) {
        if (state != ObjectStates.ACTIVE) {
            UIHandler.Instance.showTabletMessage("", runeIcons, 0);
        }
    }

    // Switch and Trigger Behaviors here
    protected override void doOnTriggerObject(bool enable) {
        Debug.Log("Goal Triggered! ");
        getNextRune();
    }

    protected override void doOnResetObject() {
        Debug.Log("Resetting Object!");
        resetGoalData();
        anim.SetTrigger("close");
    }

    // misc actions
    protected void resetGoalData() {
        runeCtr = -1;
        runeLimit = switchRunes.Length;
        getRuneIcons();
        getNextRune();
    }

    protected void getNextRune() {
        if (runeLimit <= 0) return;
        runeCtr++;
        if (runeCtr < runeLimit) {
            Debug.Log("Current runeCtr: " + runeCtr + " - runeLimit: " + runeLimit);
            currentRune = switchRunes[runeCtr].GetComponent<SwitchRune>();
            if (currentRune) {
                currentRune.activateObject(true);
                currentRune.doOnRuneCollected += doOnTriggerObject;
            }
        }
        else {
            openGoalGate();
        }
    }
    
    protected void getRuneIcons() {
        if (runeLimit > 0) {
            runeIcons = new List<Sprite>();
            for (int i = 0; i < runeLimit; i++) {
                runeIcons.Add(switchRunes[i].GetComponent<SwitchRune>().getRuneIcon());
            }
        }
    }

    protected void openGoalGate() {
        // only activate when all runes are collected
        state = ObjectStates.ACTIVE;
        anim.SetTrigger("open");
    }


}
