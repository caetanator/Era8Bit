using System.IO;
using System.Linq;
using CustomClasses.IO;

namespace EndianAwareBinaryReaderTesting
{
    public class EndianAwareBinaryReaderTest
    {
        /// <summary>
        ///A test for ReadDouble
        ///</summary>
        public void ReadDoubleTest()
        {
            byte[] data = new byte[] { 0xF2, 0x46, 0x35, 0x13, 0xFA, 0xAD, 0xFA, 0xDF };
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            double expected = expectedBr.ReadDouble();
            double actual;
            actual = target.ReadDouble();
           // Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadInt16
        ///</summary>
        public void ReadInt16Test()
        {
            byte[] data = new byte[] { 0xF2, 0x46 };
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            short expected = expectedBr.ReadInt16();
            short actual;
            actual = target.ReadInt16();
           // Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadInt32
        ///</summary>
        public void ReadInt32Test()
        {
            byte[] data = new byte[] { 0xF2, 0x46, 0x35, 0x13 };
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            int expected = expectedBr.ReadInt32();
            int actual;
            actual = target.ReadInt32();
           // Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ReadInt64
        ///</summary>
        public void ReadInt64Test()
        {
            byte[] data = new byte[] { 0xF2, 0x46, 0x35, 0x13, 0xFA, 0xAD, 0xFA, 0xDF };
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            long expected = expectedBr.ReadInt64();
            long actual;
            actual = target.ReadInt64();
           // Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadSingle
        ///</summary>
        public void ReadSingleTest()
        {
            byte[] data = new byte[] { 0xF2, 0x46, 0x35, 0x13,};
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            float expected = expectedBr.ReadSingle();
            float actual;
            actual = target.ReadSingle();
           // Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadUInt16
        ///</summary>
        public void ReadUInt16Test()
        {
            byte[] data = new byte[] { 0xF2, 0x46 };
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            ushort expected = expectedBr.ReadUInt16();
            ushort actual;
            actual = target.ReadUInt16();
            //Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadUInt32
        ///</summary>
        public void ReadUInt32Test()
        {
            byte[] data = new byte[] { 0xF2, 0x46, 0x35, 0x13};
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            uint expected = expectedBr.ReadUInt32();
            uint actual;
            actual = target.ReadUInt32();

           // Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ReadUInt64
        ///</summary>
        
        public void ReadUInt64Test()
        {
            byte[] data = new byte[] { 0xF2, 0x46, 0x35, 0x13, 0xFA, 0xAD, 0xFA, 0xDF };
            Stream input = new MemoryStream(data);
            bool isLittleEndian = false;
            EndianAwareBinaryReader target = new EndianAwareBinaryReader(input, isLittleEndian);
            Stream expectedStream = new MemoryStream(data.Reverse().ToArray());
            BinaryReader expectedBr = new BinaryReader(expectedStream);
            ulong expected = expectedBr.ReadUInt64();
            ulong actual;
            actual = target.ReadUInt64();
           // Assert.AreEqual(expected, actual);

        }
    }
}
