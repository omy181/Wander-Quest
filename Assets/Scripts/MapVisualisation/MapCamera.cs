using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Vector2 CamPosition2D=> new Vector2(transform.position.x,transform.position.z);
}
