using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BuffAsset : PlayableAsset
{
    public BuffSkillConfig buffSkill;
    public float bonusDuration;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<BuffBehaviour>.Create(graph);
        BuffBehaviour behaviour = playable.GetBehaviour();
        behaviour.ownerGo = go;
        behaviour.playAsset = this;

        return playable;
    }
}
