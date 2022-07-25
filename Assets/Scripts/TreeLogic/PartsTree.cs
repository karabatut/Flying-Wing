using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsTree 
{
    List<TreeItem> treeItems = new List<TreeItem>();

    public void startTree(Part firstItem)
    {
        if(isTreeEmpty())
        {
            TreeItem newItem = new TreeItem();
            newItem.thisObject = firstItem;
            newItem.connectedObject = null;
            treeItems.Add(newItem);
        }
    }

    public void addPartToTree(Part connectedObject, Part thisObject)
    {
        foreach (TreeItem item in treeItems)
        {
            if(item.thisObject == connectedObject)
            {
                TreeItem newItem = new TreeItem();
                newItem.thisObject = thisObject;
                newItem.connectedObject = connectedObject;

                treeItems.Add(newItem);
                break;
            }
        }
    }

    public bool contains(Part partObject)
    {
        foreach (TreeItem item in treeItems)
        {
            if(item.thisObject == partObject)
            {
                return true;
            }
        }
        return false;
    }

    public List<TreeItem> getTreeItems()
    {
        return treeItems;
    }

    public void clearTreeList()
    {
        treeItems.Clear();
    }

    public bool isTreeEmpty()
    {
        if(treeItems.Count == 0)
        {
            return true;
        }
        return false;
    }
}
