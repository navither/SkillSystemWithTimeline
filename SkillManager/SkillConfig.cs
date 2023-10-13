using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public enum ReleaseType
{
    Defaut,
    Self,
    Circle
};
[CreateAssetMenu(fileName = "SkillConfig.asset", menuName = "Skill/SkillConfig")]
public class SkillConfig : SkillConfigBase
{

    [Tooltip("技能伤害")]
    public int Damage = 1;

    [Tooltip("技能释放位置")]
    public ESkillTargetType SkillTargetType;

    [Header("额外技能设定")]
    [Space(6)]
    [Tooltip("额外的技能特效物体")]
    public GameObject SkillVFX;

    [Tooltip("技能指示器")]
    public GameObject MouseDirector;
    [Tooltip("范围指示器")]
    public GameObject RangeDirector;

    public ReleaseType releaseType = ReleaseType.Defaut;

}
