using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理技能伤害与buff
/// </summary>
public class SkillUtility : MonoBehaviour
{

    /// <summary>
    /// 减少生命值前要计算伤害：（技能伤害+攻击力）* 减伤，
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="victimLife">受害者</param>
    /// <param name="skillDamage">技能基本伤害</param>
    /// <returns></returns>
    public static int CaculateDamage(GameObject attacker, LifeBodyComponent victimLife, int skillDamage)
    {
        int damage = 0;

        if (!attacker.TryGetComponent(out LifeBodyComponent attackerLife))
        {
            attackerLife = attacker.GetComponentInParent<LifeBodyComponent>();
        }

        if (attacker.TryGetComponent<ElfBase>(out ElfBase elf))      //如果攻击是由妖精发动的,伤害就是妖精的攻击力
        {
            damage = elf.GetComponent<LifeBodyComponent>().elfInfo.attackValue;
        }
        else
        {
            damage = attackerLife.attackValue;
        }

        damage += skillDamage;
        damage -= (int)Math.Round(damage * victimLife.defense);
        //Debug.Log(damage);
        return damage;
    }

    /// <summary>
    /// 计算伤害，强化类技能不造成伤害, 治疗技能damage为负值
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="victimLife"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static int CaculateDamage(GameObject attacker, LifeBodyComponent victimLife, SkillConfig skill)
    {
        int damage = 0;

        if (!attacker.TryGetComponent(out LifeBodyComponent attackerLife))
        {
            attackerLife = attacker.GetComponentInParent<LifeBodyComponent>();
        }

        if (attacker.TryGetComponent<ElfBase>(out ElfBase elf))      //如果攻击是由妖精发动的,伤害就是妖精的攻击力
        {
            damage = elf.GetComponent<LifeBodyComponent>().elfInfo.attackValue;
        }
        else
        {
            damage = attackerLife.attackValue;
        }

        damage += skill.Damage;
        damage -= (int)Math.Round(damage * victimLife.defense);

        switch (skill.SkillType)
        {
            case ESkillType.Heal:       //如果是治疗技能，伤害变为负值
                damage = -damage;
                break;
            case ESkillType.Amplify:    //如果是强化技能，不造成伤害
                damage = 0;
                break;
            default:
                break;
        }
        //Debug.Log(damage);
        return damage;
    }

    /// <summary>
    /// 施加buff
    /// </summary>
    /// <param name="buffSkill"></param>
    /// <param name="target"></param>
    public static void ApplyBuff(BuffSkillConfig buffSkill, GameObject target, float bonusDuration = 0)
    {
        if (target.TryGetComponent(out SkillCpt skillCpt))
        {
            foreach (ActiveBuff buff in skillCpt.activeBuffList)
            {
                if (buff.buffData == buffSkill)             //如果已有了此buff
                {
                    skillCpt.StackBuff(buff);               //添加层数
                    buff.duration += bonusDuration;
                    return;
                }
            }

            ActiveBuff activeBuff = new(buffSkill);         //没有该buff就new一个buff并激活

            skillCpt.AddBuff(activeBuff);
            activeBuff.duration += bonusDuration;
            skillCpt.ActivateBuff(activeBuff);           //激活buff效果

        }
    }

    /// <summary>
    /// 获取该游戏物体上的lifebody
    /// </summary>
    /// <param name="Go"></param>
    /// <returns></returns>
    public static LifeBodyComponent GetLifebody(GameObject Go)
    {
        //如果没有找到lifebody，此时的攻击者一般是玩家，而玩家的碰撞体在子物体，lifebody在父物体
        if (!Go.TryGetComponent(out LifeBodyComponent lifeBody))
        {
            lifeBody = Go.GetComponentInParent<LifeBodyComponent>();
        }

        return lifeBody;
    }
}
