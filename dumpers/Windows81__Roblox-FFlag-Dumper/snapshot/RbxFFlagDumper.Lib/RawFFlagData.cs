using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbxFFlagDumper.Lib
{
    internal class RawFFlagData
    {
        public int DataTypeId;

        public int ByteParam;

        public string Name;

        public RawFFlagData(int dataTypeId, int byteParam, string name)
        {
            DataTypeId = dataTypeId;
            ByteParam = byteParam;
            Name = name;
        }
    }
}
