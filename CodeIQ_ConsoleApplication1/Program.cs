using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeIQ_ConsoleApplication1
{
    class Program
    {
        private const string GEMS = "abbbbcddddeefggg";
        private static int GEM_NUM = GEMS.Length;

        static void Main(string[] args)
        {
            // 並列処理
            var task1 = Task.Factory.StartNew<ulong>(() =>
            {
                return calc("", "b");
            });
            var task2 = Task.Factory.StartNew<ulong>(() =>
            {
                return calc("b", "c");
            });
            var task3 = Task.Factory.StartNew<ulong>(() =>
            {
                return calc("c", "d");
            });
            var task4 = Task.Factory.StartNew<ulong>(() =>
            {
                return calc("d", "e");
            });
            var task5 = Task.Factory.StartNew<ulong>(() =>
            {
                return calc("e", "eagcdfbe");// 王女様の求める宝石パターンまで
            });

            // 処理結果
            ulong task1Result = task1.Result;
            ulong task2Result = task2.Result;
            ulong task3Result = task3.Result;
            ulong task4Result = task4.Result;
            ulong task5Result = task5.Result;
            Console.WriteLine("TASK1 RESULT:" + task1Result);
            Console.WriteLine("TASK2 RESULT:" + task2Result);
            Console.WriteLine("TASK3 RESULT:" + task3Result);
            Console.WriteLine("TASK4 RESULT:" + task4Result);
            Console.WriteLine("TASK5 RESULT:" + task5Result);

            // 日数
            ulong days = task1Result + task2Result + task3Result + task4Result + task5Result;
            Console.WriteLine(days + "日目");
        }

        private static ulong calc(string startPattern, string endPattern)
        {
            string gemPattern = startPattern;
            ulong days = 0;
            do
            {
                // 日数インクリメント
                days++;

                // 次のジェムパターンを取得
                gemPattern = getNextGemPattern(gemPattern);
                if (days % 1000000 == 0)
                {
                    Console.WriteLine(DateTime.Now + " / " + gemPattern + " : " + days);
                }
            } while (gemPattern != endPattern);

            Console.WriteLine("CALC END : " + gemPattern);
            return days;
        }

        private static string getNextGemPattern(string lastGemPattern)
        {
            string nextGemPattern = null;

            if (lastGemPattern.Length < GEM_NUM)
            {   // すべてのジェムを使用していない場合
                List<char> gemsCopy = GEMS.ToList();

                // 使用中のジェムを削除
                foreach (char gem in lastGemPattern)
                {
                    gemsCopy.Remove(gem);
                }

                // 使用中のジェムを削除し、残ったものの先頭が次のジェムになる
                nextGemPattern = lastGemPattern + gemsCopy[0];  // 次のジェムパターン
            }
            else
            {   // すべてのジェムを使用している場合
                char[] lastGemPatternArray = lastGemPattern.ToArray();
                List<char> gemsRemain = new List<char>();
                StringBuilder sb = new StringBuilder();

                for (int i = 1; i <= lastGemPattern.Length && nextGemPattern == null; i++)
                {
                    // 最後尾から順に文字を削除していく
                    char tailGem = lastGemPatternArray[lastGemPatternArray.Length - i];

                    // 削除した文字を、使用できるジェムのリストに追加
                    gemsRemain.Add(tailGem);

                    // 文字を昇順にソート
                    gemsRemain.Sort();

                    foreach (char gem in gemsRemain)
                    {
                        // 次のジェム候補を作る
                        sb.Clear();
                        for (int j = 0; j < lastGemPatternArray.Length - i; j++)
                        {
                            sb.Append(lastGemPatternArray[j]);
                        }
                        sb.Append(gem);

                        // 文字列比較で、前回のジェムパターンより大きいかで判定
                        string gemPattern = sb.ToString();
                        if (gemPattern.CompareTo(lastGemPattern.Substring(0, gemPattern.Length)) > 0)
                        {
                            nextGemPattern = gemPattern;    // 次のジェムパターン
                            break;
                        }
                    }
                }
            }

            return nextGemPattern;
        }
    }
}
