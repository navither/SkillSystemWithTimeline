using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SelfHealingPlayableBehaviour : PlayableBehaviour
{
    public GameObject ownerGo;

    public GameObject VFX;

    public SelfHealingPlayableAsset playableAsset;

    private GameObject temp;

    LifeBodyComponent lifeBody;
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
    }
    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        lifeBody =ownerGo.GetComponent<LifeBodyComponent>();  
    }
    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (lifeBody.CurrentHP < lifeBody.MaxHP)
        {
            if (playableAsset.skillConfig != null)
            {

                playableAsset.healingAmout = playableAsset.skillConfig.Damage;
            }
            if (ownerGo.transform.Find("PlayerModel") != null)
            {
                if (ownerGo.transform.Find("PlayerModel").transform.Find("Healing(Clone)") == null)
                {
                    temp = GameObject.Instantiate(VFX, ownerGo.transform.Find("PlayerModel").transform);
                    Vector3 p = temp.transform.position;
                    temp.transform.position = new Vector3(p.x, p.y + 1, p.z);
                }
            }

            lifeBody.AddHP(playableAsset.healingAmout);
        }
        else
        {
            if (ownerGo.transform.Find("PlayerModel") != null)
            {
                Transform t = ownerGo.transform.Find("PlayerModel").transform.Find("Healing(Clone)");
                if (t != null)
                {
                    GameObject.Destroy(t.gameObject);
                }
            }

        }
    }
    public bool CanUse()
    {
        return true;
    }
}
