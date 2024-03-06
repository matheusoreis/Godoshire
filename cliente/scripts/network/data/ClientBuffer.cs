using System;
using System.Text;

namespace Network.Buffer;

public class ClientBuffer
{
    private byte[] _buffer;
    private int _bufferSize;
    private int _writeHead;
    private int _readHead;

    public ClientBuffer()
    {
        _buffer = new byte[1024];
        Flush();
    }

    private void Allocate(int additionalSize)
    {
        _bufferSize += additionalSize;
        Array.Resize(ref _buffer, _bufferSize);
    }

    public void Flush()
    {
        _writeHead = 0;
        _readHead = 0;
        _bufferSize = 0;
        Array.Clear(_buffer, 0, _buffer.Length);
    }

    public void Trim()
    {
        if (_readHead >= _bufferSize)
        {
            Flush();
        }
    }

    public int Count
    {
        get { return _bufferSize; }
    }

    public int Length
    {
        get { return _bufferSize - _readHead; }
    }

    public string GetString()
    {
        return Encoding.UTF8.GetString(_buffer, _readHead, _bufferSize - _readHead);
    }

    public void WriteByte(byte value)
    {
        if (_writeHead >= _bufferSize)
        {
            Allocate(1);
        }

        _buffer[_writeHead++] = value;
    }

    public void WriteBytes(byte[] values)
    {
        if (_writeHead + values.Length > _bufferSize)
        {
            Allocate(values.Length);
        }

        Array.Copy(values, 0, _buffer, _writeHead, values.Length);
        _writeHead += values.Length;
    }

    public void WriteInteger(int value)
    {
        byte[] intBytes = BitConverter.GetBytes(value);
        WriteBytes(intBytes);
    }

    public void WriteString(string value)
    {
        byte[] stringBytes = Encoding.UTF8.GetBytes(value);
        int stringLength = stringBytes.Length;

        WriteInteger(stringLength);

        if (stringLength <= 0)
        {
            return;
        }

        if (_writeHead + stringLength > _bufferSize)
        {
            Allocate(stringLength);
        }

        Array.Copy(stringBytes, 0, _buffer, _writeHead, stringLength);
        _writeHead += stringLength;
    }

    public byte ReadByte()
    {
        return _buffer[_readHead++];
    }

    public byte[] ReadBytes(int length, bool moveReadHead = true)
    {
        byte[] result = new byte[length];
        Array.Copy(_buffer, _readHead, result, 0, length);

        if (moveReadHead)
        {
            _readHead += length;
        }

        return result;
    }

    public int ReadInteger()
    {
        byte[] buffer = ReadBytes(4);
        return BitConverter.ToInt32(buffer, 0);
    }

    public string ReadString(bool moveReadHead = true)
    {
        int stringLength = ReadInteger();

        if (stringLength <= 0)
        {
            return "";
        }

        if (_bufferSize < _readHead + stringLength)
        {
            throw new Exception("Not enough bytes in buffer");
        }

        string result = Encoding.UTF8.GetString(_buffer, _readHead, stringLength);
        if (moveReadHead) _readHead += stringLength;

        return result;
    }
}
