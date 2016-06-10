using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ChessRule
{
  public  abstract class Abstract_Rule
    {
      
        public virtual bool Rule_Judge(ref int[] Position, Str_ChessInfo chessinfo)
         {
            switch (chessinfo.category)
                 {
                     case 10:
                     case -10:
                     case 11:
                     case -11:
                         {
                             return Rook_Category(ref Position, chessinfo);
                         }
                     case -12:
                     case -13:
                     case -14:
                     case -15:
                     case -16:
                     case 12:
                     case 13:
                     case 14:
                     case 15:
                     case 16:
                         {
                             return Pawn_Category(ref Position, chessinfo);
                         }
                     case 8:
                     case 9:
                     case -8:
                     case -9:
                         {
                             return Cannon_Category(ref Position, chessinfo);
                         }
                     case -4:
                     case -5:
                     case 4:
                     case 5:
                         {
                             return Bishop_Category(ref Position, chessinfo);
                         }
                     case -2:
                     case -3:
                     case 2:
                     case 3:
                         {
                             return Guard_Category(ref Position, chessinfo);
                         }
                     case 1:
                     case -1:
                         {
                             return King_Category(ref Position, chessinfo);
                         }
                     case 6:
                     case 7:
                     case -6:
                     case -7:
                         {
                             return Knight_Category(ref Position, chessinfo);
                         }
                 }
                 return false;
             
           }
        protected abstract bool Rook_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool Pawn_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool Cannon_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool Bishop_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool Guard_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool King_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool Knight_Category(ref int[] Position, Str_ChessInfo chessinfo);
        protected abstract bool IsKingCheck_Right(ref int[] Position, Str_ChessInfo chessinfo);
    }
}
