using ProtoBuf;
using System.Collections.Generic;
using Lit.Unity;

namespace Lit.Protobuf {
    public static class ExcelConfigManager {
        const string CLASS_NAME = "ExcelConfigManager";
        static private DynamicFactory factory = new DynamicFactory();
        static private Dictionary<string, ExcelConfigSet> allConfigures = new Dictionary<string, ExcelConfigSet>();

        public static DynamicFactory Factory {
            get { return factory; }
        }

       // [GlobalEvent(GlobalEventAttribute.Event.OnCachePrepared)]
        static public void StartupInit() {
          //  if (InitRecorder.IsInitialed(CLASS_NAME)) return;

            factory.Register(FileTools.GetDocFilePath("Protocol/pb_header_v3.pb"));
            factory.Register(FileTools.GetDocFilePath("Protocol/config.all.pb"));

        //    InitRecorder.MarkInitialed(CLASS_NAME);
        }

        static public ExcelConfigSet AddConfig(string config_name, string protocol_name = "", string file_name = "") {
            if (protocol_name.Length == 0) {
                protocol_name = string.Format("lit.config.{0}", config_name);
            }
            else
            {
                protocol_name = string.Format("lit.config.{0}", protocol_name);
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
            foreach (var info in factory.LastError)
            {
                LitLogger.Error(info);
            }
        }

        static public ExcelConfigSet Get(string name) {
            ExcelConfigSet ret;
            if (allConfigures.TryGetValue(name, out ret)) {
                return ret;
            }
            return null;
        }
    }

}