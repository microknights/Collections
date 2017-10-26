using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MicroKnights.Collections
{
    [Serializable]
    [DebuggerDisplay("{DisplayName}[{Value}]")]
    public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
        where TEnumeration : Enumeration<TEnumeration>
    {
        protected Enumeration(int value, string displayName)
            : base(value, displayName)
        {
        }

        public static TEnumeration FromInt32(int value)
        {
            return FromValue(value);
        }

        public static bool TryFromInt32(int listItemValue, out TEnumeration result)
        {
            return TryParse(listItemValue, out result);
        }
    }

    [Serializable]
    [DebuggerDisplay("{DisplayName}[{Value}]")]
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        [DataMember(Order = 1)]
        readonly string _displayName;

        [DataMember(Order = 0)]
        readonly TValue _value;

        protected Enumeration(TValue value, string displayName)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrWhiteSpace(displayName)) throw new ArgumentNullException(nameof(displayName));

            _value = value;
            _displayName = displayName;
        }

        public TValue Value => _value;
        public string DisplayName => _displayName;

        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
        }

        public sealed override string ToString()
        {
            return DisplayName;
        }

        public static TEnumeration[] GetAll()
        {
            return Enumerations.Value;
        }

        private static TEnumeration[] GetEnumerations()
        {
            Type enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        public bool Equals(TEnumeration other)
        {
            return other != null && ValueEquals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return !Equals(left, right);
        }

        public static TEnumeration FromValue(TValue value)
        {
            return Parse(item => item.Value.Equals(value));
        }

        public static TEnumeration FromValueOrDefault(TValue value, TEnumeration defaultEnumeration)
        {
            return ParseOrDefault(item => item.Value.Equals(value), defaultEnumeration);
        }

        public static TEnumeration Parse(string displayName)
        {
            return Parse(item => item.DisplayName == displayName);
        }

        public static TEnumeration ParseOrDefault(string displayName, TEnumeration defaultEnumeration)
        {
            return ParseOrDefault(item => item.DisplayName == displayName, defaultEnumeration);
        }

        static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll().SingleOrDefault(predicate);
            return result != null;
        }

        private static TEnumeration Parse(Func<TEnumeration, bool> predicate)
        {
            if( TryParse(predicate, out var result) )
            {
                return result;
            }

            return default(TEnumeration);
        }

        private static TEnumeration ParseOrDefault( Func<TEnumeration, bool> predicate, TEnumeration defaultEnumeration)
        {
            return TryParse(predicate, out var result) ? result : defaultEnumeration;
        }

        public static bool TryParse(TValue value, out TEnumeration result)
        {
            return TryParse(e => e.ValueEquals(value), out result);
        }

        public static bool TryParse(string displayName, out TEnumeration result)
        {
            return TryParse(e => e.DisplayName == displayName, out result);
        }

        protected virtual bool ValueEquals(TValue value)
        {
            return Value.Equals(value);
        }
    }
}
