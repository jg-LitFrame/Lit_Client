using System.Collections.Generic;
using google.protobuf;

namespace Giu.Protobuf {
    public partial class DynamicMessage {

        public DynamicMessage Set(string name, object value) {
            SetFieldValue(name, value);
            return this;
        }

        public DynamicMessage GetChild(string field_name) { return GetFieldValue(field_name) as DynamicMessage; } 
        public int GetInt(string field_name) { return (int)GetFieldValue(field_name); }
        public uint GetUInt(string field_name) { return (uint)GetFieldValue(field_name); }
        public long GetLong(string field_name) { return (long)GetFieldValue(field_name); }
        public ulong GetULong(string field_name) { return (ulong)GetFieldValue(field_name); }
        public bool GetBool(string field_name) { return (bool)GetFieldValue(field_name); }
        public float GetFloat(string field_name) { return (float)GetFieldValue(field_name); }
        public double GetDouble(string field_name) { return (double)GetFieldValue(field_name); }
        public string GetString(string field_name) { return GetFieldValue(field_name) as string; }
         
        public string ToTableCode(string argName = "DynamicMessage") {
           // return _ToTableCode(StrGen.New, this, argName).End;
            return "";
        } 

        //public static StrGen.Builder _ToTableCode(StrGen.Builder _, DynamicMessage msg, string name) {
        //    List<FieldDescriptorProto> used_fields = msg.ReflectListFields;
        //    _ = _[' '][name][name.Exist() ? "= " : ""]["{"];
        //    for (int i = 0; i < used_fields.Count; i++) {
        //        FieldDescriptorProto field = used_fields[i];
        //        if (field.label == FieldDescriptorProto.Label.LABEL_REPEATED) {
        //            List<object> field_contents = msg.GetFieldList(field);
        //            _ = _[field.name][" = "]["{ "];
        //            for (int j = 0; j < field_contents.Count; j++) {
        //                object field_content = field_contents[j];

        //                bool isString = field.type == FieldDescriptorProto.Type.TYPE_STRING;
        //                if (field.type == FieldDescriptorProto.Type.TYPE_MESSAGE) {
        //                    DynamicMessage dm = field_content as DynamicMessage;
        //                    _ToTableCode(_, dm, "");
        //                } else {
        //                    _ = _[isString ? "\"" : ""][field_content][isString ? "\"" : ""];
        //                }
        //                _ = _[j != field_contents.Count - 1 ? ", " : ""];
        //            }
        //            _ = _["} "];
        //        } else {
        //            object field_content = msg.GetFieldValue(field);
        //            bool isString = field.type == FieldDescriptorProto.Type.TYPE_STRING || field.type == FieldDescriptorProto.Type.TYPE_INT64;
        //            if (field.type == FieldDescriptorProto.Type.TYPE_MESSAGE) {
        //                DynamicMessage dm = field_content as DynamicMessage;
        //                _ToTableCode(_, dm, field.name);
        //            } else {
        //                _ = _[field.name][" = "][isString ? "\"" : ""][field_content][isString ? "\"" : ""];
        //            }
        //        }
        //        _ = _[i != used_fields.Count - 1 ? ", " : ""];
        //    }
        //    return _["} "];
        //}

    }
}
