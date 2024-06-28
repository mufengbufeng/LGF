using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

public static class Json
{
    public static T FromJson<T>(string json) where T : class
    {
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            return jsonSerializer.ReadObject(stream) as T;
        }
    }

    public static string ToJson<T>(T obj)
    {
        using (var stream = new MemoryStream())
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            jsonSerializer.WriteObject(stream, obj);
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}