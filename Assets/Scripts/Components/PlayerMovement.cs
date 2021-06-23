using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerMovement : IComponentData
{
    public float speed;
    public float rotation;
    //public KeyCode forward;
    //public KeyCode right;
    //public KeyCode left;
}
