using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHint : LevelObject
{
    public string textToDisplay = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // collision triggers here
    // Expand a tablet message 
    protected void OnTriggerEnter2D(Collider2D collider) {
        for (int i = 0; i < users.Length; i++) {
            if (collider.CompareTag(users[i])) {
                UIHandler.Instance.showTabletMessage(textToDisplay);
                break;
            }
        }

    }
}
