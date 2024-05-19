using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3());
        transform.Translate(Vector3.right * (20f * Time.deltaTime));
    }
}
