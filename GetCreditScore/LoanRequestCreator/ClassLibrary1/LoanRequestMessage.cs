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

    public static class Extensions
    {
        public static byte[] ToByteArray(this LoanRequestMessage thisObject)
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, thisObject);

            return memoryStream.ToArray();
        }

        public static LoanRequestMessage ToLoanRequestMessage(this byte[] byteArray)
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();

            memoryStream.Write(byteArray, 0, byteArray.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var obj = (object)binaryFormatter.Deserialize(memoryStream);

            return obj as LoanRequestMessage;
        }
    }
}