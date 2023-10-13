using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DefenseBuffAsset : PlayableAsset
{
    [Tooltip("防御提升的数值"), Range(0f,1f)]
    public float defenseBonus;

    [Tooltip("技能特效")]
    public GameObject VFXPrefab;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<DefenseBuffBehaviour>.Create(graph);
        DefenseBuffBehaviour behaviour=playable.GetBehaviour();
        behaviour.ownerGo = go;
        behaviour.playAsset = this;

        return playable;
    }
}
