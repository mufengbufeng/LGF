using System.Collections.Generic;
using System.IO;

namespace LGF.Util
{
    public static class FileUtil
    {
        /**
         * 获取目录下所有文件
         * @param dir 目录
         * @param filter 过滤条件
         * @param searchOption 搜索选项
         * @return 文件列表
         */
        public static List<string> GetFiles(string dir, string filter = "*",
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            var files = Directory.GetFiles(dir, filter, searchOption);
            var fileList = new List<string>();
            foreach (var file in files)
            {
                fileList.Add(PathUtil.ToSame(file));
            }

            return fileList;
        }

        /// <summary>
        /// 转换为相对路径
        /// </summary>
        /// <param name="rootPath">根目录</param>
        /// <param name="fullPath">完整路径</param>
        /// <returns></returns>
        public static string ToRelativePath(string rootPath, string fullPath)
        {
            var rootArr = rootPath.Split('/'); // 分割路径
            var fullArr = fullPath.Split('/'); // 分割路径
            int same = 0; // 相同路径开始位置
            for (var i = 0; i < rootArr.Length && i < fullArr.Length; i++)
            {
                if (rootArr[i] == fullArr[i]) same++; // 对比路径
                else break;
            }

            var sb = new System.Text.StringBuilder();
            for (int i = same; i < rootArr.Length; i++)
            {
                sb.Append("../");
            }

            return string.Concat(sb.ToString(), string.Join("/", fullArr, same, fullArr.Length - same));
        }

        /// <summary>
        /// 获取指定深度的目录名称
        /// </summary>
        /// <param name="path">完整路径</param>
        /// <param name="deep">深度</param>
        /// <returns>指定深度的目录名称</returns>
        public static string DirName(string path, int deep = 1)
        {
            if (deep == 0) return path;
            var index = path.LastIndexOf('/');
            if (index != 1) return DirName(path.Substring(0, index), deep - 1);
            return "";
        }
    }
}