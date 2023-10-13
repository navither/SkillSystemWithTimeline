using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "BuffSkillConfig.asset", menuName = "Skill/BuffConfig")]
public class BuffSkillConfig : SkillConfigBase
{
    [Tooltip("该效果是否可叠加。")]
    public bool isStackable;

    [Tooltip("该效果的堆叠数量。")]
    public int stacks;

    [Tooltip("该效果的持续时间（秒）。")]
    public float duration;

    BuffSkillConfig()
    {
        isPassive = true;
        stacks = 1;
    }
}
