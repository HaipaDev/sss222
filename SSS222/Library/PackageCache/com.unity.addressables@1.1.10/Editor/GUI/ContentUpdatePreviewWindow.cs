﻿using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityEditor.AddressableAssets.GUI
{
    class ContentUpdatePreviewWindow : EditorWindow
    {
        /// <summary>
        /// Opens ContentUpdatePreviewWindow.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="buildPath"></param>
        /// <returns>True if successful; false if otherwise (failed to get modified entries).</returns>
        public static bool PrepareForContentUpdate(AddressableAssetSettings settings, string buildPath)
        {
            var modifiedEntries = ContentUpdateScript.GatherModifiedEntries(settings, buildPath);
            if (modifiedEntries == null)
                return false;
            var previewWindow = GetWindow<ContentUpdatePreviewWindow>();
            previewWindow.Show(settings, modifiedEntries);
            return true;
        }

        void OnEnable()
        {
            titleContent = new GUIContent("Content Update Preview");
        }


        class ContentUpdateTreeView : TreeView
        {
            class Item : TreeViewItem
            {
                internal AddressableAssetEntry entry;
                internal bool enabled;
                public Item(AddressableAssetEntry entry) : base(entry.guid.GetHashCode())
                {
                    this.entry = entry;
                    enabled = true;
                }
            }

            ContentUpdatePreviewWindow m_Preview;
            public ContentUpdateTreeView(ContentUpdatePreviewWindow preview, TreeViewState state, MultiColumnHeaderState mchs) : base(state, new MultiColumnHeader(mchs))
            {
                m_Preview = preview;
            }

            internal List<AddressableAssetEntry> GetEnabledEntries()
            {
                var result = new List<AddressableAssetEntry>();
                foreach (var i in GetRows())
                {
                    var item = i as Item;
                    if (item != null)
                    {
                        if (item.enabled)
                            result.Add(item.entry);
                    }
                }
                return result;
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem(-1, -1);
                root.children = new List<TreeViewItem>();
                foreach (var k in m_Preview.m_Entries)
                    root.AddChild(new Item(k));

                return root;
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                var item = args.item as Item;
                if (item == null)
                {
                    base.RowGUI(args);
                    return;
                }
                for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                {
                    CellGUI(args.GetCellRect(i), item, args.GetColumn(i));
                }
            }

            void CellGUI(Rect cellRect, Item item, int column)
            {
                if (column == 0)
                {
                    item.enabled = EditorGUI.Toggle(cellRect, item.enabled);
                }
                else if (column == 1)
                {
                    EditorGUI.LabelField(cellRect, item.entry.address);
                }
                else if (column == 2)
                {
                    EditorGUI.LabelField(cellRect, item.entry.AssetPath);
                }
                else if(column == 3)
                {
                    EditorGUI.LabelField(cellRect, item.entry.parentGroup.Name);
                }
            }

            internal static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
            {
                var retVal = new MultiColumnHeaderState.Column[4];
                retVal[0] = new MultiColumnHeaderState.Column();
                retVal[0].headerContent = new GUIContent("Include", "Include change in Update");
                retVal[0].minWidth = 50;
                retVal[0].width = 50;
                retVal[0].maxWidth = 50;
                retVal[0].headerTextAlignment = TextAlignment.Left;
                retVal[0].canSort = true;
                retVal[0].autoResize = true;

                retVal[1] = new MultiColumnHeaderState.Column();
                retVal[1].headerContent = new GUIContent("Address", "Data Value");
                retVal[1].minWidth = 300;
                retVal[1].width = 300;
                retVal[1].maxWidth = 1000;
                retVal[1].headerTextAlignment = TextAlignment.Left;
                retVal[1].canSort = true;
                retVal[1].autoResize = true;

                retVal[2] = new MultiColumnHeaderState.Column();
                retVal[2].headerContent = new GUIContent("Path", "Asset Path");
                retVal[2].minWidth = 300;
                retVal[2].width = 300;
                retVal[2].maxWidth = 1000;
                retVal[2].headerTextAlignment = TextAlignment.Left;
                retVal[2].canSort = true;
                retVal[2].autoResize = true;

                retVal[3] = new MultiColumnHeaderState.Column();
                retVal[3].headerContent = new GUIContent("Modified Group", "The modified Addressable group");
                retVal[3].minWidth = 300;
                retVal[3].width = 300;
                retVal[3].maxWidth = 1000;
                retVal[3].headerTextAlignment = TextAlignment.Left;
                retVal[3].canSort = true;
                retVal[3].autoResize = true;

                return new MultiColumnHeaderState(retVal);
            }
        }

        AddressableAssetSettings m_Settings;
        List<AddressableAssetEntry> m_Entries;
        Vector2 m_ScrollPosition;
        ContentUpdateTreeView m_Tree;
        [FormerlySerializedAs("treeState")]
        [SerializeField]
        TreeViewState m_TreeState;
        [FormerlySerializedAs("mchs")]
        [SerializeField]
        MultiColumnHeaderState m_Mchs;

        public void Show(AddressableAssetSettings settings, List<AddressableAssetEntry> entries)
        {
            m_Settings = settings;
            m_Entries = entries;
            Show();
        }

        public void OnGUI()
        {
            if (m_Entries == null)
                return;
            Rect contentRect = new Rect(0, 0, position.width, position.height - 50);

            if (m_Tree == null)
            {
                if (m_TreeState == null)
                    m_TreeState = new TreeViewState();

                var headerState = ContentUpdateTreeView.CreateDefaultMultiColumnHeaderState();
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_Mchs, headerState))
                    MultiColumnHeaderState.OverwriteSerializedFields(m_Mchs, headerState);
                m_Mchs = headerState;

                m_Tree = new ContentUpdateTreeView(this, m_TreeState, m_Mchs);
                m_Tree.Reload();
            }

            if (m_Entries.Count == 0)
            {
                GUILayout.BeginArea(contentRect);
                GUILayout.BeginVertical();

                GUILayout.Label("No Addressable groups with a BundledAssetGroupSchema and ContentUpdateGroupSchema (with StaticContent enabled) appear to have been modified.");

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else
                m_Tree.OnGUI(contentRect);

            GUILayout.BeginArea(new Rect(0, position.height - 50, position.width, 50));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
                Close();
            using (new EditorGUI.DisabledScope(m_Tree.GetEnabledEntries().Count == 0))
            {
                if (GUILayout.Button("Apply Changes"))
                {
                    ContentUpdateScript.CreateContentUpdateGroup(m_Settings, m_Tree.GetEnabledEntries(), "Content Update");
                    Close();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}