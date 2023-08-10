using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGO.Util
{
    [Serializable]
    public class Propety<T>
    {
        private  T mValue = default(T);

        private event Action mValueChangedEvent;
        public T Value
        {
            get => mValue;
            set
            {
                if (!value.Equals(mValue))
                {
                    mValue = value;
                    mValueChangedEvent?.Invoke();
                }
            }
        }
        public Propety()
        {

        }

        public Propety(T initValue)
        {
            Value = initValue;
        }
        
        //×¢²á»Øµ÷
        public void RegisterValueChanged(Action onValueChanged)
        {
            mValueChangedEvent += onValueChanged;
        }
    }

}
