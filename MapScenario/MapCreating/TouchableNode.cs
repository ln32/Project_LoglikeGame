using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchableNode : MonoBehaviour
{
    public BoxCollider2D coll;
    Action<int> onClick;
    public int index;


    public void Setting(int _index, Action<int> _onClick)
    {
        coll = transform.AddComponent<BoxCollider2D>();
        coll.size *= 2.5f;
        onClick = _onClick;
        index = _index;
    }


    private void OnMouseUp()
    {
        onClick(index);
    }


    public void Destroy()
    {
        Destroy(coll);
        Destroy(this);
    }
}

