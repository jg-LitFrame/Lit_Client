using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Lit.Unity;
public class Test : MonoBehaviour {


    private void Start()
    {
        SerializeEntity data = new SerializeEntity();
        data.Add("int1", "22.2")
            .Add("str", "str111")
            .Add("float", 1.244f)
            .Add("double", 2.55);
        LitLogger.Log(data);

        float intV = data["float"];

        LitLogger.Log(intV);
        LitLogger.Log(data.Get("int1").GetJsonType());
    }

    [ContextMenu("TestRotation")]
    public void TestRotation()
    {
        //Debug.Log(transform.localRotation.eulerAngles);
        //Debug.Log(transform.localRotation);


        //Vector3 v = new Vector3(345, 345, 304.9997f);

        // Debug.Log(Quaternion.Euler(v));
    }
}
