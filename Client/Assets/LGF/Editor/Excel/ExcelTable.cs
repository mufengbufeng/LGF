using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace LGF.Editor
{
    /// <summary>
    ///  Excel列
    /// </summary>
    public class ExcelColumn
    {
        public int col; // 列
        public int index; // 索引
        public string desc; // 描述
        public string name; // 名称
        public string type; // 类型
        public int typeInt => TypeStrToTypeInt[type]; // 类型
        public bool isServer; // 是否服务器字段

        static Dictionary<string, int> TypeStrToTypeInt = new()
        {
            { "uint", 1 }, // 无符号整数
            { "int", 2 }, // 整数
            { "short", 3 }, // 短整数
            { "byte", 4 }, // 字节
            { "bool", 5 }, // 布尔
            { "string", 6 }, // 字符串
            { "array", 10 }, // 数组
        };
    }

    public class ExcelTable
    {
        public string name;
        public string sysName;
        public string desc;
        public string ext;
        public int keyCount = 1;
        public List<string> descCols = new();

        public List<ExcelColumn> clientCols = new();

        // public List<ExcelColumn> serverCols = new List<ExcelColumn>(); TODO: 服务器字段
        public List<List<string>> dataRows = new();

        public int dataRowCount => dataRows.Count;

        public void Parse(ExcelWorksheet sheet)
        {
            var rowCount = sheet.Dimension.Rows;
            var colCount = sheet.Dimension.Columns;
            keyCount = 1;
            for (int row = 1; row <= rowCount; row++)
            {
                if (row >= 7)
                {
                    int isUse = int.Parse(ExcelUtil.GetValueString(sheet, row, 2, "-1"));
                    if (isUse > 0)
                        dataRows.Add(ExcelUtil.GetRowValueList(sheet, row));
                }
                else if (row == 1)
                {
                }
                else if (row == 2)
                {
                    ext = ExcelUtil.GetValueString(sheet, row, 2);
                    try
                    {
                        keyCount = int.Parse(ext);
                    }
                    catch (FormatException)
                    {
                        Debug.LogError($"错误的表格数据{sheet.Name},行：{row},列：1");
                    }
                }
                else if (row == 3)
                {
                    descCols = ExcelUtil.GetRowValueList(sheet, row);
                }
                else if (row == 4)
                {
                    ExcelUtil.ForEachColumn(sheet, row, (col, val) =>
                    {
                        var typeInfo = ExcelUtil.GetValueString(sheet, 5, col);
                        var useStr = ExcelUtil.GetValueString(sheet, 6, col).Split(',');
                        var mDesc = ExcelUtil.GetValueString(sheet, 3, col);
                        var isUseCline = useStr.Any(item => item.Contains("c"));
                        var isUseServer = useStr.Any(item => item.Contains("s"));
                        if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(typeInfo))
                        {
                            var ec = new ExcelColumn();
                            ec.col = col;
                            ec.desc = mDesc;
                            ec.index = col - 1;
                            ec.name = val;
                            ec.type = typeInfo;
                            if (isUseCline) clientCols.Add(ec);
                        }
                    });
                }
            }
        }

        public string GetValue(int rowIndex, ExcelColumn col)
        {
            Debug.Log($"行{rowIndex} 列：{col.index}");
            var val = dataRows[rowIndex][col.index - 1];
            if (col.type == "uint" || col.type == "int" || col.type == "short" || col.type == "byte")
            {
                if (string.IsNullOrEmpty(val)) return "0";
            }
            else if (col.type == "string")
            {
                // if (col.isServer) return string.Concat("<<\"", val, "\"/utf8>>");
                return $"\"{val}\"";
            }
            else if (col.type == "bool")
            {
                val = val.ToLower();
                if (val == "0" || val.Contains("false")) return "false";
                return "true";
            }
            else if (col.type == "array" || col.type.StartsWith("array"))
            {
                if (string.IsNullOrEmpty(val)) return "[]";
                var str = val.Replace('{', '[').Replace('}', ']').Replace('(', '[').Replace(')', ']');
                return str;
            }
            else if (col.type == "term")
            {
                if (string.IsNullOrEmpty(val)) return "[]";
            }

            return val;
        }

        public string GetSource(int rowIndex, ExcelColumn col)
        {
            return dataRows[rowIndex][col.index];
        }

        public string GetDesc(ExcelColumn col)
        {
            return descCols[col.index];
        }
    }
}