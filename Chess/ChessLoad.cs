using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Chess.ChessRule;
using System.Configuration;
using Chess.Engine;
namespace Chess
{
    /// <summary>
    /// 棋子装载类
    /// </summary>
   public static class ChessLoad
    {
      static Dictionary<int, int> diPiece;
      static Dictionary<int,string> dicImg;
      static Dictionary<int, string> dicMap;
      static Dictionary<int, int> diScore;
      //得分位置部分
      static Dictionary<int, int> diPawn_Red;
      static Dictionary<int, int> diPawn_Black;
      //加载搜索的深度
      static int depth;

      public static int Depth
      {
          get { return ChessLoad.depth; }
          set { ChessLoad.depth = value; }
      }
      //搜索剪枝分数限制
      static int limit_score;

      public static int limit_Score
      {
          get { return ChessLoad.limit_score; }
          set { ChessLoad.limit_score = value; }
      }
      /// <summary>
      /// 棋子记录数据字典 黑色 对应棋子类型-位置
      /// </summary>
      public static Dictionary<int, int> DiPiece
      {
          get { return ChessLoad.diPiece; }
          set { ChessLoad.diPiece = value; }
      }
      /// <summary>
      ///棋盘辅助数据结构
      /// </summary>
      public static int[] Auxiliary_array;
      /// <summary>
      ///数据字典 位置信息-棋子信息
      /// </summary>
      public static Dictionary<int, string> DicMap
      {
          get { return ChessLoad.dicMap; }
          set { ChessLoad.dicMap = value; }
      }
       /// <summary>
       /// 数据字典 棋子id-图像信息
       /// </summary>
       public static Dictionary<int, string> DicImg
       {
         get { return dicImg; }
         set { dicImg = value; }
       }
       //-----------------------------------------得分部分数据字典声明----------------------------------------------//

       /// <summary>
       /// 数据字典 棋子-棋子分数信息
       /// </summary>
       public static Dictionary<int, int> DiScore
       {
           get { return ChessLoad.diScore; }
           set { ChessLoad.diScore = value; }
       }
       /// <summary>
       /// 数据字典 附加得分部分 红色兵得分部分 位置-得分
       /// </summary>
       public static Dictionary<int, int> DiPawn_Red
       {
           get { return ChessLoad.diPawn_Red; }
           set { ChessLoad.diPawn_Red = value; }
       }
       /// <summary>
       /// 数据字典 附加得分部分 黑色兵得分部分 位置-得分
       /// </summary>
       public static Dictionary<int, int> DiPawn_Black
       {
           get { return ChessLoad.diPawn_Black; }
           set { ChessLoad.diPawn_Black = value; }
       }
       //-------------------------------------------棋子位置部分--------------------------------------------//
       public static int[] arr_bishop_black;
       public static int[] arr_bishop_red;
       public static int[] arr_guard_black;
       public static int[] arr_guard_red;
       public static int[] arr_king_black;
       public static int[] arr_king_red;
       public static int black_king;
       public static int red_king;
       //-------------------------------------------规则部分------------------------------------------------//
       public static Abstract_Rule rule;
       public static Queue<Struct_Simple_State> same_action_state;
     
       /// <summary>
       /// 装载最初棋盘和棋子对应图片
       /// </summary>
       public static void  Load()
       {
           same_action_state = new Queue<Struct_Simple_State>(2);
           //加载棋盘规则辅助数组
           //相
           arr_bishop_black = new int[7] { 54, 84, 118, 88, 58, 92, 122 };
           arr_bishop_red = new int[7] { 198, 164, 134, 168, 202, 172, 138 };
           //士
           arr_guard_black = new int[5] { 55, 57, 72, 87, 89 };
           arr_guard_red = new int[5] { 167, 169, 184, 199, 201 };
           //帅
           arr_king_black = new int[9] { 55, 56, 57, 71, 72, 73, 87, 88, 89 };
           arr_king_red = new int[9] { 199, 200, 201, 183, 184, 185, 167, 168, 169 };
           black_king = 56;
           red_king = 200;
           //辅助数组加载棋盘
           Auxiliary_array = new int[256];
           DicImg = new Dictionary<int, string>();
           XmlDocument doc = new XmlDocument();
           doc.Load("ChessImg.xml");
           int index=0;string url;
           XmlElement rootElem = doc.DocumentElement;
           XmlNodeList Nodes = rootElem.GetElementsByTagName("id_img");
           foreach(XmlNode node in Nodes)
           {
               index = Convert.ToInt32(((XmlElement)node).GetAttribute("id"));
               url = ((XmlElement)node).GetAttribute("img");
               dicImg.Add(index, url);
           }
           dicMap= new Dictionary<int, string>();
           doc = new XmlDocument();
           //加载开局信息
           doc.Load("Manual.xml");
           rootElem =doc.DocumentElement;
           diPiece= new Dictionary<int, int>();
           Nodes = rootElem.GetElementsByTagName("position");
           foreach (XmlNode node in Nodes)
           {
               index = Convert.ToInt32(((XmlElement)node).GetAttribute("id"));
               url = ((XmlElement)node).GetAttribute("value");
               Auxiliary_array[index] = Convert.ToInt32(url);
               dicMap.Add(index, url);
               //加载棋子信息

               diPiece.Add(index, Convert.ToInt32(url));
           }
           diScore = new Dictionary<int, int>();
           doc = new XmlDocument();
           //加载得分部分
           doc.Load("Score.xml");
           rootElem = doc.DocumentElement;
           Nodes = rootElem.GetElementsByTagName("evaluate");
           foreach (XmlNode node in Nodes)
           {
               diScore.Add(Convert.ToInt32(((XmlElement)node).GetAttribute("id")), Convert.ToInt32(((XmlElement)node).GetAttribute("score")));
              
           }
           //兵进位辅助得分
           //红色
           doc.Load("Score.xml");
           Nodes = rootElem.GetElementsByTagName("pawn_red");
           diPawn_Red = new Dictionary<int, int>();
           foreach (XmlNode node in Nodes)
           {
               diPawn_Red.Add(Convert.ToInt32(((XmlElement)node).GetAttribute("position")), Convert.ToInt32(((XmlElement)node).GetAttribute("score"))*20);

           }
           //黑色
           Nodes = rootElem.GetElementsByTagName("pawn_black");
           diPawn_Black = new Dictionary<int, int>();
           foreach (XmlNode node in Nodes)
           {
               diPawn_Black.Add(Convert.ToInt32(((XmlElement)node).GetAttribute("position")), Convert.ToInt32(((XmlElement)node).GetAttribute("score"))*20);

           }
           //规则类加载
           string IRule_type = ConfigurationManager.ConnectionStrings["IRule"].ConnectionString;
           Type type = Type.GetType(IRule_type);
           rule = (Abstract_Rule)Activator.CreateInstance(type); 
           //剪枝树限制
           depth =Convert.ToInt32(ConfigurationManager.ConnectionStrings["depth"].ConnectionString);
           limit_score = Convert.ToInt32(ConfigurationManager.ConnectionStrings["score"].ConnectionString);
       }
    }
}
