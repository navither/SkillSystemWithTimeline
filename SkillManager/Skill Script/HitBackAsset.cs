using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class HitBackAsset : PlayableAsset
{
    [Tooltip("鸡腿的距离")]
    public float distance;
    [Tooltip("击退的速度")]
    public float hitbackSpeed = 5f;

    public GameObject VFXPrefab;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<HitBackBehaviour>.Create(graph);
        HitBackBehaviour behaviour = playable.GetBehaviour();
        behaviour.ownerGo = go;
        behaviour.playAsset = this;

        return playable;
    }
}
