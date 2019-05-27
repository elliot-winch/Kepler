using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Quantity4 : QuantityN
{
    private SciNumber W => values[3];

    public Quantity4() : base() { }

    public Quantity4(Quantity3 xyz, SciNumber w) : 
        base(new List<SciNumber>() { xyz.X.TrueValue, xyz.Y.TrueValue, xyz.Z.TrueValue, w }, xyz.units)
    { }

    public Quantity4(SciNumber x, SciNumber y, SciNumber z, SciNumber w, Units units = null) :
        base(new List<SciNumber>() { x, y, z, w }, units)
    { }

    public Quantity3 DropDimension => new Quantity3(values.Take(3).Select(a => a / W).ToList(), units);

    public Quantity4 Mul(Matrix4x4 mat)
    {
        //using unity mul - for now
        Vector4 mulled = mat * new Vector4(values[0].FloatValue, values[1].FloatValue, values[2].FloatValue, values[3].FloatValue);

        return new Quantity4(new SciNumber(mulled.x), new SciNumber(mulled.y), new SciNumber(mulled.z), new SciNumber(mulled.w), units);
    }
}
