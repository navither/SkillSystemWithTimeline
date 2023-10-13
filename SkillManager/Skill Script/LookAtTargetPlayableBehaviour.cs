using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class LookAtTargetPlayableBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public LookAtTargetPlayableAsset playableAsset;

    private Transform target;

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
    }
    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
    }
    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);

        target = ownerGo.GetComponent<SkillCpt>().SkillTarget;
        ownerGo.transform.LookAt(target);
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

    }
}
