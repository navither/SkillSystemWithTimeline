using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkillConfigBase : ScriptableObject
{
    [Tooltip("播放的timeline")]
    public PlayableAsset playableAsset;

    [Tooltip("技能ID")]
    public int SkillID;

    [Tooltip("技能类型")]
    public ESkillType SkillType;

    [Tooltip("技能名字")]
    public string SkillName;

    [Tooltip("技能冷却时间")]
    public float CoolDownTime;

    [Tooltip("技能是否为被动")]
    public bool isPassive;

    [Tooltip("技能图标")]
    public Sprite SkillSprite;

    [Tooltip("技能描述")]
    [TextArea]
    public string SkillDescription;
}


public enum ESkillType
{
    Default,
    Damage,
    Heal,
    Amplify,
    Money
}

/// <summary>
/// 技能目标类型，无或以自身为目标、指向性目标、非指向性目标
/// </summary>
public enum ESkillTargetType
{
    SelfTarget,

    [Tooltip("非指向性目标：在某个position创建技能效果")]
    NonDirectivity,

    [Tooltip("指向性目标：在某个实体的transform创建技能效果")]
    Directivity
}
