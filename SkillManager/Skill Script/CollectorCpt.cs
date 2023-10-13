using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CollectorCpt : MonoBehaviour
{
    [NonSerialized] public float lifeTime;

    [Tooltip("受击特效")] public GameObject HitEffect;
    [Tooltip("技能粒子特效，播放完之后粒子会自己销毁")] public ParticleSystem skillVFX;

    public LayerMask OnTriggerEnterLayer;

    public delegate bool CheckColliderCanHitted(GameObject victim, Collider hitCollider);

    [NonSerialized] public CheckColliderCanHitted delCheckColliderCanHitted;

    private List<WeakReference<GameObject>> inCollectorGos = new List<WeakReference<GameObject>>();

    [Tooltip("施加给目标的buff")]public BuffSkillConfig buffSkill; 
    
    private int totalDamage;

    [Tooltip("攻击者，即制造这个收集器的游戏物体"), NonSerialized] public GameObject attacker;

    [NonSerialized] public SkillConfig skill;

    public List<WeakReference<GameObject>> GetInCollectorGos()
    {
        return inCollectorGos;
    }

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (null == rb)
        {
            rb = this.AddComponent<Rigidbody>();
           
        }

        rb.useGravity = false;

        
        if(lifeTime<=0)
        {
            return;            //防止collector只能在PlayableGraph结束后销毁，设置一个lifetime使其在behaviour结束后销毁
        }
        StartCoroutine(AutoDestroyCoroutine(lifeTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        ElfBase elfBase = other.gameObject.GetComponent<ElfBase>();
        if (elfBase != null && elfBase.isSummoning)
        {
            return;
        }

        if (!LayerMaskInclude(OnTriggerEnterLayer, other.gameObject.layer))
        {
            return;
        }

        if (null != HitEffect)
        {
            ApplyEffect(other.transform);
        }


        if (OnEnterCollector(other.gameObject))
        {
            //Debug.Log(other.gameObject);
            var targetLife = other.GetComponent<LifeBodyComponent>();
            if (null == targetLife)
            {
                targetLife = other.GetComponentInParent<LifeBodyComponent>();
            }

            if (targetLife != null)
            {
                int damage = SkillUtility.CaculateDamage(attacker, targetLife, skill);        //计算伤害

                targetLife.AddHP(-damage);
                totalDamage += damage > targetLife.CurrentHP ? targetLife.CurrentHP : damage;
            }

            //施加buff
            if(buffSkill != null)
            {
                SkillUtility.ApplyBuff(buffSkill, other.gameObject);
                //Debug.Log("apply");
            }
        }


    }

    public static bool CanAttack(GameObject attacker, GameObject target)
    {
        if (attacker == null || target == null)
            return false;

        if (attacker == target)
            return false;

        return true;
    }

    /// <summary>
    /// LayerMask中是否包含layer
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool LayerMaskInclude(LayerMask mask, int layer)
    {
        return (mask.value & 1 << layer) > 0;
    }

    private void ApplyEffect(Transform effectPoint)
    {
        var hitEffect = Instantiate(HitEffect, effectPoint);

        ParticleSystem particle = hitEffect.GetComponent<ParticleSystem>();

        particle.Play();
        
    }

    private bool OnEnterCollector(GameObject triggerGo)
    {
        foreach (var inCollectorGo in inCollectorGos)
        {
            inCollectorGo.TryGetTarget(out var rootGo);
            if (rootGo == triggerGo)
            {
                return false;
            }
        }

        WeakReference<GameObject> go = new WeakReference<GameObject>(triggerGo);
        if (!inCollectorGos.Contains(go))
        {
            inCollectorGos.Add(go);
        }

        return true;
    }
    private void OnExitCollector(GameObject triggerGo)
    {
        for (var index = 0; index < inCollectorGos.Count; index++)
        {
            var inCollectorGo = inCollectorGos[index];
            inCollectorGo.TryGetTarget(out var rootGo);
            if (rootGo == triggerGo)
            {
                inCollectorGos.Remove(inCollectorGo);
            }
        }
    }

    private IEnumerator AutoDestroyCoroutine(float limit)
    {
        yield return new WaitForSeconds(limit);

        Destroy(gameObject);
    }
    public void PlayerDamage()
    {
        PointsManager.Instance.OnPlayerDamage(totalDamage);
    }
}
