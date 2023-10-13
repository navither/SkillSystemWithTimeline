using BehaviorDesigner.Runtime;
using EF.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class ShockBuffBehaviour : PlayableBehaviour
{
    //目标
    public GameObject ownerGo;
    public ShockBuffAsset playableAsset;

    private BehaviorTree behaviorTree;
    private GameObject shockVFX;
    private Monster monsterCpt;
    private Animator monsterAnim;
    private PlayableDirector director;

    private float yOffset;      //特效在y轴上的偏移，保证特效生成在目标的头顶上

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        if (ownerGo.TryGetComponent(out behaviorTree))
        {
            behaviorTree.enabled = false;       //关闭怪物的行为树，使其无法攻击（释放技能）
        }
        if (ownerGo.TryGetComponent(out monsterCpt))
        {
            monsterCpt.SetMoveSpeed(0f);        //把怪物的移速减为0
        }
        //if(ownerGo.TryGetComponent(out monsterAnim))
        //{
        //    monsterAnim.SetBool("isWalking", false);
        //    monsterAnim.SetBool("isAttacking", false);
        //}
        SkillCpt ownerSkill=ownerGo.GetComponent<SkillCpt>();
        ownerSkill.SwitchPlay(playableAsset.shockAnim);


        Vector3 monsterSize = ownerGo.GetComponent<BoxCollider>().size;

        yOffset = monsterSize.y;
        shockVFX = Object.Instantiate(playableAsset.shockVFXPrefab, ownerGo.transform);
        shockVFX.transform.localScale *= (monsterSize.x + monsterSize.z) / 2f;

        shockVFX.transform.localPosition = new Vector3(shockVFX.transform.localPosition.x, shockVFX.transform.localPosition.y + yOffset, shockVFX.transform.localPosition.z);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        if (behaviorTree != null)
        {
            behaviorTree.enabled = true;
        }

        if(monsterCpt != null)
        {
            monsterCpt.SetMoveSpeed(monsterCpt.speed);
        }

        if (shockVFX != null)
        {
            Object.Destroy(shockVFX);
        }
    }

}
