using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for current gameState;
public enum GameState {
    MAIN_MENU,
    LOADING,
    IN_GAME_RESET,
    IN_GAME_START,
    IN_GAME_PAUSED,
    IN_GAME_VICTORY,
    IN_GAME_DEFEAT
}

/// <summary>
/// Base Game Unit class
/// </summary>
public class GameUnit : MonoBehaviour
{
    public float HP, moveSpeed;
    public static GameState gameState = GameState.MAIN_MENU;
    public static bool isGamePaused = false;

    protected int Level = 1, xDirection = 1, yDirection = 1;
    protected float HPMax, HPTemp;
    protected float ATKbase, ATKmax, ATKTemp, ATKdelay, ATKTimer, ATKRange;
    protected bool isAlive = true, isActive, isPaused = false, isImmune = false, isNPC = false, isIllusion;
    
    [SerializeField]
    protected string unitName;

    // getters
    public string Name {
        get { return unitName; }
    }
    
    public bool IsAlive {
        get { return isAlive; }
    }

    public bool IsActive {
        get { return isActive; }
    }

    public bool IsPaused {
        get { return isPaused; }
        set { isPaused = value; }
    }

    public bool IsImmune {
        get { return isImmune; }
    }

    public bool IsNPC {
        get { return isNPC; }
    }

    public bool IsIllusion {
        get { return isIllusion; }
    }

    // common unit behavior
    public bool takesDamage(float DMG) {
        Debug.Log("Target takes damage! " + this);
        if (!isImmune) {
            doOnTakeDamage(DMG);
            return true;
        }
        return false;
    }

    // overrideable add buff and removeBuff methods for game units

    // force kill a unit
    public bool killUnit() {
        if (isAlive) {
            doOnDeath();
            return true;
        }
        return false;
    }
 
    // Overrideables
    public virtual void restartUnit(string gameMode) {}
    protected virtual void doOnTakeDamage(float DMG) {}
    protected virtual void doOnDeath() {}

}
