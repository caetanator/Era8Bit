public class MyBinaryWriter : BinaryWriter
{
    private bool isLittleEndian = true;
    private byte[] buffer = new byte[8];

    public MyBinaryWriter(Stream output, Encoding encoding, bool isLittleEndian)
        : base(output, encoding)
    {
        this.isLittleEndian = isLittleEndian;
    }

    public MyBinaryWriter(Stream output, bool isLittleEndian)
        : this(output, Encoding.UTF8, isLittleEndian)
    {
    }

    public bool IsLittleEndian
    {
        get { return isLittleEndian; }
        set { isLittleEndian = value; }
    }

    public override unsafe void Write(double value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }

        ulong num = *((ulong*)&value);
        this.buffer[7] = (byte)num;
        this.buffer[6] = (byte)(num >> 8);
        this.buffer[5] = (byte)(num >> 0x10);
        this.buffer[4] = (byte)(num >> 0x18);
        this.buffer[3] = (byte)(num >> 0x20);
        this.buffer[2] = (byte)(num >> 40);
        this.buffer[1] = (byte)(num >> 0x30);
        this.buffer[0] = (byte)(num >> 0x38);
        this.OutStream.Write(buffer, 0, 8);
    }

    public override void Write(short value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        this.buffer[1] = (byte)value;
        this.buffer[0] = (byte)(value >> 8);
        this.OutStream.Write(buffer, 0, 2);
    }

    public override void Write(int value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        this.buffer[3] = (byte)value;
        this.buffer[2] = (byte)(value >> 8);
        this.buffer[1] = (byte)(value >> 0x10);
        this.buffer[0] = (byte)(value >> 0x18);
        this.OutStream.Write(buffer, 0, 4);
    }

    public override void Write(long value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        this.buffer[7] = (byte)value;
        this.buffer[6] = (byte)(value >> 8);
        this.buffer[5] = (byte)(value >> 0x10);
        this.buffer[4] = (byte)(value >> 0x18);
        this.buffer[3] = (byte)(value >> 0x20);
        this.buffer[2] = (byte)(value >> 40);
        this.buffer[1] = (byte)(value >> 0x30);
        this.buffer[0] = (byte)(value >> 0x38);
        this.OutStream.Write(buffer, 0, 8);
    }

    public override unsafe void Write(float value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        uint num = *((uint*)&value);
        this.buffer[3] = (byte)num;
        this.buffer[2] = (byte)(num >> 8);
        this.buffer[1] = (byte)(num >> 0x10);
        this.buffer[0] = (byte)(num >> 0x18);
        this.OutStream.Write(buffer, 0, 4);
    }

    [CLSCompliant(false)]
    public override void Write(ushort value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        this.buffer[1] = (byte)value;
        this.buffer[0] = (byte)(value >> 8);
        this.OutStream.Write(buffer, 0, 2);
    }

    [CLSCompliant(false)]
    public override void Write(uint value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        this.buffer[3] = (byte)value;
        this.buffer[2] = (byte)(value >> 8);
        this.buffer[1] = (byte)(value >> 0x10);
        this.buffer[0] = (byte)(value >> 0x18);
        this.OutStream.Write(buffer, 0, 4);
    }

    [CLSCompliant(false)]
    public override void Write(ulong value)
    {
        if (isLittleEndian)
        {
            base.Write(value);
            return;
        }
        this.buffer[7] = (byte)value;
        this.buffer[6] = (byte)(value >> 8);
        this.buffer[5] = (byte)(value >> 0x10);
        this.buffer[4] = (byte)(value >> 0x18);
        this.buffer[3] = (byte)(value >> 0x20);
        this.buffer[2] = (byte)(value >> 40);
        this.buffer[1] = (byte)(value >> 0x30);
        this.buffer[0] = (byte)(value >> 0x38);
        this.OutStream.Write(buffer, 0, 8);
    }
}




public class MyBinaryReader : BinaryReader
{
    private bool isLittleEndian;
    private byte[] buffer = new byte[8];

    public MyBinaryReader(Stream input, Encoding encoding, bool isLittleEndian)
        : base(input, encoding)
    {
        this.isLittleEndian = isLittleEndian;
    }

    public MyBinaryReader(Stream input, bool isLittleEndian)
        : this(input, Encoding.UTF8, isLittleEndian)
    {
    }

    public bool IsLittleEndian
    {
        get { return isLittleEndian; }
        set { isLittleEndian = value; }
    }

    public override unsafe double ReadDouble()
    {
        if (isLittleEndian)
            return base.ReadDouble();
        FillMyBuffer(8);
        ulong num = (ulong)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
        ulong num2 = (ulong)(((buffer[7] | (buffer[6] << 8)) | (buffer[5] << 0x10)) | (buffer[4] << 0x18));
        ulong num3 = (num2 << 0x20) | num;
        return *(((double*)&num3));
    }

    public override short ReadInt16()
    {
        if (isLittleEndian)
            return base.ReadInt16();
        FillMyBuffer(2);
        return (short)(buffer[1] | (buffer[0] << 8));
    }

    public override int ReadInt32()
    {
        if (isLittleEndian)
            return base.ReadInt32();
        FillMyBuffer(4);
        return (((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
    }

    public override long ReadInt64()
    {
        if (isLittleEndian)
            return base.ReadInt64();
        FillMyBuffer(8);
        ulong num = (ulong)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
        ulong num2 = (ulong)(((buffer[7] | (buffer[6] << 8)) | (buffer[5] << 0x10)) | (buffer[4] << 0x18));
        return (long)((num2 << 0x20) | num);
    }

    public override unsafe float ReadSingle()
    {
        if (isLittleEndian)
            return base.ReadSingle();
        FillMyBuffer(4);
        uint num = (uint)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
        return *(((float*)&num));
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
        if (isLittleEndian)
            return base.ReadUInt16();
        FillMyBuffer(2);
        return (ushort)(buffer[1] | (buffer[0] << 8));
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
        if (isLittleEndian)
            return base.ReadUInt32();
        FillMyBuffer(4);
        return (uint)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
        if (isLittleEndian)
            return base.ReadUInt64();
        FillMyBuffer(8);
        ulong num = (ulong)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
        ulong num2 = (ulong)(((buffer[7] | (buffer[6] << 8)) | (buffer[5] << 0x10)) | (buffer[4] << 0x18));
        return ((num2 << 0x20) | num);
    }

    private void FillMyBuffer(int numBytes)
    {
        int offset = 0;
        int num2 = 0;
        if (numBytes == 1)
        {
            num2 = BaseStream.ReadByte();
            if (num2 == -1)
            {
                throw new EndOfStreamException("Attempted to read past the end of the stream.");
            }
            buffer[0] = (byte)num2;
        }
        else
        {
            do
            {
                num2 = BaseStream.Read(buffer, offset, numBytes - offset);
                if (num2 == 0)
                {
                    throw new EndOfStreamException("Attempted to read past the end of the stream.");
                }
                offset += num2;
            }
            while (offset < numBytes);
        }
    }
}
