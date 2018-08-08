using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Data;
using System.Security.Cryptography;
using WebMatrix.Data;


/**
**	GMI - GERENCIADOR DE MIDIA INDOOR
** condição ? expressão1_se_true : expressão2_se_false
** NET GLOBALIZATION tem que estar como File UTF8
** quando o cara logar vai grava uma chave na sessao que sera associada ao usuario
** quando fize logout a sessão se destroi 
** a sessão deve ter tempo de expirar
**/

public class Dao
{
    public Dao(){}

	public static Mensagem mensagemErro = new Mensagem();
	public static Mensagem mensagemValidacao = new Mensagem();
	public static Mensagem mensagemSucesso = new Mensagem();
	//public static bool validacao = true;

	public static string Truncate(string value, int maxLength)
	{
		if (string.IsNullOrEmpty(value)) return value;
		return value.Length <= maxLength ? value : value.Substring(0, maxLength); 
	}

	protected  static bool validaUnique(string campo, string valor, string tabela)
	{
		bool validar = false;
		inicializa();
		
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle(String.Format("SELECT {0} FROM {1} WHERE {2} = '{3}'", campo, tabela, campo, valor));
			db.Close();
			if(sql!=null)
			{
				validar = false;
				mensagemValidacao.mensagem += "O campo: " + campo + " Não é Único";
							
			}
			else
			{
				validar = true;
				mensagemSucesso.mensagem = "O campo: " + campo + " é Único";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
			validar = false;
		}
		return validar;
	}

	protected static bool validaPresence(string campo, string valor)
	{
		if(String.IsNullOrEmpty(valor)){
			mensagemValidacao.mensagem += "O campo [" + campo + "] não pode estar vazio |";
			
			return false;
		}
		else {
			return true;
		}
	}

	protected static bool validaValor(string campo, int valor)
	{
		if(valor<=0){
			mensagemValidacao.mensagem += "O campo [" + campo + "] tem que ser maior que zero |";
			
			return false;
		}
		else {
			return true;
		}
	}

	protected static bool validaValorDecimal(string campo, decimal valor)
	{
		if(valor<=0m){
			mensagemValidacao.mensagem += "O campo [" + campo + "] tem que ser maior que zero |";
			return false;
		}
		else {
			return true;
		}
	}

	protected  static void inicializa()
	{
		
		mensagemValidacao.mensagem = String.Empty;
		mensagemValidacao.cor="Orange";
		mensagemErro.mensagem = String.Empty;
		mensagemErro.cor="Red";
		mensagemSucesso.mensagem = String.Empty;
		mensagemSucesso.cor="Green";
	}

	protected static int getLastID(string tabela)
	{
		inicializa();
		int resposta = 0;
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle(String.Format("SELECT TOP 1 ID FROM {0} ORDER BY ID DESC", tabela));
			db.Close();
			if(sql!=null)
			{
				resposta = sql.ID;
			}
			mensagemSucesso.mensagem = "Id " + resposta + " encontrado!";
						

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		//Teste.debug+="|Sucesso: " + mensagemSucesso.mensagem + "| Erro: " + mensagemErro.mensagem + "| Validação: " + mensagemValidacao.mensagem;
		return resposta;
	}

///// CRUD EMPRESA

	protected  static List<Empresa> listEmpresas()
	{
		inicializa();
		List<Empresa> es = new List<Empresa>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Empresa.* FROM Empresa ORDER BY CNPJ");
			db.Close();
			foreach(var sql in query)
			{
				Empresa e = new Empresa();
				e.Nome = String.IsNullOrEmpty(sql.NOME)?"":sql.NOME.ToString();
				e.Cnpj = String.IsNullOrEmpty(sql.CNPJ)?"":sql.CNPJ.ToString();
				e.Endereco = String.IsNullOrEmpty(sql.ENDERECO)?"":sql.ENDERECO.ToString();
				e.Fone = String.IsNullOrEmpty(sql.FONE)?"":sql.FONE.ToString();
				e.Responsavel = String.IsNullOrEmpty(sql.Responsavel)?"":sql.RESPONSAVEL.ToString();
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Empresas: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Não Há Empresas a Listar";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Empresa getEmpresa(string CNPJ)
	{
		inicializa();
		Empresa e = new Empresa();
		string CNPJSemPonto = String.IsNullOrEmpty(CNPJ)?"":CNPJ.Replace(".","").Replace("-","").Replace("/","");
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Empresa.* FROM Empresa WHERE CNPJ LIKE @0", CNPJSemPonto);
			db.Close();
			if(sql!=null)
			{
				e.Nome = String.IsNullOrEmpty(sql.NOME)?"":sql.NOME.ToString();
				e.Cnpj = String.IsNullOrEmpty(sql.CNPJ)?"":sql.CNPJ.ToString();
				e.Endereco = String.IsNullOrEmpty(sql.ENDERECO)?"":sql.ENDERECO.ToString();
				e.Fone = String.IsNullOrEmpty(sql.FONE)?"":sql.FONE.ToString();
				e.Responsavel = String.IsNullOrEmpty(sql.Responsavel)?"":sql.RESPONSAVEL.ToString();
				mensagemSucesso.mensagem = "Empresa: " + e.Nome;
				
			}
			else
			{
				mensagemValidacao.mensagem += "Empresa Não Encontrada!";
								
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setEmpresa(Empresa e)
	{
		inicializa();
		bool validacao = validaPresence("NOME", e.Nome) &&	validaPresence("CNPJ", e.Cnpj);
		if(String.IsNullOrEmpty(getEmpresa(e.Cnpj).Nome))
		{
			if(validacao)
			{
				string CNPJSemPonto = String.IsNullOrEmpty(e.Cnpj)?"":e.Cnpj.Replace(".","").Replace("-","").Replace("/","");
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Empresa (NOME, CNPJ, ENDERECO, FONE, RESPONSAVEL) VALUES (@0, @1, @2, @3, @4)", e.Nome, CNPJSemPonto, e.Endereco, e.Fone, e.Responsavel);
					db.Close();
					mensagemSucesso.mensagem = "Empresa Salva Com Sucesso!!!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Empresa Não Foi Salva!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Empresa Já Foi Salva!";
			
		}
	}

	protected  static void updateEmpresa(Empresa e)
	{
		inicializa();
		bool validacao = validaPresence("NOME", e.Nome) &&	validaPresence("CNPJ", e.Cnpj);
		if(!String.IsNullOrEmpty(getEmpresa(e.Cnpj).Nome))
		{
			if(validacao)
			{
				string CNPJSemPonto = String.IsNullOrEmpty(e.Cnpj)?"":e.Cnpj.Replace(".","").Replace("-","").Replace("/","");
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Empresa set ENDERECO=@0, FONE=@1, RESPONSAVEL=@2, NOME=@3 WHERE CNPJ=@4", e.Endereco, e.Fone, e.Responsavel, e.Nome, CNPJSemPonto);
					db.Close();
					mensagemSucesso.mensagem = "Empresa Alterada Com Sucesso!";

					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Empresa Não Foi Alterada!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Empresa Não Existe!";
			
		}
	}

	protected  static void deleteEmpresa(Empresa e)
	{
		inicializa();
		bool validacao = validaPresence("CNPJ", e.Cnpj);
		if(!String.IsNullOrEmpty(getEmpresa(e.Cnpj).Nome))
		{
			if(validacao)
			{
				string CNPJSemPonto = String.IsNullOrEmpty(e.Cnpj)?"":e.Cnpj.Replace(".","").Replace("-","").Replace("/","");
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Empresa WHERE CNPJ=@0", CNPJSemPonto);
					db.Close();
					mensagemSucesso.mensagem = "Empresa Excluída Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Empresa Não Foi Excluída!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Empresa Não Existe!";
			
		}
	}

///// CRUD CARTAZ

	protected  static List<Cartaz> listCartazes()
	{
		inicializa();
		List<Cartaz> es = new List<Cartaz>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Cartaz.* FROM Cartaz ORDER BY CABECALHO");
			db.Close();
			foreach(var sql in query)
			{
				Cartaz e = new Cartaz();
				e.ID = sql.ID>0?sql.ID:0;
				e.Cabecalho = String.IsNullOrEmpty(sql.CABECALHO)?"":sql.CABECALHO.ToString();
				e.FonteCabecalho = String.IsNullOrEmpty(sql.FONTE_CABECALHO)?"":sql.FONTE_CABECALHO.ToString();
				e.TamanhoCabecalho = sql.TAMANHO_CABECALHO>0?sql.TAMANHO_CABECALHO:20;
				e.Texto = String.IsNullOrEmpty(sql.TEXTO)?"":sql.TEXTO.ToString();
				e.FonteTexto = String.IsNullOrEmpty(sql.FONTE_TEXTO)?"":sql.FONTE_TEXTO.ToString();
				e.TamanhoTexto = sql.TAMANHO_TEXTO>0?sql.TAMANHO_TEXTO:20;
				e.Rodape = String.IsNullOrEmpty(sql.RODAPE)?"":sql.RODAPE.ToString();
				e.FonteRodape = String.IsNullOrEmpty(sql.FONTE_RODAPE)?"":sql.FONTE_RODAPE.ToString();
				e.TamanhoRodape = sql.TAMANHO_RODAPE>0?sql.TAMANHO_RODAPE:20;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Cartazes: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Cartaz a ser listado";
											
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Cartaz getCartaz(int ID)
	{
		inicializa();
		Cartaz e = new Cartaz();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Cartaz.* FROM Cartaz WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.Cabecalho = String.IsNullOrEmpty(sql.CABECALHO)?"":sql.CABECALHO.ToString();
				e.FonteCabecalho = String.IsNullOrEmpty(sql.FONTE_CABECALHO)?"":sql.FONTE_CABECALHO.ToString();
				e.TamanhoCabecalho = sql.TAMANHO_CABECALHO>0?sql.TAMANHO_CABECALHO:20;
				e.Texto = String.IsNullOrEmpty(sql.TEXTO)?"":sql.TEXTO.ToString();
				e.FonteTexto = String.IsNullOrEmpty(sql.FONTE_TEXTO)?"":sql.FONTE_TEXTO.ToString();
				e.TamanhoTexto = sql.TAMANHO_TEXTO>0?sql.TAMANHO_TEXTO:20;
				e.Rodape = String.IsNullOrEmpty(sql.RODAPE)?"":sql.RODAPE.ToString();
				e.FonteRodape = String.IsNullOrEmpty(sql.FONTE_RODAPE)?"":sql.FONTE_RODAPE.ToString();
				e.TamanhoRodape = sql.TAMANHO_RODAPE>0?sql.TAMANHO_RODAPE:20;

				mensagemSucesso.mensagem = "Cartaz: " + e.Cabecalho;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Cartaz Não Localizado!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setCartaz(Cartaz e)
	{
		inicializa();
		bool validacao = validaPresence("CABECALHO", e.Cabecalho) &&		validaPresence("TEXTO", e.Texto);
		if(String.IsNullOrEmpty(getCartaz(e.ID).Cabecalho))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Cartaz (CABECALHO, FONTE_CABECALHO, TAMANHO_CABECALHO, TEXTO, FONTE_TEXTO, TAMANHO_TEXTO, RODAPE, FONTE_RODAPE, TAMANHO_RODAPE) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)", e.Cabecalho, e.FonteCabecalho, e.TamanhoCabecalho, e.Texto, e.FonteTexto, e.TamanhoTexto, e.Rodape, e.FonteRodape, e.TamanhoRodape);
					db.Close();
					mensagemSucesso.mensagem = "Cartaz Salvo Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Cartaz Não Foi Salvo!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Cartaz Já Foi Salvo!";
			
		}
	}

	protected  static void updateCartaz(Cartaz e)
	{
		inicializa();
		bool validacao = validaPresence("CABECALHO", e.Cabecalho) &&		validaPresence("TEXTO", e.Texto);
		if(!String.IsNullOrEmpty(getCartaz(e.ID).Cabecalho))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Cartaz SET CABECALHO=@0, FONTE_CABECALHO=@1, TAMANHO_CABECALHO=@2, TEXTO=@3, FONTE_TEXTO=@4, TAMANHO_TEXTO=@5, RODAPE=@6, FONTE_RODAPE=@7, TAMANHO_RODAPE=@8 WHERE ID=@9", e.Cabecalho, e.FonteCabecalho, e.TamanhoCabecalho, e.Texto, e.FonteTexto, e.TamanhoTexto, e.Rodape, e.FonteRodape, e.TamanhoRodape, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Cartaz Alterado Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Cartaz Não Foi Alterado!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Cartaz Não Existe!";
			
		}
	}

	protected static bool temMidia(Cartaz e)
	{
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItemProgramacao.ID FROM ItemProgramacao WHERE TIPO_MIDIA='CARTAZ' AND MIDIA_ID = @0", e.ID);
			db.Close();
			if(sql!=null)
			{
				return true;
			}
			else
			{
				return false;
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return false;
	}

	protected static bool temMidia(Feed e)
	{
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItemProgramacao.ID FROM ItemProgramacao WHERE TIPO_MIDIA='FEED' AND MIDIA_ID = @0", e.ID);
			db.Close();
			if(sql!=null)
			{
				return true;
			}
			else
			{
				return false;
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return false;
	}

	protected  static void deleteCartaz(Cartaz e)
	{
		inicializa();
		if(getCartaz(e.ID).ID>0)
		{
			if(!temMidia(e))
			{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Cartaz WHERE ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Cartaz Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
			}			
			else
			{
				mensagemValidacao.mensagem += "Esse Cartaz Faz Parte de Uma Programação. Não Pode Ser Excluído!!!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Cartaz Não Existe!";
			
		}
	}


///// CRUD PROGRAMACAO

	protected  static List<Programacao> listProgramacao()
	{
		inicializa();
		List<Programacao> es = new List<Programacao>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Programacao.* FROM Programacao ORDER BY DESCRICAO");
			db.Close();
			foreach(var sql in query)
			{
				Programacao e = new Programacao();
				e.ID = sql.ID>0?sql.ID:0;
				e.Descricao = String.IsNullOrEmpty(sql.DESCRICAO)?"":sql.DESCRICAO.ToString();
				e.PeriodoInicial = String.IsNullOrEmpty(sql.PERIODOINICIAL.ToString())?"":DateTime.Parse(sql.PERIODOINICIAL.ToString());
				e.PeriodoFinal = String.IsNullOrEmpty(sql.PERIODOFINAL.ToString())?"":DateTime.Parse(sql.PERIODOFINAL.ToString());
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Programações: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhuma Programação a ser listada";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static List<Programacao> listProgramacaoEItens()
	{
		inicializa();
		List<Programacao> es = new List<Programacao>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Programacao.* FROM Programacao ORDER BY DESCRICAO");
			db.Close();
			foreach(var sql in query)
			{
				Programacao e = new Programacao();
				e.ID = sql.ID>0?sql.ID:0;
				e.Descricao = String.IsNullOrEmpty(sql.DESCRICAO)?"":sql.DESCRICAO.ToString();
				e.PeriodoInicial = String.IsNullOrEmpty(sql.PERIODOINICIAL.ToString())?"":DateTime.Parse(sql.PERIODOINICIAL.ToString());
				e.PeriodoFinal = String.IsNullOrEmpty(sql.PERIODOFINAL.ToString())?"":DateTime.Parse(sql.PERIODOFINAL.ToString());
				e.ItensProgramacao = listItensProgramacaoPorProgramacao(e.ID);
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Programações: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhuma Programação a ser listada";
												
			}
		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Programacao getProgramacao(int ID)
	{
		inicializa();
		Programacao e = new Programacao();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Programacao.* FROM Programacao WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.Descricao = String.IsNullOrEmpty(sql.DESCRICAO)?"":sql.DESCRICAO.ToString();
				e.PeriodoInicial = String.IsNullOrEmpty(sql.PERIODOINICIAL.ToString())?"":DateTime.Parse(sql.PERIODOINICIAL.ToString());
				e.PeriodoFinal = String.IsNullOrEmpty(sql.PERIODOFINAL.ToString())?"":DateTime.Parse(sql.PERIODOFINAL.ToString());
				mensagemSucesso.mensagem = "Programação: " + e.Descricao;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Programação Não Localizada!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setProgramacao(Programacao e)
	{
		inicializa();
		bool validacao = validaPresence("DESCRICAO", e.Descricao)&&validaPresence("PERIODOINICIAL", e.PeriodoInicial.ToString())&&validaPresence("PERIODOFINAL", e.PeriodoFinal.ToString());

		if(String.IsNullOrEmpty(getProgramacao(e.ID).Descricao))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Programacao (DESCRICAO, PERIODOINICIAL, PERIODOFINAL) VALUES (@0, @1, @2)", e.Descricao, e.PeriodoInicial.ToString("yyyy-MM-dd"), e.PeriodoFinal.ToString("yyyy-MM-dd"));
					db.Close();
					mensagemSucesso.mensagem = "Programação Salva Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Programação Não Foi Salva!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Programação Já Foi Salva!";
			
		}
	}

	protected  static void updateProgramacao(Programacao e)
	{
		inicializa();
		bool validacao = validaPresence("DESCRICAO", e.Descricao)&&validaPresence("PERIODOINICIAL", e.PeriodoInicial.ToString())&&validaPresence("PERIODOFINAL", e.PeriodoFinal.ToString());
		if(!String.IsNullOrEmpty(getProgramacao(e.ID).Descricao))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Programacao SET DESCRICAO=@0, PERIODOINICIAL=@1, PERIODOFINAL=@2 WHERE ID=@3", e.Descricao, e.PeriodoInicial.ToString("yyyy-MM-dd"), e.PeriodoFinal.ToString("yyyy-MM-dd"), e.ID);
					db.Close();	
					mensagemSucesso.mensagem = "Programação Alterada Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Programação Não Foi Alterada!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Programação Não Existe!";
			
		}
	}


	protected  static void deleteProgramacao(Programacao e)
	{
		inicializa();
		if(getProgramacao(e.ID).ID>0)
		{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Programacao WHERE ID=@0", e.ID);				
					db.Execute("DELETE ItemProgramacao WHERE PROGRAMACAO_ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Programação Excluída Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
		}
		else 
		{
			mensagemValidacao.mensagem += "Programação Não Existe!";
			
		}
	}



	///// CRUD ITEMPROGRAMACAO

	protected  static List<ItensProgramacao> listItensProgramacaoPorProgramacao(int ProgramacaoID)
	{
		inicializa();
		List<ItensProgramacao> es = new List<ItensProgramacao>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT ItemProgramacao.* FROM ItemProgramacao WHERE PROGRAMACAO_ID=@0 ORDER BY ItemProgramacao.ID", ProgramacaoID);
			db.Close();
			foreach(var sql in query)
			{
				ItensProgramacao e = new ItensProgramacao();
				e.ID = sql.ID>0?sql.ID:0;
				e.ProgramacaoId = sql.PROGRAMACAO_ID>0?sql.PROGRAMACAO_ID:0;
				e.TipoMidia = String.IsNullOrEmpty(sql.TIPO_MIDIA)?"":sql.TIPO_MIDIA.ToString();

				switch(e.TipoMidia)
				{
					case "IMAGEM": 
					e.ImagemID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "CARTAZ": 
					e.CartazID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "TABELA": 
					e.TabelaID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "FEED": 
					e.FeedID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
				}
				e.Ordem = sql.ORDEM>0?sql.ORDEM:0;
				e.TempoExibicao = sql.TEMPO_EXIBICAO>0?sql.TEMPO_EXIBICAO:0;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Itens de Programação: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Item de Programação a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static List<ItensProgramacao> listItensProgramacao()
		{
			inicializa();
			List<ItensProgramacao> es = new List<ItensProgramacao>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT ItemProgramacao.* FROM ItemProgramacao ORDER BY ItemProgramacao.ID");
			db.Close();
			foreach(var sql in query)
			{
				ItensProgramacao e = new ItensProgramacao();
				e.ID = sql.ID>0?sql.ID:0;
				e.ProgramacaoId = sql.PROGRAMACAO_ID>0?sql.PROGRAMACAO_ID:0;
				e.TipoMidia = String.IsNullOrEmpty(sql.TIPO_MIDIA)?"":sql.TIPO_MIDIA.ToString();
			
				switch(e.TipoMidia)
				{
					case "IMAGEM": 
					e.ImagemID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "CARTAZ": 
					e.CartazID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "TABELA": 
					e.TabelaID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "FEED": 
					e.FeedID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
				}
				e.Ordem = sql.ORDEM>0?sql.ORDEM:0;
				e.TempoExibicao = sql.TEMPO_EXIBICAO>0?sql.TEMPO_EXIBICAO:0;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Itens de Programação: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Item de Programação a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
		}
	
	protected  static ItensProgramacao getItensProgramacao(int ID)
	{
		inicializa();
		ItensProgramacao e = new ItensProgramacao();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItemProgramacao.* FROM ItemProgramacao WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{

						e.ID = sql.ID>0?sql.ID:0;
						e.Ordem = sql.ORDEM>0?sql.ORDEM:0;
						e.ProgramacaoId = sql.PROGRAMACAO_ID>0?sql.PROGRAMACAO_ID:0;
						e.TempoExibicao = sql.TEMPO_EXIBICAO>0?sql.TEMPO_EXIBICAO:0;
						e.TipoMidia = String.IsNullOrEmpty(sql.TIPO_MIDIA)?"":sql.TIPO_MIDIA.ToString();
				switch(e.TipoMidia)
				{
					case "IMAGEM": 
					e.ImagemID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "CARTAZ": 
					e.CartazID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "TABELA": 
					e.TabelaID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
					case "FEED": 
					e.FeedID = sql.MIDIA_ID>0?sql.MIDIA_ID:0; break;
				}
				
				mensagemSucesso.mensagem = "Item de Programação: " + e.TipoMidia;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Item de Programação Não Localizado!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setItensProgramacao(ItensProgramacao e)
	{
		inicializa();
		int midiaId = 0;
		bool validaMidia = false;
		

		switch(e.TipoMidia)
		{
			case "IMAGEM": 
			validaMidia = validaValor("MIDIA_ID", e.ImagemID); midiaId = e.ImagemID; break;
			case "CARTAZ": 
			validaMidia = validaValor("MIDIA_ID", e.CartazID); midiaId = e.CartazID; break;
			case "TABELA": 
			validaMidia = validaValor("MIDIA_ID", e.TabelaID); midiaId = e.TabelaID; break;
			case "FEED": 
			validaMidia = validaValor("MIDIA_ID", e.FeedID); midiaId = e.FeedID; break;
		}
		
		bool validacao = validaPresence("TIPO_MIDIA", e.TipoMidia) &&		validaValor("ORDEM", e.Ordem) &&		validaValor("PROGRAMACAO_ID", e.ProgramacaoId) && validaValor("TEMPO_EXIBICAO", e.TempoExibicao) && validaMidia;
		if(String.IsNullOrEmpty(getItensProgramacao(e.ID).TipoMidia))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO ItemProgramacao (PROGRAMACAO_ID, ORDEM, TIPO_MIDIA, MIDIA_ID, TEMPO_EXIBICAO) VALUES (@0, @1, @2, @3, @4)", e.ProgramacaoId, e.Ordem, e.TipoMidia, midiaId, e.TempoExibicao);
					db.Close();
					mensagemSucesso.mensagem = "Item de Programação Salvo Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += String.Format("Item de Programação Não Foi Salvo!: ");
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Item de Programação Já Foi Salvo!";
			
		}
	}

	protected  static void updateItensProgramacao(ItensProgramacao e)
	{
		inicializa();
		int midiaId = 0;
		bool validaMidia = false;
		
		switch(e.TipoMidia)
		{
			case "IMAGEM": 
			validaMidia = validaValor("MIDIA_ID", e.ImagemID); midiaId = e.ImagemID; break;
			case "CARTAZ": 
			validaMidia = validaValor("MIDIA_ID", e.CartazID); midiaId = e.CartazID; break;
			case "TABELA": 
			validaMidia = validaValor("MIDIA_ID", e.TabelaID); midiaId = e.TabelaID; break;
			case "FEED": 
			validaMidia = validaValor("MIDIA_ID", e.FeedID); midiaId = e.FeedID; break;
		}
		
		bool validacao = validaPresence("TIPO_MIDIA", e.TipoMidia) &&		validaValor("ORDEM", e.Ordem) &&		validaValor("PROGRAMACAO_ID", e.ProgramacaoId) && validaValor("TEMPO_EXIBICAO", e.TempoExibicao) && validaMidia;
		if(!String.IsNullOrEmpty(getItensProgramacao(e.ID).TipoMidia))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE ItemProgramacao SET ORDEM=@0, TIPO_MIDIA=@1, MIDIA_ID=@2, TEMPO_EXIBICAO=@3 WHERE ID=@4", e.Ordem, e.TipoMidia, midiaId, e.TempoExibicao, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Item de Programação Alterado Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Item de Programação Não Foi Alterado!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Item de Programação Não Existe!";
			
		}
	}
	

protected  static void deleteItensProgramacao(ItensProgramacao e)
	{
		inicializa();
		if(getItensProgramacao(e.ID).ID>0)
		{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE ItemProgramacao WHERE ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Item de Programação Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
						
				}
		}
		else 
		{
			mensagemValidacao.mensagem += "Item de Programação Não Existe!";
			
		}
	}


///// CRUD IMAGEM

	protected  static List<Imagem> listImagens()
	{
		inicializa();
		List<Imagem> es = new List<Imagem>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Imagem.* FROM Imagem ORDER BY ENDERECO");
			db.Close();
			foreach(var sql in query)
			{
				Imagem e = new Imagem();
				e.ID = sql.ID>0?sql.ID:0;
				e.Endereco = String.IsNullOrEmpty(sql.ENDERECO)?"":sql.ENDERECO.ToString();
				e.Altura = sql.ALTURA>0?sql.ALTURA:20;
				e.Largura = sql.LARGURA>0?sql.LARGURA:20;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de ImageNs: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Imagem a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Imagem getImagem(int ID)
	{
		inicializa();
		Imagem e = new Imagem();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Imagem.* FROM Imagem WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.Endereco = String.IsNullOrEmpty(sql.ENDERECO)?"":sql.ENDERECO.ToString();
				e.Altura = sql.ALTURA>0?sql.ALTURA:20;
				e.Largura = sql.LARGURA>0?sql.LARGURA:20;
				mensagemSucesso.mensagem = "Imagem: " + e.Endereco;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Imagem Não Localizado!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setImagem(Imagem e)
	{
		inicializa();
		bool validacao = validaPresence("ENDERECO", e.Endereco) &&		validaValor("ALTURA", e.Altura) &&		validaValor("LARGURA", e.Largura);
		if(String.IsNullOrEmpty(getImagem(e.ID).Endereco))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Imagem (ENDERECO, ALTURA, LARGURA) VALUES (@0, @1, @2)", e.Endereco, e.Altura, e.Largura);
					db.Close();
					mensagemSucesso.mensagem = "Imagem Salva Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Imagem Não Foi Salva!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Imagem Já Foi Salva!";
			
		}
	}

	protected  static void updateImagem(Imagem e)
	{
		inicializa();
		bool validacao = validaPresence("ENDERECO", e.Endereco) &&		validaValor("ALTURA", e.Altura) &&		validaValor("LARGURA", e.Largura);
		if(!String.IsNullOrEmpty(getImagem(e.ID).Endereco))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Imagem SET ENDERECO=@0, ALTURA=@1, LARGURA=@2 WHERE ID=@3", e.Endereco, e.Altura, e.Largura, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Imagem Alterada Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Imagem Não Foi Alterada!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Imagem Não Existe!";
			
		}
	}

	protected static bool temMidia(Imagem e)
	{
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItemProgramacao.ID FROM ItemProgramacao WHERE TIPO_MIDIA='IMAGEM' AND MIDIA_ID = @0", e.ID);
			db.Close();
			if(sql!=null)
			{
				return true;
			}
			else
			{
				return false;
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return false;
	}

	protected  static void deleteImagem(Imagem e)
	{
		inicializa();
		if(getImagem(e.ID).ID>0)
		{
			if(!temMidia(e))
			{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Imagem WHERE ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Imagem Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
			}																		
			else
			{
				mensagemValidacao.mensagem += "Essa Imagem Faz Parte de Uma Programação. Não Pode Ser Excluída!!!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Imagem Não Existe!";
			
		}
	}

///// CRUD TABELAS

	protected  static List<Tabelas> listTabelas()
	{
		inicializa();
		List<Tabelas> es = new List<Tabelas>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Tabelas.* FROM Tabelas ORDER BY CABECALHO");
			db.Close();
			foreach(var sql in query)
			{
				Tabelas e = new Tabelas();
				e.ID = sql.ID>0?sql.ID:0;
				e.Cabecalho = String.IsNullOrEmpty(sql.CABECALHO)?"":sql.CABECALHO.ToString();
				e.FonteCabecalho = String.IsNullOrEmpty(sql.FONTE_CABECALHO)?"":sql.FONTE_CABECALHO.ToString();
				e.TamanhoCabecalho = sql.TAMANHO_CABECALHO>0?sql.TAMANHO_CABECALHO:20;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Tabelas: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhuma Tabela a ser listada";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static List<Tabelas> listTabelasEItens()
	{
		inicializa();
		List<Tabelas> es = new List<Tabelas>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Tabelas.* FROM Tabelas ORDER BY CABECALHO");
			db.Close();
			foreach(var sql in query)
			{
				Tabelas e = new Tabelas();
				e.ID = sql.ID>0?sql.ID:0;
				e.Cabecalho = String.IsNullOrEmpty(sql.CABECALHO)?"":sql.CABECALHO.ToString();
				e.FonteCabecalho = String.IsNullOrEmpty(sql.FONTE_CABECALHO)?"":sql.FONTE_CABECALHO.ToString();
				e.TamanhoCabecalho = sql.TAMANHO_CABECALHO>0?sql.TAMANHO_CABECALHO:20;
				e.ItensTabela = listItensTabelasPorTabela(e.ID);
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Tabelas: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhuma Tabela a ser listada";
												
			}
		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Tabelas getTabelas(int ID)
	{
		inicializa();
		Tabelas e = new Tabelas();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Tabelas.* FROM Tabelas WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.Cabecalho = String.IsNullOrEmpty(sql.CABECALHO)?"":sql.CABECALHO.ToString();
				e.FonteCabecalho = String.IsNullOrEmpty(sql.FONTE_CABECALHO)?"":sql.FONTE_CABECALHO.ToString();
				e.TamanhoCabecalho = sql.TAMANHO_CABECALHO>0?sql.TAMANHO_CABECALHO:20;
				mensagemSucesso.mensagem = "Tabela: " + e.Cabecalho;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Tabela Não Localizada!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setTabelas(Tabelas e)
	{
		inicializa();
		bool validacao = validaPresence("CABECALHO", e.Cabecalho);
		if(String.IsNullOrEmpty(getTabelas(e.ID).Cabecalho))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Tabelas (CABECALHO, FONTE_CABECALHO, TAMANHO_CABECALHO) VALUES (@0, @1, @2)", e.Cabecalho, e.FonteCabecalho, e.TamanhoCabecalho);
					db.Close();
					mensagemSucesso.mensagem = "Tabela Salva Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Tabela Não Foi Salva!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Tabela Já Foi Salva!";
			
		}
	}

	protected  static void updateTabelas(Tabelas e)
	{
		inicializa();
		bool validacao = validaPresence("CABECALHO", e.Cabecalho);
		if(!String.IsNullOrEmpty(getTabelas(e.ID).Cabecalho))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Tabelas SET CABECALHO=@0, FONTE_CABECALHO=@1, TAMANHO_CABECALHO=@2 WHERE ID=@3", e.Cabecalho, e.FonteCabecalho, e.TamanhoCabecalho, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Tabela Alterada Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Tabela Não Foi Alterada!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Tabela Não Existe!";
			
		}
	}

	protected static bool temMidia(Tabelas e)
	{
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItemProgramacao.ID FROM ItemProgramacao WHERE TIPO_MIDIA='TABELA' AND MIDIA_ID = @0", e.ID);
			db.Close();
			if(sql!=null)
			{
				return true;
			}
			else
			{
				return false;
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return false;
	}

	protected  static void deleteTabelas(Tabelas e)
	{
		inicializa();
		if(getTabelas(e.ID).ID>0)
		{
			if(!temMidia(e))
			{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Tabelas WHERE ID=@0", e.ID);				
					db.Execute("DELETE ItensTabela WHERE TABELA_ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Tabela Excluída Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
			}			
			else
			{
				mensagemValidacao.mensagem += "Essa Tabela Faz Parte de Uma Programação. Não Pode Ser Excluída!!!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Tabela Não Existe!";
			
		}
	}

///// CRUD ITENSTABELAS

	protected  static List<ItensTabela> listItensTabelas()
	{
		inicializa();
		List<ItensTabela> es = new List<ItensTabela>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT ItensTabela.* FROM ItensTabela ORDER BY ItensTabela.ID");
			db.Close();
			foreach(var sql in query)
			{
				ItensTabela e = new ItensTabela();
				e.ID = sql.ID>0?sql.ID:0;
				e.TabelaID = sql.TABELA_ID>0?sql.TABELA_ID:0;
				e.Produto = String.IsNullOrEmpty(sql.PRODUTO)?"":sql.PRODUTO.ToString();
				e.FonteProduto = String.IsNullOrEmpty(sql.FONTE_PRODUTO)?"":sql.FONTE_PRODUTO.ToString();
				e.TamanhoProduto = sql.TAMANHO_PRODUTO>0?sql.TAMANHO_PRODUTO:20;
				e.Valor = sql.VALOR>0?sql.VALOR:0m;
				e.FonteValor = String.IsNullOrEmpty(sql.FONTE_VALOR)?"":sql.FONTE_VALOR.ToString();
				e.TamanhoValor = sql.TAMANHO_VALOR>0?sql.TAMANHO_VALOR:20;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Itens de Tabela: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Item de Tabela a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static List<ItensTabela> listItensTabelasPorTabela(int TabelaID)
	{
		inicializa();
		List<ItensTabela> es = new List<ItensTabela>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT ItensTabela.* FROM ItensTabela WHERE TABELA_ID=@0 ORDER BY ItensTabela.ID", TabelaID);
			db.Close();
			foreach(var sql in query)
			{
				ItensTabela e = new ItensTabela();
				e.ID = sql.ID>0?sql.ID:0;
				e.TabelaID = sql.TABELA_ID>0?sql.TABELA_ID:0;
				e.Produto = String.IsNullOrEmpty(sql.PRODUTO)?"":sql.PRODUTO.ToString();
				e.FonteProduto = String.IsNullOrEmpty(sql.FONTE_PRODUTO)?"":sql.FONTE_PRODUTO.ToString();
				e.TamanhoProduto = sql.TAMANHO_PRODUTO>0?sql.TAMANHO_PRODUTO:20;
				e.Valor = sql.VALOR>0?sql.VALOR:0m;
				e.FonteValor = String.IsNullOrEmpty(sql.FONTE_VALOR)?"":sql.FONTE_VALOR.ToString();
				e.TamanhoValor = sql.TAMANHO_VALOR>0?sql.TAMANHO_VALOR:20;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Itens de Tabela: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Item de Tabela a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static ItensTabela getItensTabelas(int ID)
	{
		inicializa();
		ItensTabela e = new ItensTabela();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItensTabela.* FROM ItensTabela WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.TabelaID = sql.TABELA_ID>0?sql.TABELA_ID:0;
				e.Produto = String.IsNullOrEmpty(sql.PRODUTO)?"":sql.PRODUTO.ToString();
				e.FonteProduto = String.IsNullOrEmpty(sql.FONTE_PRODUTO)?"":sql.FONTE_PRODUTO.ToString();
				e.TamanhoProduto = sql.TAMANHO_PRODUTO>0?sql.TAMANHO_PRODUTO:20;
				e.Valor = sql.VALOR>0?sql.VALOR:0m;
				e.FonteValor = String.IsNullOrEmpty(sql.FONTE_VALOR)?"":sql.FONTE_VALOR.ToString();
				e.TamanhoValor = sql.TAMANHO_VALOR>0?sql.TAMANHO_VALOR:20;
				mensagemSucesso.mensagem = "Item de Tabela: " + e.Produto;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Itens Não Localizados!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setItensTabelas(ItensTabela e)
	{
		inicializa();
		bool validacao = validaValor("TABELA_ID", e.TabelaID);
		if(validacao)
		{
				if(String.IsNullOrEmpty(getItensTabelas(e.ID).Produto))
				{
							try
							{
								var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
								db.Execute("INSERT INTO ItensTabela (TABELA_ID, PRODUTO, FONTE_PRODUTO, TAMANHO_PRODUTO, VALOR, FONTE_VALOR, TAMANHO_VALOR) VALUES (@0, @1, @2, @3, @4, @5, @6)", e.TabelaID, e.Produto, e.FonteProduto, e.TamanhoProduto, e.Valor, e.FonteValor, e.TamanhoValor);
								db.Close();
								mensagemSucesso.mensagem = "Item de Tabela Salvo Com Sucesso!";
								
							} catch (Exception ex) {
								mensagemErro.mensagem = ex.Message;
								
							}			
					}
				else 
				{
						mensagemValidacao.mensagem += "Item de Tabela Já Foi Salvo!";
						
				}
		} else {
					mensagemValidacao.mensagem += "Item de Tabela Não Foi Salvo!";
					
		}
	}

	protected  static void updateItensTabelas(ItensTabela e)
	{
		inicializa();
		bool validacao = validaValor("TABELA_ID", e.TabelaID);
		if(!String.IsNullOrEmpty(getItensTabelas(e.ID).Produto))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE ItensTabela SET PRODUTO=@0, FONTE_PRODUTO=@1, TAMANHO_PRODUTO=@2, VALOR=@3, FONTE_VALOR=@4, TAMANHO_VALOR=@5 WHERE ID=@6", e.Produto, e.FonteProduto, e.TamanhoProduto, e.Valor, e.FonteValor, e.TamanhoValor, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Item de Tabela Alterado Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Item de Tabela Não Foi Alterado!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Item de Tabela Não Existe!";
			
		}
	}

	protected  static void deleteItensTabelas(ItensTabela e)
	{
		inicializa();
		if(getItensTabelas(e.ID).ID>0)
		{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE ItensTabela WHERE ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Item de Tabela Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
		}
		else 
		{
			mensagemValidacao.mensagem += "Item de Tabela Não Existe!";
			
		}
	}

///// CRUD REGISTRO

	protected static bool validaLicenca(string licenca, DateTime validade)
	{
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Licenca.* FROM Licenca WHERE LICENCA=@0 AND VIGENCIA>@1", licenca, validade.ToString("yyyy-MM-dd"));
			db.Close();
			if(sql!=null)
			{
				validade = String.IsNullOrEmpty(sql.VIGENCIA.ToString())?"":DateTime.Parse(sql.VIGENCIA.ToString());
				mensagemSucesso.mensagem = "Licença válida até " + validade.ToString("dd/MM/yyyy");
				
				return true;
			}
			else
			{
				mensagemValidacao.mensagem += "A licença [" + licenca + "] expirou! |";
				
				return false;
			}
		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
			return false;
		}

	}

	protected static bool validaEmpresa(string cnpj)
	{
		try
		{
			if(!String.IsNullOrEmpty(getEmpresa(cnpj).Nome))
			{
				mensagemSucesso.mensagem = "Empresa cadastrada";	
				
				return true;
			}
			else
			{
				mensagemValidacao.mensagem += "Empresa Não Cadastrada";
				
				return false;
			}
		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
			return false;
		}

	}

	protected  static Registro getRegistro(string CNPJ)
	{
		inicializa();
		Registro e = new Registro();
		string CNPJSemPonto = String.IsNullOrEmpty(CNPJ)?"":CNPJ.Replace(".","").Replace("-","").Replace("/","");
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Registro.* FROM Registro WHERE CNPJ LIKE @0", CNPJSemPonto);
			db.Close();
			if(sql!=null)
			{
				e.Cnpj = String.IsNullOrEmpty(sql.CNPJ)?"":sql.CNPJ.ToString();
				e.LicencaAtiva = String.IsNullOrEmpty(sql.LICENCAATIVA)?"":sql.LICENCAATIVA.ToString();
				mensagemSucesso.mensagem = "Registro: " + e.Cnpj;
				
			}
			else
			{
				mensagemValidacao.mensagem += "Registro Não Encontrado!";
								
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setRegistro(Registro e)
	{
		inicializa();
		bool validacao = validaLicenca(e.LicencaAtiva, DateTime.Now)&&validaPresence("LICENCAATIVA", e.LicencaAtiva)&&validaPresence("CNPJ", e.Cnpj)&&validaEmpresa(e.Cnpj);

		if(String.IsNullOrEmpty(getRegistro(e.Cnpj).Cnpj))
		{
			if(validacao)
			{
				string CNPJSemPonto = String.IsNullOrEmpty(e.Cnpj)?"":e.Cnpj.Replace(".","").Replace("-","").Replace("/","");
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Registro (CNPJ, LICENCAATIVA) VALUES (@0, @1)", CNPJSemPonto, e.LicencaAtiva);
					db.Close();
					mensagemSucesso.mensagem = "Registro Salvo Com Sucesso!!!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Registro Não Foi Salvo!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Registro Já Foi Salvo!";
			
		}
	}

	protected  static void updateRegistro(Registro e)
	{
		inicializa();
		bool validacao = validaLicenca(e.LicencaAtiva, DateTime.Now) && 	validaPresence("CNPJ", e.Cnpj) &&		validaPresence("LICENCAATIVA", e.LicencaAtiva);
		if(!String.IsNullOrEmpty(getRegistro(e.Cnpj).Cnpj))
		{
			if(validacao)
			{
				string CNPJSemPonto = String.IsNullOrEmpty(e.Cnpj)?"":e.Cnpj.Replace(".","").Replace("-","").Replace("/","");
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Registro set LICENCAATIVA=@0 WHERE CNPJ=@1", e.LicencaAtiva, CNPJSemPonto);
					db.Close();
					mensagemSucesso.mensagem = "Registro Alterado Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Registro Não Foi Alterado!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Registro Não Existe!";
			
		}
	}

	protected  static void deleteRegistro(Registro e)
	{
		inicializa();
		bool validacao = validaPresence("CNPJ", e.Cnpj);
		if(!String.IsNullOrEmpty(getRegistro(e.Cnpj).Cnpj))
		{
			if(validacao)
			{
				string CNPJSemPonto = String.IsNullOrEmpty(e.Cnpj)?"":e.Cnpj.Replace(".","").Replace("-","").Replace("/","");
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Registro WHERE CNPJ=@0", CNPJSemPonto);
					db.Close();
					mensagemSucesso.mensagem = "Registro Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Registro Não Foi Excluído!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Registro Não Existe!";
			
		}
	}

///// CRUD FEED

	protected  static List<Feed> listFeeds()
	{
		inicializa();
		List<Feed> es = new List<Feed>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Feed.* FROM Feed ORDER BY ORDEM");
			db.Close();
			foreach(var sql in query)
			{
				Feed e = new Feed();
				e.ID = sql.ID>0?sql.ID:0;
				e.Titulo = String.IsNullOrEmpty(sql.TITULO)?"":sql.TITULO.ToString();
				e.Noticia = String.IsNullOrEmpty(sql.NOTICIA)?"":sql.NOTICIA.ToString();
				e.Ordem = sql.ORDEM>0?sql.ORDEM:20;
				e.Ativo = sql.ORDEM>0?sql.ATIVO:1;
				e.Velocidade = sql.ORDEM>0?sql.VELOCIDADE:2;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Feeds: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Feed a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static List<Feed> listFeeds(int ativo)
	{
		inicializa();
		List<Feed> es = new List<Feed>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT Feed.* FROM Feed WHERE ativo=@0 ORDER BY ORDEM", ativo);
			db.Close();
			foreach(var sql in query)
			{
				Feed e = new Feed();
				e.ID = sql.ID>0?sql.ID:0;
				e.Titulo = String.IsNullOrEmpty(sql.TITULO)?"":sql.TITULO.ToString();
				e.Noticia = String.IsNullOrEmpty(sql.NOTICIA)?"":sql.NOTICIA.ToString();
				e.Ordem = sql.ORDEM>0?sql.ORDEM:20;
				e.Ativo = sql.ORDEM>0?sql.ATIVO:1;
				e.Velocidade = sql.ORDEM>0?sql.VELOCIDADE:2;
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Feeds: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Feed a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Feed getFeed(int ID)
	{
		inicializa();
		Feed e = new Feed();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Feed.* FROM Feed WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.Titulo = String.IsNullOrEmpty(sql.TITULO)?"":sql.TITULO.ToString();
				e.Noticia = String.IsNullOrEmpty(sql.NOTICIA)?"":sql.NOTICIA.ToString();
				e.Ordem = sql.ORDEM>0?sql.ORDEM:20;
				e.Ativo = sql.ORDEM>0?sql.ATIVO:1;
				e.Velocidade = sql.ORDEM>0?sql.VELOCIDADE:2;
				mensagemSucesso.mensagem = "Feed: " + e.Noticia;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Feed Não Localizado!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return e;
	}

	protected  static void setFeed(Feed e)
	{
		inicializa();
		bool validacao = validaPresence("NOTICIA", e.Noticia) && validaValor("ORDEM", e.Ordem) && validaPresence("TITULO", e.Titulo);
		if(String.IsNullOrEmpty(getFeed(e.ID).Noticia))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Feed (ORDEM, TITULO, NOTICIA, ATIVO, VELOCIDADE) VALUES (@0, @1, @2, @3, @4)", e.Ordem, e.Titulo, e.Noticia, 1, 2);
					db.Close();
					mensagemSucesso.mensagem = "Feed Salva Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Feed Não Foi Salva!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Feed Já Foi Salva!";
			
		}
	}

	protected  static void updateFeed(Feed e)
	{
		inicializa();
		bool validacao = validaPresence("NOTICIA", e.Noticia) && validaValor("ORDEM", e.Ordem) && validaPresence("TITULO", e.Titulo);
		if(!String.IsNullOrEmpty(getFeed(e.ID).Noticia))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Feed SET NOTICIA=@0, ORDEM=@1, TITULO=@2, VELOCIDADE=@3, ATIVO=@4 WHERE ID=@5", e.Noticia, e.Ordem, e.Titulo, e.Velocidade, e.Ativo, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Feed Alterada Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Feed Não Foi Alterada!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Feed Não Existe!";
			
		}
	}

	protected static bool temFeed(Feed e)
	{
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ItemProgramacao.ID FROM ItemProgramacao WHERE TIPO_MIDIA='FEED' AND MIDIA_ID = @0", e.ID);
			db.Close();
			if(sql!=null)
			{
				return true;
			}
			else
			{
				return false;
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return false;
	}

	protected static void deleteFeed(Feed e)
	{
		inicializa();
		if(getFeed(e.ID).ID>0)
		{
			if(!temMidia(e))
			{
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Feed WHERE ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Feed Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
			}																		
			else
			{
				mensagemValidacao.mensagem += "Essa Feed Faz Parte de Uma Programação. Não Pode Ser Excluída!!!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Feed Não Existe!";
			
		}
	}

///// CRUD USUARIO

	protected  static void setUsuario(Usuario e)
	{
		inicializa();
		bool validacao = validaPresence("NOME", e.Nome) && validaPresence("EMAIL", e.Email) && validaPresence("SENHA", e.Senha) && validaUnique("EMAIL", e.Email, "Usuario");
		if(String.IsNullOrEmpty(getUsuario(e.ID).Nome))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("INSERT INTO Usuario (NOME, EMAIL, SENHA) VALUES (@0, @1, @2)", e.Nome, e.Email, criptografarSenha(e.Senha));
					db.Close();
					mensagemSucesso.mensagem = "Usuario Salvo Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Usuario Não Foi Salvo!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Usuario Já Foi Salvo!";		
		}
		//Teste.debug+="|Sucesso: " + mensagemSucesso.mensagem + "| Erro: " + mensagemErro.mensagem + "| Validação: " + mensagemValidacao.mensagem;
	}

	protected  static List<Usuario> listUsuarios()
	{
		inicializa();
		List<Usuario> es = new List<Usuario>();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var query = db.Query("SELECT ID, Nome, Email FROM Usuario ORDER BY Nome");
			db.Close();
			foreach(var sql in query)
			{
				Usuario e = new Usuario();
				e.ID = sql.ID>0?sql.ID:0;
				e.Nome = String.IsNullOrEmpty(sql.NOME)?"":sql.NOME.ToString();
				e.Email = String.IsNullOrEmpty(sql.EMAIL)?"":sql.EMAIL.ToString();
				es.Add(e);
			}
			if(es.Count>0)
			{
				mensagemSucesso.mensagem = "Total de Usuarios: " + es.Count;
								
			}
			else
			{
				mensagemValidacao.mensagem += "Nenhum Usuario a ser listado";
												
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		return es;
	}

	protected  static Usuario getUsuario(int ID)
	{
		inicializa();
		Usuario e = new Usuario();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT ID, Nome, Email FROM Usuario WHERE ID = @0", ID);
			db.Close();
			if(sql!=null)
			{
				e.ID = sql.ID>0?sql.ID:0;
				e.Nome = String.IsNullOrEmpty(sql.NOME)?"":sql.NOME.ToString();
				e.Email = String.IsNullOrEmpty(sql.EMAIL)?"":sql.EMAIL.ToString();
				mensagemSucesso.mensagem = "Usuario: " + e.Nome;
							
			}
			else
			{
				mensagemValidacao.mensagem += "Usuario Não Localizado!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
		}
		//Teste.debug+="|Sucesso: " + mensagemSucesso.mensagem + "| Erro: " + mensagemErro.mensagem + "| Validação: " + mensagemValidacao.mensagem;
		return e;
	}

	protected  static void updateUsuario(Usuario e)
	{
		inicializa();
		bool validacao = validaPresence("NOME", e.Nome) && validaPresence("EMAIL", e.Email) &&	validaUnique("EMAIL", e.Email, "Usuario");
		if(!String.IsNullOrEmpty(getUsuario(e.ID).Nome))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Usuario SET NOME=@0, EMAIL=@1 WHERE ID=@2", e.Nome, e.Email, e.ID);
					db.Close();
					mensagemSucesso.mensagem = "Usuario Alterado Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Usuario Não Foi Alterado!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Usuario Não Existe!";
			
		}
	}

	protected  static void atualizarSenha(Usuario e)
	{
		inicializa();
		bool validacao = validaPresence("EMAIL", e.Email) && validaPresence("SENHA", e.Senha) ;
		if(!String.IsNullOrEmpty(getUsuario(e.ID).Nome))
		{
			if(validacao)
			{
				try
				{
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("UPDATE Usuario SET SENHA=@0 WHERE Email=@1", criptografarSenha(e.Senha), e.Email);
					db.Close();
					mensagemSucesso.mensagem = "Usuario Alterado Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}			
			} else {
				mensagemValidacao.mensagem += "Usuario Não Foi Alterado!";
				
			}
		}
		else 
		{
			mensagemValidacao.mensagem += "Usuario Não Existe!";
			
		}

	}

	protected static void deleteUsuario(Usuario e)
	{
		inicializa();
		if(getUsuario(e.ID).ID>0)
		{
			
				try
				{				
					var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
					db.Execute("DELETE Usuario WHERE ID=@0", e.ID);				
					db.Close();
					mensagemSucesso.mensagem = "Usuario Excluído Com Sucesso!";
					
				} catch (Exception ex) {
					mensagemErro.mensagem = ex.Message;
					
				}
		}
		else 
		{
			mensagemValidacao.mensagem += "Usuario Não Existe!";
			
		}
	}

	protected  static bool validarUsuario(Usuario e)
	{
		bool validar = false;
		inicializa();
		try
		{
			var db = Database.OpenConnectionString("Data Source=|DataDirectory|\\GMI.sdf;encryption mode=platform default;Password=m1d1@;", "System.Data.SqlServerCe.4.0");
			var sql = db.QuerySingle("SELECT Nome FROM Usuario WHERE Email = @0 and Senha=@1", e.Email, criptografarSenha(e.Senha));
			db.Close();
			if(sql!=null)
			{
				validar = true;
				mensagemSucesso.mensagem = "Bemvindo: " + e.Nome;
							
			}
			else
			{
				validar = false;
				mensagemValidacao.mensagem += "Usuario/Senha Inválidos!";
					
			}

		} catch (Exception ex) {
			mensagemErro.mensagem = ex.Message;
			
			validar = false;
		}
		return validar;

	}

	protected static string gerarChaveUsuario()
	{
		string chave = System.Guid.NewGuid().ToString();
		if(validaPresence("Chave", chave))
		{
			mensagemSucesso.mensagem = "Chave Gerada Com Sucesso!";
				
		}
		else
		{
			mensagemValidacao.mensagem += "Chave Inválida!";
				
		}
		return chave;
	}

	protected static string criptografarSenha(string senha)
	{
        byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("12345"));
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        byte[] DataToEncrypt = UTF8.GetBytes(senha);
        try
        {
            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            mensagemSucesso.mensagem = "Mensagem Criptografada Com Sucesso!";
			
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return Convert.ToBase64String(Results);
	}

	protected static string decriptografarSenha(string senha)
	{
		byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("12345"));
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        byte[] DataToDecrypt = Convert.FromBase64String(senha);
        try
        {
            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            mensagemSucesso.mensagem = "Mensagem Descriptografada Com Sucesso!";
			
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return UTF8.GetString(Results);
	}
}