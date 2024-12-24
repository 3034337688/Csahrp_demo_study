using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum E_Node_Type//格子类型的枚举
{
    Walk,
    Stop,
}

/// <summary>
/// A*的格子类别
/// </summary>
public class AStarNode
{
    public float f;//寻路消耗
    public float g;//距离起点的距离
    public float h; //距离终点的距离

    public int x;  //格子对象的坐标
    public int y;

    public AStarNode father;
    public E_Node_Type type;

    public AStarNode(int x,int y,E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }


}
