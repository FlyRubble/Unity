using Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Setting.Init();

            var a = Util.Open("d://char-角色配置表.xlsm", null);
            Encoding cd = new UTF8Encoding(false);
            string json = JsonConvert.SerializeObject(a, Formatting.Indented);
            using (FileStream file = new FileStream("d://char.json", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, cd))
                {
                    writer.Write(json);
                }
            }
        }
    }
}
