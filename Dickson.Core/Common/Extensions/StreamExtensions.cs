using System;
using System.IO;
using System.Threading.Tasks;

namespace Dickson.Core.Common.Extensions
{
    public static class StreamExtensions
    {  /// <summary>
       /// 获取指定流中的所有字节。
       /// </summary>
       /// <param name="stream">指定流。</param>
       /// <returns>所有字节。</returns>
       /// <exception cref="ArgumentNullException">stream为null时引发。</exception>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();

            using (MemoryStream destination = new MemoryStream())
            {
                stream.CopyTo(destination);
                return destination.ToArray();
            }
        }

        /// <summary>
        /// 获取指定流中的所有字节。
        /// </summary>
        /// <param name="stream">指定流。</param>
        /// <returns>所有字节。</returns>
        /// <exception cref="ArgumentNullException">stream为null时引发。</exception>
        public static async Task<byte[]> ReadAllBytesAsync(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();

            using (MemoryStream destination = new MemoryStream())
            {
                await stream.CopyToAsync(destination);
                return destination.ToArray();
            }
        }
    }
}
