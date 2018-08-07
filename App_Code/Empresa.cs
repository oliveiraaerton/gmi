using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Cadastro de Empresa
/// </summary>
public class Empresa
{
    public Empresa(){}
    public Empresa(string NOME){
    	Nome = NOME;
    }
	public string Nome{get;set;}
    public string Cnpj{get;set;}
    public string Endereco{get;set;}
    public string Fone{get;set;}
    public string Responsavel{get;set;}
}
