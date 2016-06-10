using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.ChessRule;
namespace Chess.Engine
{
    /// <summary>
    /// 得分评估类
    /// </summary>
    class Score_Design
    {
        //用于剪裁剪枝树的得分限制变量
        int cut_size_score;
        int size;

        public Score_Design(int cut_size)
        {
            this.cut_size_score = cut_size;
            size = 16;
        }
        /// <summary>
        /// 棋力评估函数
        /// </summary>
        /// <param name="state">棋子状态</param>
        /// <param name="di_position_score">引用数据字典 棋局状态ID-棋子简单状态信息</param>
        public void Design(Struct_State state, ref Dictionary<Guid, Struct_Simple_State> di_position_score)
        {
            int category = state.category;
            //记录当前分数字段
            int temp_score = 0;
            //Dictionary<int, int> di_position_score = new Dictionary<int, int>();
            //规则结构信息
            Str_ChessInfo chessinfo = new Str_ChessInfo();
            //简单记录结构信息,用于记录数据字典
            Struct_Simple_State simple_state = new Struct_Simple_State();
            //黑方走棋
            if (state.isRed == false)
            {
                //边界判断值，在边界内除16余数在4-12之间
                int judge_int = 0;
                for (int i = 52; i <= 204; i++)
                {
                    judge_int = i % size;
                    if (state.array_chess[i] >= 0 && judge_int >= 4 && judge_int <= 12)
                    {
                        chessinfo.category = category;
                        chessinfo.origin_position = state.origin_position;
                        chessinfo.target = i;
                        if (ChessLoad.rule.Rule_Judge(ref state.array_chess, chessinfo))
                        {
                            if (Math.Abs(chessinfo.category) >= 12 && Math.Abs(chessinfo.category) <= 16)
                            {
                             
                                temp_score = ChessLoad.DiPawn_Black[chessinfo.target];
                               
                            }
                           
                            if (state.array_chess[chessinfo.target]>0)
                            temp_score = ChessLoad.DiScore[state.array_chess[chessinfo.target]];
                         
                            if (temp_score >= cut_size_score)
                            {
                                simple_state.target_position = chessinfo.target;
                                simple_state.score = temp_score;
                                simple_state.origin_position = chessinfo.origin_position;
                                simple_state.id = Guid.NewGuid();
                                simple_state.category = chessinfo.category;
                                simple_state.parent_id = state.id;
                                di_position_score.Add(simple_state.id, simple_state);
                            }
                        }
                    }
                }
            }
            //红方走棋
                        else
                        {
                            int judge_int = 0;
                            for (int i = 52; i <=204; i++)
                            {
                                //if (state.array_chess[i] <= 0)
                                //{
                                    judge_int = i % size;
                                    if (state.array_chess[i] <= 0 && judge_int >= 4 && judge_int <= 12)
                                    {
                                    chessinfo.category = category;
                                    chessinfo.origin_position = state.origin_position;
                                    chessinfo.target = i;
                                    if (ChessLoad.rule.Rule_Judge(ref state.array_chess, chessinfo))
                                    {
                                        if (Math.Abs(chessinfo.category) >= 12 && Math.Abs(chessinfo.category) <= 16)
                                        {
                                         
                                                temp_score = ChessLoad.DiPawn_Red[chessinfo.target]; 
                                        }
                                      
                                        if (state.array_chess[chessinfo.target]<0)
                                        temp_score = ChessLoad.DiScore[state.array_chess[chessinfo.target]];
                                    
                                        if (temp_score >= cut_size_score)
                                        {
                                            simple_state.target_position = chessinfo.target;
                                            simple_state.score = temp_score;
                                            simple_state.origin_position = chessinfo.origin_position;
                                            simple_state.id = Guid.NewGuid();
                                            simple_state.category = chessinfo.category;
                                            simple_state.parent_id = state.id;
                                            di_position_score.Add(simple_state.id, simple_state);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        
 