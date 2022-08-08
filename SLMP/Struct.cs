using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SLMP
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SLMPStringAttribute : Attribute
    {
        public int length;
        public int wordCount => length % 2 == 0 ? length / 2 + 1 : (length + 1) / 2;
    }

    /// <summary>
    /// Functionality related to encoding/decoding struct.
    /// </summary>
    public static class Struct
    {
        /// <summary>
        /// This function returns the size of the struct in terms of 
        /// device words. (16 bit values)
        /// Int16, UInt16, Boolean size: 1 word (2 bytes)
        /// Int32, UInt32 size: 2 word (4 bytes)
        /// </summary>
        /// <param name="structType">Type of the structure.</param>
        public static int GetStructSize(Type structType)
        {
            int size = 0;
            var fieldTypes = structType.GetFields();

            foreach (var field in fieldTypes)
                switch (field.FieldType.Name)
                {
                    case "Int16":
                    case "UInt16":
                    case "Boolean":
                        size++;
                        break;
                    case "Int32":
                    case "UInt32":
                        size += 2;
                        break;
                    case "String":
                        SLMPStringAttribute? attr = field
                            .GetCustomAttributes<SLMPStringAttribute>()
                            .SingleOrDefault();
                        if (attr == default(SLMPStringAttribute))
                            throw new Exception("please add a SLMPStringAttribute to the string.");

                        size += attr.wordCount;
                        break;
                    default:
                        throw new Exception($"unsupported type: {field.FieldType.Name}");
                }

            return size;
        }

        public static object? FromBytes(Type structType, ushort[] words)
        {
            if (words == null || words.Length != GetStructSize(structType))
                return null;

            object? structObject = Activator.CreateInstance(structType);
            if (structObject == null)
                return null;

            int index = 0;
            var fields = structType.GetFields();

            foreach (var field in fields)
            {
                switch (field.FieldType.Name)
                {
                    case "Int16":
                        field.SetValue(structObject, (Int16)words[index]);
                        index++;
                        break;
                    case "UInt16":
                        field.SetValue(structObject, (UInt16)words[index]);
                        index++;
                        break;
                    case "Boolean":
                        field.SetValue(structObject, words[index] != 0);
                        index++;
                        break;
                    case "Int32":
                        field.SetValue(
                            structObject, (Int32)((words[index + 1] << 16) | words[index]));
                        index += 2;
                        break;
                    case "UInt32":
                        field.SetValue(
                            structObject, (UInt32)((words[index + 1] << 16) | words[index]));
                        index += 2;
                        break;
                    case "String":
                        SLMPStringAttribute? attr = field
                            .GetCustomAttributes<SLMPStringAttribute>()
                            .SingleOrDefault();
                        if (attr == default(SLMPStringAttribute))
                            throw new Exception("please add a SLMPStringAttribute to the string.");

                        List<char> buffer = new();
                        for (int i = index; i < index + attr.wordCount; i++)
                        {
                            ushort word = words[i];
                            buffer.Add((char)(word & 0xff));
                            buffer.Add((char)(word >> 0x8));
                        }
                        field.SetValue(
                            structObject, string.Join("", buffer.GetRange(0, attr.length)));
                        index += attr.wordCount;
                        break;
                    default:
                        throw new Exception($"unsupported type: {field.FieldType.Name}");
                }
            }

            return structObject;
        }
    }
}
