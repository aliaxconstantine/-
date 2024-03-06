using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 兵牌生成器
{
    public partial class Form1 : Form
    {
        public Color color;
        public Color oldercolor;
        public Boolean IfCreate = false;

        string path;
        string savepath;
        public Form1()
        {
            InitializeComponent();
            savepath = Path.Combine(Application.StartupPath, "兵牌");
            ArmiesDispose.Init();
            label4.Text = "读取到的类别有：" + string.Join(",",ArmiesDispose.Strings.ToArray());
            button2.Enabled = false;
            button3.Enabled = false;
        }
        public void Fresh(List<Army> armyList)
        {
            listView1.Clear(); // 清空原有的内容
            listView1.View = View.Details; 
            // 添加列头
            listView1.Columns.Add("ID", 50);
            listView1.Columns.Add("国家名", 100);
            listView1.Columns.Add("颜色代码", 100);
            listView1.Columns.Add("攻击", 70);
            listView1.Columns.Add("组织度", 100);
            listView1.Columns.Add("速度", 70);
            listView1.Columns.Add("番号", 100);
            listView1.Columns.Add("类别", 100);

            // 遍历 Army 列表，添加每行数据
            foreach (Army army in armyList)
            {
                ListViewItem item = new ListViewItem(army.ID.ToString());
                item.SubItems.Add(army.Country);
                item.SubItems.Add(army.Color);
                item.SubItems.Add(army.Attack.ToString());
                item.SubItems.Add(army.Organization.ToString());
                item.SubItems.Add(army.Speed.ToString());
                item.SubItems.Add(army.Code);
                item.SubItems.Add(army.Name);
                listView1.Items.Add(item);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置文件对话框的属性
            openFileDialog.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls";
            openFileDialog.Title = "选择 Excel 文件";
            openFileDialog.Multiselect = false;

            // 显示文件对话框，并获取用户选择的文件路径
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                // 在这里可以调用读取 Excel 文件的方法，例如 ReadDataFromExcel(filePath);
                path = filePath;
                var s = ArmyLog.Inspection(filePath);
                if (s.Keys.First())
                {
                    var a = ArmiesDispose.GetArmies(path);
                    Fresh(a);
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("错误列："+  string.Join(",", s.Values.First().ToArray()),"表格内容错误");
                }
            }
            else
            {
                // 用户取消了选择，不执行任何操作
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(path != null)
            {
                if (ArmiesDispose.GetArmies(path).Count > 0)
                {
                    var checkd = ArmyLog.CheckData(ArmiesDispose.GetArmies(path));
                    if(checkd.Count == 0)
                    {
                        foreach (Army army in ArmiesDispose.GetArmies(path))
                        {

                            Image image = BitmapTool.DrawArmy(army,ArmiesDispose.power);
                            string filespath = Path.Combine(savepath);
                            BitmapTool.CreateDirectory(savepath);
                            image.Save(Path.Combine(savepath, army.Code+".png"), ImageFormat.Png);
                        }
                        Process.Start("explorer.exe", savepath);
                        MessageBox.Show("生成完毕");
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var check in checkd)
                        {
                            sb.Append(check.First().Key.Code + ":错误代码" + check.First().Value);
                        }
                        MessageBox.Show("错误:" + sb);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(ArmiesDispose.Armies.Count > 0)
            {
                var Error = ArmyLog.CheckData(ArmiesDispose.Armies);
                if (path != null)
                {
                    ArmiesDispose.Armies = ArmiesDispose.Read(path);
                    Fresh(ArmiesDispose.Armies);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
