using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ObjectBase : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        SetRotation();
    }

    protected void SetRotation()
    {
        Vector3 downDirection = transform.position - Vector3.zero;
        transform.rotation = Quaternion.FromToRotation(transform.up, downDirection);
    }
}
