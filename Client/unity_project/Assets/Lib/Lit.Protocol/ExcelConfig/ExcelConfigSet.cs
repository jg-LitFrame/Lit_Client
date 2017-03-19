using UnityEngine;
using System.Collections;
using ProtoBuf;
using System.Collections.Generic;
//using Giu.Unity5.Config;
using System;
using System.IO;
//using Giu.Logger;
using Lit.Unity;

namespace Giu.Protobuf {
    public class ExcelConfigSet {

        private string fileName;
        private string protocol;
        private DynamicFactory factory;
        private List<DynamicMessage> datas = new List<DynamicMessage>();

        public List<DynamicMessage> Datas { get { return datas; } }

        public struct Key {
            public object[] keys;

            public int Size { get { return keys == null ? 0 : keys.Length; } }
            public Key(params object[] collection) {
                keys = collection;
            }

            public Key(object data) {
                keys = new object[1] { data };
            } 

            public override int GetHashCode() {
                int ret = 0;
                for (int i = 0; i < keys.Length; ++i) {
                    ret ^= keys[i].GetHashCode();
                }

                return ret;
            }

            public bool Equals(Key obj) {
                if (Size != obj.Size) {
                    return false;
                }

                for (int i = 0; i < keys.Length; ++i) {
                    if (!keys[i].Equals(obj.keys[i])) {
                        return false;
                    }
                }

                return true;
            }

            public override bool Equals(object obj) {
                if (!(obj is Key)) {
                    return false;
                }

                return Equals((Key)obj);
            }
        }

        public delegate Key IndexFunction(DynamicMessage msg);
        public delegate bool FilterFunction(DynamicMessage msg);


        private class KVIndexData {
            public IndexFunction Handle;
            public Dictionary<Key, DynamicMessage> Index;
        }

        private class KLIndexData {
            public IndexFunction Handle;
            public Dictionary<Key, List<DynamicMessage>> Index;
            public IComparer<DynamicMessage> SortRule;
        }

        private List<FilterFunction> filters = new List<FilterFunction>();
        private List<KVIndexData> kvIndex = new List<KVIndexData>();
        private List<KLIndexData> klIndex = new List<KLIndexData>();

        public ExcelConfigSet(DynamicFactory fact, string file_name, string protocol_name) {
            factory = fact;
            fileName = file_name;
            protocol = protocol_name;
        }

        public string FileName {
            get { return fileName; }
        }

        public string Protocol {
            get { return protocol; }
        }

        public void Clear() {
            datas.Clear();

            foreach (var index in kvIndex) {
                index.Index.Clear();
            }

            foreach (var index in klIndex) {
                index.Index.Clear();
            }
        }

        public bool Reload() {
            Clear();

            string full_path = FileTools.GetDocFilePath(fileName);
            try {
                var header_desc = factory.GetMsgDiscriptor("com.owent.xresloader.pb.xresloader_datablocks");
                if (null == header_desc) {
                    LitLogger.ErrorFormat("load configure file {0} failed, com.owent.xresloader.pb.xresloader_datablocks not registered", fileName);
                    return false;
                }

                var msg_desc = factory.GetMsgDiscriptor(protocol);
                if (null == msg_desc) {
                    LitLogger.ErrorFormat("load configure file {0} failed, {1} not registered", fileName, protocol);
                    return false;
                }

                DynamicMessage data_set = factory.Decode(header_desc, File.OpenRead(full_path));
                if (null == data_set) {
                    LitLogger.ErrorFormat("load configure file {0} failed, {1}", fileName, factory.LastError);
                    return false;
                }

                foreach(var cfg_item in data_set.GetFieldList("data_block")) {
                    DynamicMessage data_item = factory.Decode(msg_desc, new MemoryStream((byte[])cfg_item));
                    if (null == data_item) {
                        LitLogger.ErrorFormat("load configure file {0} failed, {1}", fileName, factory.LastError);
                        continue;
                    }

                    bool filter_pass = true;
                    foreach (var fn in filters) {
                        filter_pass = fn(data_item);
                        if (!filter_pass) {
                            break;
                        }
                    }

                    if (!filter_pass) {
                        continue;
                    }

                    datas.Add(data_item);

                    foreach (var index in kvIndex) {
                        if (null != index.Handle) {
                            Key key = index.Handle(data_item);
                            LitLogger.LogFormat("@display_name  key : " + data_item.GetString("name"));
                            LitLogger.LogFormat("@type  key : " + data_item.GetUInt("id"));
                            LitLogger.LogFormat("@type_id  key : " + data_item.GetString("type_id"));
                            LitLogger.LogFormat("@type  key : " + data_item.GetString("type"));


                            index.Index[key] = data_item;
                        }
                    }

                    foreach (var index in klIndex) {
                        if (null != index.Handle) {
                            List<DynamicMessage> ls;
                            Key key = index.Handle(data_item);
                            if (index.Index.TryGetValue(key, out ls)) {
                                ls.Add(data_item);
                            } else {
                                index.Index[key] = new List<DynamicMessage> { data_item };
                            }
                        }
                    }
                }

                foreach (var index in klIndex) {
                    if (null != index.SortRule) {
                        foreach (var ls in index.Index) {
                            ls.Value.Sort(index.SortRule);
                        }
                    }
                }

            } catch (Exception e) {
                LitLogger.ErrorFormat("{0}", e.Message);
                return false;
            }
            LitLogger.Log("Data Count : " + datas.Count);
            return true;
        }

        public ExcelConfigSet AddKVIndex(int index, IndexFunction fn) {
            if (index < 0) {
                throw new ArgumentException("index must not be negetive");
            }

            if (null == fn) {
                throw new ArgumentNullException("IndexFunction");
            }

            while (kvIndex.Count <= index) {
                LitLogger.LogFormat("kvIndex Count : {0} , index : {1}", kvIndex.Count, index);
                KVIndexData obj = new KVIndexData();
                obj.Handle = null;
                obj.Index = new Dictionary<Key, DynamicMessage>();
                kvIndex.Add(obj);
            }

            KVIndexData index_set = kvIndex[index];
            index_set.Handle = fn;

            LitLogger.LogFormat("datas : {0}", datas.Count);
            foreach (var data_item in datas) {
                LitLogger.LogFormat("datas Count : {0} , data_item : {1}", datas.Count, data_item.GetString("display_name"));

                index_set.Index[index_set.Handle(data_item)] = data_item;
            }
            return this;
        }

        public ExcelConfigSet AddKVIndex(IndexFunction fn) { return AddKVIndex(0, fn); }
         
        public ExcelConfigSet AddKVIndexAuto(string key) { return AddKVIndex(0, item => new Key(item.GetFieldValue(key))); }

        public ExcelConfigSet AddKLIndex(int index, IndexFunction fn) {
            if (index < 0) {
                throw new ArgumentException("index must not be negetive");
            }

            if (null == fn) {
                throw new ArgumentNullException("IndexFunction");
            }

            while (klIndex.Count <= index) {
                KLIndexData obj = new KLIndexData();
                obj.Handle = null;
                obj.Index = new Dictionary<Key, List<DynamicMessage>>();
                obj.SortRule = null;
                klIndex.Add(obj);
            }

            KLIndexData index_set = klIndex[index];
            index_set.Handle = fn;

            foreach (var data_item in datas) {
                Key key = index_set.Handle(data_item);
                List<DynamicMessage> ls;
                if (index_set.Index.TryGetValue(key, out ls)) {
                    ls.Add(data_item);
                } else {
                    index_set.Index[key] = new List<DynamicMessage>() { data_item };
                }
            }

            return this;
        }

        public ExcelConfigSet SetKVSortRule(IComparer<DynamicMessage> fn) {
            return SetKVSortRule(0, fn);
        }

        public ExcelConfigSet SetKVSortRule(int index, IComparer<DynamicMessage> fn) {
            if (index < 0) {
                throw new ArgumentException("index must not be negetive");
            }

            if (index >= klIndex.Count) {
                throw new ArgumentException("index extended the ");
            }

            if (null == fn) {
                throw new ArgumentNullException("IndexFunction");
            }

            klIndex[index].SortRule = fn;
            foreach (var index_set in klIndex[index].Index) {
                index_set.Value.Sort(fn);
            }
            return this;
        }

        public int AddFilter(FilterFunction fn) {
            if (null == fn) {
                return -1;
            }

            filters.Add(fn);
            return filters.Count - 1;
        }

        public DynamicMessage GetKV(int type, Key key) {
            LitLogger.Log("kvIndex : " + kvIndex.Count);
            LitLogger.Log("type : " + type);
            LitLogger.Log("kvIndex[type].Index : " + kvIndex[type].Index.Keys.Count);
            foreach(var kv in kvIndex[type].Index){
                LitLogger.Log(kv.Key.keys[0]);
            }
            if (type < 0 || type >= kvIndex.Count) {
                return null;
            }

            DynamicMessage ret;
            if (kvIndex[type].Index.TryGetValue(key, out ret)) {
                return ret;
            }

            return null;
        }

        public DynamicMessage GetKV(Key key) {
            return GetKV(0, key);
        }

        public DynamicMessage GetKVAuto(object objKey) {
            return GetKV(0, new Key(objKey));
        }

        public List<DynamicMessage> GetKL(int type, Key key) {
            if (type < 0 || type >= klIndex.Count) {
                return null;
            }

            List<DynamicMessage> ret;
            if (klIndex[type].Index.TryGetValue(key, out ret)) {
                return ret;
            }

            return null;
        }

        public List<DynamicMessage> GetKL(Key key) {
            return GetKL(0, key);
        }
    }
}