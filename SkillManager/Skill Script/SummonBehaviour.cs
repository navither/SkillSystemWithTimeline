using EF.Monster;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SummonBehaviour : PlayableBehaviour
{
    public GameObject ownerGO;
    public SummonAsset playAsset;

    SkillCpt ownerSkill;
    GameObject portal;

    public override void OnGraphStart(Playable playable)
    {
        ownerSkill=ownerGO.GetComponent<SkillCpt>();
        portal = Object.Instantiate(playAsset.summoningPortal, ownerGO.transform);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        foreach (GameObject minion in playAsset.minions)
        {
            GameObject monster = MonsterManager.m_instance.CreateMonster(minion, portal.transform);
            Object.Instantiate(playAsset.summoningVFXOnMinion, monster.transform);
        }
    }

}
