using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace jda_mvc.Classes.StreamEnum
{
    public delegate Stream StreamFactory();

    public abstract class ReadOnlyStream : Stream
    {
        #region Stream class not supported members
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        #endregion

        protected virtual void DisposeManaged() { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                DisposeManaged();
        }

        protected static class consts
        {
            public const byte cr = (byte)13;
            public const byte lf = (byte)10;
            public const byte quote = (byte)34;
            public const byte semiColon = (byte)59;
        }
    }

    public abstract class StreamChainBase : ReadOnlyStream
    {
        private IEnumerator<Stream> _enumerator;
        private Stream _current;
        private bool _needNext = true;
        private bool _done = false;
        protected abstract IEnumerable<Stream> _Streams { get; }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _current?.Dispose();
            _enumerator?.Dispose();
        }

        private bool UpdateCurrent()
        {
            if (_done)
                return false;
            if (_needNext)
            {
                if (_enumerator == null)
                {
                    _enumerator = _Streams.GetEnumerator();
                }
                if (_enumerator.MoveNext())
                {
                    _current = _enumerator.Current;
                    _needNext = false;
                    return true;
                }
                else
                {
                    _enumerator.Dispose();
                    _done = true;
                    return false;
                }
            }
            return true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead;
            int TotalBytesRead = 0;
            while ((count > 0) && (UpdateCurrent()))
            {
                bytesRead = _current.Read(buffer, offset, count);
                if (bytesRead <= 0)
                {
                    _needNext = true;
                    _current.Dispose();
                }
                else
                {
                    count -= bytesRead;
                    offset += bytesRead;
                    TotalBytesRead += bytesRead;
                }
            }
            return TotalBytesRead;
        }
    }

    public class StreamChain : StreamChainBase
    {
        private IEnumerable<Stream> __Streams;
        protected override IEnumerable<Stream> _Streams => __Streams;
        public StreamChain(IEnumerable<Stream> Streams)
        {
            __Streams = Streams;
        }
    }

    public abstract class SingleByteStreamBase : ReadOnlyStream
    {
        protected abstract byte b { get; }
        private bool _sent = false;
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_sent || (count <= 0))
            {
                return 0;
            }
            buffer[offset] = b;
            _sent = true;
            return 1;
        }
    }

    public class QuoteStream : SingleByteStreamBase
    {
        protected override byte b => consts.quote;
    }

    public class SemiColonStream : SingleByteStreamBase
    {
        protected override byte b => consts.semiColon;
    }

    public abstract class DblBytesStreamBase : ReadOnlyStream
    {
        protected abstract byte b1 { get; }
        protected abstract byte b2 { get; }
        private int _state = 0;
        public override int Read(byte[] buffer, int offset, int count)
        {
            switch (_state)
            {
                case 0:
                    if (count >= 1)
                    {
                        buffer[offset] = b1;
                        if (count >= 2)
                        {
                            buffer[offset + 1] = b2;
                            _state = 2;
                            return 2;
                        }
                        _state = 1;
                        return 1;
                    }
                    return 0;
                case 1:
                    if (count >= 1)
                    {
                        buffer[offset] = b2;
                        _state = 2;
                        return 1;
                    }
                    return 0;
                default:
                    return 0;
            }
        }
    }

    public class DblQuotesStream : DblBytesStreamBase
    {
        protected override byte b1 => consts.quote;
        protected override byte b2 => consts.quote;
    }

    public class CRLFStream : DblBytesStreamBase
    {
        protected override byte b1 => consts.cr;
        protected override byte b2 => consts.lf;
    }

    public abstract class UnescapedStringStreamBase : ReadOnlyStream
    {
        protected char[] _chars;
        protected int _start;
        protected int _length;
        protected Encoding _encoding;

        private Encoder __encoder = null;
        private Encoder _encoder { get { if (__encoder == null) __encoder = _encoding.GetEncoder(); return __encoder; } }
        private bool _done = false;


        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_length <= 0)
            {
                _done = true;
                return 0;
            }
            else if (_done || (count <= 0))
            {
                return 0;
            }
            else
            {
                int charsUsed;
                int bytesUsed;
                bool completed;
                _encoder.Convert(_chars, _start, _length, buffer, offset, count, true, out charsUsed, out bytesUsed, out completed);
                _start += charsUsed;
                _length -= charsUsed;
                if (completed)
                {
                    _done = true;
                }
                return bytesUsed;
            }
        }
    }

    public class UnescapedStringStream : UnescapedStringStreamBase
    {
        public UnescapedStringStream(char[] chars, Encoding encoding, int start = 0, int length = -1)
        {
            if (length >= 0)
            {
                _length = length;
            }
            else
            {
                if (chars == null)
                {
                    _length = 0;
                }
                else
                {
                    _length = chars.Length;
                }
            }
            if (_length > 0)
            {
                _chars = chars;
                _start = start;
                _encoding = encoding;
            }
        }

        public UnescapedStringStream(string s, Encoding encoding, int start = 0, int length = -1)
        {
            if (length >= 0)
            {
                _length = length;
            }
            else
            {
                if (string.IsNullOrEmpty(s))
                {
                    _length = 0;
                }
                else
                {
                    _length = s.Length;
                }
            }
            if (_length > 0)
            {
                _chars = s.ToCharArray(start, _length);
                _start = 0;
                _encoding = encoding;
            }
        }
    }

    public abstract class EscapedStringStreamBase : StreamChainBase
    {
        protected string _s;
        protected Encoding _encoding;
        protected int _start;
        protected int _length;

        protected override IEnumerable<Stream> _Streams
        {
            get
            {
                if (_length > 0)
                {
                    char[] _chars = _s.ToCharArray(_start, _length);
                    Match _match = (new Regex("[;\"\\r\\n]")).Match(_s, _start, _length);
                    if (_match.Success)
                    {
                        yield return new QuoteStream();
                        _match = (new Regex("\"")).Match(_s, _match.Index, _length - _match.Index + _start);
                        while (_match.Success)
                        {
                            int __length = _match.Index - _start;
                            yield return new UnescapedStringStream(_chars, _encoding, _start, __length);
                            yield return new DblQuotesStream();
                            _start = _match.Index + 1;
                            _length = _length - __length - 1;
                            _match = _match.NextMatch();
                        }
                        yield return new UnescapedStringStream(_chars, _encoding, _start, _length);
                        yield return new QuoteStream();
                    }
                    else
                    {
                        yield return new UnescapedStringStream(_chars, _encoding, _start, _length);
                    }
                }
            }
        }
    }

    public class EscapedStringStream : EscapedStringStreamBase
    {
        public EscapedStringStream(string s, Encoding encoding, int start = 0, int length = -1)
        {
            if (length >= 0)
            {
                _length = length;
            }
            else
            {
                if (string.IsNullOrEmpty(s))
                {
                    _length = 0;
                }
                else
                {
                    _length = s.Length;
                }
            }
            if (_length > 0)
            {
                _s = s;
                _encoding = encoding;
            }
        }
    }

    public abstract class SeparatedStreamsBase : StreamChainBase
    {
        protected abstract StreamFactory _SeparatorFactory { get; }
        protected abstract IEnumerable<Stream> __Streams { get; }
        protected override IEnumerable<Stream> _Streams
        {
            get
            {
                bool started = false;
                foreach(var s in __Streams)
                {
                    if (started)
                    {
                        yield return _SeparatorFactory();
                    }
                    yield return s;
                    started = true;
                }
            }
        }
    }

    public abstract class CRLFSeparatedStreamsBase : SeparatedStreamsBase
    {
        protected override StreamFactory _SeparatorFactory => () => new CRLFStream();
    }

    public abstract class SemiColonSeparatedStreamsBase : SeparatedStreamsBase
    {
        protected override StreamFactory _SeparatorFactory => () => new SemiColonStream();
    }

    public class CsvRowStream : SemiColonSeparatedStreamsBase
    {
        protected Encoding _encoding;
        private IEnumerable<string> _enum;
        protected override IEnumerable<Stream> __Streams
        {
            get
            {
                foreach (var s in _enum)
                    yield return new EscapedStringStream(s, _encoding);
            }
        }

        public CsvRowStream(IEnumerable<string> data, Encoding encoding)
        {
            _encoding = encoding;
            _enum = data;
        }
    }

    public class CsvStream : CRLFSeparatedStreamsBase
    {
        protected Encoding _encoding = Encoding.GetEncoding(28591);
        private IEnumerable<IEnumerable<string>> _enum;
        protected override IEnumerable<Stream> __Streams
        {
            get
            {
                foreach (var s in _enum)
                    yield return new CsvRowStream(s, _encoding);
            }
        }

        public CsvStream(IEnumerable<IEnumerable<string>> data)
        {
            _enum = data;
        }
    }
}
