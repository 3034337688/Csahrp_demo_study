using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������A*������
/// </summary>
public class AStarManager
{
    private static AStarManager _instance;
    public static AStarManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AStarManager();
            }
            return _instance;
        }
    }

    public AStarNode[,] nodes;
    private List<AStarNode> openList=new List<AStarNode>();
    private List<AStarNode> closeList=new List<AStarNode>();

    private int mapW;
    private int mapH;


    /// <summary>
    /// ��ʼ����ͼ����Ϣ
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMapInfo(int w, int h)
    {
        nodes = new AStarNode[w, h];//���������Ĵ�С

        //��¼
        this.mapW = w;
        this.mapH = h;



        for (int i = 0; i < w; ++i)
        {
            for (int j = 0; j < h; ++j)
            {
                AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 20 ? E_Node_Type.Stop : E_Node_Type.Walk);//���������һ���赲
                nodes[i, j] = node;
            }
        }
    }

    /// <summary>
    /// Ѱ·�������ṩ���ⲿʹ��
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public List<AStarNode> FindPath(Vector2 startPos, Vector3 endPos)
    {
        //�жϴ���ĵ��ǲ��ǺϷ��ģ������ڵ�ͼ��Χ�ڣ�Ҫ�����赲

        //�жϴ���ĵ��ǲ��ǺϷ���
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("��ʼ���߽����ٵ�ͼ��");
            return null;
        }



        //�ǲ����赲

        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];

        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("��ʼ����߽��������赲");
            return null;
        }

        //��տ�ʼ�͹ر��б�,�����һ�ε����ݣ�����Ӱ��Ѱ·
        openList.Clear();
        closeList.Clear();

        //�ѿ�ʼ�����رյ��б��м�
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);


        while(true)
        {
            //���ú�������ÿ���ڵ��Ӧ������
            FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);//���ϣ�б�Խ�
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1f, start, end);//�ϣ�����1
            FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1f, start, end);//���1
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1f, start, end);//�ұ�1
            FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);//����1.4
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1f, start, end);//��1
            FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);//����1.4


            //��·���ж�
            if (openList.Count == 0)
            {
                Debug.Log("��·");
                return null;
            }

            //ѡ�������б� �����б���Ѱ��������С�Ľڵ�
            openList.Sort(SortOpenList);


            //����ر��б�
            closeList.Add(openList[0]);

            //�Ƴ������б�ĵ�ոյ��Ǹ��ڵ�
            start = openList[0];//�ѿ�ʼ�ĵ��ɸո��Ѿ�����ɲ��ҵĵ������һ�ֵļ���
            openList.RemoveAt(0);

            //�������������յĵ㣬���ǵõ����յĽ��
            if (start == end)
            {
                Debug.Log("�ҵ������յ�·��");
                //���ݸ��ڵ�
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father!=null)
                {
                    path.Add(end.father);
                    end = end.father;
                }

                //�б�ת��api
                path.Reverse();
                return path;
            }

        }






        //return null;
    }

    private int SortOpenList(AStarNode a, AStarNode b)//Listί�е����򷽷�
    {
        if (a.f > b.f)
            return 1;
        else
            return -1;

    }


    /// <summary>
    /// �Ѹ����ĵ���뿪���б���
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode father, AStarNode end)
    {

        //�жϱ߽�
        if (x < 0 || x >= mapW || y < 0 || y >= mapH) return;


        AStarNode node = nodes[x, y];

        //�ж��ǲ��Ǳ߽磬�赲,�ǲ����ڿ��������ǹر��б��м�
        if (node == null || node.type == E_Node_Type.Stop||closeList.Contains(node)||openList.Contains(node)) return;

        //����f��ֵ  f=g+h������Ѱ·������

        //��¼������
        node.father = father;

        //����g��ֵ��Ŀǰ�������ľ���=�Ҿ��븸�ڵ����+�Ҹ��׾������ľ���
        node.g = father.g + g;

        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;


        //��ӵ������б���
        openList.Add(node);


    }

}
