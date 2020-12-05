using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrack : MonoBehaviour
{
    public enum eCubePosition
    {
        StartPos,
        Step1,
        Step2,
        Step3,
        Step4,

        max
    }

#pragma warning disable 0649
    [SerializeField]
    Transform[] _skillCubePosArr;
#pragma warning restore

    List<GameObject> _skillCubeList = new List<GameObject>();

    GameObject SpawnSkillcube()
    {
        GameObject skillcubePrefab = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "SkillCube");
        GameObject skillCube = Instantiate(skillcubePrefab, transform);
        skillCube.transform.position = _skillCubePosArr[(int)eCubePosition.StartPos].position;
        _skillCubeList.Add(skillCube);

        return skillCube;
    }

    public void InitSkillCube()
    {
        SpawnSkillcube();
    }

    public void ShowSkillPos(int[] skillPos)
    {
        for (int n = 0; n < _skillCubeList.Count; n++)
            _skillCubeList[n].SetActive(false);

        for(int n = 0; n < skillPos.Length; n++)
        {
            if(skillPos[n] == 1)
            {
                GameObject skillcube = GetSkillCube();

                if(skillcube != null)
                {
                    skillcube.transform.position = _skillCubePosArr[n].position;
                }
                else
                {
                    skillcube = SpawnSkillcube();
                    skillcube.transform.position = _skillCubePosArr[n].position;
                }

                skillcube.SetActive(true);
            }
        }
    }

    GameObject GetSkillCube()
    {
        for (int n = 0; n < _skillCubeList.Count; n++)
        {
            if (!_skillCubeList[n].activeSelf)
                return _skillCubeList[n];
        }

        return null;
    }
}
