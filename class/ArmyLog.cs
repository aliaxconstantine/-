using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 兵牌生成器
{
    public class ArmyLog
    {


        public static List<Dictionary<Army,string>> CheckData(List<Army> armies) 
        {
            var pairs = new List<Dictionary<Army, string>>();
            foreach(var item in armies)
            {
                if (!BitmapTool.colorRegex.IsMatch(item.Color))
                {
                    pairs.Add(new Dictionary<Army, string>() { { item, item.Color } });
                }
                if (!ArmiesDispose.Strings.Contains(item.Name))
                {
                    pairs.Add(new Dictionary<Army, string>() { { item, item.Name } });
                }
            }
            return pairs;
        }
        public static Dictionary<bool,List<string>> Inspection(string message)
        {
            using(ExcelPackage package = new ExcelPackage(message))
            {
                int trueNum = 0;
                List<string> strings = new List<string>();
                if (package.Workbook.Worksheets.Count > 0)
                {
                    var steel = package.Workbook.Worksheets[0];
                    List<string> list = new List<string>()
                    {
                        "ID","国家名","颜色","攻击","组织度","速度","番号","类别"
                    };

                    for(int num = 1; num < 9; num++)
                    {
                        if (steel.Cells[1,num].Value is string a)
                        {
                            if(a == list[num - 1])
                            {
                                trueNum++;
                            }
                        }
                        else
                        {
                            strings.Add(list[num-1]);
                        }
                    }

                    if(trueNum == 8)
                    {
                        return new Dictionary<bool, List<string>> { { true,null } };
                    }
                }
                return  new Dictionary<bool, List<string>> { { false, strings } };
            }
        }
    }
}
