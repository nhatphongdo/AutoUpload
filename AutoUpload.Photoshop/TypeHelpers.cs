using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutoUpload.Photoshop
{
    public static class TypeHelpers
    {
        public static byte[] ToBytes(this int number)
        {
            byte[] intBytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            return intBytes;
        }

        public static byte[] ToBytes(this string text)
        {
            byte[] sizeArray = Encoding.UTF8.GetByteCount(text).ToBytes();
            byte[] byteArray = Encoding.UTF8.GetBytes(text);
            return sizeArray.Concat(byteArray);
        }

        public static int ToInt(this byte[] byteArray)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteArray);
            }
            return BitConverter.ToInt32(byteArray, 0);
        }

        public static int ToInt(this byte[] byteArray, ref int offset)
        {
            var intArray = new byte[sizeof(int)];
            Array.Copy(byteArray, offset, intArray, 0, intArray.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intArray);
            }
            offset += intArray.Length;
            return BitConverter.ToInt32(intArray, 0);
        }

        public static string ToString(this byte[] byteArray, out int size)
        {
            var sizeInt = sizeof(int);
            var sizeArray = new byte[sizeInt];
            Array.Copy(byteArray, sizeArray, sizeInt);
            size = sizeArray.ToInt() + sizeInt;
            return Encoding.UTF8.GetString(byteArray, sizeInt, size - sizeInt);
        }

        public static string ToString(this byte[] byteArray, ref int offset, out int size)
        {
            var sizeInt = sizeof(int);
            size = byteArray.ToInt(ref offset) + sizeInt;
            offset += size - sizeInt;
            return Encoding.UTF8.GetString(byteArray, offset - size + sizeInt, size - sizeInt);
        }

        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            T[] newArray = new T[x.Length + y.Length];
            Array.Copy(x, newArray, x.Length);
            Array.Copy(y, 0, newArray, x.Length, y.Length);
            return newArray;
        }

        public static string GetChecksum(this byte[] buffer)
        {
            using (var md5 = MD5.Create())
            {
                var md5Hash = md5.ComputeHash(buffer, 0, buffer.Length);
                return GetHexString(md5Hash);
            }
        }

        public static T[] GetSubArray<T>(this T[] array, int offset, int length)
        {
            var subArray = new T[length];
            Array.Copy(array, offset, subArray, 0, subArray.Length);
            return subArray;
        }

        public static byte[] Encode(this byte[] array, out byte[] key, out byte[] iv)
        {
            byte[] encrypted;
            using (var cipher = Aes.Create())
            {
                cipher.GenerateIV();
                cipher.GenerateKey();

                key = cipher.Key;
                iv = cipher.IV;

                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;

                using (var encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV))
                {
                    using (var to = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(array, 0, array.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
            }

            return encrypted;
        }

        public static byte[] Decode(this byte[] array, byte[] key, byte[] iv)
        {
            byte[] decrypted;
            using (var cipher = Aes.Create())
            {
                cipher.Key = key;
                cipher.IV = iv;
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;

                using (var decryptor = cipher.CreateDecryptor(cipher.Key, cipher.IV))
                {
                    using (var to = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(to, decryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(array, 0, array.Length);
                            writer.FlushFinalBlock();
                            decrypted = to.ToArray();
                        }
                    }
                }
            }

            return decrypted;
        }

        private static string GetHexString(byte[] bt)
        {
            var s = string.Empty;
            for (var i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = (int)b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + (int)'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char)(n1 - 10 + (int)'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
            }

            return s;
        }

        public static void Blend(this MagickImage image, MagickImage another, string blendMode, int x = 0, int y = 0)
        {
            switch (blendMode)
            {
                case "pass":
                    image.Composite(another, x, y, CompositeOperator.PegtopLight);
                    break;
                case "diss":
                    image.Composite(another, x, y, CompositeOperator.Dissolve);
                    break;
                case "dark":
                    image.Composite(another, x, y, CompositeOperator.Darken);
                    break;
                case "mul ":
                    image.Composite(another, x, y, CompositeOperator.Multiply);
                    break;
                case "idiv":
                    image.Composite(another, x, y, CompositeOperator.ColorBurn);
                    break;
                case "lbrn":
                    image.Composite(another, x, y, CompositeOperator.LinearBurn);
                    break;
                case "dkCl":
                    image.Composite(another, x, y, CompositeOperator.DarkenIntensity);
                    break;
                case "lite":
                    image.Composite(another, x, y, CompositeOperator.Lighten);
                    break;
                case "scrn":
                    image.Composite(another, x, y, CompositeOperator.Screen);
                    break;
                case "div ":
                    image.Composite(another, x, y, CompositeOperator.ColorDodge);
                    break;
                case "lddg":
                    image.Composite(another, x, y, CompositeOperator.LinearDodge);
                    break;
                case "lgCl":
                    image.Composite(another, x, y, CompositeOperator.LightenIntensity);
                    break;
                case "over":
                    image.Composite(another, x, y, CompositeOperator.Overlay);
                    break;
                case "sLit":
                    image.Composite(another, x, y, CompositeOperator.SoftLight);
                    break;
                case "hLit":
                    image.Composite(another, x, y, CompositeOperator.HardLight);
                    break;
                case "vLit":
                    image.Composite(another, x, y, CompositeOperator.VividLight);
                    break;
                case "lLit":
                    image.Composite(another, x, y, CompositeOperator.LinearLight);
                    break;
                case "pLit":
                    image.Composite(another, x, y, CompositeOperator.PinLight);
                    break;
                case "hMix":
                    image.Composite(another, x, y, CompositeOperator.HardMix);
                    break;
                case "diff":
                    image.Composite(another, x, y, CompositeOperator.Difference);
                    break;
                case "smud":
                    image.Composite(another, x, y, CompositeOperator.Exclusion);
                    break;
                case "fsub":
                    image.Composite(another, x, y, CompositeOperator.ModulusSubtract);
                    break;
                case "fdiv":
                    image.Composite(another, x, y, CompositeOperator.DivideDst);
                    break;
                case "hue ":
                    image.Composite(another, x, y, CompositeOperator.Hue);
                    break;
                case "sat ":
                    image.Composite(another, x, y, CompositeOperator.Saturate);
                    break;
                case "lum ":
                    image.Composite(another, x, y, CompositeOperator.Luminize);
                    break;
                case "colr":
                    image.Composite(another, x, y, CompositeOperator.Colorize);
                    break;
                case "norm":
                default:
                    image.Composite(another, x, y, CompositeOperator.Over);
                    break;
            }
        }
        
    }
}
