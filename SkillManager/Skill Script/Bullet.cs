using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField]
    private Transform target;
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float angleStandard = 45.0f;  //角度
    private Vector3 direction = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;

    [Tooltip("子弹附带的buff")]
    public BuffSkillConfig buffSkill;
    [Tooltip("产生子弹的游戏物体")]
    public GameObject shooter;
    [Tooltip("受击特效")] 
    public GameObject HitEffect;
    [Tooltip("目标位置偏移")]
    public Vector3 targetPositionOffset;

    LifeBodyComponent targetLife;

    private void Start()
    {
        targetLife = SkillUtility.GetLifebody(target.gameObject);
        //target = GameObject.Find("target").transform;
        damage = SkillUtility.CaculateDamage(shooter, targetLife, damage);
    }

    private void Update()
    {


    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {       
        if (target != null)
        {
            targetPosition = target.position + targetPositionOffset;    //加上偏移量，使得子弹击中位置正确 = target.position + targetPositionOffset;    //加上偏移量，使得子弹击中位置正确
        }

        direction = targetPosition - transform.position;
        float currentDistSqr = (transform.position - targetPosition).sqrMagnitude;
        if (currentDistSqr < 0.25f)
        {
            if (target != null)
            {
                DealDamage();
                if (null != HitEffect)
                {
                    ApplyEffect(targetPosition);
                }
            }
            Destroy(gameObject);
        }
        transform.Translate(speed * Time.deltaTime * direction.normalized);
    }

    public void SetTarget(Transform Target)
    {
        target = Target;
    }

    private void DealDamage()
    {

        if (targetLife != null)
        {

            targetLife.AddHP(-damage);
            if (buffSkill != null)
            {
                SkillUtility.ApplyBuff(buffSkill, target.gameObject);
            }
        }
        else
        {
            //Debug.Log("目标缺少LifeBodyComponent");
            return;
        }
        

        if (targetLife.IsDead())
        {

        }

    }

    /// <summary>
    /// 生成子弹击中目标的视觉特效
    /// </summary>
    /// <param name="effectPoint"></param>
    private void ApplyEffect(Transform effectPoint)
    {
        var hitEffect = Instantiate(HitEffect, effectPoint);

        ParticleSystem particle = hitEffect.GetComponent<ParticleSystem>();

        particle.Play();
    }
    private void ApplyEffect(Vector3 effectPoint)
    {
        var hitEffect = Instantiate(HitEffect, effectPoint, Quaternion.identity);

        ParticleSystem particle = hitEffect.GetComponent<ParticleSystem>();

        particle.Play();
    }
}
