using UnityEngine;

public class UIRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward,Time.deltaTime*400);
    }
}
