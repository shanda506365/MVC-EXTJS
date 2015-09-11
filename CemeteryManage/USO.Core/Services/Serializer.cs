
namespace USO.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        // Fields
        public static readonly bool CanBinarySerialize;

        // Methods
        static Serializer()
        {
            SecurityPermission permission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
            try
            {
                permission.Demand();
                CanBinarySerialize = true;
            }
            catch (SecurityException)
            {
                CanBinarySerialize = false;
            }
        }

        internal Serializer()
        {
        }

        public static object ConvertFileToObject(string path, Type objectType)
        {
            object obj2 = null;
            if ((path != null) && (path.Length > 0))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    obj2 = new XmlSerializer(objectType).Deserialize(stream);
                    stream.Close();
                }
            }
            return obj2;
        }

        public static void ConvertFromNameValueCollection(NameValueCollection nvc, ref string keys, ref string values)
        {
            ConvertFromNameValueCollection(nvc, ref keys, ref values, false);
        }

        public static void ConvertFromNameValueCollection(NameValueCollection nvc, ref string keys, ref string values, bool allowEmptyStrings)
        {
            if ((nvc != null) && (nvc.Count != 0))
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int num = 0;
                foreach (string str in nvc.AllKeys)
                {
                    if (str.IndexOf(':') != -1)
                    {
                        throw new ArgumentException("ExtendedAttributes Key can not contain the character \":\"");
                    }
                    string str2 = nvc[str];
                    if ((allowEmptyStrings && (str2 != null)) || !string.IsNullOrEmpty(str2))
                    {
                        builder.Append(str + ":S:" + num.ToString() + ":" + str2.Length.ToString() + ":");
                        builder2.Append(str2);
                        num += str2.Length;
                    }
                }
                keys = builder.ToString();
                values = builder2.ToString();
            }
        }

        //public static object ConvertSOAPToObject(string xml, Type objType)
        //{
        //    IFormatter formatter = new SoapFormatter();
        //    using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(xml)))
        //    {
        //        return formatter.Deserialize(stream);
        //    }
        //}

        public static byte[] ConvertToBytes(object objectToConvert)
        {
            byte[] buffer = null;
            if (CanBinarySerialize)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, objectToConvert);
                    stream.Position = 0L;
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Close();
                }
            }
            return buffer;
        }

        public static byte[] ConvertToBytes(string s)
        {
            return Convert.FromBase64String(s);
        }

        public static NameValueCollection ConvertToNameValueCollection(string keys, string values)
        {
            NameValueCollection values2 = new NameValueCollection();
            if (((keys != null) && (values != null)) && ((keys.Length > 0) && (values.Length > 0)))
            {
                char ch = ':';
                int num = 0;
                int num2 = 0;
                int startIndex = 0;
                int num4 = 0;
                int length = 0;
                string str = null;
                string str2 = null;
                while (-1 != (num = keys.IndexOf(ch, startIndex)))
                {
                    switch (num2)
                    {
                        case 0:
                            str = keys.Substring(startIndex, num - startIndex);
                            break;

                        case 1:
                            str2 = keys.Substring(startIndex, num - startIndex);
                            break;

                        case 2:
                            num4 = int.Parse(keys.Substring(startIndex, num - startIndex), CultureInfo.InvariantCulture);
                            break;

                        case 3:
                            length = int.Parse(keys.Substring(startIndex, num - startIndex), CultureInfo.InvariantCulture);
                            break;
                    }
                    startIndex = num + 1;
                    num2++;
                    if (num2 == 4)
                    {
                        num2 = 0;
                        if (((str2 == "S") && (num4 >= 0)) && (values.Length >= (num4 + length)))
                        {
                            values2[str] = values.Substring(num4, length);
                        }
                    }
                }
            }
            return values2;
        }

        public static object ConvertToObject(byte[] byteArray)
        {
            object obj2 = null;
            if ((CanBinarySerialize && (byteArray != null)) && (byteArray.Length > 0))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Position = 0L;
                    if (byteArray.Length > 4)
                    {
                        obj2 = formatter.Deserialize(stream);
                    }
                    stream.Close();
                }
            }
            return obj2;
        }

        public static object ConvertToObject(string xml, Type objectType)
        {
            object obj2 = null;
            if (!string.IsNullOrEmpty(xml))
            {
                using (StringReader reader = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(objectType);
                    try
                    {
                        obj2 = serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    reader.Close();
                }
            }
            return obj2;
        }

        public static object ConvertToObject(XmlNode node, Type objectType)
        {
            object obj2 = null;
            if (node != null)
            {
                using (StringReader reader = new StringReader(node.OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(objectType);
                    try
                    {
                        obj2 = serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    reader.Close();
                }
            }
            return obj2;
        }

        //public static string ConvertToSOAPString(object obj)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        IFormatter formatter = new SoapFormatter();
        //        formatter.Serialize(stream, obj);
        //        int length = (int)stream.Length;
        //        byte[] buffer = new byte[length];
        //        stream.Seek(0L, SeekOrigin.Begin);
        //        stream.Read(buffer, 0, length);
        //        stream.Close();
        //        UTF8Encoding encoding = new UTF8Encoding();
        //        return encoding.GetString(buffer).Trim();
        //    }
        //}

        public static string ConvertToString(byte[] arr)
        {
            return Convert.ToBase64String(arr);
        }

        public static string ConvertToString(object objectToConvert)
        {
            string str = null;
            if (objectToConvert != null)
            {
                XmlSerializer serializer = new XmlSerializer(objectToConvert.GetType());
                using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    serializer.Serialize((TextWriter)writer, objectToConvert);
                    str = writer.ToString();
                    writer.Close();
                }
            }
            return str;
        }

        public static object LoadBinaryFile(string path)
        {
            object obj2;
            if (!File.Exists(path))
            {
                return null;
            }
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    byte[] buffer = new byte[stream.Length];
                    reader.Read(buffer, 0, (int)stream.Length);
                    obj2 = ConvertToObject(buffer);
                }
            }
            return obj2;
        }

        public static bool SaveAsBinary(object objectToSave, string path)
        {
            if ((objectToSave != null) && CanBinarySerialize)
            {
                byte[] buffer = ConvertToBytes(objectToSave);
                if (buffer != null)
                {
                    using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            writer.Write(buffer);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void SaveAsXML(object objectToConvert, string path)
        {
            if (objectToConvert != null)
            {
                var serializer = new XmlSerializer(objectToConvert.GetType());
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize((TextWriter)writer, objectToConvert);
                    writer.Close();
                }
            }
        }

        public static string XMLDecode(string sTmp)
        {
            int[] numArray = new int[] { 0x26, 60, 0x3e, 0x22, 0x3d, 0x27 };
            for (int i = 0; i < numArray.Length; i++)
            {
                sTmp = sTmp.Replace("&#" + numArray[i].ToString() + ";", ((char)numArray[i]).ToString());
            }
            return sTmp;
        }

        public static string XMLEncode(string sTmp)
        {
            int[] numArray = new int[] { 0x26, 60, 0x3e, 0x22, 0x3d, 0x27 };
            for (int i = 0; i < numArray.Length; i++)
            {
                sTmp = sTmp.Replace(((char)numArray[i]).ToString(), "&#" + numArray[i].ToString() + ";");
            }
            return sTmp;
        }
    }
}
