using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestovoeZadaaniye_OrlovArtem
{
    public class TableField //Класс с данными о столбце
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsPrimaryKey { get; set; }

    }
}
