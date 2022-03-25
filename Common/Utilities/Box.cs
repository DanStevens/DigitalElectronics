﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DigitalElectronics.Utilities
{
    
    /// <summary>
    /// A box for a value type, that provides change notifications. For use with WPF where binding a binding
    /// to a primitive type is required but not supported as an object is required
    /// </summary>
    /// <typeparam name="T">The type of the boxed value</typeparam>
    [DebuggerDisplay("{Value}")]
    public class Box<T> : INotifyPropertyChanged, IComparable<Box<T>>, IComparable<T>, IEquatable<Box<T>>, IEquatable<T>
        where T : struct, IComparable<T>, IEquatable<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            if (other is null)
                return false;

            return Equals(other._value);
        }

        public bool Equals(T other)
        {
            return _value.Equals(other);
        }
    }
}