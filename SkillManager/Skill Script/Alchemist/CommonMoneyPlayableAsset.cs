using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CommonMoneyPlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<CommonMoneyPlayableBehaviour>.Create(graph);
        CommonMoneyPlayableBehaviour behaviour = playable.GetBehaviour();
        behaviour.OwnerObject = go;
        return playable;
    }
}
