using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementInfo", menuName = "density_charming/ElementInfo", order = 1)]
public class ElementInfo : ScriptableObject
{
    public const float baseSize = 0.05f;
    public float size = 1f;

    public Substance substance = null;
}
