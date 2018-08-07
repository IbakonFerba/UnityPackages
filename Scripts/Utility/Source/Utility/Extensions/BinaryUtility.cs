using UnityEngine;

namespace FK.Utility.Binary
{
    /// <summary>
    /// <para>Methods for bit manipulation and binary operations</para>
    /// 
    /// v1.0 06/2018
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

            return GetBitValue((uint)bits, bitIndex);
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

            return GetBitValue((ulong)bits, bitIndex);
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
    }
}