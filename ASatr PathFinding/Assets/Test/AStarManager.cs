using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例的A*管理器
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
    /// 初始化地图的信息
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMapInfo(int w, int h)
    {
        nodes = new AStarNode[w, h];//声明容器的大小

        //记录
        this.mapW = w;
        this.mapH = h;



        for (int i = 0; i < w; ++i)
        {
            for (int j = 0; j < h; ++j)
            {
                AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 20 ? E_Node_Type.Stop : E_Node_Type.Walk);//测试随机出一个阻挡
                nodes[i, j] = node;
            }
        }
    }

    /// <summary>
    /// 寻路方法，提供给外部使用
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public List<AStarNode> FindPath(Vector2 startPos, Vector3 endPos)
    {
        //判断传入的点是不是合法的，就是在地图范围内，要不是阻挡

        //判断传入的点是不是合法的
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("开始或者结束再地图外");
            return null;
        }



        //是不是阻挡

        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];

        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("开始点或者结束点事阻挡");
            return null;
        }

        //清空开始和关闭列表,清空上一次的数据，避免影响寻路
        openList.Clear();
        closeList.Clear();

        //把开始点放入关闭的列表中间
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);


        while(true)
        {
            //调用函数计算每个节点对应的消耗
            FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);//左上，斜对角
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1f, start, end);//上，正上1
            FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1f, start, end);//左边1
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1f, start, end);//右边1
            FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);//左下1.4
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1f, start, end);//下1
            FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);//右下1.4


            //死路的判断
            if (openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }

            //选出开启列表 排序列表中寻找消耗最小的节点
            openList.Sort(SortOpenList);


            //放入关闭列表
            closeList.Add(openList[0]);

            //移除开启列表的点刚刚的那个节点
            start = openList[0];//把开始的点变成刚刚已经的完成查找的点进行下一轮的计算
            openList.RemoveAt(0);

            //如果这个点事最终的点，就是得到最终的结果
            if (start == end)
            {
                Debug.Log("找到了最终的路径");
                //回溯父节点
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father!=null)
                {
                    path.Add(end.father);
                    end = end.father;
                }

                //列表翻转的api
                path.Reverse();
                return path;
            }

        }






        //return null;
    }

    private int SortOpenList(AStarNode a, AStarNode b)//List委托的排序方法
    {
        if (a.f > b.f)
            return 1;
        else
            return -1;

    }


    /// <summary>
    /// 把附近的点放入开启列表中
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode father, AStarNode end)
    {

        //判断边界
        if (x < 0 || x >= mapW || y < 0 || y >= mapH) return;


        AStarNode node = nodes[x, y];

        //判断是不是边界，阻挡,是不是在开启或者是关闭列表中间
        if (node == null || node.type == E_Node_Type.Stop||closeList.Contains(node)||openList.Contains(node)) return;

        //计算f数值  f=g+h，计算寻路的消耗

        //记录父对象
        node.father = father;

        //计算g数值。目前距离起点的距离=我距离父节点距离+我父亲距离起点的距离
        node.g = father.g + g;

        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;


        //添加到开启列表中
        openList.Add(node);


    }

}
