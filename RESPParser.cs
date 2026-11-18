using System;
using System.IO;
using System.Text;

public struct Value {
    public string type;
    public string str;
    public string bulk;
    public int num;
    public List<Value> array;

    public static Value StringValue(string text){
        return new Value { type = "string", str = text };
    }

    public static Value BulkValue(string strByteSeq){
        return new Value { type = "bulk", bulk = strByteSeq };
    }

    public static Value ArrayValue() {
        return new Value { type = "array", array = new () };
    }

    public static Value NullValue(){
        return new Value { type = "null" };
    }
}


public readonly struct TypeIdent {
    public const byte STRING = (byte)'+';
    public const byte ERROR = (byte)'-';
    public const byte INTEGER = (byte)':';
    public const byte BULK = (byte)'$';
    public const byte ARRAY = (byte)'*';
}

public class RESPParser {
    BufferedStream _buff;

    public RESPParser(byte[] input, int byteCount){
        _buff = new BufferedStream(new MemoryStream(input, 0, byteCount));
    }

    List<byte> ReadLine() {
        List<byte> line = new(); 
        bool reading = true;

        while(reading){
            int b = _buff.ReadByte();
            if(b == -1)
                throw new EndOfStreamException("Eish");

            line.Add((byte)b);

            if(line.Count >= 2 && line[line.Count - 2] == '\r'){
                reading = false;
                continue;
            }
        }
        
        line.RemoveRange(line.Count - 2, 2);

        return line;
    }

    int ReadInteger() {
        List<byte> line = ReadLine();
        string str = Encoding.ASCII.GetString(line.ToArray());
        return int.Parse(str);
    }

    public Value Read()  {
        byte typeIdent = (byte)_buff.ReadByte();

        switch(typeIdent) {
            case TypeIdent.ARRAY: 
                return ReadArray();
            case TypeIdent.BULK:
                return ReadBulk();
            default:
                return ReadBulk();
        }

    }

    Value ReadArray(){
        Value value = Value.ArrayValue();

        int len = ReadInteger();
        value.array = new();

        for(int i = 0; i < len; i++){
            Value item = Read();
            value.array.Add(item);
        }

        return value;
    }

    Value ReadBulk(){
        int len = ReadInteger();
        byte[] bytes = new byte[len];
        _buff.ReadExactly(bytes, 0, len);
        string contents = Encoding.UTF8.GetString(bytes);

        ReadLine();

        return Value.BulkValue(contents);
    }
}
