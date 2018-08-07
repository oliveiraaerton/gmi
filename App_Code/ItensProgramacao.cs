using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Licencas do Sistema
/// </summary>
public class ItensProgramacao
{
    public ItensProgramacao(){}
    public int ID {get;set;}
	public int ProgramacaoId{get;set;}
    public string TipoMidia{get;set;} //0 - imagem; 1 - Cartaz; 2 - Tabelas    
	public int Ordem{get;set;}
    public int ImagemID{get;set;}
    public int CartazID{get;set;}
    public int TabelaID{get;set;}
    public int FeedID{get;set;}
    public int TempoExibicao{get;set;}
}

