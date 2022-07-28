using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TriangleNode : BaseNode
{
    [Input] public bool sideSnap;
    [Input] public bool frontSnap;
    [Input] public bool aftSnap;
    [Input] public bool centerSnap;
    [Output] public bool exitSideSnap;
    [Output] public bool exitFrontSnap;
    [Output] public bool exitAftSnap;
    [Output] public bool exitCenterSnap;

    public int baseStartMin;
    public int baseStartMax;
    public int tipPositionXMin;
    public int tipPositionXMax;
    public int tipPositionYMin;
    public int tipPositionYMax;
    public int snapPoints;
    public int probabilityPercentage;

    public override object GetValue(NodePort port)
    {
       
        return true;
    }

    public override int[] GetFirstParameter()
    {
        int[] baseStartMargins = { baseStartMin, baseStartMax };
        return baseStartMargins;
    }
    public override int[] GetSecondParameter()
    {
        int[] tipPositionXMargins = { tipPositionXMin, tipPositionXMax };
        return tipPositionXMargins;
    }
    public override int[] GetThirdParameter()
    {
        int[] tipPositionYMargins = { tipPositionYMin, tipPositionYMax };
        return tipPositionYMargins;
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
