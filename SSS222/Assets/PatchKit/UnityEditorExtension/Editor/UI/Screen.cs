using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.UI
{
public abstract class Screen : ScriptableObject
{
    [SerializeField]
    private Window _window;

    [NotNull]
    public static T CreateInstance<T>([NotNull] Window window)
        where T : Screen
    {
        if (window == null)
        {
            throw new ArgumentNullException("window");
        }

        var view = ScriptableObject.CreateInstance<T>();
        Assert.IsNotNull(view);
        view._window = window;

        return view;
    }

    public abstract void UpdateIfActive();

    public abstract void Draw();

    [NotNull]
    protected T Push<T>()
        where T : Screen
    {
        Assert.IsNotNull(_window);
        Assert.AreEqual(this, _window.CurrentScreen);

        return _window.Push<T>();
    }

    protected void Close()
    {
        Assert.IsNotNull(_window);

        _window.Close();
    }

    protected void Pop(object result)
    {
        Assert.IsNotNull(_window);
        Assert.AreEqual(this, _window.CurrentScreen);

        _window.Pop(this, result);
    }

    protected void Dispatch([NotNull] Action action)
    {
        Assert.IsNotNull(_window);

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        EditorApplication.delayCall += () =>
        {
            action();
            _window.Repaint();
        };
    }

    public abstract string Title { get; }

    public abstract Vector2? Size { get; }

    public abstract void OnActivatedFromTop(object result);
}
}