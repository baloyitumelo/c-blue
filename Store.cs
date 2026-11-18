using System.Collections.Concurrent;

public class Store { 
    private static ConcurrentDictionary<string, string> _store = new();
    private static ConcurrentDictionary<string,
                                        ConcurrentDictionary<string, string>> _hashStore = new ();
    
    public static void Set(string key, string value){
        _store[key] = value;
    }

    public static string Get(string key){
        if(!_store.TryGetValue(key, out var value))
            return "";

        return value;
    }
    
    public static void Hset(string hash, string key, string value){
        var mapping = _hashStore.GetOrAdd(hash, _ => new ConcurrentDictionary<string, string>());
        mapping[key] = value;
    }

    public static string Hget(string hash, string key){
        if(!_hashStore.TryGetValue(hash, out var mapping))
            return "";

        if(!mapping.TryGetValue(key, out var value))
            return "";

        return value;
    }

}
