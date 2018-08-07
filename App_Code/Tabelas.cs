using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Licencas do Sistema
/// </summary>
public class Tabelas
{
    public Tabelas(){}
    public int ID{get;set;}
	public string Cabecalho{get;set;}
	public string FonteCabecalho{get;set;}
    public int TamanhoCabecalho{get;set;}
    public List<ItensTabela> ItensTabela{get;set;}
}
