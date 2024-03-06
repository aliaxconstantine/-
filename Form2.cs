using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 兵牌生成器
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            numericUpDown1.Value = ArmiesDispose.power;
            label1.Text = "预设大小：" + (65 * ArmiesDispose.power).ToString() + "," + (50 * ArmiesDispose.power).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArmiesDispose.power = (int)numericUpDown1.Value;
            label1.Text = "预设大小：" + (65 * ArmiesDispose.power).ToString() + "," + (50 * ArmiesDispose.power).ToString();
        }
    }
}
