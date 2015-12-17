using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class BaseJsonResultTester<T> where T : BaseJsonResultTester<T>
    {
        private object _jsonData;

        protected BaseJsonResultTester(object jsonData)
        {
            this._jsonData = jsonData;
        }

        public T HavingPropertyValue<ValueType>(string propertyName, ValueType value)
        {
            GetValue<ValueType>(propertyName).Should().Be(value);
            return (T)this;
        }

        public T HavingPropertyValue<ValueType>(string propertyName, Action<ValueType> action)
        {
            ValueType value = GetValue<ValueType>(propertyName);
            action(value);
            return (T)this;
        }

        public T HavingModel(object model)
        {
            _jsonData.Should().Be(model);
            return (T)this;
        }

        private ValueType GetValue<ValueType>(string propertyName)
        {
            PropertyInfo propertyInfo = _jsonData.GetType().GetProperty(propertyName);
            object propertyValue = propertyInfo.GetValue(_jsonData, null);
            propertyValue.Should().NotBeNull(propertyName + " should exist in " + _jsonData.GetType().Name);

            if (propertyValue is IEnumerable)
            {
                return (ValueType)propertyValue;
            }

            return (ValueType)Convert.ChangeType(propertyValue, typeof(ValueType));
        }
    }
}
