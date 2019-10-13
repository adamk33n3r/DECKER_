[System.Serializable]
public class IntegerReference
{
    public IntegerVariable variable;
    public int constant;
    public bool useVariable = true;
    public int Value => useVariable ? variable.Value : constant;

    public static implicit operator int(IntegerReference reference)
    {
        return reference.Value;
    }
}
