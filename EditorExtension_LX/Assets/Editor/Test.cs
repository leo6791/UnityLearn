using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowTimeAttribute))]
public class Test : PropertyDrawer
{
    //设置绘制的区域高度
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, Mathf.Max(0, property.intValue));

            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "", TimeConvert(property.intValue));
        }
        else
        {
            EditorGUI.HelpBox(position, "To use the Time Atribute," + label.ToString() + "must be int", MessageType.Error);
        }
    }

    private string TimeConvert(int value)
    {
        ShowTimeAttribute time = attribute as ShowTimeAttribute;
        if (time != null)
        {
            if (time.ShowHour)
            {
                int hours = value / (60 * 60);
                int minutes = (value % (60 * 60)) / 60;
                int seconds = value % 60;

                return string.Format("{0}:{1}:{2}(H:M:S)", hours, minutes.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
            }
        }
        else
        {
            int minutes = (value % (60 * 60)) / 60;
            int seconds = value % 60;

            return string.Format("{0}:{1}(M:S)", minutes.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
        }
        return string.Empty;
    }
}
