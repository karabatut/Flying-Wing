using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Color Parameters")]
public class ColorParameters : ScriptableObject
{
    public Color primaryColor;
    public int primaryColorProbability;
    public Color secondaryColor;
    public int secondaryColorProbability;
    public Color tertiaryColor;
    public int tertiaryColorProbability;
    public Color quternaryColor;
    public int quternaryColorProbability;

}
