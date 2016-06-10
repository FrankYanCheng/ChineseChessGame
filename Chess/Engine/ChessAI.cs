using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.ChessRule;
namespace Chess.Engine
{
  public  class ChessAI
  {  
        //确定机器方
        public static bool IsRed=false;
        //剪枝分数
        int cut_size;
        Struct_State result_state;
        /// <summary>
        /// 机器处理结果状态
        /// </summary>
        public Struct_State Result_state
        {
            get { return result_state; }
            set { result_state = value; }
        }
        public ChessAI(int cut_size)
        {
            this.cut_size = cut_size;
        }
      /// <summary>
      /// AI处理结果
      /// </summary>
      /// <param name="chessTree">棋子树</param>
      /// <param name="depth">搜索的深度</param>
      /// <returns></returns>
        public Struct_State Ai_Result(ref ChessTree chessTree,int depth)
        {
            ChessTree child_tree = chessTree;  
            Depth_Iteration(ref chessTree, depth);
            return chessTree.Max_Node_Search(chessTree);
       }
      public void Depth_Iteration(ref  ChessTree chessTree,int depth)
      {
          ChessTree child_tree = chessTree;
          ChessTree temp_tree = new ChessTree();
          if(depth >0)
          {
              --depth;
              Extend_Depth(child_tree);
              //if ((depth == ChessLoad.Depth - 2)&&(ChessLoad.same_action_state.Count==2))
              //{
              //   if ((child_tree.State.category == ChessLoad.same_action_state.Peek().category) &&
              //       (child_tree.State.origin_position == ChessLoad.same_action_state.Peek().origin_position) &&
              //       (child_tree.State.target_position == ChessLoad.same_action_state.Peek().target_position)) { }
              //   else
              //   {
              //    for (int i = 0; i < child_tree.ChessNodeList.Count; i++)
              //     {
              //        temp_tree = child_tree.ChessNodeList[i];
              //        Depth_Iteration(ref temp_tree, depth);
              //     }
              //   }
              //}
              //else
                  for (int i = 0; i < child_tree.ChessNodeList.Count; i++)
                  {
                      temp_tree = child_tree.ChessNodeList[i];
                      Depth_Iteration(ref temp_tree, depth);
                  }
          }
      }
      /// <summary>
      /// 扩展深度
      /// </summary>
      /// <param name="chessTree">搜索节点</param>
        public void Extend_Depth(ChessTree chessTree)
        {
            ChessTree temp_Tree = new ChessTree();
            Score_Design design = new Score_Design(cut_size);
            Dictionary<Guid, Struct_Simple_State> di_score = new Dictionary<Guid, Struct_Simple_State>();
            //遍历棋盘上的棋子
            for(int counts=52;counts<=204;counts++)
            {
                if (chessTree.State.isRed==false)
                {
                    if (chessTree.State.array_chess[counts]< 0)
                    {
                            temp_Tree.State.category = chessTree.State.array_chess[counts];
                            temp_Tree.State.origin_position = counts;
                            temp_Tree.State.isRed = false;
                            temp_Tree.State.array_chess = chessTree.State.array_chess;
                            temp_Tree.State.id = chessTree.State.id;
                            //获得走位点和分值
                            design.Design(temp_Tree.State, ref di_score);                            
                    }
                }
                else
                {
                    if(chessTree.State.array_chess[counts]>0)
                    {
                          temp_Tree.State.category = chessTree.State.array_chess[counts];
                          temp_Tree.State.origin_position = counts;
                          temp_Tree.State.isRed = true;
                          temp_Tree.State.array_chess = chessTree.State.array_chess;
                          temp_Tree.State.id = chessTree.State.id;
                          //获得走位点和分值
                          design.Design(temp_Tree.State, ref di_score);                    
                    }
                }
             }
          
            foreach (Guid key in di_score.Keys)
            {
                ChessTree child = new ChessTree();
                child.State.array_chess = new int[chessTree.State.array_chess.Length]; 
                child.State.category = di_score[key].category;
                child.State.origin_position = di_score[key].origin_position;
                chessTree.State.array_chess.CopyTo(child.State.array_chess, 0);
                child.State.target_position = di_score[key].target_position;
                child.State.score = di_score[key].score;
                child.State.isRed=ChangeBool(chessTree.State.isRed);
                child.State.array_chess[child.State.origin_position] = 0;
                child.State.array_chess[child.State.target_position] =child.State.category;
                child.State.id = key;
                child.State.parent_id = di_score[key].parent_id;
                chessTree.AddChild(child, chessTree);

            } 
            di_score.Clear(); 
         }
      /// <summary>
      /// 改变布尔值的方法
      /// </summary>
      /// <param name="isRed"></param>
      /// <returns></returns>
      public bool ChangeBool(bool isRed)
      {
          if(isRed==true)
              return false;
          else
              return true;
      }
     }
}
