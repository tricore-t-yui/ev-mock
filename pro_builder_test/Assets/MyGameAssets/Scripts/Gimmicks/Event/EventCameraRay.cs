using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCameraRay : MonoBehaviour
{
    Transform myCamera;
    float lastCameraRayTime;
    // Start is called before the first frame update
    void Start()
    {
        lastCameraRayTime = Time.timeSinceLevelLoad;
        myCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad - lastCameraRayTime > 0.5f)
        {
            var ray = new Ray(myCamera.position, myCamera.forward);
            var hits = Physics.RaycastAll(ray, 100.0f);
            EventTrigger closestTrigger = null;
            float closest = float.MaxValue;
            foreach (var item in hits)
            {
                var trigger = item.transform.GetComponent<EventTrigger>();
                if(trigger != null && trigger.IsViewCollide)
                {
                    var len = (trigger.transform.position - myCamera.position).magnitude;
                    if(len < trigger.ViewColideRange && len < closest)
                    {
                        closest = len;
                        closestTrigger = trigger;
                    }
                }
            }
            if(closestTrigger != null)
            {
                closestTrigger.OnViewEnter();
            }
        }
    }
}
