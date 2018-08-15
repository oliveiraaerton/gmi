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
	public static void Run(string chave, HttpResponseBase response)
	{
			if(String.IsNullOrEmpty(chave)) response.Redirect("~/Admin.cshtml");
	}

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

	public static void SalvarImagem(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Imagem e = new Imagem();
		e.Endereco = request["inputEndereco"];
		e.Largura = int.Parse(request["inputLargura"].ToString());
		e.Altura = int.Parse(request["inputAltura"].ToString());
		Dao.setImagem(e);	

		response.Redirect("~/Views/Menus/Imagem.cshtml");
	}

	public static void AlterarImagem(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Imagem e = new Imagem();
		e.ID =  int.Parse(request["inputID"].ToString());
		e.Endereco = request["inputEndereco"];
		e.Largura = int.Parse(request["inputLargura"].ToString());
		e.Altura = int.Parse(request["inputAltura"].ToString());
		Dao.updateImagem(e);	

		response.Redirect("~/Views/Menus/Imagem.cshtml");
	}

	public static Imagem BuscarImagem(string id)
	{
		Dao.inicializa();
		Imagem e = Dao.getImagem(int.Parse(id));
		return e;
	}

	public static void ExcluirImagem(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Imagem e = new Imagem();
		e.ID =  int.Parse(request["inputID"].ToString());
		Dao.deleteImagem(e);

		response.Redirect("~/Views/Menus/Imagem.cshtml");
	}

	public static List<Imagem> listaImagens()
	{
		return Dao.listaImagens();
	}

	public static IEnumerable<dynamic> listaImagem()
	{
		return Dao.listImagens();
	}

	public static IEnumerable<dynamic> listaImagem(string pesquisa)
	{
		return Dao.listImagens(pesquisa);
	}

	public static void SalvarFeed(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Feed e = new Feed();
		e.Ordem = int.Parse(request["inputOrdem"].ToString());
		e.Noticia = request["inputNoticia"];
		e.Titulo =  request["inputTitulo"];
		e.Velocidade = int.Parse(request["inputVelocidade"].ToString());
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

	public static int MaximoOrdemFeed()
	{
		Dao.inicializa();
		return Dao.maximoOrdemFeed();
	}

	public static Feed BuscarFeed(string id)
	{
		Dao.inicializa();
		Feed e = Dao.getFeed(int.Parse(id));
		return e;
	}

	public static void ExcluirFeed(HttpResponseBase response, HttpRequestBase request)
	{
		Dao.inicializa();
		Feed e = new Feed();
		e.ID =  int.Parse(request["inputID"].ToString());
		Dao.deleteFeed(e);

		response.Redirect("~/Views/Menus/Feed.cshtml");
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


	public static bool ValidaLicenca()
	{
		bool resultado = Dao.validaLicenca(ListaRegistro(ListaEmpresa().Cnpj).LicencaAtiva, DateTime.Now);
		return resultado;
	}

	public static List<Cartaz> listaCartaz()
	{
		return Dao.listCartazes();
	}

}