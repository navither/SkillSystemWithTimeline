using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using EF.Monster;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CollectorPlayableBehaviour : PlayableBehaviour
{
    GameObject OwnerObject;
    CollectorPlayableAsset PlayAsset;
    GameObject SkillObject;
    GameObject CollectorObject;

    Transform ReleasePoint;

    CollectorCpt collectorCpt;
    SkillCpt ownerSkillCpt;

    private float rotate = 120f;

    private Vector3 Pos;

    public static ScriptPlayable<CollectorPlayableBehaviour> Create(PlayableGraph graph, GameObject ownerObj, CollectorPlayableAsset playAsset)
    {
        var handle = ScriptPlayable<CollectorPlayableBehaviour>.Create(graph);
        handle.GetBehaviour().Initialize(ownerObj, playAsset);
        return handle;
    }

    private void Initialize(GameObject ownerObj, CollectorPlayableAsset playAsset)
    {
        OwnerObject = ownerObj;
        PlayAsset = playAsset;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

        
        ownerSkillCpt = OwnerObject.GetComponent<SkillCpt>();

        AdjustRelease();

        AdjustSkillObject();

        //非规则collector的旋转：当collector不是正方体，球体的时候，就需要应用预制体的旋转   
        float angleDegrees = OwnerObject.transform.rotation.eulerAngles.y;
        Quaternion newYRotation = Quaternion.AngleAxis(angleDegrees, Vector3.up);
        Quaternion rotatedRotation = newYRotation * PlayAsset.CollectorPrefab.transform.rotation;

        CollectorObject = Object.Instantiate(PlayAsset.CollectorPrefab, Pos, rotatedRotation);

        collectorCpt = CollectorObject.GetComponentInChildren<CollectorCpt>();

        if (collectorCpt != null)
        {
            collectorCpt.lifeTime = PlayAsset.Lifetime;
            collectorCpt.attacker = OwnerObject;
            collectorCpt.skill = ownerSkillCpt.curSkillConfig;

            if (collectorCpt.skillVFX != null)
            {
                collectorCpt.skillVFX.transform.SetParent(null);    //把碰撞体的粒子特效子物体分开，
                //collectorCpt.skillVFX.transform.position=Pos;
            }
        }
        else
        {
            // Debug.LogWarning("没有找到collectorCpt");
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        

        base.ProcessFrame(playable, info, playerData);

        ProcessSkillObject(playable, info);
    }

    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);

//#if UNITY_EDITOR
//        if (SkillObject.gameObject != null)
//            GameObject.DestroyImmediate(SkillObject.gameObject);
//#endif
        if (SkillObject != null)
            GameObject.Destroy(SkillObject);

        if (CollectorObject != null)
        {
            GameObject.Destroy(CollectorObject);
        }
        if (OwnerObject!=null && CollectorObject != null && MonsterManager.m_instance.gameObject.activeSelf == true)
        {
            if (OwnerObject.GetComponent<Player>() != null && collectorCpt != null)
            {
                collectorCpt.PlayerDamage();
            }
            GameObject.Destroy(CollectorObject);
        }
    }

    private void AdjustRelease()
    {
        //如果是玩家，释放点为playerModel
        if (OwnerObject.GetComponent<Player>() != null && ownerSkillCpt.curSkillConfig.SkillTargetType == ESkillTargetType.SelfTarget)
        {
            ReleasePoint = OwnerObject.GetComponent<Player>().playerModel.transform;
            Pos = ReleasePoint.position;
        }
        else if (ownerSkillCpt.curSkillConfig.SkillTargetType == ESkillTargetType.Directivity)
        {
            ReleasePoint=ownerSkillCpt.SkillTarget.transform;
            Pos = ReleasePoint.position;
        }
        else if(ownerSkillCpt.curSkillConfig.SkillTargetType==ESkillTargetType.NonDirectivity)
        {
            Pos = ownerSkillCpt.SkillVector + OwnerObject.transform.forward * PlayAsset.DistanceOffset + PlayAsset.DirOffset;
            //Debug.Log(Pos);
        }
        else
        {
            //释放类型为selftarget
            ReleasePoint = OwnerObject.transform;
            Pos = OwnerObject.transform.position + OwnerObject.transform.forward * PlayAsset.DistanceOffset + PlayAsset.DirOffset;
        }

        //Pos += OwnerObject.transform.forward * PlayAsset.DistanceOffset + PlayAsset.DirOffset;

    }

    private void AdjustSkillObject()
    {
        if (null == PlayAsset.SkillPrefab) return;

        SkillObject = Object.Instantiate(PlayAsset.SkillPrefab, ReleasePoint.transform);

        SkillObject.transform.Rotate(Vector3.up, -60f);
        SkillObject.transform.Translate(new Vector3(0f, 1f, -2f));
    }

    private void ProcessSkillObject(Playable playable, FrameData info)
    {
        if (null == SkillObject) return;

        double len = playable.GetDuration();

        float rotateSpeed = (float)(rotate / len);

        SkillObject.transform.RotateAround(ReleasePoint.transform.position, Vector3.up, rotateSpeed * info.deltaTime);
    }

    private void InitCollector()
    {

    }
}
