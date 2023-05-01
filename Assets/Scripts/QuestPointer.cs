using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointer : MonoBehaviour
{
    public Camera uiCamera;

    public Vector3 targetPosition;
    public RectTransform pointerRectTransfrom;

    void LateUpdate(){
        Vector3 previousPosition = new Vector3(500, 500);

        foreach(GameObject obj in SpawnBox.instance.Boxes){
            if(!obj.activeInHierarchy) continue;

            if(Vector3.Distance(playerManager.instance.transform.position, obj.transform.position) < Vector3.Distance(playerManager.instance.transform.position, previousPosition)){
                previousPosition = targetPosition = obj.transform.position;
            }
        }

        CalculatePosition();
    }

    void CalculatePosition(){
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVectorFloat(dir);
        pointerRectTransfrom.localEulerAngles = new Vector3(0, 0, angle);

        float borderSize = 300f;
        Vector3 TargetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffset = TargetPositionScreenPoint.x <= borderSize || TargetPositionScreenPoint.x >= Screen.width - borderSize || TargetPositionScreenPoint.y <= borderSize || TargetPositionScreenPoint.y <= Screen.height - borderSize;

        if(isOffset){
            Vector3 cappedTargetScreenPosition = TargetPositionScreenPoint;
            if(cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if(cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if(cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if(cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransfrom.position = pointerWorldPosition;
            pointerRectTransfrom.localPosition = new Vector3(pointerRectTransfrom.localPosition.x, pointerRectTransfrom.localPosition.y, 0);
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
