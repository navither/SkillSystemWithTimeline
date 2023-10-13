using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SelfHealingPlayableAsset : PlayableAsset
{
    public SkillConfig skillConfig;
    public int healingAmout;
    public GameObject VFX;
    //public ExposedReference<GameObject> VFX;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SelfHealingPlayableBehaviour>.Create(graph);
        SelfHealingPlayableBehaviour selfHealingPlayableBehaviour=playable.GetBehaviour();
        selfHealingPlayableBehaviour.ownerGo = go;
        selfHealingPlayableBehaviour.playableAsset = this;
        selfHealingPlayableBehaviour.VFX = VFX;

        return playable;
    }
}
