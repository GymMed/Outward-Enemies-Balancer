using OutwardModsCommunicator.EventBus;
using System;

namespace OutwardEnemiesBalancer.Utility.Helpers.Static
{
    public static class EventPayloadEnumHelper
    {
        public static T? GetEnum<T>(this EventPayload payload, string key, T? defaultValue = null) where T : struct, Enum
        {
            if (!payload.TryGetValue(key, out var value))
                return defaultValue;

            return TryConvertToEnum(value, out T result) ? result : defaultValue;
        }

        public static T GetEnum<T>(this EventPayload payload, string key, T defaultValue) where T : struct, Enum
        {
            if (!payload.TryGetValue(key, out var value))
                return defaultValue;

            return TryConvertToEnum(value, out T result) ? result : defaultValue;
        }

        public static bool TryGetEnum<T>(this EventPayload payload, string key, out T result) where T : struct, Enum
        {
            result = default;

            if (!payload.TryGetValue(key, out var value))
                return false;

            return TryConvertToEnum(value, out result);
        }

        private static bool TryConvertToEnum<T>(object value, out T result) where T : struct, Enum
        {
            result = default;

            if (value is T enumValue)
            {
                if (!Enum.IsDefined(typeof(T), enumValue))
                {
                    OutwardEnemiesBalancer.LogSL($"EventPayloadEnumHelper: Value '{enumValue}' is not a valid {typeof(T).Name} enum value.");
                    return false;
                }
                result = enumValue;
                return true;
            }

            if (value is string stringValue)
            {
                if (Enum.TryParse<T>(stringValue, ignoreCase: true, out var parsed))
                {
                    if (!Enum.IsDefined(typeof(T), parsed))
                    {
                        OutwardEnemiesBalancer.LogSL($"EventPayloadEnumHelper: Value '{stringValue}' is not a valid {typeof(T).Name} enum value.");
                        return false;
                    }
                    result = parsed;
                    return true;
                }

                OutwardEnemiesBalancer.LogSL($"EventPayloadEnumHelper: Could not parse '{stringValue}' to {typeof(T).Name} enum.");
                return false;
            }

            OutwardEnemiesBalancer.LogSL($"EventPayloadEnumHelper: Cannot convert {value?.GetType().Name ?? "null"} to {typeof(T).Name} enum.");
            return false;
        }
    }
}
