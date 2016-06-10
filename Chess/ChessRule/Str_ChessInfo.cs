using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ChessRule
{
    public struct Str_ChessInfo
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        public int target;
        /// <summary>
        /// 棋子类型
        /// </summary>
        public int category;
        /// <summary>
        /// 原先在位置
        /// </summary>
        public int origin_position;
    }
}
