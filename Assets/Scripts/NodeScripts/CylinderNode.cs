using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CylinderNode : BaseNode
{
    [Input] public bool sideSnap;
    [Input] public bool frontSnap;
    [Input] public bool aftSnap;
    [Output] public bool exitSideSnap;
    [Output] public bool exitFrontSnap;
    [Output] public bool exitAftSnap;

    public int lengthMin;
    public int lengthMax;
    public int subdivisionMin;
    public int subdivisionMax;
    public int radiusMin;
    public int radiusMax;
    public int snapPoints;
    public int probabilityPercentage;

    public override object GetValue(NodePort port)
    {
        return true;
    }

    public override int[] GetFirstParameter()
    {
        int[] lengthMargins = { lengthMin, lengthMax };
        return lengthMargins;
    }
    public override int[] GetSecondParameter()
    {
        int[] subdivisionMargins = { subdivisionMin, subdivisionMax };
        return subdivisionMargins;
    }
    public override int[] GetThirdParameter()
    {
        int[] radiusMargins = { radiusMin, radiusMax };
        return radiusMargins;
    }
    public override int GetFourthParameter()
    {
        return snapPoints;
    }
    public override SnappingEnum IsSideSnapped()
    {
        SnappingEnum snappingEnum = SnappingEnum.SNAP_SIDE;
        if (GetPort("frontSnap").GetInputValue<bool>())
        {
            snappingEnum = SnappingEnum.SNAP_FRONT;
        }
        if (GetPort("sideSnap").GetInputValue<bool>())
        {
            snappingEnum = SnappingEnum.SNAP_SIDE;
        }
        if (GetPort("aftSnap").GetInputValue<bool>())
        {
            snappingEnum = SnappingEnum.SNAP_AFT;
        }
        return snappingEnum;
    }
    public override int GetProbability()
    {
        return probabilityPercentage;
    }

}
