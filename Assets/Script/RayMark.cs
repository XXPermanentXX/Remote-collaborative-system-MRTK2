using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

public class RayMark : MonoBehaviour
{
    private static int mPhysicsLayer = 0;
    //��ȡ�ռ��������ڵ�ͼ��
    private static int GetSpatialMeshMask()
    {
        if (mPhysicsLayer == 0)
        {
            //��ȡSpatialAwarenessSystem�������ļ�
            var spatialMappingConfig = CoreServices.SpatialAwarenessSystem.ConfigurationProfile as MixedRealitySpatialAwarenessSystemProfile;
            if (spatialMappingConfig != null)
            {
                //�����������е������ļ�
                foreach (var config in spatialMappingConfig.ObserverConfigurations)
                {
                    //�������ļ�����ת��ΪMixedRealitySpatialAwarenessMeshObserverProfile
                    var observerProfile = config.ObserverProfile as MixedRealitySpatialAwarenessMeshObserverProfile;
                    if (observerProfile != null)
                    {
                        mPhysicsLayer |= (1 << observerProfile.MeshPhysicsLayer);
                    }
                }
            }
        }
        return mPhysicsLayer;
    }

    public GameObject markPrefab;
    public void SetMarkOnSpatialMap() 
    {
        //���屻���е�����
        RaycastHit hitInfo;
        var cameraTransform = Camera.main.transform;
        //��������
        UnityEngine.Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, 10, GetSpatialMeshMask());
        if (hitInfo.point != null)
        {
            //��������
            GameObject.Instantiate(markPrefab, (Vector3)hitInfo.point, Quaternion.identity);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SetMarkOnSpatialMap();
        }
    }
}