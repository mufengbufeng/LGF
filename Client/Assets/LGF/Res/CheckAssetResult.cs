namespace LGF.Res
{
    public enum CheckAssetResult
    {
        //资源不存在
        NotExist = 0,

        // 资源需要远端下载
        AssetOnline = 1,

        // 资源存在磁盘上
        AssetOnDisk = 2,

        // 资源在文件系统里
        AssetInFileSystem = 3,

        // 资源地址无效
        Valid = 4,
    }
}