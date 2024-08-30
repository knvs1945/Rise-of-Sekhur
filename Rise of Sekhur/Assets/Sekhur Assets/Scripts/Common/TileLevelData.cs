using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwitchTriggerPair
{
    public Vector3Int switchPosition;
    public GameObject[] enableTriggers, disableTriggers;
}

[CreateAssetMenu(fileName = "TileLevelData", menuName = "Custom/Tile Level Data")]
public class TileLevelData : ScriptableObject
{
    public List<SwitchTriggerPair> triggerMapsList = new List<SwitchTriggerPair>();

    private Dictionary <Vector3Int, GameObject[]> enableMaps;
    private Dictionary <Vector3Int, GameObject[]> disableMaps;

    public void OnEnable() {
        enableMaps = new Dictionary<Vector3Int, GameObject[]>();
        disableMaps = new Dictionary<Vector3Int, GameObject[]>();
        foreach (var pair in triggerMapsList) {
            enableMaps[pair.switchPosition] = pair.enableTriggers;
        }

        foreach (var pair in triggerMapsList) {
            disableMaps[pair.switchPosition] = pair.disableTriggers;
        }
    }

    // grab a switch's triggers at a given position
    public GameObject[] GetEnableMap(Vector3Int switchPosition) {
        if (enableMaps.TryGetValue(switchPosition, out GameObject[] value)) {
            return value;
        }
        return null;
    }

    public GameObject[] GetDisableMap(Vector3Int switchPosition) {
        if (disableMaps.TryGetValue(switchPosition, out GameObject[] value)) {
            return value;
        }
        return null;
    }
}
