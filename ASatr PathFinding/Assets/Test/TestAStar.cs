using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    //左上角第一个的位置
    public int beginX;
    public int beginY;

    //之后每一个的之间的偏移位置
    public int offestX;
    public int offestY;

    //地图的快高
    public int mapW;
    public int mapH;

    public Material myMaterial;
    public Material red;
    public Material yellow;
    public Material green;

    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
    private Vector2 beginPos = Vector2.right * -1;
    private Vector2 endPos = Vector2.right * -1;

    public bool isNewPath;

    // Start is called before the first frame update
    void Start()
    {

        AStarManager.instance.InitMapInfo(mapW, mapH);

        //创建立方体
        for(int i=0;i<mapW;++i)
        {
            for(int j=0;j<mapH;++j)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Renderer renderer = obj.GetComponent<Renderer>();
                renderer.material = myMaterial;
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);//存到字典中

                obj.transform.position = new Vector3(beginX + i * offestX, beginY + j * offestY, 0);

                //得到格子判断是不是阻挡
                AStarNode node = AStarManager.instance.nodes[i, j];
                if(node.type==E_Node_Type.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out info, 1000))
            {
                string[] strs = info.collider.gameObject.name.Split('_');
                Vector2 clickedPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));

                if (beginPos == Vector2.right * -1)
                {
                    // 设置起点
                    beginPos = clickedPos;
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                    Debug.Log("起点设置为: " + info.collider.gameObject.name);
                }
                else if (endPos == Vector2.right * -1)
                {
                    // 设置终点
                    endPos = clickedPos;
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow; // 假设您有一个红色材质表示终点
                    Debug.Log("终点设置为: " + info.collider.gameObject.name);

                    // 寻路
                    List<AStarNode> path = AStarManager.instance.FindPath(beginPos, endPos);
                    if (path != null && path.Count > 0)
                    {
                        Debug.Log("寻路成功");
                        foreach (AStarNode node in path)
                        {
                            if (cubes.TryGetValue(node.x + "_" + node.y, out GameObject cube))
                            {
                                cube.GetComponent<MeshRenderer>().material = green;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("寻路失败");
                    }

                    if(path==null)
                    {
                        //Debug.Log(path.Count);
                        Debug.Log("这个寻路算法有一点问题");
                    }

                    // 重置起点和终点，为下一次寻路做准备
                    beginPos = Vector2.right * -1;
                    endPos = Vector2.right * -1;


                }
            }
        }
    }
}

/*
 * 
 * if(Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out info,1000))
            {
                //记录开始点
                
                if(beginPos==Vector2.right*-1)
                {
                    string[] strs = info.collider.gameObject.name.Split("_");
                    //得到行列的位置
                    beginPos = new Vector2(int.Parse(strs[0]),int.Parse(strs[1]));

                    //把点击到的对象改为黄色
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                    Debug.Log(info.collider.gameObject.name);

                }

                else
                {
                    string[] strs = info.collider.gameObject.name.Split("_");
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));


                    //寻路方法
                    List<AStarNode> list = AStarManager.instance.FindPath(beginPos, endPos);
                    if (list != null)
                    {
                        Debug.Log("寻找成功");

                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                    else
                    {
                        Debug.Log("寻路没有找成功");
                    }
                }

            }
        }
 * 
 */
