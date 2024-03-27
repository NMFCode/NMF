using System;
using System.Buffers;
using System.IO;
using System.Text.Json;

namespace NMF.Serialization.Json
{
    /// <summary>
    /// Denotes a reader that reads directly from a buffer
    /// </summary>
    public ref struct Utf8JsonStreamReader
    {
        private readonly Stream _stream;
        private readonly int _bufferSize;

        private SequenceSegment _firstSegment;
        private int _firstSegmentStartIndex;
        private SequenceSegment _lastSegment;
        private int _lastSegmentEndIndex;

        internal Utf8JsonReader _jsonReader;
        private bool _keepBuffers;
        private bool _isFinalBlock;

        /// <summary>
        /// Creates a new stream reader
        /// </summary>
        /// <param name="stream">the underlying stream</param>
        /// <param name="bufferSize">the buffer size</param>
        public Utf8JsonStreamReader(Stream stream, int bufferSize)
        {
            _stream = stream;
            _bufferSize = bufferSize;

            _firstSegment = null;
            _firstSegmentStartIndex = 0;
            _lastSegment = null;
            _lastSegmentEndIndex = -1;

            _jsonReader = default;
            _keepBuffers = false;
            _isFinalBlock = false;
        }

        /// <summary>
        /// Attempts to read the next token
        /// </summary>
        /// <returns>true, if the underlying reader could be moved forward, otherwise false</returns>
        public bool Read()
        {
            // read could be unsuccessful due to insufficient bufer size, retrying in loop with additional buffer segments
            while (!_jsonReader.Read())
            {
                if (_isFinalBlock)
                    return false;

                MoveNext();
            }

            return true;
        }

        private void MoveNext()
        {
            var firstSegment = _firstSegment;
            _firstSegmentStartIndex += (int)_jsonReader.BytesConsumed;

            // release previous segments if possible
            if (!_keepBuffers)
            {
                while (firstSegment?.Memory.Length <= _firstSegmentStartIndex)
                {
                    _firstSegmentStartIndex -= firstSegment.Memory.Length;
                    firstSegment.Dispose();
                    firstSegment = (SequenceSegment?)firstSegment.Next;
                }
            }

            // create new segment
            var newSegment = new SequenceSegment(_bufferSize, _lastSegment);

            if (firstSegment != null)
            {
                _firstSegment = firstSegment;
                newSegment.Previous = _lastSegment;
                _lastSegment?.SetNext(newSegment);
                _lastSegment = newSegment;
            }
            else
            {
                _firstSegment = _lastSegment = newSegment;
                _firstSegmentStartIndex = 0;
            }

            // read data from stream
            _lastSegmentEndIndex = _stream.Read(newSegment.Buffer.Memory.Span);
            _isFinalBlock = _lastSegmentEndIndex < newSegment.Buffer.Memory.Length;
            _jsonReader = new Utf8JsonReader(new ReadOnlySequence<byte>(_firstSegment, _firstSegmentStartIndex, _lastSegment, _lastSegmentEndIndex), _isFinalBlock, _jsonReader.CurrentState);
        }

        /// <summary>
        /// Disposes the reader
        /// </summary>
        public readonly void Dispose() => _lastSegment?.Dispose();

        /// <summary>
        /// Gets the current depth of the reader
        /// </summary>
        public readonly int CurrentDepth => _jsonReader.CurrentDepth;

        /// <summary>
        /// True, if the underlying reader has a value sequence, otherwise False
        /// </summary>
        public readonly bool HasValueSequence => _jsonReader.HasValueSequence;
        
        /// <summary>
        /// Gets the index, the last processed token starts
        /// </summary>
        public readonly long TokenStartIndex => _jsonReader.TokenStartIndex;

        /// <summary>
        /// Gets the type of the last processed token
        /// </summary>
        public readonly JsonTokenType TokenType => _jsonReader.TokenType;

        /// <summary>
        /// Gets the raw value sequence of the last processed token
        /// </summary>
        public readonly ReadOnlySequence<byte> ValueSequence => _jsonReader.ValueSequence;

        /// <summary>
        /// Gets the raw value span of the last processed token
        /// </summary>
        public readonly ReadOnlySpan<byte> ValueSpan => _jsonReader.ValueSpan;

        /// <summary>
        /// Gets the boolean value of the last processed token
        /// </summary>
        /// <returns>true or false</returns>
        public bool GetBoolean() => _jsonReader.GetBoolean();

        /// <summary>
        /// Gets the byte value of the last processed token
        /// </summary>
        /// <returns>the byte value</returns>
        public byte GetByte() => _jsonReader.GetByte();

        /// <summary>
        /// Gets the bytes value extracted from a base64 string
        /// </summary>
        /// <returns>a byte array</returns>
        public byte[] GetBytesFromBase64() => _jsonReader.GetBytesFromBase64();
        
        /// <summary>
        /// Parses the current token value as a comment
        /// </summary>
        /// <returns>the comment</returns>
        public string GetComment() => _jsonReader.GetComment();
        
        /// <summary>
        /// Parses the current toen value as a date time
        /// </summary>
        /// <returns>the datetime</returns>
        public DateTime GetDateTime() => _jsonReader.GetDateTime();
        
        /// <summary>
        /// Parses the current token value as a data time offset
        /// </summary>
        /// <returns>the date time offset</returns>
        public DateTimeOffset GetDateTimeOffset() => _jsonReader.GetDateTimeOffset();
        
        /// <summary>
        /// Parses the current token value as decimal
        /// </summary>
        /// <returns>the decimal</returns>
        public decimal GetDecimal() => _jsonReader.GetDecimal();
        
        /// <summary>
        /// Parses the current token value as double
        /// </summary>
        /// <returns>the double</returns>
        public double GetDouble() => _jsonReader.GetDouble();
        
        /// <summary>
        /// Parses the current token value as guid
        /// </summary>
        /// <returns>the Guid</returns>
        public Guid GetGuid() => _jsonReader.GetGuid();
        
        /// <summary>
        /// Parses the current token value as short
        /// </summary>
        /// <returns>the short</returns>
        public short GetInt16() => _jsonReader.GetInt16();
        
        /// <summary>
        /// Parses the current token value as integer
        /// </summary>
        /// <returns>the integer</returns>
        public int GetInt32() => _jsonReader.GetInt32();
        
        /// <summary>
        /// Parses the current token value as long
        /// </summary>
        /// <returns>the long</returns>
        public long GetInt64() => _jsonReader.GetInt64();

        /// <summary>
        /// Parses the current token value as signed byte
        /// </summary>
        /// <returns>the signed byte</returns>
        public sbyte GetSByte() => _jsonReader.GetSByte();

        /// <summary>
        /// Parses the current token value as single
        /// </summary>
        /// <returns>the single</returns>
        public float GetSingle() => _jsonReader.GetSingle();

        /// <summary>
        /// Parses the current token value as string
        /// </summary>
        /// <returns>the string</returns>
        public string GetString() => _jsonReader.GetString();

        /// <summary>
        /// Parses the current token value as unsigned integer
        /// </summary>
        /// <returns>the unsigned integer</returns>
        public uint GetUInt32() => _jsonReader.GetUInt32();

        /// <summary>
        /// Parses the current token value as unsigned long
        /// </summary>
        /// <returns>the unsigned long</returns>
        public ulong GetUInt64() => _jsonReader.GetUInt64();

        /// <summary>
        /// Tries to parse the current token value as decimal
        /// </summary>
        /// <param name="value">the decimal or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetDecimal(out byte value) => _jsonReader.TryGetByte(out value);

        /// <summary>
        /// Tries to parse the current token value as base64 string
        /// </summary>
        /// <param name="value">the byte array represented as base64 or null, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetBytesFromBase64(out byte[] value) => _jsonReader.TryGetBytesFromBase64(out value);

        /// <summary>
        /// Tries to parse the current token value as datetime
        /// </summary>
        /// <param name="value">the datetime or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetDateTime(out DateTime value) => _jsonReader.TryGetDateTime(out value);

        /// <summary>
        /// Tries to parse the current token value as datetime with time zone
        /// </summary>
        /// <param name="value">the datetime with timezone or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetDateTimeOffset(out DateTimeOffset value) => _jsonReader.TryGetDateTimeOffset(out value);

        /// <summary>
        /// Tries to parse the current token value as decimal
        /// </summary>
        /// <param name="value">the decimal or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetDecimal(out decimal value) => _jsonReader.TryGetDecimal(out value);

        /// <summary>
        /// Tries to parse the current token value as double
        /// </summary>
        /// <param name="value">the double or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetDouble(out double value) => _jsonReader.TryGetDouble(out value);

        /// <summary>
        /// Tries to parse the current token value as Guid
        /// </summary>
        /// <param name="value">the Guid or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetGuid(out Guid value) => _jsonReader.TryGetGuid(out value);

        /// <summary>
        /// Tries to parse the current token value as short
        /// </summary>
        /// <param name="value">the short or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetInt16(out short value) => _jsonReader.TryGetInt16(out value);

        /// <summary>
        /// Tries to parse the current token value as integer
        /// </summary>
        /// <param name="value">the integer or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetInt32(out int value) => _jsonReader.TryGetInt32(out value);

        /// <summary>
        /// Tries to parse the current token value as long
        /// </summary>
        /// <param name="value">the long or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetInt64(out long value) => _jsonReader.TryGetInt64(out value);

        /// <summary>
        /// Tries to parse the current token value as signed byte
        /// </summary>
        /// <param name="value">the signed byte or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetSByte(out sbyte value) => _jsonReader.TryGetSByte(out value);

        /// <summary>
        /// Tries to parse the current token value as single
        /// </summary>
        /// <param name="value">the single or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetSingle(out float value) => _jsonReader.TryGetSingle(out value);

        /// <summary>
        /// Tries to parse the current token value as unsigned short
        /// </summary>
        /// <param name="value">the unsigned short or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetUInt16(out ushort value) => _jsonReader.TryGetUInt16(out value);

        /// <summary>
        /// Tries to parse the current token value as unsigned integer
        /// </summary>
        /// <param name="value">the unsigned integer or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetUInt32(out uint value) => _jsonReader.TryGetUInt32(out value);

        /// <summary>
        /// Tries to parse the current token value as unsigned long
        /// </summary>
        /// <param name="value">the unsigned long or zero, if the token could not be parsed</param>
        /// <returns>true, if the value was successfully parsed, otherwise false</returns>
        public bool TryGetUInt64(out ulong value) => _jsonReader.TryGetUInt64(out value);

        private sealed class SequenceSegment : ReadOnlySequenceSegment<byte>, IDisposable
        {
            internal IMemoryOwner<byte> Buffer { get; }
            internal SequenceSegment? Previous { get; set; }
            private bool _disposed;

            public SequenceSegment(int size, SequenceSegment? previous)
            {
                Buffer = MemoryPool<byte>.Shared.Rent(size);
                Previous = previous;
                
                Memory = Buffer.Memory;
                RunningIndex = previous?.RunningIndex + previous?.Memory.Length ?? 0;
            }

            public void SetNext(SequenceSegment next) => Next = next;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    Buffer.Dispose();
                    Previous?.Dispose();
                }
            }
        }
    }
}
