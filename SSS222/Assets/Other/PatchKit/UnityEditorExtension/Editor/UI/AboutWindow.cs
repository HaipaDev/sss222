using UnityEditor;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.UI
{
public class AboutWindow : Window
{
    [MenuItem("Tools/PatchKit/About", false, -50)]
    public static void ShowWindow()
    {
        var window = (AboutWindow) GetWindow(
            typeof(AboutWindow),
            false,
            "About");

        Assert.IsNotNull(window);

        window.Push<AboutScreen>().Initialize();
    }
}
}