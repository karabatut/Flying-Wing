using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SphereNode : BaseNode
{
    [Input] public bool sideSnap;
    [Input] public bool frontSnap;
    [Input] public bool aftSnap;
    [Output] public bool exitSideSnap;
    [Output] public bool exitFrontSnap;
    [Output] public bool exitAftSnap;

    public int radiusMin;
    public int radiusMax;
    public int subdivisionMin;
    public int subdivisionMax;
    public int baseRadiusMin;
    public int baseRadiusMax;
    public int snapPoints;
    public int probabilityPercentage;

    public override object GetValue(NodePort port)
    {
        return true;
    }

    public override int[] GetFirstParameter()
    {
        int[] lengthMargins = { radiusMin, radiusMax };
        return lengthMargins;
    }
    public override int[] GetSecondParameter()
    {
        int[] subdivisionMargins = { subdivisionMin, subdivisionMax };
        return subdivisionMargins;
    }
    public override int[] GetThirdParameter()
    {
        int[] radiusMargins = { baseRadiusMin, baseRadiusMax };
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
