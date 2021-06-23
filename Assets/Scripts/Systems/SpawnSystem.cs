using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class SpawnSystem : ComponentSystem
{
    private Unity.Mathematics.Random random;
    private int counter = 10;

    protected override void OnCreate()
    {
        random = new Unity.Mathematics.Random(10);
    }

    protected override void OnUpdate()
    {
        if (counter != 10) counter++;

        Entities.ForEach((ref PrefabHolder prefab) =>
        {
            if (prefab.rocksSpawned > 0)
            {
                prefab.rocksSpawned -= 1;
                Entity spawnRock = EntityManager.Instantiate(prefab.prefabRock);
                EntityManager.SetComponentData(spawnRock, new GoForward { dir = new float3(random.NextFloat(-5, 5), random.NextFloat(-5, 5), 0) });
                EntityManager.SetComponentData(spawnRock, new Translation { Value = new float3(random.NextFloat(-5,5), random.NextFloat(-5, 5), 0) });
            }
            
            if (Input.GetKey("space"))
            {
                if (counter >= 10)
                {
                    Entity spawnLaser = EntityManager.Instantiate(prefab.prefabLaser);
                    ComponentDataFromEntity<Translation> DataTranslation = GetComponentDataFromEntity<Translation>(true);
                    Translation playerPos = DataTranslation[prefab.player];
                    ComponentDataFromEntity<Rotation> DataRotation = GetComponentDataFromEntity<Rotation>(true);
                    Rotation playerRot = DataRotation[prefab.player];
                    EntityManager.SetComponentData(spawnLaser, new Rotation { Value = playerRot.Value.value });
                    EntityManager.SetComponentData(spawnLaser, new Translation { Value = playerPos.Value });
                    Vector3 newDir = math.mul(playerRot.Value, new Vector3(0, 5, 0));
                    EntityManager.SetComponentData(spawnLaser, new GoForward { dir = newDir });

                    counter = 0;
                }
            }
            
        });
    } 
}
