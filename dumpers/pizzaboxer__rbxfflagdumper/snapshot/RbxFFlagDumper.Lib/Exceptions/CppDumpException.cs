using System;
using System.Collections.Generic;
using System.Text;

namespace RbxFFlagDumper.Lib.Exceptions
{
    public class CppDumpException : Exception
    {
        public CppDumpException(string message)
            : base(message)
        {
        }
    }
}
