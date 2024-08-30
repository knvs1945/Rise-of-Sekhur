using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Button Tile")]
public class CustomButtonTile : RuleTile<CustomButtonTile.Neighbor> {
    
    public TileLevelData triggerMaps;
    
    public bool customField;
    public LevelObject switchButton;
    public LevelObject[] enableTargets, disableTargets;
    public Sprite activeSprite, initialSprite;

    protected SpriteRenderer rndr;

    // store a tile's info separately to avoid updating ALL instances of that tile's tileDATA
    private Dictionary <Vector3Int, bool> tileStates = new Dictionary<Vector3Int, bool>();

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
        if (go != null) {
            Tilemap tm = tilemap.GetComponent<Tilemap>();
            switchButton = go.GetComponent<SwitchButton>();
            switchButton.enableTargets = enableTargets;
            switchButton.disableTargets = disableTargets;
            go.transform.position = tm.CellToWorld(position) + tm.tileAnchor;

            // store this tile's data into the dictionary;
            if (!tileStates.ContainsKey(position)) {
                tileStates[position] = false;
                resetSprite(position, tm);
            }
            
            rndr = go.GetComponent<SpriteRenderer>();
            if (rndr != null) {
                //rndr.sprite = initialSprite;
            }

            SwitchButton trigger = switchButton.GetComponent<SwitchButton>();
            if (trigger != null) {
                trigger.doOnButtonPressed += () => updateSprite(position, tm);
                trigger.doOnButtonReset += () => resetSprite(position, tm);
            }
        }
        return base.StartUp(position, tilemap, go);
    }
    
    // get tile data here - works on both editor and runtime
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        base.GetTileData(position, tilemap, ref tileData);
        // switchButton = tileData.gameObject.GetComponent<SwitchButton>();

        if (tileStates.ContainsKey(position) && tileStates[position]) {
            tileData.sprite = activeSprite;
        }
        else {
            tileData.sprite = initialSprite;
        }
    }

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }
        return base.RuleMatch(neighbor, tile);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
        base.RefreshTile(position, tilemap);
    }    

    private void updateSprite(Vector3Int position, Tilemap tm) {
        Debug.Log("Button Triggered!");
        tileStates[position] = true;
        tm.RefreshTile(position);
    }

    private void resetSprite(Vector3Int position, Tilemap tm) {
        // Debug.Log("Button Reset!");
        tileStates[position] = false;
        tm.RefreshTile(position);
    }
}