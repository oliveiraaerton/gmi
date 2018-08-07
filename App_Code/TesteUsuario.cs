using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Data;
using WebMatrix.Data;

/**
**	GMI - GERENCIADOR DE MIDIA INDOOR
**  TESTES
**/

public class TesteUsuario:Dao
{
	public static Usuario seedUsuario(string Nome, string Email, string Senha)
	{
		Usuario c = new Usuario();
		c.Nome = Nome;
		c.Email = Email;
		c.Senha = Senha;
		Dao.setUsuario(c);				
		int  ID = Dao.getLastID("Usuario");
		return Dao.getUsuario(ID);
	}

	public static void TesteSeed()
	{
		Teste.inicializaTeste();
		Usuario u = seedUsuario("Aerton","aerton@teste.com.br", "123456");
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);
	}

	public static void TesteCriptografar()
	{
		Teste.inicializaTeste();
		String chave = Dao.criptografarSenha("123456");
		Teste.respostaTeste();
		String senha = Dao.decriptografarSenha(chave);
		Teste.debug+="|Chave: " + chave + "| Senha:" + senha;
		Teste.respostaTeste();
	}

	public static void TesteChave()
	{
		Teste.inicializaTeste();
		String chave = Dao.gerarChaveUsuario();
		Teste.respostaTeste();
		Teste.debug+="|Chave: " + chave;
	}

	public static void TesteExisteNome()
	{
		Teste.inicializaTeste();
		Usuario u = seedUsuario(String.Empty, "aerton@teste.com.br", "123456");
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteExisteEmail()
	{
		Teste.inicializaTeste();
		Usuario u = seedUsuario("aerton@teste.com.br", String.Empty, "123456");
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteExisteSenha()
	{
		Teste.inicializaTeste();
		Usuario u = seedUsuario("aerton@teste.com.br", "aerton@teste.com.br", String.Empty);
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteExisteEmailUnico()
	{
		Teste.inicializaTeste();
		Usuario u1 = seedUsuario("Usuario u1", "aerton@teste.com.br", "12345678");
		Teste.respostaTeste();
		Usuario u2 = seedUsuario("Usuario u2", "aerton@teste.com.br", "123456");
		Teste.respostaTeste();
		Teste.debug+="Nome U1: " + u1.Nome + "| Email: " + u1.Email ;
		Teste.debug+="| Nome U2: " + u2.Nome + "| Email: " + u2.Email ;
		Teste.debug+="|" + validaUnique("EMAIL", "aerton@teste.com.br", "Usuario");
		Teste.respostaTeste();
		Dao.deleteUsuario(u1);		
		Dao.deleteUsuario(u2);		
	}

	public static void TesteListaUsuarios()
	{
		Usuario u1 = seedUsuario("Usuario u1", "aerton1@teste.com.br", "12345678");
		Usuario u2 = seedUsuario("Usuario u2", "aerton2@teste.com.br", "123456");
		Usuario u3 = seedUsuario("Usuario u3", "aerton3@teste.com.br", "123456");
		Teste.inicializaTeste();
		List<Usuario> us = Dao.listUsuarios();
		Teste.respostaTeste();
		Dao.deleteUsuario(u1);		
		Dao.deleteUsuario(u2);		
		Dao.deleteUsuario(u3);		
	}

	public static void TesteAtualizaUsuarioEmailVazioENomeVazio()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		u.Email = String.Empty;
		Teste.inicializaTeste();
		Dao.updateUsuario(u);
		Teste.respostaTeste();
		u.Nome = String.Empty;
		Dao.updateUsuario(u);
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteAtualizaEmailUnico()
	{
		Usuario u1 = seedUsuario("Usuario u1", "aerton@teste.com.br", "12345678");
		Usuario u2 = seedUsuario("Usuario u2", "aerton2@teste.com.br", "123456");
		u2.Email="aerton@teste.com.br";
		Teste.inicializaTeste();
		Dao.updateUsuario(u2);
		Teste.respostaTeste();
		Teste.debug+="Nome U1: " + u1.Nome + "| Email: " + u1.Email ;
		Teste.debug+="| Nome U2: " + u2.Nome + "| Email: " + u2.Email ;
		Teste.debug+="|" + validaUnique("EMAIL", "aerton@teste.com.br", "Usuario");
		Dao.deleteUsuario(u1);		
		Dao.deleteUsuario(u2);		
	}	

	public static void TesteAtualizaUsuarioEmailValido()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		u.Email = "aerton.gestao@gmail.com";
		Teste.inicializaTeste();
		Dao.updateUsuario(u);
		Teste.respostaTeste();
		u.Nome =" Aerton";
		Dao.updateUsuario(u);
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteAtualizarSenhaInvalido()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		u.Email = String.Empty;
		Teste.inicializaTeste();
		Dao.atualizarSenha(u);
		Teste.respostaTeste();
		u.Senha = String.Empty;
		Dao.atualizarSenha(u);
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteAtualizarSenhaValido()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		Teste.inicializaTeste();
		Dao.atualizarSenha(u);
		Teste.respostaTeste();
		u.Senha = "1234567";
		Dao.atualizarSenha(u);
		Teste.respostaTeste();
		Teste.debug+="|Nome: " + u.Nome + "| Email: " + u.Email ;
		Dao.deleteUsuario(u);		
	}

	public static void TesteValidarUsuarioEmailErrado()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		Teste.inicializaTeste();
		Usuario e = new Usuario();
		e.Email = "aerton2@teste.com.br";
		e.Senha = "123456";
		bool x = Dao.validarUsuario(e);
		Teste.respostaTeste();
		Teste.debug+="|Validação = " + x ;
		Dao.deleteUsuario(u);
	}

	public static void TesteValidarUsuarioSenhaErrada()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		Teste.inicializaTeste();
		Usuario e = new Usuario();
		e.Email = "aerton@teste.com.br";
		e.Senha = "1234567";
		bool x = Dao.validarUsuario(e);
		Teste.respostaTeste();
		Teste.debug+="|Validação = " + x ;
		Dao.deleteUsuario(u);
	}

	public static void TesteValidarUsuarioValido()
	{
		Usuario u = seedUsuario("aerton", "aerton@teste.com.br", "123456");
		Teste.inicializaTeste();
		Usuario e = new Usuario();
		e.Email = "aerton@teste.com.br";
		e.Senha = "123456";
		bool x = Dao.validarUsuario(e);
		Teste.respostaTeste();
		Teste.debug+="|Validação = " + x ;
		Dao.deleteUsuario(u);
	}
}