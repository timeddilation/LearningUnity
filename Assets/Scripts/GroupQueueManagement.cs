using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupQueueManagement : MonoBehaviour {

    public QueueUp[] queuesInGroup;
    public int unoccupiedPottiesInGroup;

    private void Start()
    {
        queuesInGroup = transform.GetComponentsInChildren<QueueUp>();
    }

    //void CheckForQueueTransfers()
    //{
    //    int queueSizeDifference = 0;
    //    foreach (QueueUp queue in queuesInGroup)
    //    {
    //        queueSizeDifference -= queue.queuedSims.Count();
    //    }
    //    queueSizeDifference = Mathf.Abs(queueSizeDifference);
    //}
}
