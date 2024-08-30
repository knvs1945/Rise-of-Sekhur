using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : Handler
{
    // Singleton Instance
    protected static UIHandler instance;

    [SerializeField]    
    protected GameObject panelTabletMessage;

    public static UIHandler Instance {
        get { return instance; }
    }

    // Deploy panel messages here
    public void showTabletMessage(string text, List<Sprite> icons = null, float timeout = 0) {
        if (panelTabletMessage == null) {
            Debug.Log ("no panelTabletMessage assigned");
            return;
        }

        GameObject tempMessage = GameObject.Instantiate(panelTabletMessage);
        if (tempMessage != null) {
            // if the invoker is a gate, include icons. otherwise it is mostly just text
            if (icons != null && icons.Count > 0) tempMessage.GetComponent<TabletPanel>().createPanel(icons, timeout, text);
            else tempMessage.GetComponent<TabletPanel>().createPanel(text, timeout);
            tempMessage.transform.SetParent(this.transform);
            tempMessage.transform.position = transform.position;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        setInstance();
    }

    protected void setInstance() {
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
}
