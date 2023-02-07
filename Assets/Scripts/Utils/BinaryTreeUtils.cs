using UnityEngine;
using System.Collections.Generic;
using SnakeMaze.BSP;
using SnakeMaze.Structures;

namespace SnakeMaze.Utils
{
    public static class BinaryTreeUtils<T>
    {
        public static string InOrder(BinaryTree<T> t, int level)
        {
            string datastring = "";
            if (t != null)
            {
                datastring += string.Format("{0}:", level);
                datastring += "Left(" + InOrder(t.Left, level + 1) + ")"
                            + "[" + t.Root + "]"
                            + "Right(" + InOrder(t.Right, level + 1) + ")";
            }

            return string.Format("{0}", datastring);
        }

        //It shows the tree in order in an horizontal way to ease the interpretation of its structure.
        public static string InOrderHorizontal(BinaryTree<T> t, int level)
        {
            string datastring = "";

            if (t != null)
            {
                if (t.Left != null)
                    datastring = string.Concat(datastring, BinaryTreeUtils<T>.InOrderHorizontal(t.Left, level + 1));

                for (int i = 1; i <= level; i++)  //Space to identify the different levels/childs of a node
                    datastring += "     ";

                datastring += " " + level.ToString() + ":";
                datastring = string.Concat(datastring, "<Root(", t.Root.ToString(), ">");
                //datastring += "<Root(" + t.Root.ToString() + ">";
                datastring = string.Concat(datastring, "\n");
                //datastring += "\n";

                if (t.Right != null)
                    datastring += BinaryTreeUtils<T>.InOrderHorizontal(t.Right, level + 1);
            }

            datastring = datastring.Replace("\\n", "\n"); //This is because \n is interpreted as '\\n' in a string. 
                                                          //it is a problem of string management in Unity.
            return string.Format("{0}", datastring);
        }

        public static void GetAllChildren(BinaryTree<T> tree, ref List<T> nodeList)
        {
            if (tree != null)
            {
                if (tree.IsALeaf())
                {
                    nodeList.Add(tree.Root);
                    return;
                }

                if (tree.Left != null)
                {
                    GetAllChildren(tree.Left, ref nodeList);
                }

                if (tree.Right != null)
                {
                    GetAllChildren(tree.Right, ref nodeList);
                }
            }
        }

        public static void DrawGizmosPartitions(BinaryTree<BSPData> t)
        {
            BSPData root;
            if (t != null)
            {
                root = t.Root;
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(root.Center, new Vector3(root.PartitionBounds.size.x, root.PartitionBounds.size.y, 0f));

                if (t.Left != null)
                    DrawGizmosPartitions(t.Left);

                if (t.Right != null)
                    DrawGizmosPartitions(t.Right);
            }
        }

        public static void DrawGizmosCorridorList(List<Corridor> corridorList)
        {
            if (corridorList != null && corridorList.Count > 0)
            {
                Gizmos.color = Color.blue;

                foreach (Corridor c in corridorList)
                {
                    Gizmos.DrawWireCube(c.Center, new Vector3(c.Width, c.Height, 0f));
                }
            }
        }

        public static void DrawGizmosRoomList(List<Room> roomList)
        {
            if (roomList != null && roomList.Count > 0)
            {
                Gizmos.color = Color.red;

                foreach (Room r in roomList)
                {
                    Gizmos.DrawWireCube(r.Center, new Vector3(r.Size.x,r.Size.y,0));
                }
            }
        }
    }
}