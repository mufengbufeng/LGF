using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace LGF.Editor
{
    public static class ExcelUtil
    {
        public static ExcelTable Read(string fileName, string sheetName = null)
        {
            using FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); // 打开文件                              
            using ExcelPackage package = new ExcelPackage(stream); // 创建包

            ExcelWorksheet ws = null; // 表
            ExcelTable et = new ExcelTable(); // 创建表
            et.name = System.IO.Path.GetFileNameWithoutExtension(fileName); // 文件名
            et.sysName = $"sys_{et.name}"; // 系统名
            if (!string.IsNullOrEmpty(sheetName)) ws = package.Workbook.Worksheets[sheetName]; // 读取指定表
            ws ??= package.Workbook.Worksheets[0]; // 读取第一个表
            et.Parse(ws); // 解析表
            return et;
        }

        /**
         * 判断单元格是否为空
         * @param ws 表
         * @param row 行
         * @param col 列
         */
        public static bool IsValueNull(ExcelWorksheet ws, int row, int col)
        {
            return ws.Cells[row, col].Value == null; // || string.IsNullOrEmpty(ws.Cells[row, col].Value.ToString());
        }

        public static string GetValueString(ExcelWorksheet ws, int row, int col, string def = "")
        {
            var val = ws.Cells[row, col].Value;
            if (val == null) return def;
            return val.ToString();
        }

        public static List<string> GetRowValueList(ExcelWorksheet ws, int row)
        {
            List<string> list = new List<string>();
            for (int i = 2; i <= ws.Dimension.Columns; i++)
            {
                list.Add(GetValueString(ws, row, i));
            }

            return list;
        }

        public static void ForEachColumn(ExcelWorksheet ws, int row, System.Action<int, string> action)
        {
            for (int i = 2; i <= ws.Dimension.Columns; i++)
            {
                action(i, GetValueString(ws, row, i));
            }
        }
    }
}