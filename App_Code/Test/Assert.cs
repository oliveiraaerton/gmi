using System;
using System.Data;
using System.Collections.Generic;

public class Assert
{
	public static List<string> Erro = new List<string>();

	public static void Clear()
	{
		Erro = new List<string>();		
	}

	public static void IsNotNull(Object teste)
	{
		Erro.Add("IsNotNull");
		if(teste!=null)
		{
			Erro.Add("Success: Object is Not Null |" + teste.ToString());
		}
		else
		{	
			Erro.Add("Failed: Object is Null |");
		}
	}

	public static void IsNull(Object teste)
	{
		Erro.Add("IsNull");
		if(teste==null)
		{
			Erro.Add("Success: Object is Null |");
		}
		else
		{	
			Erro.Add("Failed: Object is Not Null |"  + teste.ToString());
		}
	}

	public static void IsTrue(bool condition)
	{
		Erro.Add("IsTrue");
		if(condition)
		{
			Erro.Add("Success: Condition Is True");
		}
		else
		{
			Erro.Add("Failed: Condition Is False");
		}
	}

	public static void IsFalse(bool condition)
	{
		Erro.Add("IsFalse");
		if(condition)
		{
			Erro.Add("Success: Condition Is False");
		}
		else
		{
			Erro.Add("Failed: Condition Is True");
		}
	}

	public static void AreEqual(double expected, double actual,	double delta)
	{
		Erro.Add("AreEqual(double)");
		double potencia = 10;
		double deltaZ = Math.Pow(potencia, delta);
      	
      	if ((((int)(expected*deltaZ)) - ((int)(actual*deltaZ))) == 0)
      	{
      		Erro.Add("Success: Values Are Equals |" + expected.ToString() + "|" + actual.ToString() + "|" + delta.ToString());
      	}
		else
		{
			Erro.Add("Failed: Values Are Not Equals |" + expected.ToString()+ "|" + actual.ToString() + "|" + delta.ToString());
		}      	
    	
	}

	public static void AreEqual(object expected, object actual)
	{
		Erro.Add("AreEqual(object)");
      	if(expected.Equals(actual))
      	{
      		Erro.Add("Success: Values Are Equals |" + expected.ToString() + "|" + actual.ToString());
      	}
		else
		{
			Erro.Add("Failed: Values Are Not Equals |" + expected.ToString()+ "|" + actual.ToString());
		}      	
	}

	public static void AreEqual(float expected, float actual, double delta)
	{
		Erro.Add("AreEqual(float)");
		double potencia = 10;
		float deltaZ = (float) Math.Pow(potencia, delta);
      	
      	if (((int)(expected*deltaZ) - (int)(actual*deltaZ)) == 0)
      	{
      		Erro.Add("Success: Values Are Equals |" + expected.ToString() + "|" + actual.ToString() + "|" + delta.ToString());
      	}
		else
		{
			Erro.Add("Failed: Values Are Not Equals |" + expected.ToString()+ "|" + actual.ToString() + "|" + delta.ToString());
		}      	
	}

	public static void AreEqual(string expected, string actual,	bool ignoreCase)
	{
		Erro.Add("AreEqual(string)");
		if(ignoreCase)
		{
			if(expected.ToUpper().Equals(actual.ToUpper()))
			{
      			Erro.Add("Success: Values Are Equals |" + expected.ToString() + "|" + actual.ToString() + "|" + ignoreCase.ToString());
			}
			else
			{
				Erro.Add("Failed: Values Are Not Equals |" + expected.ToString()+ "|" + actual.ToString() + "|" + ignoreCase.ToString());
			} 			
		}
		else
		{
			if(expected.Equals(actual))
			{
    	  		Erro.Add("Success: Values Are Equals |" + expected.ToString() + "|" + actual.ToString() + "|" + ignoreCase.ToString());
			}
			else
			{
				Erro.Add("Failed: Values Are Not Equals |" + expected.ToString()+ "|" + actual.ToString() + "|" + ignoreCase.ToString());
			} 				
		}
	}

	public static void IsInstanceOfType(Object o, Type t)
	{
		if(t.IsInstanceOfType(o))
		{
			Erro.Add("Success: Object Is Instance Of Type |" + o.ToString() + "|" + t.ToString());
		}
		else
		{
			Erro.Add("Failed: Object Is Not Instance Of Type |" + o.ToString() + "|" + t.ToString());
		}
	}

}
