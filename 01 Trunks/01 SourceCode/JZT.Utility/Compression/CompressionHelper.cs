using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace JZT.Utility.Compression
{
    /*----------------------------------------------------------------
// Copyright (C) 2011 九州通集团有限公司
// 版权所有。 
//
// 文件名：CompressionHelper.cs
// 文件功能描述：压缩帮助类
//     
// 
// 创建标识：徐凯 2011-1-27
//
// 修改标识：
// 修改描述
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
    public static class CompressionHelper
    {
        private const int BUFFER_SIZE = 4096;

        public static Stream Compress(Stream inputStream)
        {
            Stream stream = new MemoryStream();
            using (GZipStream output = new GZipStream(stream, CompressionMode.Compress, true))
            {
                int read;
                byte[] buffer = new byte[BUFFER_SIZE];

                while ((read = inputStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream Decompress(Stream inputStream)
        {
            Stream stream = new MemoryStream();
            using (GZipStream output = new GZipStream(inputStream, CompressionMode.Decompress, true))
            {
                int read;
                byte[] buffer = new byte[BUFFER_SIZE];

                while ((read = output.Read(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    stream.Write(buffer, 0, read);
                }
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }


    }
}
