using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : Handler
{
    // Singleton instance
    protected static LevelHandler instance;

    // holds all the checkpoint children within a scene's set of levels
    [SerializeField]
    protected Transform checkpointParent;

    protected GameObject checkpoint;
    private Dictionary<int, GameObject> checkpointList = new Dictionary<int, GameObject>();

    public static LevelHandler Instance {
        get { return instance; }
    }

    public GameObject Checkpoint {
        get { return checkpoint; }
    }

    // Start is called before the first frame update
    void Start()
    {
        doOnRestartHandler();
    }

    void Awake()
    {
        setInstance();
    }

    // set the singleton instance here
    private void setInstance() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // register events and checkpoints here
    protected void registerCheckpoints() {
        if (checkpointParent == null) {
            Debug.Log("Checkpoint Parent is not assigned.");
            return;
        }

        int children = checkpointParent.childCount;
        if (children <= 0) {
            Debug.Log("Checkpoint Parent has no children.");
            return;
        }
        for (int i = 0; i < children; i++) {
            Transform child = checkpointParent.GetChild(i);
            checkpointList[i+1] = child.gameObject; // Levels start at 1, and not zero.
        }
    }

    protected GameObject getCheckpointAtLevel(int level) {
        if (!checkpointList.ContainsKey(level)) {
            Debug.Log("No checkpoint found at level: " + level);
            return null;
        }
        if (checkpointList[level] != null) return checkpointList[level];
        return null;
    }

    protected override void doOnRestartHandler() {
        gameLevel = 1;
        registerCheckpoints();
        checkpoint = getCheckpointAtLevel(gameLevel);
    }

}
