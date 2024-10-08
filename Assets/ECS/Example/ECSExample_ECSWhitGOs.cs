using System.Collections.Generic;
using UnityEngine;
using FlyEngine;

public class ECSExample_ECSWhitGOs : MonoBehaviour
{
    public int entityCount = 100;
    public float velocity = 10.0f;
    public GameObject prefab;

    private Dictionary<uint, GameObject> entities;

    void Start()
    {
        entities = new Dictionary<uint, GameObject>();
        for (int i = 0; i < entityCount; i++)
        {
            uint entityID = ECSManager.CreateEntity();
            ECSManager.AddComponent<PositionComponent>(entityID,new PositionComponent(0, -i, 0));
            ECSManager.AddComponent<VelocityComponent>(entityID,
                new VelocityComponent(velocity, Vector3.right.x, Vector3.right.y, Vector3.right.z));
            entities.Add(entityID, Instantiate(prefab, new Vector3(0, -i, 0), Quaternion.identity));
        }
    }

    void Update()
    {
        ECSManager.Tick(Time.deltaTime);
    }

    void LateUpdate()
    {
        foreach (KeyValuePair<uint, GameObject> entity in entities)
        {
            PositionComponent position = ECSManager.GetComponent<PositionComponent>(entity.Key);
            entity.Value.transform.SetPositionAndRotation(new Vector3(position.X, position.Y, position.Z), Quaternion.identity);
        }
    }
}
