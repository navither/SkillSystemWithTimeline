using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class TreeMoneyPlayableBehaviour : PlayableBehaviour
{
    public GameObject OwnerObject;
    private AlchemistElf alchemistElf;
    private GameObject LightEffectPrefab;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        alchemistElf = OwnerObject.GetComponent<AlchemistElf>();
        LightEffectPrefab = alchemistElf.LightEffectPrefabs[(int)AlchemistElf.AlchemistSkillType.Tree];
        CreateLight();
    }

    private float costTime = 3.0f;
    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        costTime += info.deltaTime;
        if (costTime >= 3.0f)
        {
            costTime = 0.0f;
            CreateTreeMoneyBalls();
        }
    }

    private void CreateTreeMoneyBalls()
    {
        Collider[] colliders = Physics.OverlapSphere(OwnerObject.transform.position, alchemistElf.attackRange, LayerMask.GetMask("Attackable"));
        foreach (Collider collider in colliders)
        {
            LifeBodyComponent lifeBodyComponent = collider.gameObject.GetComponent<LifeBodyComponent>();
            if (lifeBodyComponent.IsDead()) { continue; }
            alchemistElf.CreateMoneyBall(collider.gameObject);
            collider.gameObject.GetComponent<AttackableTree>().AttackByAlchemist(10);
        }
    }

    private void CreateLight()
    {
        Object.Instantiate(LightEffectPrefab, OwnerObject.transform);
    }
}
