using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Licencas do Sistema
/// </summary>
public class Feed
{
    public Feed(){}
    public int ID{get;set;}
    public int Ordem{get;set;}
    public string Titulo{get;set;}
	public string Noticia{get;set;}
	public int Ativo{get;set;} //0-Inativo; 1-Ativo
	public int Velocidade{get;set;} //1 2 3
}
