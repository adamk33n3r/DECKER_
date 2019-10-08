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

    public override bool Equals(object other)
    {
        if (other is IntegerVariable)
            return this.Value == ((IntegerVariable)other).Value;
        else
            return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
