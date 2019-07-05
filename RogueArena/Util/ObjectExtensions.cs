namespace RogueArena.Util
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter { Context = new StreamingContext(StreamingContextStates.Clone) };

                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

        public static bool DeepCompare<T>(this T obj1, T obj2)
        {
            var obj1String = GetBinaryString(obj1);
            var obj2String = GetBinaryString(obj2);

            return obj1String == obj2String;
        }

        private static string GetBinaryString<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter { Context = new StreamingContext(StreamingContextStates.Clone) };

                formatter.Serialize(stream, obj);
                stream.Position = 0;

                var builder = new StreamReader(stream);
                var obj1String = builder.ReadToEnd();
                return obj1String;
            }
        }
    }
}