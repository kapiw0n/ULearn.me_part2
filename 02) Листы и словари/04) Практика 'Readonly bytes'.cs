using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    public class ReadonlyBytes : IEquatable<ReadonlyBytes>, IEnumerable<byte>
    {
        private readonly byte[] _data;
        private int? _cachedHash;

        public ReadonlyBytes(params byte[] bytes)
        {
            _data = (byte[])bytes?.Clone() ?? throw new ArgumentNullException(nameof(bytes));
        }

        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)_data).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public byte this[int index] => _data[index];
        public int Length => _data.Length;

        public override bool Equals(object? obj) => Equals(obj as ReadonlyBytes);
        
        public bool Equals(ReadonlyBytes? other)
        {
            if (other is null || GetType() != other.GetType())
                return false;
                
            return StructuralComparisons.StructuralEqualityComparer.Equals(_data, other._data);
        }

        public override int GetHashCode()
        {
            _cachedHash ??= StructuralComparisons.StructuralEqualityComparer.GetHashCode(_data);
            return _cachedHash.Value;
        }

        public static bool operator ==(ReadonlyBytes? left, ReadonlyBytes? right) 
            => EqualityComparer<ReadonlyBytes?>.Default.Equals(left, right);
        
        public static bool operator !=(ReadonlyBytes? left, ReadonlyBytes? right) 
            => !(left == right);

        public override string ToString() => $"[{string.Join(", ", _data)}]";
    }
}