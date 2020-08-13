using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.Core
{
public static class PluginAssert
{
    [ContractAnnotation("value:null => halt")]
    public static void IsNotNull(object value, string message = null)
    {
        if (value == null)
        {
            Fail("Value was null", message);
        }
    }

    [ContractAnnotation("value:notNull => halt")]
    public static void IsNull(object value, string message = null)
    {
        if (value != null)
        {
            Fail("Value was not null", message);
        }
    }

    [ContractAnnotation("value:false => halt")]
    public static void IsTrue(bool value, string message = null)
    {
        if (!value)
        {
            Fail("Value was false", message);
        }
    }

    [ContractAnnotation("value:true => halt")]
    public static void IsFalse(bool value, string message = null)
    {
        if (value)
        {
            Fail("Value was true", message);
        }
    }

    public static void AreEqual<T>(T expected, T actual, string message = null)
    {
        AreEqual(expected, actual, message, EqualityComparer<T>.Default);
    }

    public static void AreEqual<T>(
        T expected,
        T actual,
        string message,
        [NotNull] IEqualityComparer<T> equalityComparer)
    {
        Assert.IsNotNull(equalityComparer);

        if (!equalityComparer.Equals(expected, actual))
        {
            Fail(
                string.Format(
                    "Values are not equal (expected: {0}, actual: {1})",
                    expected,
                    actual),
                message);
        }
    }

    public static void AreNotEqual<T>(
        T expected,
        T actual,
        string message = null)
    {
        AreNotEqual(expected, actual, message, EqualityComparer<T>.Default);
    }

    public static void AreNotEqual<T>(
        T expected,
        T actual,
        string message,
        [NotNull] IEqualityComparer<T> equalityComparer)
    {
        Assert.IsNotNull(equalityComparer);

        if (equalityComparer.Equals(expected, actual))
        {
            Fail(
                string.Format("Values are equal (value: {0})", expected),
                message);
        }
    }

    private static void Fail(string message, string userMessage)
    {
        if (userMessage != null)
        {
            message = userMessage + "\n" + message;
        }

        try
        {
            throw new PluginAssertionException(message);
        }
        catch (Exception e)
        {
            string dialogMessage =
                "PatchKit Unity Editor Extensions has encountered an fatal error.\n\n" +
                message +
                "\n\n" +
                e.StackTrace +
                "\n\n" +
                "Please try to reimport the plugin.\n" +
                "If issue would remain, please contact us at contact@patchkit.net";

            EditorUtility.DisplayDialog("Error!", dialogMessage, "OK");
        }
    }
}
}