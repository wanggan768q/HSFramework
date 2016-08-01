using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HS.IO
{
    public static class HS_ByteRead
    {
        public static bool ReadCsvFile(string Filename, out string csvContent)
        {
            csvContent = "";
            try
            {
                //Debug.Log(fullName);
                StreamReader readerConfig = new StreamReader(Filename, Encoding.UTF8);
                csvContent = readerConfig.ReadToEnd();
                readerConfig.Close();
            }
            catch (Exception e)
            {
                e.GetType();
                return false;
            }
            return true;
        }

        public static bool ReadBinFile(string Filename, out byte[] binContent)
        {
            binContent = null;
            //        try
            //        {
            //            string fullName = "table/" + Filename;
            //            FileInfo fileInfo = new FileInfo(fullName);
            //            binContent = new byte[fileInfo.Length];
            //            FileStream file = new FileStream(fullName, FileMode.Open);
            //            file.Seek(0, SeekOrigin.Begin);
            //            file.Read(binContent, 0, (int)fileInfo.Length);
            //            file.Close();
            //        }
            //        catch( Exception e )
            //        {
            //            e.GetType();
            //            return false;
            //        }
            return true;
        }

        public static int ReadString(byte[] ioBuffer, int ioIndex, out string value)
        {
            int readPos = ioIndex;
            int len = 0;
            readPos += ReadInt32Variant(ioBuffer, readPos, out len);
            value = System.Text.Encoding.UTF8.GetString(ioBuffer, readPos, len);
            readPos += len;
            return (readPos - ioIndex);
        }
        public static int ReadFloat(byte[] ioBuffer, int ioIndex, out float value)
        {
            int readPos = ioIndex;
            int len = 0;
            readPos += ReadInt32Variant(ioBuffer, readPos, out len);
            //		value = System.Text.Encoding.UTF8.GetString(ioBuffer, readPos, len);
            value = BitConverter.ToSingle(ioBuffer, readPos);
            readPos += len;
            return (readPos - ioIndex);
        }

        public static int ReadBool(byte[] ioBuffer, int ioIndex, out bool value)
        {
            int readPos = ioIndex;
            int len = 0;
            readPos += ReadInt32Variant(ioBuffer, readPos, out len);
            value = BitConverter.ToBoolean(ioBuffer, readPos);
            readPos += len;
            return (readPos - ioIndex);
        }

        public static List<string> readCsvLine(string CsvContent, ref int offset)
        {
            List<string> splitList = new List<string>();
            bool bInQuato = false;

            int len = CsvContent.Length;
            string word = "";
            while (offset < len)
            {
                if (!bInQuato && CsvContent[offset] == '"')
                {
                    bInQuato = true;
                    offset++;
                    continue;
                }
                if (bInQuato && CsvContent[offset] == '"')
                {
                    if (CsvContent[offset + 1] == '"')
                    {
                        word += '"';
                        offset += 2;
                        continue;
                    }
                    bInQuato = false;
                    offset++;
                    continue;
                }

                if (!bInQuato)
                {
                    if (CsvContent[offset] == ',')
                    {
                        splitList.Add(word);
                        offset++;
                        word = "";
                        continue;
                    }
                    if (CsvContent[offset] == '\r')
                    {
                        offset++;
                        continue;
                    }
                    if (CsvContent[offset] == '\n')
                    {
                        splitList.Add(word);
                        offset++;
                        return splitList;
                    }
                }
                word += CsvContent[offset];
                offset++;
            }
            return splitList;
        }

        public static int ReadInt32Variant(byte[] ioBuffer, int ioIndex, out int value)
        {
            int readPos = ioIndex;
            value = ioBuffer[readPos++];
            if ((value & 0x80) == 0) { value = Zag(value); return 1; }
            value &= 0x7F;

            int chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 2; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 3; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 4; }

            chunk = ioBuffer[readPos];
            value |= chunk << 28; // can only use 4 bits from this chunk
            value = Zag(value);
            return 5;
        }

        public static int ReadInt64Variant(byte[] ioBuffer, int ioIndex, out long value)
        {
            int readPos = ioIndex;
            value = ioBuffer[readPos++];
            if ((value & 0x80) == 0) { value = Zag(value); return 1; };
            value &= 0x7F;

            long chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 2; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 3; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 4; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 28;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 5; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 35;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 6; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 42;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 7; }


            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 49;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 8; }

            chunk = ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 56;
            if ((chunk & 0x80) == 0) { value = Zag(value); return 9; }

            chunk = ioBuffer[readPos];
            value |= chunk << 63; // can only use 1 bit from this chunk
            value = Zag(value);
            return 10;
        }


        private const long Int64Msb = ((long)1) << 63;
        private const int Int32Msb = ((int)1) << 31;
        private static int Zag(int ziggedValue)
        {
            int value = (int)ziggedValue;
            return (-(value & 0x01)) ^ ((value >> 1) & ~Int32Msb);
        }
        private static long Zag(long ziggedValue)
        {
            long value = (long)ziggedValue;
            return (-(value & 0x01L)) ^ ((value >> 1) & ~Int64Msb);
        }
    }
}


