using System.Collections;
using System.Collections.Generic;
using EGO.Util;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

//test脚本由Testing下创建
//window-general-test runner
//窗口打开后会有对应的脚本xxx+SimplePasses，双击后会执行代码
public class PlayGround
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayGroundSimplePasses()
    {
        var p = new Propety<int>(10);
        Debug.Log(JsonUtility.ToJson(p));
        Assert.IsTrue(true);
    }
}
