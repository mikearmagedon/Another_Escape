using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<CheckPoint> CheckPoints { get { return checkPoints; } }
    public CheckPoint CurCheckPoint { get { return checkPoints.Any() ? checkPoints[curIndex] : null; } }
    public static CheckPointManager Instance { get { return instance; } }

    List<CheckPoint> checkPoints = new List<CheckPoint>();
    int curIndex = 0;
    static CheckPointManager instance = null;

    void Awake()
    {
        instance = this;

        // find all my check points children
        for (int i = 0; i < transform.childCount; ++i)
        {
            CheckPoint checkpoint = transform.GetChild(i).GetComponent<CheckPoint>();
            //checkpoint.onTrigger += OnCheckPointTriggered;
            checkPoints.Add(checkpoint);

        }
    }

    public void OnCheckPointTriggered(CheckPoint newCheckPoint)
    {
        curIndex = checkPoints.IndexOf(newCheckPoint);
    }
}
