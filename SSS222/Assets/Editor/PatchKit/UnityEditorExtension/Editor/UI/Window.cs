using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace PatchKit.UnityEditorExtension.UI
{
public class Window : EditorWindow
{
    [SerializeField]
    [NotNull]
    private List<Screen> _screens = new List<Screen>();

    [NotNull]
    private IEnumerable<Screen> Screens
    {
        get { return _screens.Where(x => x != null); }
    }

    public Screen CurrentScreen
    {
        get { return Screens.LastOrDefault(); }
    }

    private Vector2? GetCurrentSize()
    {
        foreach (Screen screen in Screens.Reverse())
        {
            if (screen.Size.HasValue)
            {
                return screen.Size.Value;
            }
        }

        return null;
    }

    private string GetCurrentTitle()
    {
        foreach (Screen screen in Screens.Reverse())
        {
            if (screen.Title != null)
            {
                return screen.Title;
            }
        }

        return null;
    }

    [NotNull]
    public T Push<T>()
        where T : Screen
    {
        var screen = Screen.CreateInstance<T>(this);

        _screens.Add(screen);

        EditorApplication.delayCall += () =>
        {
            AdjustToCurrentScreen();
            Repaint();
        };

        return screen;
    }

    public void Pop([NotNull] Screen screen, object result)
    {
        if (!_screens.Contains(screen))
        {
            throw new InvalidOperationException();
        }

        while (_screens.Contains(screen))
        {
            _screens.RemoveAt(_screens.Count - 1);
        }

        if (CurrentScreen != null)
        {
            CurrentScreen.OnActivatedFromTop(result);
        }

        EditorApplication.delayCall += () =>
        {
            AdjustToCurrentScreen();
            Repaint();
        };
    }

    [NotNull]
    public T ClearAndPush<T>()
        where T : Screen
    {
        _screens.Clear();
        return Push<T>();
    }

    private void AdjustToCurrentScreen()
    {
        if (CurrentScreen == null)
        {
            return;
        }

        Vector2? size = GetCurrentSize();

        if (size.HasValue)
        {
            Rect rect = position;

            Vector2 rectCenter = rect.center;

            rect.width = size.Value.x;
            rect.height = size.Value.y;

            rect.center = rectCenter;

            position = rect;
        }

        EditorGUIUtility.editingTextField = false;
    }

    private void OnGUI()
    {
        Screen currentScreen = CurrentScreen;
        if (currentScreen != null)
        {
            string titleValue = GetCurrentTitle();

            if (titleValue != null)
            {
                titleContent = new GUIContent(titleValue);
            }

            if (Event.current != null &&
                Event.current.type == EventType.Repaint)
            {
                currentScreen.UpdateIfActive();
            }

            currentScreen.Draw();
        }
    }
}
}