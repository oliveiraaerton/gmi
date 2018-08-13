using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Data;
using System.Security.Cryptography;
using WebMatrix.Data;

public class Aplicativo:Dao
{	
	public static void ValidaUsuario(HttpResponseBase response, HttpRequestBase request, HttpContextBase context)
	{
		Usuario u = new Usuario();
		u.Email = request["email"];
		u.Senha = request["password"];

		bool x = Dao.validarUsuario(u);
		if(x)
		{
			context.Session["user"] = Dao.gerarChaveUsuario();
			context.Session.Timeout = 9999;
			response.Redirect("~/Views/Menus/Admin.cshtml");
		}
		else 
		{
			context.Session["user"] = String.Empty;
			response.Redirect("~/Views/Menus/Admin.cshtml");
		}		
	}

	public static void LogOut(HttpResponseBase response, HttpRequestBase request, HttpContextBase context)
	{
		context.Session["user"] = String.Empty;
		response.Redirect("~/Views/Menus/Admin.cshtml");
	}

	public static Empresa ListaEmpresa()
	{
		List<Empresa> empresas = Dao.listEmpresas();
		Empresa e = new Empresa();
		try {
			e = empresas[0];	
		} catch (Exception ex){
			Dao.mensagemErro.mensagem = ex.Message;
		}
		return e;	
	}

	public static bool existeEmpresa(string cnpj)
	{
		if(String.IsNullOrEmpty(Dao.getEmpresa(cnpj).Nome))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public static void AtualizarEmpresa(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Empresa e = new Empresa();
		e.Nome = request["inputNome"];
		e.Cnpj = request["Cnpj"];
		e.Endereco = request["inputEndereco"];
		e.Fone = request["inputFone"];
		e.Responsavel = request["inputResponsavel"];

		Dao.updateEmpresa(e);
		response.Redirect("~/Views/Menus/Admin.cshtml");
	}

	public static void SalvarEmpresa(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Empresa e = new Empresa();
		e.Nome = request["inputNome"];
		e.Cnpj = request["inputCnpj"];
		e.Endereco = request["inputEndereco"];
		e.Fone = request["inputFone"];
		e.Responsavel = request["inputResponsavel"];

		Dao.setEmpresa(e);	

		response.Redirect("~/Views/Menus/Admin.cshtml");
	}

	public static Registro ListaRegistro(string Cnpj)
	{
		Registro e = Dao.getRegistro(Cnpj);
		return e;
	}

	public static bool existeRegistro(string cnpj)
	{
		if(String.IsNullOrEmpty(Dao.getRegistro(cnpj).LicencaAtiva))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public static void AtualizarRegistro(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Registro e = new Registro();
		e.LicencaAtiva = request["inputLicenca"];
		e.Cnpj = request["Cnpj"];

		Dao.updateRegistro(e);
		response.Redirect("~/Views/Menus/Admin.cshtml");
	}

	public static void SalvarRegistro(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Registro e = new Registro();
		e.LicencaAtiva = request["inputLicenca"];
		e.Cnpj = request["inputCnpj"];

		Dao.setRegistro(e);	

		response.Redirect("~/Views/Menus/Admin.cshtml");
	}

	public static void SalvarFeed(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Feed e = new Feed();
		e.Ordem = int.Parse(request["inputOrdem"].ToString());
		e.Noticia = request["inputNoticia"];
		e.Titulo =  request["inputTitulo"];
		Dao.setFeed(e);	

		response.Redirect("~/Views/Menus/Feed.cshtml");
	}

	public static void AlterarFeed(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Feed e = new Feed();
		e.ID =  int.Parse(request["inputID"].ToString());
		e.Ordem = int.Parse(request["inputOrdem"].ToString());
		e.Ativo = int.Parse(request["inputAtivo"].ToString());
		e.Noticia = request["inputNoticia"];
		e.Titulo =  request["inputTitulo"];
		e.Velocidade = int.Parse(request["inputVelocidade"].ToString());
		Dao.updateFeed(e);	

		response.Redirect("~/Views/Menus/Feed.cshtml");
	}

	public static bool ValidaLicenca()
	{
		bool resultado = Dao.validaLicenca(ListaRegistro(ListaEmpresa().Cnpj).LicencaAtiva, DateTime.Now);
		return resultado;
	}

	public static List<Cartaz> listaCartaz()
	{
		return Dao.listCartazes();
	}

	public static List<Feed> listaFeeds(int ativo)
	{
		return Dao.listaFeeds(ativo);
	}

	public static IEnumerable<dynamic> listaFeed(int ativo)
	{
		return Dao.listFeeds(ativo);
	}

	public static IEnumerable<dynamic> listaFeed(string pesquisa)
	{
		return Dao.listFeeds(pesquisa);
	}


}