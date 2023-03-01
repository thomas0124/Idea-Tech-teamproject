using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;

public class RhythmObject : MonoBehaviour
{
    private List<Transform> grandchildrenObj = new List<Transform>();
    private Dictionary<int, GameObject> activeObjects = new Dictionary<int, GameObject>();
    private int MinimumRangeValue;
    private int MaximumRangeValue;
    private bool isEnable = false;

    

    private void Start()
    {
        foreach(Transform child in GetComponentsInChildren<Transform>(true))
        {
            if(child.gameObject == this.gameObject) continue;
            grandchildrenObj.Add(child);
        }

        switch(this.gameObject.name)
        {
            case "Gold":
                MinimumRangeValue = 4;
                MaximumRangeValue = 9;
                break;
            case "Red":
                MinimumRangeValue = 2;
                MaximumRangeValue = 6;
                break;
            case "Blue":
                MinimumRangeValue = 3;
                MaximumRangeValue = 7;
                break;
            case "Gold2":
                MinimumRangeValue = 5;
                MaximumRangeValue = 9;
                break;
            case "Red2":
                MinimumRangeValue = 1;
                MaximumRangeValue = 5;
                break;
            case "Blue2":
                MinimumRangeValue = 3;
                MaximumRangeValue = 6;
                break;
        }

        StartCoroutine(Rhythm());
    }

    public void IsActive(bool enable)
    {
        isEnable = enable;
    }

    IEnumerator Rhythm()
    {
        while(true)
        {
            int activeNum;
            activeNum = UnityEngine.Random.Range(MinimumRangeValue, MaximumRangeValue);
            if(ActiveBackGroundObject(activeNum))
            {
                yield return new WaitForSeconds(0.1f);
                ActiveOrInactiveObjects(activeObjects, false);
                activeObjects.Clear();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private bool ActiveBackGroundObject(int index)
    {

        if(!isEnable) return false;

        foreach(Transform child in grandchildrenObj)
        {
            Match match = Regex.Match(child.name, "[0-9]+");
            if(match.Success)
            {
                int objectIndex = int.Parse(match.Value);

                if(objectIndex <= index)
                {
                    activeObjects.Add(objectIndex, child.gameObject);
                }
            }
            else
            {
                Debug.LogError("数値が含まれていないオブジェクトが存在します。");
                return false;
            }
        }

        var sortedDictionary = activeObjects.OrderBy(x => x.Key);
        ActiveOrInactiveObjects(activeObjects, true);
        return true;

    }

    private void ActiveOrInactiveObjects(Dictionary<int, GameObject> objects, bool enable)
    {

        foreach(var obj in objects)
        {
            GameObject targetObj = obj.Value;
            DOVirtual.DelayedCall (0.1f, ()=> targetObj.SetActive(enable), false);
        }

    }
}
