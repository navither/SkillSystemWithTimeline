using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ShockDamageAsset : PlayableAsset
{
    [Tooltip("眩晕的buffconfig")]
    public BuffSkillConfig shockBuff;

    [Tooltip("目标眩晕时的伤害倍率")]
    public float damageFactor;

    [Tooltip("技能特效")]
    public GameObject VFXPrefab;
    [Tooltip("翻倍技能特效")]
    public GameObject SpecialVFXPrefab;
    [Tooltip("自身特效")]
    public GameObject ElfVFXPrefab;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<ShockDamageBehaviour>.Create(graph);
        ShockDamageBehaviour behaviour = playable.GetBehaviour();
        behaviour.ownerGo = go;
        behaviour.playAsset = this;

        return playable;
    }
}
