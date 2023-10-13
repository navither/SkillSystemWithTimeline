using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class CommonMoneyPlayableBehaviour : PlayableBehaviour
{
    /// <summary>
    /// 妖精实例
    /// </summary>
    public GameObject OwnerObject;
    private AlchemistElf alchemistElf;
    private GameObject LightEffectPrefab;
    private List<Vector3> CommonMoneyEffectPositions;
    private Transform modelTransform;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        alchemistElf = OwnerObject.GetComponent<AlchemistElf>();
        LightEffectPrefab = alchemistElf.LightEffectPrefabs[(int)AlchemistElf.AlchemistSkillType.Common];
        CommonMoneyEffectPositions = alchemistElf.CommonMoneyEffectPositions;
        modelTransform = alchemistElf.Model.transform;
        CreateLight();
    }

    private float CostTime = 0.8f;
    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        CostTime += info.deltaTime;
        if (CostTime >= 0.8f)
        {
            CostTime = 0.0f;
            CreateCommonMoney(3, 5);
        }
    }

    private int CommonBallIndex;
    private void CreateCommonMoney(int min, int max)
    {
        alchemistElf.CreateMoney(min, max, modelTransform.TransformPoint(CommonMoneyEffectPositions[CommonBallIndex]));
        ++CommonBallIndex;
        CommonBallIndex %= 3;
    }

    private void CreateLight()
    {
        Object.Instantiate(LightEffectPrefab, OwnerObject.transform);
    }

}
