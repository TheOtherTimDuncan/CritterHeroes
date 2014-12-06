using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CH.Domain.Contracts;
using Newtonsoft.Json;

namespace CH.Domain.StateManagement
{
    public class StateSerializer : IStateSerializer
    {
        public string Serialize(object data)
        {
            string json = JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.Unicode.GetBytes(json);

            using (MemoryStream original = new MemoryStream(bytes))
            {
                using (MemoryStream compressed = new MemoryStream())
                {
                    using (GZipStream gz = new GZipStream(compressed, CompressionMode.Compress))
                    {
                        original.CopyTo(gz);
                    }
                    return Convert.ToBase64String(compressed.ToArray());
                }
            }
        }

        public T Deserialize<T>(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);

            using (MemoryStream decompressed = new MemoryStream())
            {
                using (MemoryStream compressed = new MemoryStream(bytes))
                {
                    using (GZipStream gz = new GZipStream(compressed, CompressionMode.Decompress ))
                    {
                        gz.CopyTo(decompressed);
                    }
                    string json = Encoding.Unicode.GetString(decompressed.ToArray());
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
        }
    }
}
