using System.Collections;
using System.Collections.Generic;
using EGO.Util;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

//test�ű���Testing�´���
//window-general-test runner
//���ڴ򿪺���ж�Ӧ�Ľű�xxx+SimplePasses��˫�����ִ�д���
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
