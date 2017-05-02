using System;

namespace Lit.Data
{
    public enum VType
    {
        None = 0,
        Bool = 1,
        Int = 2,
        Long = 3,
        UInt = 4,
        ULong = 5,
        Float = 6,
        Double = 7,
        String = 8,
    }
    public abstract class Var : IComparable<Var>, IEquatable<Var>
    {

        public abstract object Value { get; }
        public VType Type { get; protected set; }

        public virtual bool Equals(Var other)
        {
            return CompareTo(other) == 0;
        }
        public virtual int CompareTo(Var other)
        {
            return Type - other.Type;
        }

        public static implicit operator Var(double v) { return new VDouble(v); }

        public static implicit operator double(Var v) { return (VDouble)v; }
    }

    public class VDouble:Var
    {
        public double Val;
        public override object Value { get { return Val; } }

        public VDouble() { }
        public VDouble(double d) { Val = d; Type = VType.Double; }

        public static implicit operator double(VDouble v) { return v.Val; }
        public static implicit operator VDouble(double d) { return new VDouble(d); }

        public override int CompareTo(Var other)
        {
            int ret = base.CompareTo(other);
            return ret != 0 ? ret : Val.CompareTo(((VDouble)other).Val);
        }
        public override int GetHashCode() { return Val.GetHashCode(); }

    }


}
