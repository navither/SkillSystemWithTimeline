using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ShooterPlayableAsset : PlayableAsset
{
    public GameObject bullet;
    public BuffSkillConfig buffSkill;
    [Tooltip("受击特效")]
    public GameObject HitEffect;

    [Tooltip("子弹飞行速度")]
    public float bulletSpeed;

    [Tooltip("目标位置偏移量")]
    public Vector3 targetPositionOffset;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<ShooterPlayableBehaviour>.Create(graph);
        var shooterPlayableBehaviour = playable.GetBehaviour();
        shooterPlayableBehaviour.ownerGo = go;
        shooterPlayableBehaviour.playableAsset = this;

        return playable;
    }
}
