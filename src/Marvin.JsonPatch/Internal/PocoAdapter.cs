﻿using Marvin.JsonPatch.NetStandard.Properties;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace Marvin.JsonPatch.Internal
{
    public class PocoAdapter : IAdapter
    {
        public bool TryAdd(
            object target,
            string segment,
            IContractResolver contractResolver,
            object value,
            out string errorMessage)
        {
            if (!TryGetJsonProperty(target, contractResolver, segment, out var jsonProperty))
            {
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                return false;
            }

            if (!jsonProperty.Writable)
            {
                errorMessage = string.Format(Resources.CannotUpdateProperty, segment);
                return false;
            }

            if (!TryConvertValue(value, jsonProperty.PropertyType, out var convertedValue))
            {
                errorMessage = string.Format(Resources.InvalidValueForProperty, value);
                return false;
            }

            jsonProperty.ValueProvider.SetValue(target, convertedValue);

            errorMessage = null;
            return true;
        }

        public bool TryGet(
            object target,
            string segment,
            IContractResolver contractResolver,
            out object value,
            out string errorMessage)
        {
            if (!TryGetJsonProperty(target, contractResolver, segment, out var jsonProperty))
            {
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                value = null;
                return false;
            }

            if (!jsonProperty.Readable)
            {
                errorMessage = string.Format(Resources.CannotReadProperty, segment);
                value = null;
                return false;
            }

            value = jsonProperty.ValueProvider.GetValue(target);
            errorMessage = null;
            return true;
        }

        public bool TryRemove(
            object target,
            string segment,
            IContractResolver contractResolver,
            out string errorMessage)
        {
            if (!TryGetJsonProperty(target, contractResolver, segment, out var jsonProperty))
            {
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                return false;
            }

            if (!jsonProperty.Writable)
            {
                errorMessage = string.Format(Resources.CannotUpdateProperty, segment);
                return false;
            }

            // Setting the value to "null" will use the default value in case of value types, and
            // null in case of reference types
            object value = null;
            if (jsonProperty.PropertyType.IsValueType
                && Nullable.GetUnderlyingType(jsonProperty.PropertyType) == null)
            {
                value = Activator.CreateInstance(jsonProperty.PropertyType);
            }

            jsonProperty.ValueProvider.SetValue(target, value);

            errorMessage = null;
            return true;
        }

        public bool TryReplace(
            object target,
            string segment,
            IContractResolver
            contractResolver,
            object value,
            out string errorMessage)
        {
            if (!TryGetJsonProperty(target, contractResolver, segment, out var jsonProperty))
            {
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                return false;
            }

            if (!jsonProperty.Writable)
            {
                errorMessage = string.Format(Resources.CannotUpdateProperty, segment);
                return false;
            }

            if (!TryConvertValue(value, jsonProperty.PropertyType, out var convertedValue))
            {
                errorMessage = string.Format(Resources.InvalidValueForProperty, value);
                return false;
            }

            jsonProperty.ValueProvider.SetValue(target, convertedValue);

            errorMessage = null;
            return true;
        }

        public bool TryTraverse(
            object target,
            string segment,
            IContractResolver contractResolver,
            out object value,
            out string errorMessage)
        {
            if (target == null)
            {
                value = null;
                errorMessage = null;
                return false;
            }

            if (TryGetJsonProperty(target, contractResolver, segment, out var jsonProperty))
            {
                value = jsonProperty.ValueProvider.GetValue(target);
                errorMessage = null;
                return true;
            }

            value = null;
            errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
            return false;
        }

        private bool TryGetJsonProperty(
            object target,
            IContractResolver contractResolver,
            string segment,
            out JsonProperty jsonProperty)
        {
            if (contractResolver.ResolveContract(target.GetType()) is JsonObjectContract jsonObjectContract)
            {
                var pocoProperty = jsonObjectContract
                    .Properties
                    .FirstOrDefault(p => string.Equals(p.PropertyName, segment, StringComparison.OrdinalIgnoreCase));

                if (pocoProperty != null)
                {
                    jsonProperty = pocoProperty;
                    return true;
                }
            }

            jsonProperty = null;
            return false;
        }

        private bool TryConvertValue(object value, Type propertyType, out object convertedValue)
        {
            var conversionResult = ConversionResultProvider.ConvertTo(value, propertyType);
            if (!conversionResult.CanBeConverted)
            {
                convertedValue = null;
                return false;
            }

            convertedValue = conversionResult.ConvertedInstance;
            return true;
        }
    }
}
