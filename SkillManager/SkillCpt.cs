using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SkillCpt : MonoBehaviour
{
    [Tooltip("所有技能列表")]
    public List<SkillConfig> SkillList;

    [Tooltip("当前的主动技能")]
    public SkillConfig curSkillConfig;

    [Tooltip("开火点")]
    public Transform firePoint;

    [Tooltip("指向性技能效果产生的位置"), NonSerialized]
    public Transform SkillTarget;

    [NonSerialized]
    public Vector3 SkillVector;

    private PlayableDirector director;

    private Transform playerTransform;

    [Tooltip("技能冷却倍速，默认为1倍"), SerializeField]
    private float coolDownTimeRate = 1f;

    [Tooltip("技能冷却字典，一个技能对于一个float冷却时间，冷却时间为0才可释放技能")]
    private Dictionary<SkillConfig, float> coolDownDict = new Dictionary<SkillConfig, float>();

    //被动播放的playableGraph
    private PlayableGraph passivePlayableGraph;
    //最后要摧毁掉的Playable Graph
    private List<PlayableGraph> graphList = new List<PlayableGraph>();

    public List<ActiveBuff> activeBuffList = new List<ActiveBuff>();

    private void Start()
    {
        //初始化PlayableDirector
        director = GetComponent<PlayableDirector>();
        director.playOnAwake = false;
        //director.playableAsset = curSkillConfig.playableAsset;

        if (GetComponent<Player>() != null)
        {
            playerTransform = GetComponent<Player>().playerModel.transform;
        }

        InitCoolDownDict();
    }

    private void Update()
    {
        ActivePassiveSkill();
    }
    /// <summary>
    /// 增加技能
    /// </summary>
    /// <param name="skill"></param>
    public void AddSkill(SkillConfig skill)
    {
        SkillList.Add(skill);
        coolDownDict.Add(skill, 0f);
    }

    /// <summary>
    /// 移除技能
    /// </summary>
    /// <param name="skill"></param>
    public void RemoveSkill(SkillConfig skill)
    {
        SkillList.Remove(skill);
        coolDownDict.Remove(skill);
    }

    /// <summary>
    /// 新加buff；增加已有buff层数
    /// </summary>
    /// <param name="buffSkill"></param>
    /// <param name="stack">层数</param>
    public void AddBuff(ActiveBuff buff, int stack = 1)
    {
        buff.stacks = stack;
        buff.duration = buff.buffData.duration;
        activeBuffList.Add(buff);
    }

    public void StackBuff(ActiveBuff buff, int stack = 1)
    {
        buff.stacks += stack;
        buff.duration = buff.buffData.duration;
    }

    /// <summary>
    /// 从buff列表中移除buff
    /// </summary>
    /// <param name="buffSkill"></param>
    public void RemoveBuff(ActiveBuff buff)
    {
        activeBuffList.Remove(buff);
    }
    /// <summary>
    /// 读取技能列表
    /// </summary>
    /// <returns></returns>
    public List<SkillConfig> GetSkillList()
    {
        return SkillList;
    }

    /// <summary>
    /// 获得特定技能的当前CD时间
    /// </summary>
    /// <param name="skillConfig"></param>
    /// <returns></returns>
    public float GetCurrentSkillCD(SkillConfig skillConfig)
    {
        return coolDownDict[skillConfig];
    }

    /// <summary>
    /// 获取技能的冷却字典
    /// </summary>
    /// <returns></returns>
    public Dictionary<SkillConfig, float> GetCoolDownList()
    {
        return coolDownDict;
    }

    /// <summary>
    /// 初始化所有技能的冷却时间为0，即所有技能一开始都可以释放
    /// </summary>
    private void InitCoolDownDict()
    {
        foreach (SkillConfig skillConfig in SkillList)
        {
            if (!coolDownDict.ContainsKey(skillConfig))
            {
                coolDownDict.Add(skillConfig, 0f);
            }
        }
    }

    public List<int> GetSkillsCanUse()
    {
        List<int> res = new List<int>();
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].isPassive) continue;
            if (coolDownDict[SkillList[i]] <= 0) res.Add(i);
        }
        return res;
    }

    #region 主角技能释放与技能指示器
    public ReleaseType GetReleaseType(int i)
    {
        return SkillList[i].releaseType;
    }

    public SkillConfig GetSkillConfig(int i)
    {
        return SkillList[i];
    }
    public bool CheckRelease(int i)
    {
        if (director.playableGraph.IsValid())
        {
            if (director.playableGraph.IsPlaying())
                return false;
        }

        curSkillConfig = SkillList[i];
        if (CheckTarget(i) && CanUseSkill(i))
        {
            return true;
        }

        return false;
    }

    private bool CheckTarget(int i)
    {

        if (ESkillTargetType.SelfTarget == SkillList[i].SkillTargetType)
        {
            SkillTarget = transform;
            return true;
        }
        if (ESkillTargetType.NonDirectivity == SkillList[i].SkillTargetType)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            //if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, SkillList[i].layerMask))
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
            {
                // Vector3 targetPoint = raycastHit.point;
                //if ((targetPoint - transform.Find("PlayerModel").position).magnitude <= 15)
                //{
                SkillVector = raycastHit.point;
                return true;
                //}
                //else
                //{
                //    Debug.Log("距离不足");
                //}
            }
            else
            {
                //Debug.Log("无效目标");
                return false;
            }
        }
        if (ESkillTargetType.Directivity == SkillList[i].SkillTargetType)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Enemy")))
            {
                SkillTarget = raycastHit.transform;
                return true;
            }
            return false;
        }
        return false;
    }

    /// <summary>
    /// 玩家释放技能
    /// </summary>
    /// <param name="i">技能列表index</param>
    public void ReleaseSkill(int i)
    {
        if (i >= 3)
        {
            //Debug.Log("技能不存在");         //  因为主角只有三个技能，所以index不能超过3          
            return;                          //  
        }

        curSkillConfig = SkillList[i];

        if (CanUseSkill(i))
        {
            CameraManager.Instance.FixVcam();
            director.playableAsset = SkillList[i].playableAsset;

            director.Play();

            StartCooldown(curSkillConfig);
        }
    }
    #endregion

    /// <summary>
    /// 尝试释放当前主动技能
    /// </summary>
    public void TryUseSkill()
    {
        if (CanUseSkill())
        {
            director.playableAsset = curSkillConfig.playableAsset;
            director.Play();
            StartCooldown(curSkillConfig);
        }
    }

    public SkillConfig GetSkillConfigBySkillID(int skillID)
    {
        foreach (var skillInfo in SkillList)
        {
            if (skillInfo != null)
            {
                var skillConfig = skillInfo as SkillConfig;
                if (skillConfig != null && skillConfig.SkillID == skillID)
                {
                    return skillConfig;
                }
            }
        }

        return null;
    }

    public void ReleaseSkill(int i, Transform target)
    {
        SkillTarget = target;
        if (director.playableGraph.IsValid())
        {
            if (director.playableGraph.IsPlaying())
            {
                return;
            }
        }

        curSkillConfig = SkillList[i];

        if (CanUseSkill(i))
        {

            director.playableAsset = SkillList[i].playableAsset;
            //确定目标
            director.Play();
            StartCooldown(curSkillConfig);
        }
    }


    public void ReleaseSkill(Transform target)
    {
        SkillTarget = target;
        TryUseSkill();
    }

    public void ReleaseSkill(Vector3 position)
    {
        SkillVector = position;
        TryUseSkill();
    }

    /// <summary>
    /// 停止当前在使用的主动技能
    /// </summary>
    public void StopCurrentSkill()
    {
        if(director==null) return;
        if (director.playableGraph.IsValid())
        {
            if (director.playableGraph.IsPlaying())
            {
                director.playableGraph.Destroy();
            }   
        }
    }

    /// <summary>
    /// 切换当前角色的动画
    /// </summary>
    /// <param name="asset"></param>
    public void SwitchPlay(PlayableAsset asset)
    {
        director.playableAsset = asset;
        Animator animator = gameObject.GetComponent<Animator>();
        TimelineAsset timelineAsset = (TimelineAsset)director.playableAsset;

        // 遍历 Timeline 中的轨道
        foreach (TrackAsset track in timelineAsset.GetOutputTracks())
        {
            // 检查是否为 Animation Track
            if (track is AnimationTrack)
            {
                director.SetGenericBinding(track, animator);
            }
        }
        director.Play();
    }

    /// <summary>
    /// 检测技能能否使用
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private bool CanUseSkill(int i)
    {
        if (coolDownDict[SkillList[i]] > 0)
        {
            return false;
        }

        //如果有别的主动技能在播放中，则无法使用其他主动技能
        if (director.playableGraph.IsValid())
        {
            if (director.playableGraph.IsPlaying())
                return false;
        }

        return true;
    }

    private bool CanUseSkill()
    {
        if (coolDownDict[curSkillConfig] > 0)
        {
            return false;
        }

        //如果有别的主动技能在播放中，则无法使用其他主动技能
        if (director.playableGraph.IsValid())
        {
            if (director.playableGraph.IsPlaying())
                return false;
        }

        return true;
    }

    /// <summary>
    /// 释放技能后进入冷却
    /// </summary>
    /// <param name="skill">释放的技能</param>
    /// <param name="cooldownTime">冷却时间</param>
    public void StartCooldown(SkillConfig skill)
    {
        coolDownDict[skill] = skill.CoolDownTime;

        StartCoroutine(CooldownCoroutine(skill));
    }

    private IEnumerator CooldownCoroutine(SkillConfig skill)
    {
        while (coolDownDict[skill] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            if (coolDownDict.ContainsKey(skill))        //cd后技能可能会被移除，此时结束cd协程
            {
                coolDownDict[skill] -= 0.1f * coolDownTimeRate;
            }
            else
            {
                break;
            }
            
        }

    }

    public void ApplySkillVFX()
    {
        var skillVFX = Instantiate(curSkillConfig.SkillVFX, playerTransform);

        //Destroy(skillVFX, 2f);
    }
    /// <summary>
    /// 激活技能列表中所有被动技能
    /// </summary>
    public void ActivePassiveSkill()
    {
        foreach (SkillConfig skill in SkillList)
        {
            if (skill.isPassive)
            {
                if (coolDownDict[skill] > 0)
                {
                    return;
                }
                passivePlayableGraph = PlayableGraph.Create(skill.SkillName);
                var my_playable = skill.playableAsset.CreatePlayable(passivePlayableGraph, gameObject);

                passivePlayableGraph.Play();
                StartCoroutine(CleanPlayableGraphCoroutine(passivePlayableGraph, 1f));
                StartCooldown(skill);
            }
        }
    }

    /// <summary>
    /// 激活buff效果:播放playablegraph
    /// </summary>
    /// <param name="buff"></param>
    public void ActivateBuff(ActiveBuff buff)
    {
        PlayableGraph Graph = PlayableGraph.Create(buff.buffData.SkillName);

        graphList.Add(Graph);//每生成一个Playable Graph就加入这个列表，当这个游戏物体被摧毁后，同时摧毁这些Playable Graph

        buff.buffData.playableAsset.CreatePlayable(Graph, gameObject);

        Graph.Play();

        buff.buffCoroutine = CleanBuffPlayableGraphCoroutine(Graph, buff);
        StartCoroutine(buff.buffCoroutine);
    }

    /// <summary>
    /// 一定时间后摧毁PlayableGraph
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator CleanPlayableGraphCoroutine(PlayableGraph graph, float duration)
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(0.1f);

            duration -= 0.1f;
        }
        graph.Destroy();
    }

    /// <summary>
    /// buff效果协程，用于计算buff持续时间并摧毁PlayableGraph, 从而结束buff效果
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="buff"></param>
    /// <returns></returns>
    private IEnumerator CleanBuffPlayableGraphCoroutine(PlayableGraph graph, ActiveBuff buff)
    {
        while (buff.duration > 0)
        {
            yield return new WaitForSeconds(0.1f);
            //Debug.Log(buff.duration);
            buff.duration -= 0.1f;
        }
        graph.Destroy();
        activeBuffList.Remove(buff);
    }

    private void OnDisable()
    {
        if (passivePlayableGraph.IsValid())
        {
            passivePlayableGraph.Destroy();
        }

    }
    private void OnDestroy()
    {
        if (passivePlayableGraph.IsValid())
        {
            passivePlayableGraph.Destroy();
        }

        foreach (PlayableGraph graph in graphList)
        {
            if (graph.IsValid())
            {
                graph.Destroy();
            }
        }
    }
    
    //当前是否正在播放技能
    public bool IsPlaying()
    {
        if (director.playableGraph.IsValid())
        {
            if (director.playableGraph.IsPlaying())
            {
                return true;
            }
        }
        return false;
    }
}

/// <summary>
/// 在激活的buff类，使用buff效果时需要实例化一个AcitveBuff
/// </summary>
public class ActiveBuff
{
    public BuffSkillConfig buffData;        //buff数据
    public int stacks;                      //buff目前层数
    public float duration;                  //buff目前持续时间

    [Tooltip("此buff关联的CleanPlayable Graph协程")]
    public IEnumerator buffCoroutine;

    public ActiveBuff(BuffSkillConfig buffSkill)
    {
        buffData = buffSkill;
        stacks = 1;
        duration = buffData.duration;
    }
}