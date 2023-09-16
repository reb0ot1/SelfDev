using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.Spans
{
    public class Guider
    {

        private const char EqualsChar = '=';
        private const char HyphenChar = '-';
        private const char UnderScoreChar = '_';
        private const char Slash = '/';
        private const byte SlashByte = (byte)'/';
        private const char Plus = '+';
        private const byte PlusByte = (byte)'+';

        public static string ToStringFromGuid(Guid id)
        {
            var test1 = Convert.ToBase64String(id.ToByteArray());
            test1 = test1.Replace("/", "-");
            test1 = test1.Replace("+", "_")
                .Replace("=", string.Empty);

            return test1;
        }

        public static Guid ToGuidFromString(string id)
        { 
            var efficientBase64 = Convert.FromBase64String(id.Replace("-","/").Replace("_","+") + "==");

            return new Guid(efficientBase64);
        }

        public static Guid ToGuidFromStringOp(ReadOnlySpan<char> id)
        {
            //Using Span in order not allocate additional memory in the hash
            //Working with chars
            Span<char> base64Chars = stackalloc char[24];
            for (int i = 0; i < 22; i++)
            {
                base64Chars[i] = id[i] switch {
                    '-' => '/',
                    '_' => '+',
                    _ => id[i]
                };
            }
            base64Chars[22] = '=';
            base64Chars[23] = '=';

            //Get byte array already allocated as memory
            Span<byte> byteRepresentation = stackalloc byte[16];

            //Convert charArray into byteArray
            Convert.TryFromBase64Chars(base64Chars, byteRepresentation, out _);

            //Generate guid;
            return new Guid(byteRepresentation);
        }

        public static string ToStringFromGuidOp(Guid id)
        {
            Span<byte> idBytes = stackalloc byte[16];
            Span<byte> base64Bytes = stackalloc byte[24];

            MemoryMarshal.TryWrite(idBytes, ref id);
            Base64.EncodeToUtf8(idBytes, base64Bytes, out _, out _);

            Span<char> finalCharacters = stackalloc char[22];

            for (int i = 0; i < 22; i++)
            {
                finalCharacters[i] = base64Bytes[i] switch
                {
                    SlashByte => HyphenChar,
                    PlusByte => UnderScoreChar,
                    _ => (char)base64Bytes[i]
                };
            }

            return new string(finalCharacters);
        }
    }
}
