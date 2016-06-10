using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Engine
{
    /// <summary>
    /// 棋局树
    /// </summary>
   public class ChessTree
    {
        /// <summary>
        /// 孩子链表
        /// </summary>
       public  List<ChessTree> ChessNodeList;
       public Struct_State State;
       public ChessTree()
       {
           ChessNodeList = new List<ChessTree>();
       }
       //public Guid ParentNodeId;
       //public Guid Id;
     /// <summary>
     /// 节点添加
     /// </summary>
     /// <param name="ChildTree">子节点</param>
     /// <param name="tree">父节点</param>
     /// <returns></returns>
       public bool AddChild(ChessTree ChildTree,ChessTree tree)
        {
            if (ChildTree.State.category != 0)
            {
                tree.ChessNodeList.Add(ChildTree);
                return true;
            }
            else
                return false;
        }
     /// <summary>
     /// 需要修改的孩子节点
     /// </summary>
     /// <param name="new_state">修改后的状态</param>
     /// <param name="alter_tree">需要修改的节点</param>
     /// <returns></returns>
       public bool AlterChild(Struct_State new_state,ChessTree alter_tree)
       {
           if (new_state.category!=0)
           {
               alter_tree.State = new_state;//更新节点信息
               return true;
           }
           else
               return false;
       }
       /// <summary>
       /// 删除子节点
       /// </summary>
       /// <param name="child_tree">需要删除的孩子节点</param>
       /// <returns></returns>
       public bool DeleteChild(ChessTree child_tree)
       {
           bool isExist = false;
           for(int count=0;count<ChessNodeList.Count;count++)
           {
               if(ChessNodeList[count]==child_tree)
               {
                   isExist = true;
                   ChessNodeList.RemoveAt(count);
                   break;
               }
           }
           return isExist;
       }
     
       public Struct_State Max_Node_Search(ChessTree tree)
       { 
           Dictionary<Guid, Struct_Simple_State> Di_All_Node = new Dictionary<Guid, Struct_Simple_State>();
           //深度搜索获得所有节点
           Dps_search(ref Di_All_Node, tree);    
           return Select_Score_Max(Di_All_Node);  
       }
       private Struct_State Select_Score_Max(Dictionary<Guid,Struct_Simple_State> Di_select_node)
       {
           Struct_State struct_state = new Struct_State();
           Di_select_node.Remove(new Guid());
           int temp_value=int.MinValue;
           foreach(Guid key in Di_select_node.Keys)
           {
               if(temp_value<Di_select_node[key].score)
               {
                   struct_state.id = key;
                   temp_value = Di_select_node[key].score;
               }
           }
           struct_state.origin_position=Di_select_node[struct_state.id].origin_position;
           struct_state.category = Di_select_node[struct_state.id].category;
           struct_state.parent_id = Di_select_node[struct_state.id].parent_id;
           struct_state.score = Di_select_node[struct_state.id].score;
           struct_state.target_position = Di_select_node[struct_state.id].target_position;
           if (Di_select_node[struct_state.id].category < 0)
               struct_state.isRed =false;
           else
               struct_state.isRed =true;
           return struct_state;

       }
        ///<summary>
        ///深度搜索遍历方法
        ///</summary>
        ///<param name="Di_tree_content"></param>
        ///<param name="tree"></param>
       private void Dps_search(ref Dictionary<Guid,Struct_Simple_State> Di_tree_content,ChessTree tree)
       {
           Stack<ChessTree> stack_tree = new Stack<ChessTree>();
           Dictionary<Guid, Struct_Simple_State> leaf_node = new Dictionary<Guid, Struct_Simple_State>();
           stack_tree.Push(tree);
           ChessTree temp_tree=new ChessTree();
           Struct_Simple_State simple_state=new Struct_Simple_State();
           while(stack_tree.Count!=0)
           {
               temp_tree=stack_tree.Pop();
               simple_state.id=temp_tree.State.id;
               simple_state.origin_position=temp_tree.State.origin_position;
               simple_state.parent_id=temp_tree.State.parent_id;
               simple_state.target_position=temp_tree.State.target_position;
               simple_state.score = temp_tree.State.score;
               simple_state.category=temp_tree.State.category;
               Di_tree_content.Add(simple_state.id, simple_state);
               if(temp_tree.ChessNodeList.Count==0)
               {
                   leaf_node.Add(temp_tree.State.id, simple_state);
               }
               for(int counts=0;counts<temp_tree.ChessNodeList.Count;++counts)
               {
               
                   stack_tree.Push(temp_tree.ChessNodeList[counts]);
               }
           }
           Search_Leaf_Node(ref Di_tree_content, leaf_node);
       }
     
       /// <summary>
       /// 给出哈希树的叶子节点，并进行剪枝操作,黑色棋子作为AI时
       /// </summary>
       /// <param name="Di_tree_Content">哈希树字典内容</param>
       /// <param name="leaf_node">叶子节点哈希表</param>
       public void  Search_Leaf_Node
       (ref Dictionary<Guid,Struct_Simple_State> Di_tree_Content,Dictionary<Guid,Struct_Simple_State> leaf_node)
       {
           bool isChildNode =true;
           while (isChildNode)
           {
               //用于计算父节点的分值，进行比较选出最大或最小的分数情况
               Dictionary<Guid, int> di_temp_score = new Dictionary<Guid, int>();
               //id临时值，避免频繁声明
               Guid temp_guid; Struct_Simple_State temp_simple_struct = new Struct_Simple_State();
               //循环叶节点，改变父节点临时节点字典的分值
               foreach (Guid key in leaf_node.Keys)
               {
                   //获取父节点id
                   temp_guid = leaf_node[key].parent_id;
                   //如果父节点是根节点则停止搜索
                   if (temp_guid == new Guid())
                   {
                       isChildNode = false;
                       //Di_tree_Content = leaf_node;
                       break;
                   }
                   if (!di_temp_score.ContainsKey(temp_guid))
                       di_temp_score.Add(temp_guid, leaf_node[key].score);
                   else
                   {
                       if (di_temp_score[temp_guid] < leaf_node[key].score)
                           di_temp_score[temp_guid] = leaf_node[key].score;
                   }
                   //删除哈希树的节点
                   Di_tree_Content.Remove(key);
               }
               leaf_node.Clear();
               if (isChildNode == true)
               {
                   //父节点和与临时节点进行分值相减,对父节点分值进行重新赋值,再将叶节点进行更新
                   foreach (Guid key in di_temp_score.Keys)
                   {
                       temp_simple_struct = Di_tree_Content[key];
                       temp_simple_struct.score = Di_tree_Content[key].score - di_temp_score[key];
                       Di_tree_Content[key] = temp_simple_struct;
                       leaf_node.Add(key, temp_simple_struct);
                   }
                   di_temp_score.Clear();
               }
           }
       }
    }
}
