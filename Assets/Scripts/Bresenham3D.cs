using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bresenham3D : MonoBehaviour {

    public class Node3D
    {
        public int x;
        public int y;
        public int z;
        public Node3D(int _x,int _y,int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public Node3D(Node3D node3D)
        {
            x = node3D.x;
            y = node3D.y;
            z = node3D.z;
        }
    }

    public GameObject _cube;
    public Node3D StartPos = new Node3D(0,0,0);
    public Node3D EndPos = new Node3D(0,0,0);

    public Text startX;
    public Text startY;
    public Text startZ;
    public Text endX;
    public Text endY;
    public Text endZ;
    public Text Note;

    private int p0;             //第一个决策值
    private int pCurrent;
    private int pNext;

    private int pZ0;
    private int pZCurrent;
    private int pZNext;

    private int dx;
    private int dy;
    private int dz;
    private float k1;
    private float k2;

    private Node3D currentNode;
    private Node3D nextNode = new Node3D(0, 0, 0);

    #region 输入初始点x,y与终点x,y坐标
    public void EnterStartX()
    {
        try
        {
            Note.gameObject.SetActive(false);
            StartPos.x = int.Parse(startX.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterStartY()
    {
        try
        {
            Note.gameObject.SetActive(false);
            StartPos.y = int.Parse(startY.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterStartZ()
    {
        try
        {
            Note.gameObject.SetActive(false);
            StartPos.z = int.Parse(startZ.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterEndX()
    {
        try
        {
            Note.gameObject.SetActive(false);
            EndPos.x = int.Parse(endX.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterEndY()
    {
        try
        {
            Note.gameObject.SetActive(false);
            EndPos.y = int.Parse(endY.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    public void EnterEndZ()
    {
        try
        {
            Note.gameObject.SetActive(false);
            EndPos.z = int.Parse(endZ.text);
        }
        catch (FormatException e)
        {
            Note.gameObject.SetActive(true);
            Note.text = "Input should be integer!";
        }
    }
    #endregion

    #region 画线
    public void DrawLine()
    {
        dx = EndPos.x - StartPos.x;
        dy = EndPos.y - StartPos.y;
        dz = EndPos.z - StartPos.z;
        k1 = (float)dy / (float)dx;
        k2 = (float)dz / (float)dy;

        if (EndPos.x < StartPos.x )
        {
            Node3D temp = new Node3D(0, 0, 0);
            temp = EndPos;
            EndPos = StartPos;
            StartPos = temp;
        }

        currentNode = new Node3D(StartPos.x, StartPos.y,StartPos.z);
        Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, currentNode.z), Quaternion.Euler(new Vector3(0, 0, 0)));
  
        Debug.Log("K = " + k1);

        //斜率大于1
        if (k1 >= 1 && dx != 0)
        {
            p0 = 2 * dx - dy;
            pCurrent = p0;
            pZ0 = 2 * dy - dz;
            pZCurrent = pZ0;
            NextNode1();
        }
        //斜率大于0小于1
        else if (k1 >= 0 && k1 < 1)
        {
            p0 = 2 * dy - dx;
            pCurrent = p0;
            pZ0 = 2 * dz - dy;
            pZCurrent = pZ0;
            NextNode2();
        }
        //斜率大于-1小于0
        else if (k1 >= -1 && k1 < 0)
        {
            p0 = dx - 2 * dy;
            pCurrent = p0;
            pZ0 = dy - 2 * dz;
            pZCurrent = pZ0;
            NextNode3();
        }
        //斜率小于-1
        else if (k1 < -1 && dx != 0)
        {
            p0 = 2 * dx + dy;
            pCurrent = p0;
            pZ0 = 2 * dy + dz;
            pZCurrent = pZ0;
            NextNode4();
        }
        //斜率不存在
        else
        {
            if(dz == 0)
            {
                Debug.Log("???");
                NextNode5();
            }
            else
            {
                p0 = 2 * dx - dy;
                pCurrent = p0;
                pZ0 = 2 * dy - dz;
                pZCurrent = pZ0;
                NextNode1();
            }
           
        }
    }
    #endregion

    #region 斜率大于1
    private void NextNode1()
    {
        while (currentNode.x <= EndPos.x && currentNode.y <= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dx;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dx - 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;

            if (pZCurrent < 0)
            {
                nextNode.z = currentNode.z + 1;
                pZNext = pZCurrent + 2 * dy;
            }
            else
            {
                nextNode.z = currentNode.z + 1;
                pZNext = pZCurrent + 2 * dy - 2 * dz;
            }
            pZCurrent = pZNext;
            currentNode.z = nextNode.z;

            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, currentNode.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion

    #region 斜率大于0小于1
    private void NextNode2()
    {
        while (currentNode.x <= EndPos.x && currentNode.y <= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y;
                pNext = pCurrent + 2 * dy;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dy - 2 * dx;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;

            if (pZCurrent < 0)
            {
                nextNode.z = currentNode.z;
                pZNext = pZCurrent + 2 * dz;
            }
            else
            {
                nextNode.z = currentNode.z + 1;
                pZNext = pZCurrent + 2 * dz - 2 * dy;
            }
            pZCurrent = pZNext;
            currentNode.z = nextNode.z;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, currentNode.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion

    #region 斜率大于-1小于0
    private void NextNode3()
    {
        while (currentNode.x <= EndPos.x && currentNode.y >= EndPos.y )
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y;
                pNext = pCurrent - 2 * dy;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y - 1;
                pNext = pCurrent - 2 * dx - 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;
            if (pZCurrent < 0)
            {
                nextNode.z = currentNode.z;
                pZNext = pZCurrent - 2 * dz;
            }
            else
            {
                nextNode.z = currentNode.z - 1;
                pZNext = pZCurrent - 2 * dy - 2 * dz;
            }
            pZCurrent = pZNext;
            currentNode.z = nextNode.z;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, currentNode.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion

    #region 斜率小于-1
    private void NextNode4()
    {
        while (currentNode.x <= EndPos.x && currentNode.y >= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x;
                nextNode.y = currentNode.y - 1;
                pNext = pCurrent + 2 * dx;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y - 1;
                pNext = pCurrent + 2 * dx + 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;
            if (pZCurrent < 0)
            {
                nextNode.z = currentNode.z - 1;
                pZNext = pZCurrent + 2 * dy;
            }
            else
            {
                nextNode.z = currentNode.z - 1;
                pZNext = pZCurrent + 2 * dy + 2 * dz;
            }
            pZCurrent = pZNext;
            currentNode.z = nextNode.z;
            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, currentNode.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion

    #region 斜率不存在
    private void NextNode5()
    {
        while (currentNode.x <= EndPos.x && currentNode.y <= EndPos.y)
        {
            if (pCurrent < 0)
            {
                nextNode.x = currentNode.x;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dx;
            }
            else
            {
                nextNode.x = currentNode.x + 1;
                nextNode.y = currentNode.y + 1;
                pNext = pCurrent + 2 * dx - 2 * dy;
            }
            pCurrent = pNext;
            currentNode.x = nextNode.x;
            currentNode.y = nextNode.y;

            if (pZCurrent < 0)
            {
                nextNode.z = currentNode.z;
                pZNext = pZCurrent + 2 * dy;
            }
            else
            {
                nextNode.z = currentNode.z;
                pZNext = pZCurrent + 2 * dy - 2 * dz;
            }
            pZCurrent = pZNext;
            currentNode.z = nextNode.z;

            Instantiate(_cube, new Vector3(currentNode.x, currentNode.y, currentNode.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
    #endregion
}
