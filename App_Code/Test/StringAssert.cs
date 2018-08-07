using System.Text.RegularExpressions;
using System;
using System.Data;
using System.Collections.Generic;

public class StringAssert
{
	public static List<string> Erro = new List<string>();

	public static void Clear()
	{
		Erro = new List<string>();		
	}

	public static void Contains(String x, String y)
	{
		//Verifica se a primeira cadeia contem a segunda cadeia de caracteres
		if(x.Contains(y))
		{
			Erro.Add("Success: String x Contains String y |" + x.ToString() + "|" + y.ToString());
		}
		else
		{
			Erro.Add("Failed: String x Not Contains String y |" + x.ToString() + "|" + y.ToString());
		}
	}

	public static void EndsWith(String x, String y)
	{
		//Verifica se a primeira cadeia termina com a segunda cadeia de caracteres
		if(x.EndsWith(y))
		{
			Erro.Add("Success: String x Ends With String y |" + x.ToString() + "|" + y.ToString());
		}
		else
		{
			Erro.Add("Failed: String x Not Ends With String y |" + x.ToString() + "|" + y.ToString());
		}
	}

	public static void StartsWith(String x, String y)
	{
		//Verifica se a primeira cadeia começa com a segunda cadeia de caracteres
		if(x.StartsWith(y))
		{
			Erro.Add("Success: String x Starts With String y |" + x.ToString() + "|" + y.ToString());
		}
		else
		{
			Erro.Add("Failed: String x Not Starts With String y |" + x.ToString() + "|" + y.ToString());
		}
	}

	public static void DoesNotMatch(String x, Regex y)
	{
		//Verifica se a primeira cadeia não corresponde a expressão regular
		if(!y.IsMatch(x))
		{
			Erro.Add("Success: String Not Match Regular Expression |" + x.ToString() + "|" + y.ToString());
		}
		else
		{	
			Erro.Add("Failed: String Match Regular Expression |" + x.ToString() + "|" + y.ToString());
		}
	}

	public static void Matches(String x, Regex y)
	{
		//Verifica se a primeira cadeia corresponde a expressão regular
		if(y.IsMatch(x))
		{
			Erro.Add("Success: String Match Regular Expression |" + x.ToString() + "|" + y.ToString());
		}
		else
		{	
			Erro.Add("Failed: String Not Match Regular Expression |" + x.ToString() + "|" + y.ToString());
		}
	}


}