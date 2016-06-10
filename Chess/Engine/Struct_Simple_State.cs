using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Engine
{
    /// <summary>
    /// 棋局简单状态结构-不包括棋盘信息
    /// </summary>
   public struct Struct_Simple_State
    {
       /// <summary>
       /// 原始位置
       /// </summary>
       public int origin_position;
       /// <summary>
       /// 目标位置
       /// </summary>
       public int target_position;
       /// <summary>
       /// 得分
       /// </summary>
       public int score;
       /// <summary>
       /// 棋子ID
       /// </summary>
       public Guid id;
       /// <summary>
       /// 棋子种类
       /// </summary>
       public int category;
       /// <summary>
       /// 父节点
       /// </summary>
       public Guid parent_id;

    }
}
