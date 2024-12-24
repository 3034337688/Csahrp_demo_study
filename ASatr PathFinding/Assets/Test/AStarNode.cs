using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum E_Node_Type//�������͵�ö��
{
    Walk,
    Stop,
}

/// <summary>
/// A*�ĸ������
/// </summary>
public class AStarNode
{
    public float f;//Ѱ·����
    public float g;//�������ľ���
    public float h; //�����յ�ľ���

    public int x;  //���Ӷ��������
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
