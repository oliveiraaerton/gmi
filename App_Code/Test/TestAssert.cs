using System.Text.RegularExpressions;
using System;
using System.Data;
using System.Collections.Generic;

public class TestAssert
{
    public static void Asserts()
    {
        Assert.Clear();

        //arrange:
        String s="Nao nulo";
        String u = null;
        double d = 1.35;
        double e = 1.351;
 		double delta1 = 2;
 		double delta2 = 3;
        float f = 1.35f;
        float g = 1.351f;
        String a = "TestE";
        String b = "teste";
        //act:
        //assert:
        Assert.IsNotNull(s);
        Assert.IsNotNull(u);

		Assert.IsNull(u);
		Assert.IsNull(s);

		Assert.IsTrue(true);
		Assert.IsTrue(false);

		Assert.AreEqual(e, d, delta1);
		Assert.AreEqual(e, d, delta2);

		Assert.AreEqual(d, d);
		Assert.AreEqual(d, e);

		Assert.AreEqual(f, g, delta1);
 		Assert.AreEqual(f, g, delta2);

 		Assert.AreEqual(a, b, true);
 		Assert.AreEqual(a, b, false);

 		Assert.IsInstanceOfType(a, typeof(String));
 		Assert.IsInstanceOfType(f, typeof(String));
    }

    public static void StringAsserts()
    {
        StringAssert.Clear();

        String a = "Primeira contem Segunda";
        String b = "Segunda";
        String c = "segunda";
        String d = "Primeira";

        // Define a regular expression for repeated words.
        Regex rx = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        String reg = "The dog dog lazy fog";

        StringAssert.Contains(a, b);
        StringAssert.Contains(a, c);

        StringAssert.EndsWith(a, b);
        StringAssert.EndsWith(a, c);

        StringAssert.StartsWith(a, d);
        StringAssert.StartsWith(a, c);

        StringAssert.DoesNotMatch(a, rx);
        StringAssert.DoesNotMatch(reg, rx);

        StringAssert.Matches(reg, rx);
        StringAssert.Matches(a, rx);
    }

}