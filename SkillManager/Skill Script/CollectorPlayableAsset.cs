using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CollectorPlayableAsset : PlayableAsset
{
    [Tooltip("技能收集器Prefab")]
    [SerializeField]
    internal GameObject CollectorPrefab;

    [Tooltip("技能释放点")]
    [SerializeField]
    internal Transform releasePoint;

    public GameObject SkillPrefab;

    [Tooltip("技能收集器生成偏移方向")]
    [SerializeField]
    internal Vector3 DirOffset;

    [Tooltip("技能收集器生成距离偏移")]
    [SerializeField]
    internal float DistanceOffset;

    [Tooltip("技能收集器持续时间")]
    public float Lifetime = 0f;
    //[Tooltip("插值跟随父节点比例")] public float lerpFollow = 1f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        if (CollectorPrefab != null)
        {
            return CollectorPlayableBehaviour.Create(graph, go, this);
        }
        else
        {
            //Debug.LogWarning("CollectorPlayableAsset Error: Not Find CollectorPrefab");
        }

        return Playable.Create(graph);
    }
}
