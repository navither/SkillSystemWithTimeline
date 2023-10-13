using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class DefenseBuffBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;
    public DefenseBuffAsset playAsset;
    LifeBodyComponent lifeBody;
    GameObject VFX;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        lifeBody = SkillUtility.GetLifebody(ownerGo);
        lifeBody.defense += playAsset.defenseBonus;
        VFX=Object.Instantiate(playAsset.VFXPrefab, ownerGo.transform);
        //VFX.transform.position = new Vector3(VFX.transform.position.x, VFX.transform.position.y + ownerGo.GetComponent<BoxCollider>().size.y, VFX.transform.position.z);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        lifeBody.defense -= playAsset.defenseBonus;
        Object.Destroy(VFX);
    }
}
