using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Lit.Unity;
using UnityEngine.UI;
using Lit.Protobuf;

public class Test : LitBehaviour {



    bool isStart = false;

    private void Update()
    {
        if (!isStart) return;
        var c = PoolMgr.Pools["fight"].Spawn("Cube");
        c.SetParent(null);
        WaitFor(0.1f, () => {
            PoolMgr.Pools["fight"].Despawn(c);
        });
    }
    [ContextMenu("testPool")]
    public void testPool()
    {
        isStart = !isStart;
    }



    private void Start()
    {
        SerializeEntity data = new SerializeEntity();
        data.Add("int1", "22.2")
            .Add("str", "str111")
            .Add("float", 1.244f)
            .Add("double", 2.55);
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




    [ContextMenu("TestTimer")]
    public void TestTimer()
    {
        curEvent = FaceMgr.timerMgr.RegistTimer(() =>
        {
            LitLogger.Log("Test Timer");
        }, 0, 10);
    }

    private TimerEvent curEvent = null;
    [ContextMenu("TestStopTimer")]
    public void TestStopTimer(){

        curEvent.Stop();
    }
}
