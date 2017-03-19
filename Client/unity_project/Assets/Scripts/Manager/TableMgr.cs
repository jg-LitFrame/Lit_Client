using UnityEngine;
using System.Collections;
using Lit.Unity;
using Giu.Protobuf;
public class TableMgr : Singleton<TableMgr>
{

    public override void Init()
    {
        ExcelConfigManager.StartupInit();
        AddConfig("hero_skill_jg", "type");
        LitLogger.Log("hero_skill");

        ExcelConfigManager.ReloadAll();
    }

    public static void  AddConfig(string config_name, string row_id_name)
    {
        if (ExcelConfigManager.Get(config_name) == null)
            ExcelConfigManager.AddConfig(config_name).AddKVIndexAuto(row_id_name);
    }

    public static DynamicMessage GetTableRow(string table_name, object row_id)
    {
        var tb = ExcelConfigManager.Get(table_name);
        LitLogger.Log(tb);
        return tb.GetKVAuto(row_id);
    }
}
