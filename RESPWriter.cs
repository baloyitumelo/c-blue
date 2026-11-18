using System;
using System.Text;

public class RESPWriter{
   
    private static readonly byte[] CRLF = [   (byte)'\r',
                                              (byte)'\n'
                                          ];
    private Value _value; 

    public RESPWriter(Value value){
        _value = value;    
    }
    
    private byte[] IntToBytes(int num){
        return StringToBytes(num.ToString());
    }

    private byte[] StringToBytes(string str){
        return Encoding.UTF8.GetBytes(str);
    }

    private byte[] WriteString(){
        List<byte> bytes = new();

        bytes.Add(TypeIdent.STRING);

        byte[] str = StringToBytes(_value.str); 
        bytes.AddRange(str);

        bytes.AddRange(CRLF);

        return bytes.ToArray();
    }

    private byte[] WriteBulk(){
        List<byte> bytes = new();

        bytes.Add(TypeIdent.BULK);

        byte[] count = IntToBytes(_value.bulk.Length);
        bytes.AddRange(count);
    
        bytes.AddRange(CRLF);

        byte[] bulk = StringToBytes(_value.bulk);
        bytes.AddRange(bulk);

        bytes.AddRange(CRLF);

        return bytes.ToArray();
    }

    private byte[] WriteArray(){
        List<byte> bytes = new();
        bytes.Add(TypeIdent.ARRAY);
        
        int len = _value.array.Count;

        byte[] count = IntToBytes(len);
        bytes.AddRange(count);

        bytes.AddRange(CRLF);

        for(int i = 0; i < len; i++){
            RESPWriter element = new (_value.array[i]);  
            bytes.AddRange(element.Write());
        }

        return bytes.ToArray();
    }

    private byte[] WriteError(){
        List<byte> bytes = new();

        bytes.Add(TypeIdent.ERROR);

        byte[] errorMessage = StringToBytes(_value.str);
        bytes.AddRange(errorMessage);

        bytes.AddRange(CRLF);

        return bytes.ToArray();
    }

    private byte[] WriteNull(){
        string str = "$-1\r\n"; 
        return StringToBytes(str);
    }

    public byte[] Write(){
       switch(_value.type){
            case "array":
                return WriteArray();
            case "bulk":
                return WriteBulk();
            case "error":
                return WriteError();
            case "string":
                return WriteString();
            default:
                return WriteNull();
       }
    }
}

