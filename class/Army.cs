using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 兵牌生成器
{
    public class Army
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string Color { get; set; }
        public int Attack { get; set; }
        public int Organization { get; set; }
        public double Speed { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Army() { }
    }
}
