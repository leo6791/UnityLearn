using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EGO
{

    [Serializable]
    public class TodoList
    {
        public List<Todo> todos = new List<Todo>();
        public static TodoList Load()
        {
            var todoContent = EditorPrefs.GetString(EGOGlobalString.EGO_TODOS, string.Empty);
            if (string.IsNullOrEmpty(todoContent))
            {
                return new TodoList();
            }

            try
            {
                //将旧的保存数据转成新的形式
                var deprecated = JsonUtility.FromJson<Deprecated.TodoList>(todoContent);
                if (deprecated != null && deprecated.todos.Count > 0)
                {
                    var todos = deprecated.todos.Select(todo => new Todo() { Content = todo }).ToList();
                    var mtodoContent = JsonUtility.ToJson(new TodoList() { todos = todos });
                    EditorPrefs.SetString(EGOGlobalString.EGO_TODOS, JsonUtility.ToJson(mtodoContent));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return JsonUtility.FromJson<TodoList>(todoContent);

        }

        public void Save()
        {
            EditorPrefs.SetString(EGOGlobalString.EGO_TODOS, JsonUtility.ToJson(this));
        }
    }

    [Serializable]
    public class Todo
    {
        public string Content = String.Empty;
        public bool Finished = false;
    }
}

namespace EGO.Deprecated
{
    //弃用
    [Obsolete]
    [Serializable]
    public class TodoList
    {
        public List<string> todos = new List<string>();
    }
}
