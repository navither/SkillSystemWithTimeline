using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class ShooterPlayableBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public ShooterPlayableAsset playableAsset;
    public Transform target;

    Bullet bulletCpt;
    SkillCpt ownerSkill;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        ownerSkill = ownerGo.GetComponent<SkillCpt>();

        target = ownerSkill.SkillTarget;

        if(target == null)
        {
            //Debug.Log("not found enemy");
        }
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (null == target)
        {
            return;
        }

        CreateBullet();

    }

    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - ownerGo.transform.position;
        //Debug.Log(direction);
        direction.y = 0f; //确保角色只在水平方向旋转

        if (direction != Vector3.zero)
        {
            // 使用LookRotation方法将角色朝向目标位置
            Quaternion rotation = Quaternion.LookRotation(direction);
            ownerGo.transform.rotation = rotation;
        }
    }

    public void CreateBullet()
    {
        Transform firePoint = ownerSkill.firePoint;
        
        if (null == firePoint)
        {
            //Debug.Log(ownerGo + "无开火点");
            firePoint = ownerGo.transform;
        }

        var bullet = Object.Instantiate(playableAsset.bullet, firePoint.position, Quaternion.identity);

        if (null == bullet.GetComponent<Bullet>())
        {
            bulletCpt = bullet.AddComponent<Bullet>();            
        }

        bulletCpt.SetTarget(target.transform);
        bulletCpt.damage = ownerSkill.curSkillConfig.Damage;
        bulletCpt.buffSkill = playableAsset.buffSkill;
        bulletCpt.shooter = ownerGo;
        bulletCpt.HitEffect = playableAsset.HitEffect;
        bulletCpt.speed = playableAsset.bulletSpeed;
        bulletCpt.targetPositionOffset = playableAsset.targetPositionOffset;
    }
}
