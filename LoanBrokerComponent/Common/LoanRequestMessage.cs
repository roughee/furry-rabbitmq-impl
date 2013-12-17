using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    [Serializable]
    public class LoanRequestMessage
    {
        public int Ssn { get; set; }
        public int Amount { get; set; }
        /// <summary>
        /// Represented in days.
        /// </summary>
        public int Duration { get; set; }
    }

    [Serializable]
    public class CreditScoreMessage : LoanRequestMessage
    {
        public int CreditScore { get; set; }
    }

    [Serializable]
    public class BanksListMessage : CreditScoreMessage
    {
        public List<String> BanksList { get; set; }
    }

    public static class Extensions
    {
        public static byte[] ToByteArray(this string thisObject)
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, thisObject);

            return memoryStream.ToArray();
        }

        public static byte[] ToByteArray(this Object thisObject)
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, thisObject);

            return memoryStream.ToArray();
        }

        public static T ToRequestMessage<T>(this byte[] byteArray) where T : class
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();

            memoryStream.Write(byteArray, 0, byteArray.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var obj = binaryFormatter.Deserialize(memoryStream);

            return obj as T;
        }
    }
}