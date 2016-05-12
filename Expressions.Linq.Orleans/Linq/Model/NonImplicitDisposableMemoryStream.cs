using System.IO;

namespace NMF.Expressions.Linq.Orleans.Model
{
    internal class NonImplicitDisposableMemoryStream : MemoryStream
    {
        public bool CanDispose { get; set; } = false;
        protected override void Dispose(bool disposing)
        {
            if(CanDispose)
                base.Dispose(disposing);
        }

        public NonImplicitDisposableMemoryStream()
        {
        }

        public NonImplicitDisposableMemoryStream(byte[] buffer) : base(buffer)
        {
        }

        public NonImplicitDisposableMemoryStream(int capacity) : base(capacity)
        {
        }

        public NonImplicitDisposableMemoryStream(byte[] buffer, bool writable) : base(buffer, writable)
        {
        }

        public NonImplicitDisposableMemoryStream(byte[] buffer, int index, int count) : base(buffer, index, count)
        {
        }

        public NonImplicitDisposableMemoryStream(byte[] buffer, int index, int count, bool writable) : base(buffer, index, count, writable)
        {
        }

        public NonImplicitDisposableMemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible) : base(buffer, index, count, writable, publiclyVisible)
        {
        }
    }
}