using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chess.ChessRule;
using System.Configuration;
using Chess.Engine;
namespace Chess
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Button SelectButton; //被选中的按钮，uid存储类型，name存储位置
        Abstract_Rule rule;
        bool IsRed=true;//判断是否是红方走棋，否则为黑方
        int depth;//迭代深度
        int score;//剪枝分数
        /// <summary>
        ///开局加载方法
        /// </summary>
        public void Load()
        {
            ChessLoad.Load();
            //反射获取程序集内容
            rule = ChessLoad.rule;
            SelectButton = new Button();
            depth = ChessLoad.Depth;
            score = ChessLoad.limit_Score;
            int chess_style; Grid gd; string img;
            foreach (int poistion in ChessLoad.DicMap.Keys)
            {
                ImageBrush berriesBrush = new ImageBrush();
                Button bt = new Button();
                chess_style = Convert.ToInt32(ChessLoad.DicMap[poistion]);
                img = ChessLoad.DicImg[chess_style];
                berriesBrush.ImageSource =
                     new BitmapImage(
                         new Uri(img, UriKind.Relative)
                     );
                gd = (Grid)this.GetType().GetField("po_" + poistion, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                bt.Background = berriesBrush; bt.HorizontalAlignment = HorizontalAlignment.Left; bt.VerticalAlignment = VerticalAlignment.Top;
                bt.Height = 65; bt.Width = 65; bt.Uid = chess_style.ToString(); bt.Name = "po_" + poistion;
                bt.Click += bt_Click;
                gd.Children.Add(bt);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        void bt_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            if (SelectButton.Uid != "")
            {
                //棋子移动信息
                Str_ChessInfo chessinfo = new Str_ChessInfo();
                chessinfo.category = Convert.ToInt32(SelectButton.Uid);
                if ((chessinfo.category > 0 && IsRed == true) || (chessinfo.category < 0 && IsRed == false))
                {
                    Grid origin_grid = (Grid)this.GetType().GetField(SelectButton.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                    Grid now_grid = (Grid)this.GetType().GetField(bt.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                    int origin_Position = cast_Name(origin_grid.Name);
                    int now_Position = cast_Name(now_grid.Name);
                    chessinfo.origin_position = origin_Position;
                    chessinfo.target = now_Position;
                    if (rule.Rule_Judge(ref ChessLoad.Auxiliary_array, chessinfo))
                    {
                        ChessLoad.Auxiliary_array[chessinfo.target] = chessinfo.category;
                        ChessLoad.Auxiliary_array[chessinfo.origin_position] = 0;
                        origin_grid.Children.Clear();
                        now_grid.Children.Clear();            
                        SelectButton.Name = now_grid.Name;
                        SelectButton.Uid = chessinfo.category.ToString();
                        now_grid.Children.Add(SelectButton);
                        ChessAI ai = new ChessAI(score);
                        ChessTree tree = new ChessTree();
                        tree.State = new Struct_State();
                        tree.State.array_chess = ChessLoad.Auxiliary_array;
                        tree.State.isRed = false;
                        tree.State = ai.Ai_Result(ref tree,depth);
                        Button AiButton = new Button();
                        ChessLoad.Auxiliary_array[tree.State.target_position] = tree.State.category;
                        ChessLoad.Auxiliary_array[tree.State.origin_position] = 0;
                        origin_grid = (Grid)this.GetType().GetField("po_" + tree.State.origin_position, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        now_grid = (Grid)this.GetType().GetField("po_" + tree.State.target_position, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                        AiButton = (Button)origin_grid.Children[0];
                        AiButton.Name = "po_" + tree.State.target_position;
                        AiButton.Uid = tree.State.category.ToString();
                        //避免重复走棋
                        //if (ChessLoad.same_action_state.Count == 2)
                        //    ChessLoad.same_action_state.Dequeue();
                        //Struct_Simple_State simple_state = new Struct_Simple_State();
                        //simple_state.category = tree.State.category;
                        //simple_state.origin_position = tree.State.origin_position;
                        //simple_state.target_position = tree.State.target_position;
                        //ChessLoad.same_action_state.Enqueue(simple_state);
                        origin_grid.Children.Clear();
                        now_grid.Children.Clear();
                        now_grid.Children.Add(AiButton);

                       
                    }
                    else
                        SelectButton = bt;
                }
            }     
             SelectButton = bt;
        }

        private void grid3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                Str_ChessInfo chessinfo = new Str_ChessInfo();//棋子移动信息
                Grid now_grid = (Grid)sender;//当前的Grid值
                if (SelectButton.Uid != "")
                {
                    //通过反射获取选中棋子的信息
                    Grid origin_grid = (Grid)this.GetType().GetField(SelectButton.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                    int origin_Position = cast_Name(origin_grid.Name);
                    int now_Position = cast_Name(now_grid.Name);
                    chessinfo.category = Convert.ToInt32(SelectButton.Uid);
                    if ((chessinfo.category < 0 && IsRed == false) || chessinfo.category > 0 && IsRed == true)
                    {
                        chessinfo.origin_position = origin_Position;
                        chessinfo.target = now_Position;
                        //棋盘模块判断
                        if (rule.Rule_Judge(ref ChessLoad.Auxiliary_array, chessinfo))
                        {
                            ChessLoad.Auxiliary_array[chessinfo.target] = chessinfo.category;
                            ChessLoad.Auxiliary_array[chessinfo.origin_position] = 0;
                            origin_grid.Children.Clear();
                            SelectButton.Name = now_grid.Name;
                            now_grid.Children.Add(SelectButton);
                            ChessAI ai = new ChessAI(score);
                            ChessTree tree = new ChessTree();
                            tree.State = new Struct_State();
                            tree.State.array_chess = new int[ChessLoad.Auxiliary_array.Length];
                            ChessLoad.Auxiliary_array.CopyTo(tree.State.array_chess, 0);
                            tree.State.isRed = false;
                            tree.State = ai.Ai_Result(ref tree,depth);
                            ChessLoad.Auxiliary_array[tree.State.target_position] = tree.State.category;
                            ChessLoad.Auxiliary_array[tree.State.origin_position] = 0;
                            Button AiButton = new Button();
                            origin_grid = (Grid)this.GetType().GetField("po_" + tree.State.origin_position, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                            now_grid = (Grid)this.GetType().GetField("po_" + tree.State.target_position, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                            AiButton = (Button)origin_grid.Children[0];
                            AiButton.Name = "po_" + tree.State.target_position;
                            AiButton.Uid = tree.State.category.ToString();
                            //避免重复走棋
                            //if(ChessLoad.same_action_state.Count==2)
                            //ChessLoad.same_action_state.Dequeue();
                            //Struct_Simple_State simple_state = new Struct_Simple_State();
                            //simple_state.category = tree.State.category;
                            //simple_state.origin_position = tree.State.origin_position;
                            //simple_state.target_position = tree.State.target_position;
                            //ChessLoad.same_action_state.Enqueue(simple_state);

                            origin_grid.Children.Clear();
                            now_grid.Children.Clear();
                            now_grid.Children.Add(AiButton);
                        }
                    }
                }
        }
        /// <summary>
        /// 获取Grid的数字值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int cast_Name(string name)
        {
           int index=name.IndexOf('_');
           name = name.Substring(index+1, name.Length - index-1);
           return Convert.ToInt32(name);
        }
        /// <summary>
        /// 改变bool类型
        /// </summary>
        /// <param name="isRed"></param>
        public void ChangeBool(ref bool isRed)
        {
            if (isRed == true)
                isRed = false;
            else
                isRed = true;
        }
        private void wrapPanelGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
          
         
        }  

    }
}
