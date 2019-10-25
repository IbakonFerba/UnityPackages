using System;
using System.Collections.Generic;
using UnityEngine;

namespace FK.Utility.Binary
{
    /// <summary>
    /// <para>Methods for bit manipulation and binary operations</para>
    /// 
    /// v1.1 10/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class BinaryUtility
    {
        #region GET_BIT_VALUE

        private static bool GetBit(ulong bits, int bitIndex)
        {
            return ((bits >> bitIndex) & 1) == 1;
        }


        /// <summary>
        /// Takes a byte and an index between 0 and 7 and returns true if the corresponding bit is 1 or false if it is 0
        /// </summary>
        /// <param name="bits">The byte you want to read a bit from</param>
        /// <param name="bitIndex">Index of the bit you want to read, the least significant bit is index 0</param>
        /// <returns></returns>
        public static bool GetBitValue(this byte bits, int bitIndex)
        {
            if (bitIndex > 7)
            {
                Debug.LogWarningFormat("Cannot read bit {0} of a byte (has 8 bits), will default to 0", bitIndex);
                return false;
            }

            return GetBit(bits, bitIndex);
        }

        /// <summary>
        /// Takes an integer and an index between 0 and 31 and returns true if the corresponding bit is 1 or false if it is 0
        /// </summary>
        /// <param name="bits">The int you want to read a bit from</param>
        /// <param name="bitIndex">Index of the bit you want to read, the least significant bit is index 0</param>
        /// <returns></returns>
        public static bool GetBitValue(this int bits, int bitIndex)
        {
            if (bitIndex > 31)
            {
                Debug.LogWarningFormat("Cannot read bit {0} of an int (has 32 bits), will default to 0", bitIndex);
                return false;
            }

            return GetBitValue((uint) bits, bitIndex);
        }

        /// <summary>
        /// Takes an unsigned integer and an index between 0 and 31 and returns true if the corresponding bit is 1 or false if it is 0
        /// </summary>
        /// <param name="bits">The uint you want to read a bit from</param>
        /// <param name="bitIndex">Index of the bit you want to read, the least significant bit is index 0</param>
        /// <returns></returns>
        public static bool GetBitValue(this uint bits, int bitIndex)
        {
            if (bitIndex > 31)
            {
                Debug.LogWarningFormat("Cannot read bit {0} of an int (has 32 bits), will default to 0", bitIndex);
                return false;
            }

            return GetBit(bits, bitIndex);
        }

        /// <summary>
        /// Takes a long and an index between 0 and 63 and returns true if the corresponding bit is 1 or false if it is 0
        /// </summary>
        /// <param name="bits">The long you want to read a bit from</param>
        /// <param name="bitIndex">Index of the bit you want to read, the least significant bit is index 0</param>
        /// <returns></returns>
        public static bool GetBitValue(this long bits, int bitIndex)
        {
            if (bitIndex > 63)
            {
                Debug.LogWarningFormat("Cannot read bit {0} of an int (has 64 bits), will default to 0", bitIndex);
                return false;
            }

            return GetBitValue((ulong) bits, bitIndex);
        }

        /// <summary>
        /// Takes an unsingend long and an index between 0 and 63 and returns true if the corresponding bit is 1 or false if it is 0
        /// </summary>
        /// <param name="bits">The ulong you want to read a bit from</param>
        /// <param name="bitIndex">Index of the bit you want to read, the least significant bit is index 0</param>
        /// <returns></returns>
        public static bool GetBitValue(this ulong bits, int bitIndex)
        {
            if (bitIndex > 63)
            {
                Debug.LogWarningFormat("Cannot read bit {0} of an int (has 64 bits), will default to 0", bitIndex);
                return false;
            }

            return GetBit(bits, bitIndex);
        }

        #endregion

        #region FIND_FIRST_SET_BIT

        private static readonly Dictionary<UInt64, int> BIT_LOOCKUP = new Dictionary<UInt64, int>()
        {
            {1, 0}, {2, 1}, {4, 2}, {8, 3}, {16, 4}, {32, 5}, {64, 6}, {128, 7}, {256, 8}, {512, 9}, {1024, 10}, {2048, 11}, {4096, 12}, {8192, 13}, {16384, 14}, {32768, 15}, {65536, 16},
            {131072, 17}, {262144, 18}, {524288, 19}, {1048576, 20}, {2097152, 21}, {4194304, 22}, {8388608, 23}, {16777216, 24}, {33554432, 25}, {67108864, 26}, {134217728, 27}, {268435456, 28},
            {536870912, 29}, {1073741824, 30}, {2147483648, 31}, {4294967296, 32}, {8589934592, 33}, {17179869184, 34}, {34359738368, 35}, {68719476736, 36}, {137438953472, 37}, {274877906944, 38},
            {549755813888, 39}, {1099511627776, 40}, {2199023255552, 41}, {4398046511104, 42}, {8796093022208, 43}, {17592186044416, 44}, {35184372088832, 45}, {70368744177664, 46},
            {140737488355328, 47}, {281474976710656, 48}, {562949953421312, 49}, {1125899906842624, 50}, {2251799813685248, 51}, {4503599627370496, 52}, {9007199254740992, 53},
            {18014398509481984, 54}, {36028797018963968, 55}, {72057594037927936, 56}, {144115188075855872, 57}, {288230376151711744, 58}, {576460752303423488, 59}, {1152921504606846976, 60},
            {2305843009213693952, 61}, {4611686018427387904, 62}, {9223372036854775808, 63}
        };

        /// <summary>
        /// Returns the position of the least significant bit that is set. -1 if no bit is set
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetLeastSignificantBitPosition(this byte value)
        {
            return value == 0 ? -1 : BIT_LOOCKUP[(byte) (value & (~value + 1))];
        }

        /// <summary>
        /// Returns the position of the least significant bit that is set. -1 if no bit is set
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetLeastSignificantBitPosition(this UInt16 value)
        {
            return value == 0 ? -1 : BIT_LOOCKUP[(UInt16) (value & (~value + 1))];
        }

        /// <summary>
        /// Returns the position of the least significant bit that is set. -1 if no bit is set
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetLeastSignificantBitPosition(this uint value)
        {
            return value == 0 ? -1 : BIT_LOOCKUP[(uint) (value & (~value + 1))];
        }

        /// <summary>
        /// Returns the position of the least significant bit that is set. -1 if no bit is set
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetLeastSignificantBitPosition(this ulong value)
        {
            return value == 0 ? -1 : BIT_LOOCKUP[(ulong) (value & (~value + 1))];
        }

        #endregion
    }
}