using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Oper.Sql.Common
{
    public static class Extensions
    {
        public static T Deserialize<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static string Serialize<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static string DataContractSerializeObject<T>(this T objectToSerialize)
        {
            using (MemoryStream memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, objectToSerialize);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            var attr = (T[])property.GetCustomAttributes(typeof(T), false);
            if (attr.Length > 0) return attr[0];
            return null;
        }

        public static string PrepareListParameter(this string value)
        {
            if (value != "0")
            {
                var parts = value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                value = string.Join("~", parts);
                value = $"~{value}~";
            }
            return value;
        }
    }
}