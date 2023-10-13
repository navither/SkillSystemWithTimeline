using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SetMoveSpeedAsset : PlayableAsset
{
    [Range(-1f,1f), Tooltip("增加或减少百分之多少的移速, 正值加速，负值减速")]
    public float percent;

    [Tooltip("该buff技能config")]
    public BuffSkillConfig buffSkill;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SetMoveSpeedBuffBehaviour>.Create(graph);
        SetMoveSpeedBuffBehaviour setMoveSpeedBehaviour = playable.GetBehaviour();
        setMoveSpeedBehaviour.ownerGo = go;
        setMoveSpeedBehaviour.playableAsset = this;
        SkillCpt skillCpt = go.GetComponent<SkillCpt>();

        return playable;
    }
}
