using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : Node
{


    public virtual int[] GetFirstParameter()
    {
        return new int[2];
    }
    public virtual int[] GetSecondParameter()
    {
        return new int[2];
    }
    public virtual int[] GetThirdParameter()
    {
        return new int[2];
    }
    public virtual int GetFourthParameter()
    {
        return 0;
    }

    public virtual SnappingEnum IsSideSnapped()
    {
        return SnappingEnum.SNAP_SIDE;
    }

    public virtual int GetProbability()
    {
        return 0;
    }

}
