using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//
public static class Player_Targeting {
    //
    public static void GetTargets(Vector3 pos) {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        List<GameObject> finalTargets = new List<GameObject>();
        Vector3 target;
        if (finalTargets.Count == 0) {
            for (int i = 0; i < targets.Length - 1; i++) {
                Vector3 targetPoint = Camera_Controller.instance.cam.WorldToViewportPoint(targets[i].transform.position);
                if (targetPoint.z > 0 && targetPoint.x > 0 && targetPoint.x < 1 && targetPoint.y > 0 && targetPoint.y < 1 == true) {
                    finalTargets.Add(targets[i]);
                }
                //if (targets[i].GetComponent<>() = null) {

                //}
            }
        }
        finalTargets.Sort(delegate (GameObject a, GameObject b) {
            return Vector2.Distance(Camera_Controller.instance.cam.WorldToViewportPoint(pos), Camera_Controller.instance.cam.WorldToViewportPoint(a.transform.position))
            .CompareTo(
              Vector2.Distance(Camera_Controller.instance.cam.WorldToViewportPoint(pos), Camera_Controller.instance.cam.WorldToViewportPoint(b.transform.position)));
        });
        target = finalTargets[0].transform.position;
        //if (targets.Length == 0) {
        //    Debug.Log("no targets");
        //}
        //
        //Debug.Log(finalTargets.Count);
    }
}
        
