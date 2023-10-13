using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class BuffBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public BuffAsset playAsset;

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        SkillUtility.ApplyBuff(playAsset.buffSkill, ownerGo, playAsset.bonusDuration);
    }


}
