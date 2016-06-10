using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Chess.ChessRule
{
    /// <summary>
    /// 棋盘规则类
    /// </summary>
    public class Rule : Abstract_Rule
    {
        int size;
        public Rule()
        {
            size = 16;

        }
        public  override bool Rule_Judge(ref int[] Position, Str_ChessInfo chessinfo)
        {
            return base.Rule_Judge(ref Position,chessinfo);
        }
        protected override bool Rook_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            //空位或不同棋子即可走
            if ((Position[chessinfo.target] == 0 || Position[chessinfo.target] * chessinfo.category < 0))
            {
                int temp = Position[chessinfo.target];
                int min = Math.Min(chessinfo.origin_position, chessinfo.target);
                int max = Math.Max(chessinfo.origin_position, chessinfo.target);
                bool isOK = false;
                int count = 0;
                if (min / size == max / size)
                {
                    for (count = min + 1; count < max; ++count)
                    {
                        if (Position[count] != 0)
                        {
                            isOK = false;
                            break;
                        }
                    }
                    if (count == max)
                    {
                        Position[chessinfo.origin_position] = 0;
                        Position[chessinfo.target] = chessinfo.category;
                        if (IsKingCheck_Right(ref Position, chessinfo))
                            isOK = true;
                    }
                    else
                        isOK = false;
                }
                else
                {
                    for (count = min + size; count < max; count = count + size)
                    {
                        if (Position[count] != 0)
                        {
                            isOK = false;
                            break;
                        }
                    }
                    if (count == max)
                    {
                        Position[chessinfo.origin_position] = 0;
                        Position[chessinfo.target] = chessinfo.category;
                        if (IsKingCheck_Right(ref Position, chessinfo))
                            isOK = true;                   
                    }
                    else
                        isOK = false;
                }
                if (isOK == true)
                {
                    Position[chessinfo.target] = temp;
                    Position[chessinfo.origin_position] = chessinfo.category;
                    return true;
                }

            }
            return false;
        }
        protected override bool Pawn_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            bool isOK = false;
            int temp = Position[chessinfo.target];
            if (chessinfo.category * Position[chessinfo.target] <= 0)
            {
                //black
                if (chessinfo.category < 0)
                {
                    if (chessinfo.origin_position <= 128)
                    {
                        if (chessinfo.origin_position + size == chessinfo.target)
                        {
                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                                isOK = true;
                            else
                            {
                                Position[chessinfo.origin_position] = chessinfo.category;
                                Position[chessinfo.target] = temp;
                            }
                        }
                    }

                    else
                    {
                        if ((chessinfo.origin_position + size == chessinfo.target)
                        || chessinfo.origin_position + 1 == chessinfo.target || chessinfo.origin_position - 1 == chessinfo.target)
                        {

                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                                isOK = true;
                            else
                            {
                                Position[chessinfo.origin_position] = chessinfo.category;
                                Position[chessinfo.target] = temp;
                            }
                        }
                    }
                }
                //red
                else
                {
                    if (chessinfo.origin_position >= 128)
                    {
                        if (chessinfo.origin_position - size == chessinfo.target)
                        {
                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                                isOK = true;
                            else
                            {
                                Position[chessinfo.origin_position] = chessinfo.category;
                                Position[chessinfo.target] = temp;
                            }
                        }
                    }
                    else
                    {
                        if ((chessinfo.origin_position - size == chessinfo.target)
                             || chessinfo.origin_position + 1 == chessinfo.target || chessinfo.origin_position - 1 == chessinfo.target)
                        {
                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                                isOK = true;
                            else
                            {
                                Position[chessinfo.origin_position] = chessinfo.category;
                                Position[chessinfo.target] = temp;
                            }
                        }
                    }
                }
            }
            if (isOK == true)
            {
                Position[chessinfo.target] = temp;
                Position[chessinfo.origin_position] = chessinfo.category;
                return true;
            }
            return isOK;

        }
        protected override bool Cannon_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            bool isOK = false;
            int min = Math.Min(chessinfo.origin_position, chessinfo.target);
            int temp = Position[chessinfo.target];
            int max = Math.Max(chessinfo.origin_position, chessinfo.target);
            if (Position[chessinfo.origin_position] * Position[chessinfo.target] < 0)
            {
                int count = 0;
                if (min / size == max / size)
                {
                    for (int i = min + 1; i < max; i++)
                    {
                        if (Position[i] != 0)
                        {
                            ++count;
                        }
                    }
                    if (count == 1)
                    {
                        Position[chessinfo.origin_position] = 0;
                        Position[chessinfo.target] = chessinfo.category;
                        if (IsKingCheck_Right(ref Position, chessinfo))
                        {
                            isOK = true;
                            Position[chessinfo.target] = chessinfo.category;
                            Position[chessinfo.origin_position] = 0;
                        }

                    }
                }
                if (min % size == max % size)
                {
                    for (int i = min + size; i < max; i = i + size)
                    {
                        if (Position[i] != 0)
                        {
                            ++count;
                        }
                    }
                    if (count == 1)
                    {
                        Position[chessinfo.origin_position] = 0;
                        Position[chessinfo.target] = chessinfo.category;
                        if (IsKingCheck_Right(ref Position, chessinfo))
                        {
                            isOK = true;
                            Position[chessinfo.target] = chessinfo.category;
                            Position[chessinfo.origin_position] = 0;
                        }      
                    }
                }
            }
            else if (Position[chessinfo.origin_position] * Position[chessinfo.target] == 0)
            {
                int count = 0;
                if (min / size == max / size)
                {
                    for (count = min + 1; count < max; ++count)
                    {
                        if (Position[count] != 0)
                        {
                            isOK = false;
                            break;
                        }
                    }
                    if (count == max)
                    {
                        Position[chessinfo.origin_position] = 0;
                        Position[chessinfo.target] = chessinfo.category;
                        if (IsKingCheck_Right(ref Position, chessinfo))
                            isOK = true;
                    }

                }
                else
                {
                    for (count = min + size; count < max; count = count + size)
                    {
                        if (Position[count] != 0)
                        {
                            isOK = false;
                            break;
                        }
                    }
                    if (count == max)
                    {
                        Position[chessinfo.origin_position] = 0;
                        Position[chessinfo.target] = chessinfo.category;
                        if (IsKingCheck_Right(ref Position, chessinfo))
                            isOK = true;
                        //else
                        //{
                        //    Position[chessinfo.origin_position] = chessinfo.category;
                        //    Position[chessinfo.target] = temp;
                        //}
                    }
                }
            }
            if (isOK == true)
            {
                Position[chessinfo.target] = temp;
                Position[chessinfo.origin_position] = chessinfo.category;
                return true;
            }
         
            return isOK;
        }
        protected override bool Bishop_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            int min = Math.Min(chessinfo.origin_position, chessinfo.target);
            int max = Math.Max(chessinfo.origin_position, chessinfo.target);
            int temp = Position[chessinfo.target];
            bool isOK = false;
            int dis = max - min;
            if (Position[chessinfo.target] * Position[chessinfo.origin_position] <= 0)
            {
            if (chessinfo.category < 0 && dis < 35 && dis > 16&&dis!=26)
            {
               
                    int average = 0;
                    for (int i = 0; i < ChessLoad.arr_bishop_black.Length; i++)
                    {
                        average = (chessinfo.origin_position + chessinfo.target) / 2;
                        if ((chessinfo.target == ChessLoad.arr_bishop_black[i]) && Position[average] == 0)
                        {
                            if (Position[chessinfo.target] * chessinfo.category <= 0)
                            {
                                Position[chessinfo.origin_position] = 0;
                                Position[chessinfo.target] = chessinfo.category;
                                if (IsKingCheck_Right(ref Position, chessinfo))
                                    isOK = true; 
                            }
                        }
                    }
                }
            else if (chessinfo.category > 0 && dis < 35 && dis > 16 && dis != 26)
                {
                    int average = 0;
                    for (int i = 0; i < ChessLoad.arr_bishop_red.Length; i++)
                    {
                        average = (chessinfo.origin_position + chessinfo.target) / 2;
                        if ((chessinfo.target == ChessLoad.arr_bishop_red[i]) && Position[average] == 0)
                        {
                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                                isOK = true;
                        }
                    }
                }
            }
            if (isOK == true)
            {
                Position[chessinfo.target] = temp;
                Position[chessinfo.origin_position] = chessinfo.category;
            }
            return isOK;
        }
        protected override bool Guard_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            bool isOK = false;
            int distance = Math.Abs(chessinfo.target - chessinfo.origin_position);
            int temp = Position[chessinfo.target];
            //士在方框内走，且目标位置不能有己方棋子
            if (distance <= 20 && distance >= 10)
            {
                if (chessinfo.category < 0)
                {
                    for (int i = 0; i < ChessLoad.arr_guard_black.Length; i++)
                    {
                        //士在方框内走，且目标位置不能有己方棋子
                        if ((ChessLoad.arr_guard_black[i] == chessinfo.target) && Position[chessinfo.target] * chessinfo.category <= 0)
                        {
                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                            {
                                isOK = true;
                                break;
                            }
                        }
                    }
                }
                if (chessinfo.category > 0)
                {
                    for (int i = 0; i < ChessLoad.arr_guard_red.Length; i++)
                    {

                        if ((ChessLoad.arr_guard_red[i] == chessinfo.target) && Position[chessinfo.target] * chessinfo.category <= 0)
                        {
                            Position[chessinfo.origin_position] = 0;
                            Position[chessinfo.target] = chessinfo.category;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                            {
                                isOK = true;
                                break;
                            }
                        }
                    }
                }
            }
        
                Position[chessinfo.target] = temp;
                Position[chessinfo.origin_position] = chessinfo.category;
       
            return isOK;

        }
        protected override bool King_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            bool isOK= false;
            int temp = Position[chessinfo.target];
            int distance = Math.Abs(chessinfo.target - chessinfo.origin_position);
            int[] temp_king_square; int[] temp_king_against_square;
            int enemy_king = 0;//敌方将领
            int king;
            if (chessinfo.category < 0)
            {
                temp_king_square = ChessLoad.arr_king_black;
                temp_king_against_square = ChessLoad.arr_king_red;
                enemy_king = ChessLoad.red_king;
                king = ChessLoad.black_king;
            }
            else
            {
                temp_king_square = ChessLoad.arr_king_red;
                temp_king_against_square = ChessLoad.arr_king_black;
                enemy_king = ChessLoad.black_king;
                king = ChessLoad.red_king;
            }
            for (int i = 0; i < temp_king_square.Length; i++)
            {
                //在方框内
                if (chessinfo.target == temp_king_square[i])
                {
                    //不能吃己方子且只能移动一格
                    if ((Position[chessinfo.target] * chessinfo.category <= 0) && ((distance == size || distance == 1)))
                    {

                        if (IsKingCheck_Right(ref Position, chessinfo))
                        {
                            if (chessinfo.category < 0)
                            {
                                ChessLoad.black_king = chessinfo.target;
                            }
                            if (chessinfo.category > 0)
                            {
                                ChessLoad.red_king = chessinfo.target;
                            }
                            Position[chessinfo.target] = chessinfo.category;
                            Position[chessinfo.origin_position] = 0;
                            if (IsKingCheck_Right(ref Position, chessinfo))
                            {
                                isOK = true;
                                break;
                            }
                        }


                    }
                }
            }
            if (isOK == true)
            {
                Position[chessinfo.target] = temp;
                Position[chessinfo.origin_position] = chessinfo.category;

            }
            return isOK;
        }
        protected override bool Knight_Category(ref int[] Position, Str_ChessInfo chessinfo)
        {
            bool isOK = false;
            int forbiden_position;
            int temp = Position[chessinfo.target];
            int distance = Math.Abs(chessinfo.target - chessinfo.origin_position);
            if (Position[chessinfo.target] * chessinfo.category <= 0)
            {
                switch (distance)
                {
                    case 18:
                        {
                            if ((chessinfo.origin_position > chessinfo.target))
                            {
                                forbiden_position = chessinfo.origin_position - 1;
                            }
                            else
                            {
                                forbiden_position = chessinfo.origin_position + 1;
                            }
                            //未蹩马脚
                            if (Position[forbiden_position] == 0)
                            {
                                Position[chessinfo.origin_position] = 0;
                                Position[chessinfo.target] = chessinfo.category;
                                if (IsKingCheck_Right(ref Position, chessinfo))
                                    isOK = true;
                                else
                                {
                                    Position[chessinfo.origin_position] = chessinfo.category;
                                    Position[chessinfo.target] = temp;
                                }
                            }
                            break;
                        }
                    case 14:
                        {
                            if ((chessinfo.origin_position > chessinfo.target))
                            {
                                forbiden_position = chessinfo.origin_position + 1;
                            }
                            else
                            {
                                forbiden_position = chessinfo.origin_position - 1;
                            }
                            //未蹩马脚
                            if (Position[forbiden_position] == 0)
                            {
                                Position[chessinfo.origin_position] = 0;
                                Position[chessinfo.target] = chessinfo.category;
                                if (IsKingCheck_Right(ref Position, chessinfo))
                                    isOK = true;
                                else
                                {
                                    Position[chessinfo.origin_position] = chessinfo.category;
                                    Position[chessinfo.target] = temp;
                                }
                            }
                            break;
                        }
                    case 31:
                    case 33:
                        {
                            if ((chessinfo.origin_position > chessinfo.target))
                            {
                                forbiden_position = chessinfo.origin_position - size;
                            }
                            else
                            {
                                forbiden_position = chessinfo.origin_position + size;
                            }
                            //未蹩马脚
                            if (Position[forbiden_position] == 0)
                            {
                                Position[chessinfo.origin_position] = 0;
                                Position[chessinfo.target] = chessinfo.category;
                                if (IsKingCheck_Right(ref Position, chessinfo))
                                    isOK = true;
                                else
                                {
                                    Position[chessinfo.origin_position] = chessinfo.category;
                                    Position[chessinfo.target] = temp;
                                }
                            }
                            break;
                        }
                }
            }
            if (isOK == true)
            {
                Position[chessinfo.target] = temp;
                Position[chessinfo.origin_position] = chessinfo.category;
                return true;
            }
            return isOK;
        }
        protected override bool IsKingCheck_Right(ref int[] Position, Str_ChessInfo chessinfo)
        {

            int enemy_king = 0;//敌方将领
            int king = 0;
            if (chessinfo.category < 0)
            {
                enemy_king = ChessLoad.red_king;
                if (Math.Abs(chessinfo.category) == 1)
                    king = chessinfo.target;
                else
                    king = ChessLoad.black_king;
            }
            else
            {
                enemy_king = ChessLoad.black_king;
                if (Math.Abs(chessinfo.category) == 1)
                    king = chessinfo.target;
                else
                    king = ChessLoad.red_king;
            }
            //两帅不能对立
            if (enemy_king % size == king % size)
            {
                int total_count = 0;
                int min = Math.Min(enemy_king, king);
                int max = Math.Max(enemy_king, king);
                for (int counts = min + size; counts < max; counts = counts + 16)
                {
                    if (Position[counts] == 0)
                        ++total_count;
                }
                if (total_count == (max - min) / size - 1)
                {
                    return false;
                }
            }
            return true;
        }

    }
}