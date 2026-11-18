public class CommandHandler{
    public static Value HandleCommand(List<Value> command){
        string cmd = command[0].bulk.ToUpper(); 

        int count = command.Count;
        List<Value> args = command.GetRange(1, count - 1);
       
       switch(cmd) {
            case "PING":
                return Pong(args);
            case "SET":
                return Set(args);
            case "GET":
                return Get(args);
            case "HGET":
                return Hget(args);
            case "HSET":
                return Set(args);
       }

       return new Value();
    }

    private static Value Pong(List<Value> args) {
            string response;

            if(args.Count == 0)
                response = "PONG";      
            else 
                response = args[0].bulk;

            return Value.StringValue(response);
    }

    private static Value Set(List<Value> args){
        string key = args[0].bulk;
        string keyValue = args[1].bulk;

        Store.Set(key, keyValue);

        string response = "OK";
        return Value.StringValue(response);
    }

    private static Value Get(List<Value> args){
        string key = args[0].bulk;

        string keyValue = Store.Get(key);
        
        string response = keyValue;
        return Value.BulkValue(response);
    }

    private static Value Hget(List<Value> args){
        string hash = args[0].bulk;
        string key = args[1].bulk;
        
        string keyValue = Store.Hget(hash, key);
        
        string response = keyValue;
        return Value.BulkValue(keyValue);
    }

    private static Value Hset(List<Value> args){
        string hash = args[0].bulk;
        string key = args[1].bulk;
        string keyValue = args[2].bulk;
        
        Store.Hset(hash, key, keyValue);

        string response = "OK";
        return Value.StringValue(response);
    }

}
