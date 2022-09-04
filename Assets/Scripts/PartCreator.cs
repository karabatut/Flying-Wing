using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Random = UnityEngine.Random;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PartCreator : MonoBehaviour
{

    public MeshRenderer meshRenderer;
    public ShapeGrammar ruleNodes;
    public ColorParameters colorParameters;
    List<BaseNode> grammarRule;
    Dictionary<BaseNode, Part> partsOfNodes = new Dictionary<BaseNode, Part>();
    Dictionary<Part, NodePort> partsOfNodesConnected = new Dictionary<Part, NodePort>();
    void Start()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        createRuleSet();
        PartsTree partsTree = new PartsTree();

        foreach (BaseNode node in grammarRule)
        {
            GameObject partObject = new GameObject();

            Part part;
            if (node.GetType() == typeof(CylinderNode))
            {
                part = new Cylinder();
                partObject.name = "Cylinder";
            }
            else if(node.GetType() == typeof(TriangleNode))
            {
                part = new Triangle();
                partObject.name = "Triangle";
            }
            else if(node.GetType() == typeof(SphereNode))
            {
                part = new Sphere();
                partObject.name = "Sphere";
            }
            else if (node.GetType() == typeof(RectangleNode))
            {
                part = new Rectangle();
                partObject.name = "Rectangle";
            }
            else if(node.GetType() == typeof(TorusNode))
            {
                part = new Torus();
                partObject.name = "Torus";
            }
            else
            {
                part = new Cone();
                partObject.name = "Cone";
            }

            Part newPart = part.CreatePart(node.GetFirstParameter(), node.GetSecondParameter(), node.GetThirdParameter(), node.GetFourthParameter(), colorParameters);

            partObject.AddComponent<MeshFilter>();
            partObject.AddComponent<MeshRenderer>();

            partObject.transform.parent = this.transform;
            partObject.GetComponent<MeshFilter>().mesh = newPart.mesh;
            partObject.GetComponent<MeshRenderer>().material = meshRenderer.material;

            newPart.partObject = partObject;

            partsOfNodes.Add(node, newPart);



        }

        foreach (BaseNode tempNode in partsOfNodes.Keys)
        {
            if(tempNode.GetInputPort("frontSnap").Connection == null && tempNode.GetInputPort("sideSnap").Connection == null && tempNode.GetInputPort("aftSnap").Connection == null)
            {
                partsTree.startTree(partsOfNodes[tempNode]);
            }
        }

        for (int i = 0; i < partsOfNodes.Count; i++)
        {
            for (int j = 0; j < partsOfNodes.Count ; j++)
            {
                if (partsOfNodes.ElementAt(j).Key.GetInputPort("frontSnap").Connection != null)
                {
                    BaseNode tempNode = partsOfNodes.ElementAt(j).Key.GetInputPort("frontSnap").Connection.node as BaseNode;
                    if (partsTree.contains(partsOfNodes[tempNode]) && !partsTree.contains(partsOfNodes[partsOfNodes.ElementAt(j).Key]))
                    {
                        NodePort port = partsOfNodes.ElementAt(j).Key.GetInputPort("frontSnap").Connection;
                        partsOfNodesConnected.Add(partsOfNodes[partsOfNodes.ElementAt(j).Key], port);
                        partsOfNodes[partsOfNodes.ElementAt(j).Key].isSideSnapped = SnappingEnum.SNAP_FRONT;
                        partsTree.addPartToTree(partsOfNodes[tempNode], partsOfNodes[partsOfNodes.ElementAt(j).Key]);
                    }
                }
                if (partsOfNodes.ElementAt(j).Key.GetInputPort("sideSnap").Connection != null)
                {
                    BaseNode tempNode = partsOfNodes.ElementAt(j).Key.GetInputPort("sideSnap").Connection.node as BaseNode;
                    if (partsTree.contains(partsOfNodes[tempNode]) && !partsTree.contains(partsOfNodes[partsOfNodes.ElementAt(j).Key]))
                    {
                        NodePort port = partsOfNodes.ElementAt(j).Key.GetInputPort("sideSnap").Connection;
                        partsOfNodesConnected.Add(partsOfNodes[partsOfNodes.ElementAt(j).Key], port);
                        partsOfNodes[partsOfNodes.ElementAt(j).Key].isSideSnapped = SnappingEnum.SNAP_SIDE;
                        partsTree.addPartToTree(partsOfNodes[tempNode], partsOfNodes[partsOfNodes.ElementAt(j).Key]);
                    }
                }
                if (partsOfNodes.ElementAt(j).Key.GetInputPort("aftSnap").Connection != null)
                {
                    BaseNode tempNode = partsOfNodes.ElementAt(j).Key.GetInputPort("aftSnap").Connection.node as BaseNode;
                    if (partsTree.contains(partsOfNodes[tempNode]) && !partsTree.contains(partsOfNodes[partsOfNodes.ElementAt(j).Key]))
                    {
                        NodePort port = partsOfNodes.ElementAt(j).Key.GetInputPort("aftSnap").Connection;
                        partsOfNodesConnected.Add(partsOfNodes[partsOfNodes.ElementAt(j).Key], port);
                        partsOfNodes[partsOfNodes.ElementAt(j).Key].isSideSnapped = SnappingEnum.SNAP_AFT;
                        partsTree.addPartToTree(partsOfNodes[tempNode], partsOfNodes[partsOfNodes.ElementAt(j).Key]);
                    }
                }
                if (partsOfNodes.ElementAt(j).Key.GetInputPort("centerSnap").Connection != null)
                {
                    BaseNode tempNode = partsOfNodes.ElementAt(j).Key.GetInputPort("centerSnap").Connection.node as BaseNode;
                    if (partsTree.contains(partsOfNodes[tempNode]) && !partsTree.contains(partsOfNodes[partsOfNodes.ElementAt(j).Key]))
                    {
                        NodePort port = partsOfNodes.ElementAt(j).Key.GetInputPort("centerSnap").Connection;
                        partsOfNodesConnected.Add(partsOfNodes[partsOfNodes.ElementAt(j).Key], port);
                        partsOfNodes[partsOfNodes.ElementAt(j).Key].isSideSnapped = SnappingEnum.SNAP_CENTER;
                        partsTree.addPartToTree(partsOfNodes[tempNode], partsOfNodes[partsOfNodes.ElementAt(j).Key]);
                    }
                }
            }
            
        }




        List<TreeItem> treeItems = partsTree.getTreeItems();

        foreach (TreeItem item in treeItems)
        {
            if (item.connectedObject != null)
            {

                Vector3 connectingPoint = new Vector3(0,0,0);

                NodePort port = partsOfNodesConnected[item.thisObject];
                Vector3 connectedObjPos = item.connectedObject.partObject.transform.position;
                if (port.fieldName == "exitSideSnap")
                {
                    int snapCountConnected = item.connectedObject.sideSnapPoints.Count;
                    int randomsnapConnected = Random.Range(0, snapCountConnected);
                    connectingPoint = item.connectedObject.sideSnapPoints[randomsnapConnected];
                    connectingPoint = connectingPoint + connectedObjPos;

                }
                else if(port.fieldName == "exitFrontSnap")
                {
                    connectingPoint = item.connectedObject.frontSnapPoints[0];
                    connectingPoint = connectingPoint + connectedObjPos;
                }
                else if(port.fieldName == "exitAftSnap")
                {
                    connectingPoint = item.connectedObject.frontSnapPoints[1];
                    connectingPoint = connectingPoint + connectedObjPos;
                }
                else if (port.fieldName == "exitCenterSnap")
                {
                    connectingPoint = connectedObjPos;
                }


                if (item.thisObject.isSideSnapped == SnappingEnum.SNAP_SIDE)
                {
                    int snapCountThisObj = item.thisObject.sideSnapPoints.Count;
                    int randomsnapThisObj = Random.Range(0, snapCountThisObj);

                    Vector3 translateVector = connectingPoint - item.thisObject.sideSnapPoints[randomsnapThisObj];
                    Vector2 thisObjectPos = new Vector2(item.thisObject.sideSnapPoints[randomsnapThisObj].x + translateVector.x, item.thisObject.sideSnapPoints[randomsnapThisObj].z + translateVector.z);
                    item.thisObject.partObject.transform.Translate(translateVector);
                    

                    Vector3 connectedPositionNormal = new Vector3(connectingPoint.x - connectedObjPos.x,0, connectingPoint.z - connectedObjPos.z);
                    Vector3 thisPositionNormal = new Vector3(item.thisObject.partObject.transform.position.x - connectingPoint.x,0, item.thisObject.partObject.transform.position.z - connectingPoint.z);

                    float angle = Vector3.SignedAngle(connectedPositionNormal, thisPositionNormal, Vector3.up);
                    item.thisObject.partObject.transform.RotateAround(connectingPoint, new Vector3(0f, 1, 0f), -angle);
                }
                else if(item.thisObject.isSideSnapped == SnappingEnum.SNAP_CENTER)
                {
                    Vector3 translateVector = connectingPoint - item.thisObject.partObject.transform.position;
                    item.thisObject.partObject.transform.Translate(translateVector);
                }
                else 
                {
                    int snapCount = item.connectedObject.frontSnapPoints.Count;
                    
                    int thisSnapPoint;
                    if(item.thisObject.isSideSnapped == SnappingEnum.SNAP_FRONT)
                    {
                        thisSnapPoint = 0;
                    }
                    else
                    {
                        thisSnapPoint = 1;
                    }
                    Vector3 translateVector = connectingPoint - item.thisObject.frontSnapPoints[thisSnapPoint];
                    Vector2 thisObjectPos = new Vector2(item.thisObject.frontSnapPoints[0].x, item.thisObject.frontSnapPoints[0].z);
                    item.thisObject.partObject.transform.Translate(translateVector);
                    //item.thisObject.partObject.transform.Rotate(item.connectedObject.partObject.transform.rotation.eulerAngles);
                
                }
                item.thisObject.partObject.transform.parent = item.connectedObject.partObject.transform;
            }
        }

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log(elapsedTime);

    }

    public float getAngle(Vector2 me, Vector2 target)
    {
        return Mathf.Atan2(target.y - me.y, target.x - me.x) * (180 / Mathf.PI);
    }

   
    private void createRuleSet()
    {
        grammarRule = new List<BaseNode>();

        foreach (BaseNode baseNode in ruleNodes.nodes)
        {
            if (baseNode.GetInputPort("frontSnap").Connection != null)
            {
                BaseNode tempNode = baseNode.GetInputPort("frontSnap").Connection.node as BaseNode;
                if (!grammarRule.Contains(tempNode))
                {
                    continue;
                }
            }
            if (baseNode.GetInputPort("sideSnap").Connection != null)
            {
                BaseNode tempNode = baseNode.GetInputPort("sideSnap").Connection.node as BaseNode;
                if (!grammarRule.Contains(tempNode))
                {
                    continue;
                }
            }
            if (baseNode.GetInputPort("aftSnap").Connection != null)
            {
                BaseNode tempNode = baseNode.GetInputPort("aftSnap").Connection.node as BaseNode;
                if (!grammarRule.Contains(tempNode))
                {
                    continue;
                }
            }
            if (baseNode.GetInputPort("centerSnap").Connection != null)
            {
                BaseNode tempNode = baseNode.GetInputPort("centerSnap").Connection.node as BaseNode;
                if (!grammarRule.Contains(tempNode))
                {
                    continue;
                }
            }

            int probability = baseNode.GetProbability();

            int chance = Random.Range(0, 100);
            if(chance <= probability)
            {
                grammarRule.Add(baseNode);
            }
        }

    }
}