using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Vector2 CamPosition2D=> new Vector2(transform.position.x,transform.position.z);
    public Vector3 CamPosition3D => transform.position;

    public float CamSpeed;


    public void Move(Vector2 addition)
    {
        transform.position += new Vector3(addition.x,0,addition.y) * Time.deltaTime * CamSpeed;
    }

    public void Teleport(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }
}
