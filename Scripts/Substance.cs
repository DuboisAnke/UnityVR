using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Substance", menuName = "density_charming/Substance", order = 0)]
public class Substance : ScriptableObject
{
    public int density = 1;
    public Material material = null;
}