using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Engine
{
    /// <summary>
    /// 棋局当前状态
    /// </summary>
   public struct Struct_State
    {
       /// <summary>
       /// 棋子原坐标
       /// </summary>
      public  int origin_position;
       /// <summary>
       /// 棋子目标位置
       /// </summary>
      public  int target_position;
      /// <summary>
      /// 棋子种类
      /// </summary>
      public  int category;
       /// <summary>
       /// 棋盘数组
       /// </summary>
      public  int[] array_chess;
       /// <summary>
       /// 确定走位方
       /// </summary>
      public bool isRed;
       /// <summary>
       /// 该局面分数
       /// </summary>
      public int score;
       /// <summary>
       /// 节点id
       /// </summary>
      public Guid id;
       /// <summary>
       /// 父节点id
       /// </summary>
      public Guid parent_id;
    }
}
