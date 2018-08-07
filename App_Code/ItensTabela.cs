using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Licencas do Sistema
/// </summary>
public class ItensTabela
{
    public ItensTabela(){}
    public int ID{get;set;}
    public int TabelaID{get;set;}
	public string Produto{get;set;}
    public string FonteProduto{get;set;}
    public int TamanhoProduto{get;set;}
    public decimal Valor{get;set;}
    public string FonteValor{get;set;}
    public int TamanhoValor{get;set;}
}
