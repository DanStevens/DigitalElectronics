using System;
using System.ComponentModel;
using System.Diagnostics;

#nullable enable

namespace DigitalElectronics.Concepts
{
    
    /// <summary>
    /// A mutable box for a value type, that provides change notifications.
    /// For use with WPF where binding a binding to a primitive type is required
    /// but not supported as an object is required.
    /// </summary>
    /// <typeparam name="T">The type of the boxed value</typeparam>
    /// <note>Warning: As a mutable object, don't use objects of this type
    /// as keys in a hash table.</note>
    [DebuggerDisplay("{Value}")]
    public class Box<T> : INotifyPropertyChanged, IComparable<Box<T>>, IComparable<T>, IEquatable<Box<T>>, IEquatable<T>
        where T : struct, IComparable<T>, IEquatable<T>
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private T _value;

        public Box(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public static implicit operator Box<T>(T value)
        {
            return new Box<T>(value);
        }

        public static implicit operator T(Box<T> box)
        {
            return box._value;
        }

        public static bool operator ==(Box<T> lhs, Box<T> rhs)
        {
            if (ReferenceEquals(lhs, null))
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Box<T> lhs, Box<T> rhs)
        {
            return !(lhs == rhs);
        }

        public int CompareTo(Box<T>? other)
        {
            if (other is null)
                return 1;
            
            return CompareTo(other._value);
        }

        public int CompareTo(T other)
        {
            return _value.CompareTo(other);
        }

        public bool Equals(Box<T>? other)
        {
            if (ReferenceEquals(other, this))
                return true;
            
            if (other is null)
                return false;

            return Equals(other._value);
        }

        public bool Equals(T other)
        {
            return _value.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Box<T> box)
            {
                return Equals(box);
            }

            if (obj is T t)
            {
                return Equals(t);
            }

            return false;
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString()
        {
            return $"Box<{typeof(T)}>({_value})";
        }
    }
}
