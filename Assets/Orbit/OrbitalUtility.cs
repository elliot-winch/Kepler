using UnityEngine;

public static class OrbitalUtility
{
    public static Matrix4x4 PerifocalToEquitorial(OrbitalElements oe)
    {
        return PerifocalToEquitorial((float)oe.Inclination, (float)oe.LongitudeOfAscendingNode, (float)oe.ArgumentOfPeriapsis);
    }


    //using floats so we can use unity matrix mul
    //I is inclination, a is ascending (big omega), and p is argument of periapsis (little omega)
    public static Matrix4x4 PerifocalToEquitorial(float i, float bigOmega, float littleOmega)
    {
        float r00 = Mathf.Cos(bigOmega) * Mathf.Cos(littleOmega) - Mathf.Sin(bigOmega) * Mathf.Sin(littleOmega) * Mathf.Cos(i);
        float r01 = -Mathf.Cos(bigOmega) * Mathf.Sin(littleOmega) - Mathf.Sin(bigOmega) * Mathf.Cos(littleOmega) * Mathf.Cos(i);
        float r02 = Mathf.Sin(bigOmega) * Mathf.Sin(i);

        float r10 = Mathf.Sin(bigOmega) * Mathf.Cos(littleOmega) + Mathf.Cos(bigOmega) * Mathf.Sin(littleOmega) * Mathf.Cos(i);
        float r11 = -Mathf.Sin(bigOmega) * Mathf.Sin(littleOmega) + Mathf.Cos(bigOmega) * Mathf.Cos(littleOmega) * Mathf.Cos(i);
        float r12 = -Mathf.Cos(bigOmega) * Mathf.Sin(i);

        float r20 = Mathf.Sin(littleOmega) * Mathf.Sin(i);
        float r21 = Mathf.Cos(littleOmega) * Mathf.Sin(i);
        float r22 = Mathf.Cos(i);

        //Column major!
        return new Matrix4x4(
            new Vector4(r00, r10, r20, 0),
            new Vector4(r01, r11, r21, 0),
            new Vector4(r02, r12, r22, 0),
            new Vector4(0, 0, 0, 1f)
        );
    }
}
