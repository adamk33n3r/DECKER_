using UnityEngine;

[CreateAssetMenu]
public class IntegerVariable : ScriptableObject
{
    public int Value;

    public IntegerVariable(int v)
    {
        Value = v;
    }

    public static bool operator ==(IntegerVariable a, IntegerVariable b)
    {
        return a.Value == b.Value;
    }

    public static bool operator !=(IntegerVariable a, IntegerVariable b)
    {
        return a.Value != b.Value;
    }

    public static IntegerVariable operator ++(IntegerVariable a)
    {
        a.Value++;
        return a;
    }

    public static IntegerVariable operator --(IntegerVariable a)
    {
        a.Value--;
        return a;
    }

    public static implicit operator int(IntegerVariable v) => v.Value;
    public static implicit operator IntegerVariable(int v)
    {
        var iv = CreateInstance<IntegerVariable>();
        iv.Value = v;
        return iv;
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
