using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class OutofBoundsSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((ref LocalToWorld position, ref Translation translation) =>
        {
            if (position.Value.c3.y > 5) translation.Value.y = -5;
            if (position.Value.c3.y < -5) translation.Value.y = 5;
            if (position.Value.c3.x > 10) translation.Value.x = -10;
            if (position.Value.c3.x < -10) translation.Value.x = 10;

        }).Run();

        return default;
    }
}
