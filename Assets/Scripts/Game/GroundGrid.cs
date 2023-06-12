using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGrid : MonoBehaviour
{
    public SpriteRenderer sr_select;

    public void UpdateSelectFrame(bool b)
    {
        sr_select.enabled = b;
    }
}
