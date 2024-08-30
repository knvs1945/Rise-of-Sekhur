using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : Handler
{

    // Singleton instance
    protected static PlayerHandler instance;

    public GameObject sekhur;

    public static PlayerHandler Instance { 
        get { return instance; }
    }

    void Start() {
        subscribeToEvents();
    }

    void Awake()
    {
        setInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // set the singleton instance here
    private void setInstance() {
        if (instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // subscribe to the player events here;
    protected void subscribeToEvents() {
        doOnRestartLevel += restartLevel;
        if (sekhur != null)  {
            Sekhur playerClass = sekhur.GetComponent<Sekhur>();
            if (playerClass) {
                playerClass.doOnSekhurDied += onSekhurDeath;
            }
        }
    }

    // Handler actions here
    protected override void restartLevel() {
        Debug.Log("Player Handler - Restarting Level... ");
        GameObject checkpoint = LevelHandler.Instance.Checkpoint;
        if (checkpoint != null) {
            sekhur.transform.position = checkpoint.transform.position;
            sekhur.GetComponent<Sekhur>().resetPlayerStats();
            Animator cpAnim = checkpoint.GetComponent<Animator>();
            if (cpAnim != null) cpAnim.Play("checkpoint_spawn");
        }

    }

    // called when sekhur dies
    protected void onSekhurDeath() {
        invokeRestartLevel();
    }
}
