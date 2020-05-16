using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

class AdaptivePerformanceEditorTests
{

    [UnityTest]
    public IEnumerator DummyEditorTest()
    {
       yield return null;

       Assert.AreEqual(true, true);
    }

}
