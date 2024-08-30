using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// level object enum for states
public enum ObjectStates {
    ASLEEP,
    INITIAL,
    TRIGGERED,
    ACTIVE,
    DISABLED
}

public class LevelObject : MonoBehaviour
{
    protected const float gridSize = 1.0f;

    // GameState enum found in gameUnit.cs
    public static GameState gameState = GameState.MAIN_MENU;
    
    // objects to activate/deactivate when triggered
    public LevelObject[] enableTargets, disableTargets;

    // object tags to be used to detect trigger users
    public string[] users;

    public ObjectStates state = ObjectStates.ASLEEP;
    public bool isReusable = false;

    public void activateObject(bool enable) {
        if (state == ObjectStates.ASLEEP) return;
        doOnActivateObject(enable);
    }
    
    public void triggerObject(bool enable) {
        if (state == ObjectStates.ASLEEP) return;
        doOnTriggerObject(enable);
    }

    public void resetObject() {
        state = ObjectStates.INITIAL;
        doOnResetObject();
    }

    // Snap to grid on object start
    protected void snapToGrid() {
        Debug.Log("Snapping to Grid @ " + transform.position + " to " + getClosestGridPosition());
        transform.position = getClosestGridPosition();
    }

    protected Vector2 getClosestGridPosition() {
        Renderer renderer = GetComponent<Renderer>();
        float width = renderer.bounds.size.x / 2;
        float height = renderer.bounds.size.y;
        float x = (Mathf.Round(transform.position.x / gridSize) * gridSize) + width;
        float y = (Mathf.Round(transform.position.y / gridSize) * gridSize) + height;
        return new Vector2(x,y);
    }

    // inheritables here
    // called if the level object is triggered by another level object e.g. switch/trap
    protected virtual void doOnTriggerObject(bool enable) {}
    
    // called if the level object is set to active another level object.
    // Ideally used by Goal Triggers to activate the next Rune Switch to be enabled
    protected virtual void doOnActivateObject(bool enable) {}

    // called if the level object is reset to an initial state
    protected virtual void doOnResetObject() {}


}
