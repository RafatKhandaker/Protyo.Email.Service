using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Helper
{
    public class StringCompressionHelper
    {
        public byte[] CompressString(string inputString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }
        public string DecompressBytes(byte[] compressedBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (StreamReader streamReader = new StreamReader(gzipStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        public bool TryParseByteArray(string byteArrayString, out byte[] result)
        {
            string[] byteStrings = byteArrayString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            result = new byte[byteStrings.Length];

            for (int i = 0; i < byteStrings.Length; i++)
            {
                if (!byte.TryParse(byteStrings[i], out result[i]))
                {
                    // Parsing failed
                    return false;
                }
            }

            return true;
        }
    }
}
