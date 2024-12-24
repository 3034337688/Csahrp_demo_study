using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    //���Ͻǵ�һ����λ��
    public int beginX;
    public int beginY;

    //֮��ÿһ����֮���ƫ��λ��
    public int offestX;
    public int offestY;

    //��ͼ�Ŀ��
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

        //����������
        for(int i=0;i<mapW;++i)
        {
            for(int j=0;j<mapH;++j)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Renderer renderer = obj.GetComponent<Renderer>();
                renderer.material = myMaterial;
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);//�浽�ֵ���

                obj.transform.position = new Vector3(beginX + i * offestX, beginY + j * offestY, 0);

                //�õ������ж��ǲ����赲
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
                    // �������
                    beginPos = clickedPos;
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                    Debug.Log("�������Ϊ: " + info.collider.gameObject.name);
                }
                else if (endPos == Vector2.right * -1)
                {
                    // �����յ�
                    endPos = clickedPos;
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow; // ��������һ����ɫ���ʱ�ʾ�յ�
                    Debug.Log("�յ�����Ϊ: " + info.collider.gameObject.name);

                    // Ѱ·
                    List<AStarNode> path = AStarManager.instance.FindPath(beginPos, endPos);
                    if (path != null && path.Count > 0)
                    {
                        Debug.Log("Ѱ·�ɹ�");
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
                        Debug.Log("Ѱ·ʧ��");
                    }

                    if(path==null)
                    {
                        //Debug.Log(path.Count);
                        Debug.Log("���Ѱ·�㷨��һ������");
                    }

                    // ���������յ㣬Ϊ��һ��Ѱ·��׼��
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
                //��¼��ʼ��
                
                if(beginPos==Vector2.right*-1)
                {
                    string[] strs = info.collider.gameObject.name.Split("_");
                    //�õ����е�λ��
                    beginPos = new Vector2(int.Parse(strs[0]),int.Parse(strs[1]));

                    //�ѵ�����Ķ����Ϊ��ɫ
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                    Debug.Log(info.collider.gameObject.name);

                }

                else
                {
                    string[] strs = info.collider.gameObject.name.Split("_");
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));


                    //Ѱ·����
                    List<AStarNode> list = AStarManager.instance.FindPath(beginPos, endPos);
                    if (list != null)
                    {
                        Debug.Log("Ѱ�ҳɹ�");

                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                    else
                    {
                        Debug.Log("Ѱ·û���ҳɹ�");
                    }
                }

            }
        }
 * 
 */
