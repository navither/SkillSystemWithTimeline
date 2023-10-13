using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TreeMoneyPlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<TreeMoneyPlayableBehaviour>.Create(graph);
        TreeMoneyPlayableBehaviour behaviour = playable.GetBehaviour();
        behaviour.OwnerObject = go;
        return playable;
    }
}
