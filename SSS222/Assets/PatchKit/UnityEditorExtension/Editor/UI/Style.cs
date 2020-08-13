using System;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public static class Style
{
    public static readonly Color
        GreenPastel = new Color(0.502f, 0.839f, 0.839f);

    public static readonly Color GreenOlive = new Color(0.502f, 0.839f, 0.031f);
    public static readonly Color RedPastel = new Color(0.839f, 0.502f, 0.502f);

    private class Disposable : IDisposable
    {
        private Action _action;

        public Disposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            if (_action == null)
            {
                return;
            }

            _action();
            _action = null;
        }
    }

    public static IDisposable Colorize(Color color)
    {
        Color previousColor = GUI.color;

        GUI.color = color;

        return new Disposable(() => GUI.color = previousColor);
    }

    public static IDisposable ColorizeBackground(Color color)
    {
        Color previousColor = GUI.backgroundColor;

        GUI.backgroundColor = color;

        return new Disposable(() => GUI.backgroundColor = previousColor);
    }

    public static IDisposable ChangeEnabled(bool isEnabled)
    {
        bool previousEnabled = GUI.enabled;

        GUI.enabled = isEnabled;

        return new Disposable(() => GUI.enabled = previousEnabled);
    }
}
}