
//using Giu.Unity5;
//using Giu.Unity5.Config;
using ProtoBuf;
using System.Collections.Generic;
using Lit.Unity;
//using Giu.Logger;
//using Giu.Basic;

namespace Giu.Protobuf {
    public static class ExcelConfigManager {
        const string CLASS_NAME = "ExcelConfigManager";
       // public static Log.er _Log = Log.GetLogger(CLASS_NAME);
        static private DynamicFactory factory = new DynamicFactory();
        static private Dictionary<string, ExcelConfigSet> allConfigures = new Dictionary<string, ExcelConfigSet>();

        public static DynamicFactory Factory {
            get { return factory; }
        }

        // Use this for initialization

       // [GlobalEvent(GlobalEventAttribute.Event.OnCachePrepared)]
        static public void StartupInit() {
          //  if (InitRecorder.IsInitialed(CLASS_NAME)) return;

            factory.Register(FileTools.GetDocumentPath("Protocol/pb_header_v3.pb"));
            factory.Register(FileTools.GetDocumentPath("Protocol/config.all.pb"));

        //    InitRecorder.MarkInitialed(CLASS_NAME);
        }

        static public ExcelConfigSet AddConfig(string config_name, string protocol_name = "", string file_name = "") {
            if (protocol_name.Length == 0) {
                protocol_name = string.Format("hello.config.{0}", config_name);
            }
            else
            {
                protocol_name = string.Format("hello.config.{0}", protocol_name);
            }

            if (file_name.Length == 0) {
                file_name = string.Format("Table/{0}.bin", config_name);
            }

            if (null != Get(config_name)) {
                LitLogger.ErrorFormat("configure name {0} already registered, can not register again", config_name);
                return new ExcelConfigSet(factory, file_name, protocol_name);
            }

            ExcelConfigSet ret = new ExcelConfigSet(factory, file_name, protocol_name);
            allConfigures[config_name] = ret;
            return ret;
        }

        static public void ReloadAll() {
            foreach (var cfg in allConfigures) {
                cfg.Value.Reload();
            }
        }

        static public ExcelConfigSet Get(string name) {
            ExcelConfigSet ret;
            if (allConfigures.TryGetValue(name, out ret)) {
                return ret;
            }

            return null;
        }

        static private void RegisterAllConfigure() {
            //Check TableConfigure
             
            ////AddConfig("const_parameter_cfg")
            ////    .AddKVIndex((DynamicMessage item) =>{
            ////    return new ExcelConfigSet.Key(item.GetFieldValue("id"));
            ////    }).AddFilter((DynamicMessage item) => {
            ////    return (uint)item.GetFieldValue("id") > 0;
            ////});

            //AddConfig("const_parameter_cfg").AddKVIndexAuto("id").AddFilter(item => (uint)item.GetFieldValue("id") > 0);
            ////DynamicMessage msg = Get("errorcode_cfg").GetKVAuto(0);
            ////msg.GetFieldValue("level");


            //AddConfig("errorcode_cfg").AddKVIndex((DynamicMessage item) => {
                 
            //    return new ExcelConfigSet.Key(item.GetFieldValue("id"));
            //});

            //// ============= teste code here =============
            //ReloadAll();
            ////ExcelConfigSet errcode_set = Get("errorcode_cfg");
            ////_Log.Debug("error code [{0}] = {1}", 0, errcode_set.GetKV(new ExcelConfigSet.Key(0)).GetFieldValue("text"));
            ////_Log.Debug("error code [{0}] = {1}", 1, errcode_set.GetKV(new ExcelConfigSet.Key(1)).GetFieldValue("text"));

            ////Get("errorcode_cfg").GetKV(new ExcelConfigSet.Key(0));

        }
    }

}