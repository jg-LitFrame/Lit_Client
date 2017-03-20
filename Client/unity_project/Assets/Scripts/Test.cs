using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Lit.Unity;
using UnityEngine.UI;
using Lit.Protobuf;

public class Test : MonoBehaviour {


    private void Start()
    {
        SerializeEntity data = new SerializeEntity();
        data.Add("int1", "22.2")
            .Add("str", "str111")
            .Add("float", 1.244f)
            .Add("double", 2.55);
    }

    [ContextMenu("TestRotation")]
    public void TestRotation()
    {
        //Debug.Log(transform.localRotation.eulerAngles);
        Debug.Log(transform.localEulerAngles);

        transform.localEulerAngles = new Vector3(29, 75, 44.6f);
        //Vector3 v = new Vector3(345, 345, 304.9997f);

        // Debug.Log(Quaternion.Euler(v));
    }

    [ContextMenu("testBool")]
    public void testBool()
    {
        
        LitLogger.Log(System.Convert.ToBoolean("false"));
    }

    [ContextMenu("testText")]
    public void testText()
    {
        Text t = GetComponent<Text>();
        LitLogger.Log(t.alignment.ToString());
    }


    [ContextMenu("TestConfig")]
    public void TestConfig()
    {
        uint key = 40;
        var row = TableMgr.GetTableRow("hero_skill_jg", key);
        LitLogger.Log(row.GetString("name"));

        var items = row.GetFieldList("items");
        LitLogger.Log("items Count : " + items.Count);
        foreach (var i in items){
            LitLogger.Log(i);
        }


        var datas = row.GetFieldList("datas");
        LitLogger.Log("datas Count : " + datas.Count);
        foreach (var d in datas)
        {
            var Md = d as DynamicMessage;
            LitLogger.Log(Md.GetInt("price"));
        }
    }
}
