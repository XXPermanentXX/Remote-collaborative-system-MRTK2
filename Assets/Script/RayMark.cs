using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

public class RayMark : MonoBehaviour
{
    private static int mPhysicsLayer = 0;
    //获取空间网格所在的图层
    private static int GetSpatialMeshMask()
    {
        if (mPhysicsLayer == 0)
        {
            //获取SpatialAwarenessSystem的配置文件
            var spatialMappingConfig = CoreServices.SpatialAwarenessSystem.ConfigurationProfile as MixedRealitySpatialAwarenessSystemProfile;
            if (spatialMappingConfig != null)
            {
                //遍历所有现有的配置文件
                foreach (var config in spatialMappingConfig.ObserverConfigurations)
                {
                    //将配置文件类型转化为MixedRealitySpatialAwarenessMeshObserverProfile
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
        //定义被击中的物体
        RaycastHit hitInfo;
        var cameraTransform = Camera.main.transform;
        //发射射线
        UnityEngine.Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, 10, GetSpatialMeshMask());
        if (hitInfo.point != null)
        {
            //生成物体
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