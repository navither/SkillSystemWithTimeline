using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LookAtTargetPlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<LookAtTargetPlayableBehaviour>.Create(graph);
        LookAtTargetPlayableBehaviour lookAtTarget = playable.GetBehaviour();

        lookAtTarget.ownerGo = go;
        lookAtTarget.playableAsset = this;

        return playable;
    }
}
