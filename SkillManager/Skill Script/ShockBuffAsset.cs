using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ShockBuffAsset : PlayableAsset
{
    public GameObject shockVFXPrefab; //眩晕视觉效果
    public PlayableAsset shockAnim;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<ShockBuffBehaviour>.Create(graph);
        ShockBuffBehaviour shockBuffBehaviour = playable.GetBehaviour();
        shockBuffBehaviour.ownerGo = go;
        shockBuffBehaviour.playableAsset = this;
        SkillCpt skillCpt = go.GetComponent<SkillCpt>();

        return playable;
    }
}
