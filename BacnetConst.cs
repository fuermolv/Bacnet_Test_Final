using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class BacnetConst
    {
        public const Byte BIT0 = (0x01);
        public const Byte BIT1 = (0x02);
        public const Byte BIT2 = (0x04);
        public const Byte BIT3 = (0x08);
        public const Byte BIT4 = (0x10);
        public const Byte BIT5 = (0x20);
        public const Byte BIT6 = (0x40);
        public const Byte BIT7 = (0x80);
        public const Byte BACNET_INSTANCE_BITS = 22;
        public const uint BACNET_MAX_OBJECT = (0x3FF);
        public const uint BACNET_MAX_INSTANCE = (0x3FFFFF);
        public const uint BACNET_ARRAY_ALL = 0xFFFFFFFFU;
        public const uint MAX_APDU = 1024;
        public const uint MAX_CHARACTER_STRING_BYTES = MAX_APDU - 6;
        
    }
}
