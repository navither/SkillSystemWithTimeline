using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class HitBackBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public HitBackAsset playAsset;

    SkillCpt ownerSkill;
    Vector3 destination;    //击退的目的地
    Vector3 start;          //被击退的目标的初始位置
    Transform transform;    //目标的transform
    GameObject VFX;         //技能特效

    bool isFinished = false;

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        //Object.Destroy(VFX);
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        ownerSkill = ownerGo.GetComponent<SkillCpt>();

        transform = ownerSkill.SkillTarget.transform;
        start = transform.position;
        destination = transform.position - ownerGo.transform.position;
        destination.Normalize();

        if (ownerSkill.SkillTarget.TryGetComponent(out SkillCpt skillCpt))
            skillCpt.StopCurrentSkill();

        int damage = SkillUtility.CaculateDamage(ownerGo, ownerSkill.SkillTarget.GetComponent<LifeBodyComponent>(), 0);
        LifeBodyComponent lifeBody = SkillUtility.GetLifebody(ownerSkill.SkillTarget.gameObject);
        lifeBody.AddHP(-damage);

        //VFX = Object.Instantiate(playAsset.VFXPrefab, transform);
        VFX =Object.Instantiate(playAsset.VFXPrefab, transform.position, Quaternion.identity);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
        
        if (!isFinished)
        {
            

            if (Vector3.Distance(start, transform.position) > playAsset.distance)
            {
                isFinished = true;
            }

            transform.position += info.deltaTime * playAsset.hitbackSpeed * destination;

        }

    }

}
