using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace 兵牌生成器
{
    public static class ArmiesDispose
    {

        public static List<string> Strings = new List<string>()
        {
            "侦察连", "反坦克连", "工兵连", "机械化步兵连", 
            "机械化骑兵连", "步兵连", "炮兵连", "空降步兵连", 
            "空降炮兵连", "装甲连", "运输连", "防空连", "骑兵连"
        };

        public static List<string> Group = new List<string>() 
        {
            "旅","师","军","集团军","集团军群","战区"
        };

        public static List<Army> Armies { get; set; }

        public static int power { get; set; }

        public static void Init()
        {
            Strings = GetImageFileNames(Path.Combine(Application.StartupPath, "连级兵牌"));
            power = 1;
        }
        public static List<Army> Read(string name) 
        {
            var file = new FileInfo(name);
            List<Army> armies = new List<Army>();
            using (var package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets[0];
                // TODO: 读取工作表数据
                var dataRange = worksheet.Cells["A2:H" + worksheet.Dimension.End.Row];
                for(int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    Army army = new Army();
                    army.ID = BitmapTool.StringToInt(worksheet.Cells[i, 1].Value.ToString());
                    army.Country = worksheet.Cells[i, 2].Value.ToString();
                    army.Color = worksheet.Cells[i,3].Value.ToString(); 
                    army.Attack = BitmapTool.StringToInt(worksheet.Cells[i,4].Value.ToString());
                    army.Organization = BitmapTool.StringToInt(worksheet.Cells[i, 5].Value.ToString());
                    army.Speed = BitmapTool.StringToInt(worksheet.Cells[i, 6].Value.ToString());
                    army.Code = worksheet.Cells[i, 7].Value.ToString();
                    army.Name = worksheet.Cells[i, 8].Value.ToString();
                    armies.Add(army);   
                }
            }
            return armies;
        }

        public static List<Army> GetArmies(string name)
        {
            if(Armies is null)
            {
                Armies = Read(name);
                return Armies;
            }
            return Armies;
        }


        public static List<string> GetImageFileNames(string folderPath)
        {
            var extensions = new[] { ".png", ".jpg" };
            var fileNames = Directory.GetFiles(folderPath, "*.*")
            .Where(fileName => extensions.Contains(Path.GetExtension(fileName)))
            .Select(fileName => Path.GetFileNameWithoutExtension(fileName))
            .ToList();
            return fileNames;
        }
    }
}
