using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public Vector2 e1;
    public Vector2 e2;
    public Vector2 e3;

    public Vector3 v1;
    public Vector3 v2;
    public Vector3 v3;

    public Vector3 worldPoint;

    public Vector3 worldV1 { get { return worldPoint + v1; } }
    public Vector3 worldV2 { get { return worldPoint + v2; } }
    public Vector3 worldV3 { get { return worldPoint + v3; } } 
}
