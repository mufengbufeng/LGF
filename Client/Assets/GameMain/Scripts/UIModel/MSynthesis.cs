
using System;
using LGF.MVC;

namespace GameMain.Scripts
{
    /// <summary>
    /// 移动方向
    /// </summary>
    public enum EMoveDirection
    {
        Left, // 左滑
        Right, // 右滑
        Up, // 上滑
        Down // 下滑
    }
    
    public class GridData
    {
        public int Number;
    }

    public class SynthesisData
    {
        public GridData[] GridData;
        public int Rows; //行数
        public int Columns; //列数
    }
    
    public class MSynthesis:LGFModel
    {
        public SynthesisData Data;

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="direction"></param>
        void Move(EMoveDirection direction)
        {
            switch (direction)
            {
                case EMoveDirection.Left:
                    for (int i = 0; i < Data.Rows; i++)
                    {
                        JudgeSame(Data.Rows * (i + 1),i,-1);
                    }
                    break;
                case EMoveDirection.Right:
                    for (int i = 0; i < Data.Rows; i++)
                    {
                        JudgeSame(i,Data.Rows * (i + 1),1);
                    }
                    break;
                case EMoveDirection.Down:
                    for (int i = 0; i < Data.Columns; i++)
                    {
                        JudgeSame(Data.Rows * (i + 1),i,-Data.Columns);
                    }
                    break;
                case EMoveDirection.Up:
                    for (int i = 0; i < Data.Columns; i++)
                    {
                        JudgeSame(i,Data.Rows * (i + 1),Data.Columns);
                    }
                    break;
            }
        }

        /// <summary>
        /// 合成
        /// </summary>
        void Merge()
        {
            
        }

        /// <summary>
        /// 判断跟邻位是否相同
        /// </summary>
        /// <param name="index">从这个数开始判断</param>
        /// <param name="stopIndex"></param>
        /// <param name="sign"> </param>
        void JudgeSame(int index,int stopIndex,int sign)
        {
            if (index - sign == stopIndex) return;
            if (Data.GridData[index] == Data.GridData[index - sign])
            {
                Merge();
                JudgeSame(index - 2 * sign,stopIndex,sign);
            }
            else 
            {
                JudgeSame(index - sign,stopIndex,sign);
            }
        }
        
        
    }
}