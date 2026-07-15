using System;
using System.Collections.Generic;
using System.Globalization;

namespace RbxFFlagDumper.Lib
{
    internal class PatternScanner
    {
        private readonly byte[] _binary;

        private readonly List<byte?> _pattern = new List<byte?>();

        private readonly int _end;

        private int _pos = 0;

        public PatternScanner(byte[] binary, string patternStr, int start, int length)
        {
            _binary = binary;
            _pos = start;

            foreach (string bit in patternStr.Split(' '))
            {
                if (bit == "??")
                    _pattern.Add(null); // lol
                else
                    _pattern.Add(Byte.Parse(bit, NumberStyles.HexNumber));
            }

            _end = start + length - _pattern.Count;
        }

        public PatternScanner(byte[] binary, string patternStr)
            : this(binary, patternStr, 0, binary.Length)
        {
        }

        public int FindNext()
        {
            // meh good enough
            while (!Finished())
            {
                for (int i = 0; i < _pattern.Count; i++)
                {
                    if (_pattern[i] is null)
                        continue;
                    else if (_binary[_pos+i] != _pattern[i])
                        break;
                    else if (i == _pattern.Count-1)
                    {
                        int result = _pos;
                        _pos += _pattern.Count;
                        return result;
                    }
                }

                _pos++;
            }

            return -1;
        }

        public bool Finished() 
            => _pos >= _end;

        public void Reset()
            => _pos = 0;
    }
}
