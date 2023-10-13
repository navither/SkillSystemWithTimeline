using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillReleaseDirector : MonoBehaviour
{
    private SkillCpt skillCpt;
    private GameObject mouseDirector;
    private GameObject rangeDirector;
    private SkillConfig config;


    private void Start()
    {
        skillCpt = gameObject.GetComponent<SkillCpt>();
        config = null;
    }

    private void CreateMouseDirector(int i)
    {
        if (mouseDirector != null) DestoryRangeDirector();
        mouseDirector = GameObject.Instantiate(config.MouseDirector);
    }
    private void CreateRangeDirector(int i)
    {
        if (rangeDirector != null) DestoryMouseDirector();
        rangeDirector = GameObject.Instantiate(config.RangeDirector, transform.Find("PlayerModel"));
    }

    private void DestoryRangeDirector()
    {
        Destroy(rangeDirector);
        rangeDirector = null;
    }
    private void DestoryMouseDirector()
    {
        Destroy(mouseDirector);
        mouseDirector = null;
    }

    private void Update()
    {
        if (config != null)
        {
            if (config.releaseType == ReleaseType.Circle) CircleUpdate();
        }
    }
    

    private void CircleUpdate()
    {
        if (mouseDirector != null)
        {
            if (inCircle && skillTime < 0.9f)
            {
                skillTime += Time.deltaTime;
                return;
            }
            else if (inCircle)
            {
                CursorManager.Instance.ShowCursor();
                inCircle = false;
            }
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
            if ((raycastHit.point - transform.Find("PlayerModel").position).magnitude <= 15)
            {
                mouseDirector.transform.position = raycastHit.point;
            }
            else
            {
                DestoryRangeDirector();
                DestoryMouseDirector();
                CameraManager.Instance.HeadVisionBySkill = false;
                CameraManager.Instance.VCamToBack();
                inCircle = false;
                skillTime = 0.0f;
            }

        }
        if (rangeDirector != null)
        {
            rangeDirector.transform.position = gameObject.transform.Find("PlayerModel").transform.position;
        }
    }
    private float skillTime = 0.0f;
    private bool inCircle;
    public void ReleaseSkillStart(int i)
    {
        if (skillCpt.CheckRelease(i))
        {
            config = skillCpt.GetSkillConfig(i);
            switch (skillCpt.GetReleaseType(i))
            {
                case ReleaseType.Defaut:
                    skillCpt.ReleaseSkill(i);
                    break;
                case ReleaseType.Self:
                    skillCpt.ReleaseSkill(i);
                    break;
                case ReleaseType.Circle:
                    CreateMouseDirector(i);
                    CreateRangeDirector(i);
                    CameraManager.Instance.HeadVisionBySkill = true;
                    CameraManager.Instance.VCamToHead();
                    CursorManager.Instance.HideCursor();
                    skillTime = 0.0f;
                    inCircle = true;
                    break;
            }
        }
    }
    public void ReleaseSkillCancel(int i)
    {
        if (skillCpt.CheckRelease(i))
        {
            config = skillCpt.GetSkillConfig(i);
            switch (skillCpt.GetReleaseType(i))
            {
                case ReleaseType.Defaut:
                    break;
                case ReleaseType.Self:
                    break;
                case ReleaseType.Circle:
                    if (mouseDirector!=null ) // 处理按下前未冷却的情况
                    {
                        DestoryMouseDirector();
                        DestoryRangeDirector();
                        CameraManager.Instance.HeadVisionBySkill = false;
                        CameraManager.Instance.VCamToBack();
                        inCircle = false;
                        skillTime = 0.0f;
                        if (InputManager.instance.playerInput.PlayerInputMap.Exit.WasPressedThisFrame())
                        {
                            return;
                        }
                        CursorManager.Instance.HideCursor();
                        skillCpt.ReleaseSkill(i);
                    }
                    break;
            }
        }
    }
}
