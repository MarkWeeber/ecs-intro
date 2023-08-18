using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;

public class SimpleEntity : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Awake()
    {
        Debug.Log("AWOKED");
    }
    
    public void OnCreate()
    {
        Debug.Log("CREATED");
    }

    public void Start()
    {
        Debug.Log("STARTED");
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log("CONVERTED");
        //dstManager.AddComponentData<SimpleObjectData>(entity, new SimpleObjectData());
    }
}

public struct SimpleObjectData : IComponentData
{
    public float value;
}
