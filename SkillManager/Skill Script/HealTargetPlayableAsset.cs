using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class HealTargetPlayableAsset : PlayableAsset
{
    public GameObject VFX;
    public GameObject HealCircle;
    public GameObject HandVFX;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<HealTargetPlayableBehaviour>.Create(graph);
        HealTargetPlayableBehaviour HealTargetPlayableBehaviour = playable.GetBehaviour();

        HealTargetPlayableBehaviour.ownerGo = go;
        HealTargetPlayableBehaviour.playableAsset = this;
        HealTargetPlayableBehaviour.VFX = VFX;
        HealTargetPlayableBehaviour.HandVFX = HandVFX;
        HealTargetPlayableBehaviour.HealCircle = HealCircle;

        return playable;
    }
}
