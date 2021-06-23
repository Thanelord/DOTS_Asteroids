using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GoForward : IComponentData
{
    public float speed;
    public Vector3 dir;
}
