﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ConeNode : BaseNode
{

    [Input] public bool sideSnap;
    [Input] public bool frontSnap;
    [Input] public bool aftSnap;
    [Input] public bool centerSnap;
    [Output] public bool exitSideSnap;
    [Output] public bool exitFrontSnap;
    [Output] public bool exitAftSnap;
    [Output] public bool exitCenterSnap;

    public int lengthMin;
    public int lengthMax;
    public int baseRadiusMin;
    public int baseRadiusMax;
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
        int[] temp = { 0, 0 };
        return temp;
    }
    public override int[] GetThirdParameter()
    {
        int[] radiusMargins = { baseRadiusMin, baseRadiusMax };
        return radiusMargins;
    }
    public override int GetFourthParameter()
    {
        return 0;
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
