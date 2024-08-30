using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Panel for showing tablet messages
/// </summary>
public class TabletPanel : Panel
{
    
    [SerializeField]
    protected Text message;

    public List<Sprite> spriteMessage;
    public Transform tabletBody;
    public float iconSpacing = 50f, tabletPadding = 50f;

    protected float timer = 0f, tabletBounds = 0f, iconAreaMaxWidth;
    bool isTimed = false;

    protected void Awake() {
        message.text = "";

        if (tabletBody != null) {
            Image tabletSprite = tabletBody.gameObject.GetComponent<Image>();
            if (tabletSprite != null) {
                iconAreaMaxWidth = tabletSprite.sprite.rect.width - tabletPadding;
                // Debug.Log("iconAreaMaxWidth: " + iconAreaMaxWidth);
            }
        }
    }

    protected void Start() {
        // displaySpriteMsg();
    }

    protected override void Update()
    {
        // player has clicked on attack or spyke button
        if (testMode && Input.anyKeyDown) {
            closePanel();
        }

        // close modal if timed
        if (isTimed && timer < Time.time) {
            closePanel();
        }
        
    }

    // if a message only includes text right now
    public void createPanel(string msg, float timeout = 0) {
        message.text = msg;
        modalActive = true;

        if (timeout > 0) {
            isTimed = true;
            timer = Time.time + timeout;
        }
    }

    // if a message includes a list of images
    public void createPanel(List<Sprite> msg, float timeout = 0, string txt = "") {
        spriteMessage = msg;
        message.text = txt;
        modalActive = true;
        displaySpriteMsg();

        if (timeout > 0) {
            isTimed = true;
            timer = Time.time + timeout;
        }
    }

    // Display sprite messages here
    protected void displaySpriteMsg() {
        GameObject temp;
        float totalWidth = 0f;
        Vector2 basePos = new Vector2(tabletBody.position.x - (iconAreaMaxWidth / 2f), tabletBody.position.y);
        Vector2 nextPos = basePos;
        if (spriteMessage.Count > 0) {
            for (int i = 0; i < spriteMessage.Count; i++) {
                temp = new GameObject("icon_" + i);
                if (temp) {
                    Image newSprite = temp.AddComponent<Image>();
                    newSprite.sprite = spriteMessage[i];
                    temp.transform.SetParent(tabletBody);
                    
                    // set the position of the new image message 
                    temp.transform.position = nextPos;
                    nextPos.x = temp.transform.position.x + spriteMessage[i].rect.width + iconSpacing;

                    // calculate the totalWidth
                    totalWidth += spriteMessage[i].rect.width + iconSpacing;
                    if (totalWidth >= iconAreaMaxWidth) {
                        nextPos.y = spriteMessage[i].rect.height + 50;
                        nextPos.x = basePos.x;
                    }
                    
                }
            }
        }
    }

    public void closePanel() {
        modalActive = false;
        Destroy(gameObject);
    }
}
