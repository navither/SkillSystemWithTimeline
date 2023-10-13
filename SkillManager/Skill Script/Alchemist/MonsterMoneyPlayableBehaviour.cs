using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class MonsterMoneyPlayableBehaviour : PlayableBehaviour
{
    public GameObject OwnerObject;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        GameObject LightPrefab = OwnerObject.GetComponent<AlchemistElf>().LightEffectPrefabs[(int)AlchemistElf.AlchemistSkillType.Monster];
        Object.Instantiate(LightPrefab, OwnerObject.transform);
    }
}
