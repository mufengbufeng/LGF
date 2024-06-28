using System.Collections.Generic;

namespace LGF.Util
{
    public static class PathUtil
    {
        /// <summary>
        /// 转换为通用路径
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string ToSame(string fullName)
        {
            return fullName.Replace("\\", "/");
        }

        /// <summary>
        /// 转为相对路径
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string ToRelative(string rootPath, string fullPath)
        {
            var rootArr = rootPath.Split('/');
            var fullArr = fullPath.Split('/');
            var same = 0;
            for (int i = 0; i < rootArr.Length && i < fullArr.Length; i++)
            {
                if (rootArr[i] == fullArr[i]) same++;
                else break;
            }

            var sb = new System.Text.StringBuilder();
            for (int i = same; i < rootArr.Length; i++)
            {
                sb.Append("../");
            }

            return string.Concat(sb.ToString(), string.Join("/", fullArr, same, fullArr.Length - same));
        }

        /**
         * 获取目录名
         * @param path 文件路径
         */
        public static string DirName(string path, int deep = 1)
        {
            if (deep == 0) return path;
            var index = path.LastIndexOf('/');
            if (index != -1) return DirName(path.Substring(0, index), deep - 1);
            return "";
        }

        /**
         * 获取文件名
         * @param path 文件路径
         */
        public static string BaseName(string path)
        {
            var index = path.LastIndexOf('/');
            if (index != -1) return path.Substring(index + 1);
            return path;
        }

        /// <summary>
        /// 获取名字，删掉扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FirstName(string path)
        {
            path = BaseName(path);
            var i = path.LastIndexOf(".");
            if (i == -1) return path;
            return path.Substring(0, i);
        }

        /// <summary>
        /// 获取扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LastName(string path)
        {
            string basename = BaseName(path);
            int i = basename.LastIndexOf(".");
            if (i < 0) return "";
            return basename.Substring(i + 1);
        }

        /// <summary>
        /// 获取路径+名字，删掉扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string DirAndFirstName(string path)
        {
            path = ToSame(path);
            int i = path.LastIndexOf(".");
            if (i == -1) return path;
            return path.Substring(0, i);
        }

        /// <summary>
        /// 修正路径（../ ./ 这种
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FitPath(string path)
        {
            var list = new List<string>();
            var arr = path.Split('/');
            foreach (var t in arr)
            {
                if (t == ".") continue;
                if (t == "..")
                {
                    list.RemoveAt(list.Count - 1);
                    continue;
                }

                list.Add(t);
            }

            return string.Join('/', list);
        }
        
        
    }
}