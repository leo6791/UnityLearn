using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUILayout = UnityEngine.GUILayout;

namespace EGO
{
    public static class EGOGlobalString
    {
        public const string EGO_TODOS = "EGO_TODOS";
    }
    public class MainWindow : EditorWindow
    {
        private bool mShowing = false;

        [MenuItem("EGOUnity/MainWindow %w")]
        private static void ShowWindow()
        {
            var window = GetWindow<MainWindow>();
            if (!window.mShowing)
            {
                window.mTodoList = TodoList.Load();
                Texture2D tex = Resources.Load<Texture2D>("home");
                window.titleContent = new GUIContent("EGO Window", tex);
                window.Show();
                window.mShowing = true;
            }
            else
            {
                window.Close();
                window.mShowing = false;
            }
        }

        private TodoList mTodoList = new TodoList();
        private string mInputContent = string.Empty;
        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            //输入框输入字符后点击添加新增toggle，并保存todolist数据，通过json工具序列化与反序列化
            mInputContent = GUILayout.TextField(mInputContent);
            if (GUILayout.Button("添加"))
            {
                if (!string.IsNullOrEmpty(mInputContent))
                {
                    mTodoList.todos.Add(new Todo() { Content = mInputContent });
                    mInputContent = String.Empty;
                    mTodoList.Save();
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical("box");

            //从后往前遍历，因为有可能要删除元素
            for (int i = mTodoList.todos.Count - 1; i >= 0; i--)
            {
                var todo = mTodoList.todos[i];
                EditorGUILayout.BeginHorizontal();
                todo.FinishedValue = GUILayout.Toggle(todo.FinishedValue, todo.Content);
                if (todo.FinishedChaged)
                {
                    todo.FinishedChaged = false;
                    mTodoList.Save();
                }
                if (GUILayout.Button("删除"))
                {
                    mTodoList.todos.Remove(todo);
                    mTodoList.Save();
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

        }
    }
}
