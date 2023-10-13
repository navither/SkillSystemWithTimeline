using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class HealTargetPlayableBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public SkillCpt skillCpt;

    public GameObject VFX;
    public GameObject HandVFX;
    public GameObject HealCircle;

    private GameObject temp;
    private BodyBonds bonds;


    public HealTargetPlayableAsset playableAsset;

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
    }
    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        skillCpt = ownerGo.GetComponent<SkillCpt>();
        bonds = ownerGo.GetComponent<BodyBonds>();
    }
    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        Transform target = skillCpt.SkillTarget;

        if (target != null)
        {
            //ownerGo.transform.LookAt(target);

            LifeBodyComponent curLifeBody = target.GetComponent<LifeBodyComponent>();
            if (curLifeBody == null) { curLifeBody = target.GetComponentInParent<LifeBodyComponent>(); }

            int value = SkillUtility.CaculateDamage(ownerGo, curLifeBody, skillCpt.curSkillConfig.Damage);
            curLifeBody.AddHP(value);
            GameObject.Instantiate(VFX, target);
            temp = GameObject.Instantiate(HealCircle, target);

            {
                GameObject.Instantiate(HandVFX, bonds.LeftHand);
                //GameObject.Instantiate(HandVFX, bonds.RightHand);
            }
        }

    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (temp != null)
        {
            temp.transform.Translate(new Vector3(0, 2 * Time.deltaTime, 0));
            Vector3 curScale = temp.transform.localScale;
            temp.transform.localScale = curScale * (1 - 0.6f * Time.deltaTime);
        }

        //if (skillCpt.curSkillConfig != null) { ownerGo.transform.LookAt(skillCpt.SkillTarget); }
    }

}
