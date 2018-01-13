﻿using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Marvin.JsonPatch.NetStandard.Properties;

namespace Marvin.JsonPatch.Internal
{
    public class DictionaryAdapter<TKey, TValue> : IAdapter
    {
        public bool TryAdd(
            object target,
            string segment,
            IContractResolver contractResolver,
            object value,
            out string errorMessage)
        {
            var contract = (JsonDictionaryContract)contractResolver.ResolveContract(target.GetType());
            var key = contract.DictionaryKeyResolver(segment);
            var dictionary = (IDictionary<TKey, TValue>)target;

            // As per JsonPatch spec, if a key already exists, adding should replace the existing value
            if (!TryConvertKey(key, out var convertedKey, out errorMessage))
            {
                return false;
            }

            if (!TryConvertValue(value, out var convertedValue, out errorMessage))
            {
                return false;
            }

            dictionary[convertedKey] = convertedValue;
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
            var contract = (JsonDictionaryContract)contractResolver.ResolveContract(target.GetType());
            var key = contract.DictionaryKeyResolver(segment);
            var dictionary = (IDictionary<TKey, TValue>)target;

            if (!TryConvertKey(key, out var convertedKey, out errorMessage))
            {
                value = null;
                return false;
            }

            if (!dictionary.ContainsKey(convertedKey))
            {
                value = null;
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                return false;
            }

            value = dictionary[convertedKey];
            errorMessage = null;
            return true;
        }

        public bool TryRemove(
            object target,
            string segment,
            IContractResolver contractResolver,
            out string errorMessage)
        {
            var contract = (JsonDictionaryContract)contractResolver.ResolveContract(target.GetType());
            var key = contract.DictionaryKeyResolver(segment);
            var dictionary = (IDictionary<TKey, TValue>)target;

            if (!TryConvertKey(key, out var convertedKey, out errorMessage))
            {
                return false;
            }

            // As per JsonPatch spec, the target location must exist for remove to be successful
            if (!dictionary.ContainsKey(convertedKey))
            {
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                return false;
            }

            dictionary.Remove(convertedKey);

            errorMessage = null;
            return true;
        }

        public bool TryReplace(
            object target,
            string segment,
            IContractResolver contractResolver,
            object value,
            out string errorMessage)
        {
            var contract = (JsonDictionaryContract)contractResolver.ResolveContract(target.GetType());
            var key = contract.DictionaryKeyResolver(segment);
            var dictionary = (IDictionary<TKey, TValue>)target;

            if (!TryConvertKey(key, out var convertedKey, out errorMessage))
            {
                return false;
            }

            // As per JsonPatch spec, the target location must exist for remove to be successful
            if (!dictionary.ContainsKey(convertedKey))
            {
                errorMessage = string.Format(Resources.TargetLocationAtPathSegmentNotFound, segment);
                return false;
            }

            if (!TryConvertValue(value, out var convertedValue, out errorMessage))
            {
                return false;
            }

            dictionary[convertedKey] = convertedValue;

            errorMessage = null;
            return true;
        }

        public bool TryTraverse(
            object target,
            string segment,
            IContractResolver contractResolver,
            out object nextTarget,
            out string errorMessage)
        {
            var contract = (JsonDictionaryContract)contractResolver.ResolveContract(target.GetType());
            var key = contract.DictionaryKeyResolver(segment);
            var dictionary = (IDictionary<TKey, TValue>)target;

            if (!TryConvertKey(key, out var convertedKey, out errorMessage))
            {
                nextTarget = null;
                return false;
            }

            if (dictionary.ContainsKey(convertedKey))
            {
                nextTarget = dictionary[convertedKey];
                errorMessage = null;
                return true;
            }
            else
            {
                nextTarget = null;
                errorMessage = null;
                return false;
            }
        }

        private bool TryConvertKey(string key, out TKey convertedKey, out string errorMessage)
        {
            var conversionResult = ConversionResultProvider.ConvertTo(key, typeof(TKey));
            if (conversionResult.CanBeConverted)
            {
                errorMessage = null;
                convertedKey = (TKey)conversionResult.ConvertedInstance;
                return true;
            }
            else
            {
                errorMessage = string.Format(Resources.InvalidPathSegment, key);
                convertedKey = default(TKey);
                return false;
            }
        }

        private bool TryConvertValue(object value, out TValue convertedValue, out string errorMessage)
        {
            var conversionResult = ConversionResultProvider.ConvertTo(value, typeof(TValue));
            if (conversionResult.CanBeConverted)
            {
                errorMessage = null;
                convertedValue = (TValue)conversionResult.ConvertedInstance;
                return true;
            }
            else
            {
                errorMessage = string.Format(Resources.InvalidValueForProperty, value);
                convertedValue = default(TValue);
                return false;
            }
        }
    }
}
