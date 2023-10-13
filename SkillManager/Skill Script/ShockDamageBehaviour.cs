using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class ShockDamageBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public ShockDamageAsset playAsset;

    SkillCpt ownerSkill;
    LifeBodyComponent victimLife;

    private BodyBonds bonds;

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);

        bonds = ownerGo.GetComponent<BodyBonds>();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Object.Instantiate(playAsset.ElfVFXPrefab, bonds.LeftHand);     //释放技能时手上闪光

        ownerSkill = ownerGo.GetComponent<SkillCpt>();
        victimLife = SkillUtility.GetLifebody(ownerSkill.SkillTarget.gameObject);

        int damage = SkillUtility.CaculateDamage(ownerGo, victimLife, ownerSkill.curSkillConfig);

        if (IsTargetShocking(ownerSkill.SkillTarget))               //如果目标有眩晕buff
        {
            damage = (int)(damage * playAsset.damageFactor);        //造成多倍伤害
            Object.Instantiate(playAsset.SpecialVFXPrefab, ownerSkill.SkillTarget.transform);
        }
        else
        {
            Object.Instantiate(playAsset.VFXPrefab, ownerSkill.SkillTarget.transform);
        }

        victimLife.AddHP(-damage);
    }

    private bool IsTargetShocking(Transform target)
    {
        SkillCpt targetSkill =target.GetComponent<SkillCpt>();

        foreach (ActiveBuff activeBuff in targetSkill.activeBuffList)
        {
            if (activeBuff.buffData == playAsset.shockBuff)     //如果目标有眩晕buff
            {
                return true;
            }
        }

        return false;
    }
}
