using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SummonAsset : PlayableAsset
{
    [Tooltip("召唤物")]
    public GameObject[] minions;
    [Tooltip("召唤传送门")]
    public GameObject summoningPortal;
    [Tooltip("召唤特效")]
    public GameObject summoningVFXOnMinion;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SummonBehaviour>.Create(graph);
        SummonBehaviour summonBehaviour = playable.GetBehaviour();
        summonBehaviour.ownerGO = go;
        summonBehaviour.playAsset = this;

        return playable;
    }
}
