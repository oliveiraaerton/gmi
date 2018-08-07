using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Licencas do Sistema
/// </summary>
public class Programacao
{
    public Programacao(){}
    public int ID{get;set;}
	public string Descricao{get;set;}
    public DateTime PeriodoInicial{get;set;}
    public DateTime PeriodoFinal{get;set;}
    public List<ItensProgramacao> ItensProgramacao{get;set;}
}
