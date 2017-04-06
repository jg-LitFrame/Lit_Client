using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Database{


    private Dictionary<string, object> data = new Dictionary<string, object>();

    private static Database _inst;
    public static Database Inst
    {
        get {
            if (_inst == null)
                _inst = new Database();
            return _inst;
        }
    }



    public T GetData<T> (string dataName) where T: class {

        object o = null;
        data.TryGetValue(dataName, out o);
        if(o == null)
        {
            Debug.LogError("Database: Data for " + dataName + " does not exist!");
            return default(T);
        }
        return (T)o;
	}

	
	public void SetData (string dataName,object o) {

        if (string.IsNullOrEmpty(dataName) || data == null)
        {
            Debug.LogErrorFormat("The data is invalid : key = {0} , Value = {1}", dataName,o);
            return;
        }
        if (ContiansData(dataName))
        {
            data[dataName] = 0;
        }else
        {
            data.Add(dataName, o);
        }
	}

    public object RemoveData(string dataName)
    {
        if (ContiansData(dataName))
        {
            return data[dataName];
        }
        return null;
    }

    public bool ContiansData(string dataName)
    {
        if(!string.IsNullOrEmpty(dataName) && data.ContainsKey(dataName))
        {
            return true;
        }
        return false;
    }

}



