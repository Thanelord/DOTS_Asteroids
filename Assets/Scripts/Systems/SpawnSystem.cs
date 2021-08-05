using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class SpawnSystem : ComponentSystem
{
    
    private Unity.Mathematics.Random random;
    private int counter = 10;

    private EntityQuery spawnerQuery;
    private PrefabHolder prefabH;

    protected override void OnCreate()
    {
        random = new Unity.Mathematics.Random(10);
    }

    protected override void OnStartRunning()
    {
        spawnerQuery = GetEntityQuery(ComponentType.ReadOnly<PrefabHolder>());
        prefabH = spawnerQuery.GetSingleton<PrefabHolder>();

        while (prefabH.rocksSpawned > 0)
        {
            prefabH.rocksSpawned -= 1;
            Entity spawnRock = EntityManager.Instantiate(prefabH.prefabRock);
            EntityManager.SetComponentData(spawnRock, new GoForward { dir = new float3(random.NextFloat(-5, 5), random.NextFloat(-5, 5), 0) });
            EntityManager.SetComponentData(spawnRock, new Translation { Value = new float3(random.NextFloat(-5, 5), random.NextFloat(-5, 5), 0) });
        }
    }

    protected override void OnUpdate()
    {
        if (counter != 10) counter++;

        Entities.ForEach((ref PrefabHolder prefab) =>
        {
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

        Entities.
            WithAll<Destroy>().
            WithAll<SplitTag>().
            ForEach((ref Destroy destroy, ref Translation transl) =>
        {
            if (destroy.dead && !destroy.split)
            {
                for (int i = 0; i < 2; i++)
                {
                    Entity spawnShard = EntityManager.Instantiate(prefabH.prefabRockM);
                    EntityManager.SetComponentData(spawnShard, new GoForward { dir = new float3(random.NextFloat(-5, 5), random.NextFloat(-5, 5), 0) });
                    EntityManager.SetComponentData(spawnShard, new Translation { Value = new float3(transl.Value.x, transl.Value.y, 0) });
                }
                //Rok has split and can be destroyed
                destroy.split = true;
            }
        });
    }
    
}
