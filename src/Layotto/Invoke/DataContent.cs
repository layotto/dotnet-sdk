using System;

namespace Layotto.Invoke
{
    public class DataContent
    {
        public Memory<byte> Data { get; set; }

        public string ContentType { get; set; }
    }
}