public class CommandHandler{
    private static Store _store = new Store();    

    public static Value HandleCommand(List<Value> command){
        string cmd = command[0].bulk.ToUpper(); 

        int count = command.Count;
        List<Value> args = command.GetRange(1, count - 1);
       
       switch(cmd) {
            case "PING":
                return pong(args);
            case "SET":
                return set(args);
            case "GET":
                return get(args);
            case "HGET":
                return hget(args);
            case "HSET":
                return hset(args);
       }

       return new Value();
    }

    private static Value pong(List<Value> args) {
            Value value = new Value();
            value.type = "string";

            if(args.Count == 0){
                value.str = "PONG";
            }
            else {
                value.str = args[0].bulk;
            }

            return value;
    }

    private static Value set(List<Value> args){
        string key = args[0].bulk;
        string keyValue = args[1].bulk;

        /*
            Set key value pair in store here
        */

        Value value = new Value();
        value.type = "string";
        value.str = "OK";

        return value;
    }

    private static Value get(List<Value> args){
        string key = args[0].bulk;
        
        /*
            Get value from store here
        */

        string keyValue = "TEMPORARY";

        Value value = new Value();
        value.type = "bulk";
        value.bulk = keyValue;

        return value;
    }

    private static Value hget(List<Value> args){
        string hash = args[0].bulk;
        string key = args[1].bulk;
        
        /*
            Get value here
        */

        string keyValue = "TEMPORARY";
        Value value = new Value();
        value.type = "bulk";
        value.bulk = keyValue;

        return value;
    }

    private static Value hset(List<Value> args){
        string hash = args[0].bulk;
        string key = args[1].bulk;
        string keyValue = args[2].bulk;

        /*
             Set value here
        */

        Value value = new Value();
        value.type = "string";
        value.str = "OK";

        return value;
    }

}
