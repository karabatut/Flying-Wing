using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TorusNode : BaseNode
{

    [Input] public bool sideSnap;
    [Input] public bool frontSnap;
    [Input] public bool aftSnap;
    [Input] public bool centerSnap;
    [Output] public bool exitSideSnap;
    [Output] public bool exitFrontSnap;
    [Output] public bool exitAftSnap;
    [Output] public bool exitCenterSnap;

    public int outerRadiusMin;
    public int outerRadiusMax;
    public int innerRadiusMin;
    public int innerRadiusMax;
    public int thirdParamMin;
    public int thirdParamMax;
    public int snapPoints;
    public int probabilityPercentage;

    public override object GetValue(NodePort port)
    {
        return true;
    }

    public override int[] GetFirstParameter()
    {
        int[] outerRadiusMargins = { outerRadiusMin, outerRadiusMax };
        return outerRadiusMargins;
    }
    public override int[] GetSecondParameter()
    {
        int[] innerRadiusMargins = { innerRadiusMin, innerRadiusMax };
        return innerRadiusMargins;
    }
    public override int[] GetThirdParameter()
    {
        int[] thirdParam = { thirdParamMin, thirdParamMax };
        return thirdParam;
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
        if (GetPort("centerSnap").GetInputValue<bool>())
        {
            snappingEnum = SnappingEnum.SNAP_CENTER;
        }
        return snappingEnum;
    }
    public override int GetProbability()
    {
        return probabilityPercentage;
    }
}
