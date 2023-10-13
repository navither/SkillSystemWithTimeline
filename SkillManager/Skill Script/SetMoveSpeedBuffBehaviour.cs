using EF.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SetMoveSpeedBuffBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public SetMoveSpeedAsset playableAsset;

    //改变移速的目标
    public GameObject target;

    private SkillCpt skillCpt;
    private Monster monsterCpt;
    //原本移速
    private float originSpeed;
    //层数
    int stack;
    public void GetTargetSpeed()
    {
        if (target.TryGetComponent(out skillCpt))
        {
            foreach (ActiveBuff buff in skillCpt.activeBuffList)
            {
                if (buff.buffData == playableAsset.buffSkill)
                {
                    stack = buff.stacks;
                }
            }
        }

        if (target.TryGetComponent(out monsterCpt))
        {
            float speed = monsterCpt.speed;

            originSpeed = speed;

            float bonus = playableAsset.percent * stack;

            speed *= bonus;

            if (monsterCpt.speed + speed >= 0.5f)
            {
                monsterCpt.SetMoveSpeed(monsterCpt.speed + speed);
            }
            else
            {
                monsterCpt.SetMoveSpeed(0.5f);
            }
        }
    }

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        target = ownerGo;

        
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);

        GetTargetSpeed();
    }

    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);

        if (monsterCpt != null)
            monsterCpt.SetMoveSpeed(originSpeed);
    }
}
