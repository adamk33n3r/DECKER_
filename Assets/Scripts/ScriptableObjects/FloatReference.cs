[System.Serializable]
public class FloatReference
{
    public FloatVariable variable;
    public float constant;
    public bool useVariable = true;
    public float Value => useVariable ? variable.Value : constant;

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
