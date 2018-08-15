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

public class Teste:Dao
{
	public static bool TesteOk = true;
	public static int NumeroErros = 0;
	public static int NumeroAcertos = 0;
	public static string debug;

	//////////  TESTE CRUD Empresa

	public static void inicializaTeste()
	{
		NumeroErros = 0;
		NumeroAcertos = 0;
		TesteOk = true;
		debug = String.Empty;
		Dao.inicializa();
	}

	public static void respostaTeste()
	{
		if(!String.IsNullOrEmpty(Dao.mensagemErro.mensagem))
		{
			NumeroErros+=1;
			TesteOk = false;
			debug+="|"+Dao.mensagemErro.mensagem + "|mensagemErro|";
		}

		if(!String.IsNullOrEmpty(Dao.mensagemValidacao.mensagem))
		{
			NumeroErros+=1;
			TesteOk = false;
			debug+="|"+Dao.mensagemValidacao.mensagem + "|mensagemValidacao|";
		}

		if(!String.IsNullOrEmpty(Dao.mensagemSucesso.mensagem))
		{
			NumeroAcertos+=1;
			TesteOk = true;
			debug+="|"+Dao.mensagemSucesso.mensagem + "|mensagemSucesso|";
		}
	}


////////////////// TESTE CARTAZ

	public static Cartaz seedCartaz(string Cabecalho, string Texto)
	{
		Cartaz c = new Cartaz();
		c.Cabecalho = Cabecalho;
		c.Texto = Texto;
		Dao.setCartaz(c);		
		int  ID = Dao.getLastID("Cartaz");
		return getCartaz(ID);
	}

	public static void PesquisaCartazComIDErrado()
	{

		//Cadastrar uma empresa válida
		Cartaz c = seedCartaz("SUPERCANADA", "13.014.206.0003-84");

		//TesteCNPJErrado
		int ID = 99;

		inicializaTeste();
		Cartaz e = getCartaz(ID);
		respostaTeste();
		Dao.deleteCartaz(getCartaz(Dao.getLastID("Cartaz")));
	}

	public static void PesquisaCartazComIDCerto()
	{

		//Cadastrar uma empresa válida
		Cartaz c = seedCartaz("SUPERCANADA", "13.014.206.0003-84");
//		debug+=Dao.mensagemSucesso.mensagem + "|71|";

		//TesteCNPJErrado
		int  ID = c.ID;
//		debug+=Dao.mensagemSucesso.mensagem + "|75|";

		inicializaTeste();
		Cartaz e = getCartaz(ID);
		respostaTeste();
		Dao.deleteCartaz(c);
	}		

	public static void PesquisaListaCartazes()
	{

		Cartaz c1 = seedCartaz("SUPERCANADA", "cartaz teste 1");
		Cartaz c2 = seedCartaz("SUPERCANADA", "cartaz teste 2");
		Cartaz c3 = seedCartaz("SUPERCANADA", "cartaz teste 3");

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<Cartaz> cartazes = Dao.listCartazes();
		respostaTeste();				
		Dao.deleteCartaz(c1);
		Dao.deleteCartaz(c2);
		Dao.deleteCartaz(c3);
	}		

	public static void PesquisaListaNenhumCartazes()
	{
		//Cadastrar uma empresa válida
		List<Cartaz> cartazes = Dao.listCartazes();
		foreach(Cartaz c in cartazes)
		{
			Dao.deleteCartaz(c);
		}

		inicializaTeste();
		cartazes = Dao.listCartazes();
		respostaTeste();				
	}		

	public static void SetaCartazInvalido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Cartaz c = new Cartaz();
		Dao.setCartaz(c);
		respostaTeste();				
	}		

	public static void SetaCartazValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Cartaz c = seedCartaz("SUPERCANADA", "cartaz SetaCartazValido");
		respostaTeste();	
		Dao.deleteCartaz(c);			
	}		


	public static void SetaCartazJaSalvo()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz SetaCartazJaSalvo");
		inicializaTeste();
		Dao.setCartaz(c);
		respostaTeste();	
		Dao.deleteCartaz(c);			
	}		


// update
// --------------------------------

	public static void AtualizaCartazInvalido()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz AtualizaCartazInvalido");
		c.Cabecalho = String.Empty;
		c.Texto = String.Empty;
		inicializaTeste();
		Dao.updateCartaz(c);
		respostaTeste();	
		Dao.deleteCartaz(c);			
	}		

	public static void AtualizaCartazValido()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz AtualizaCartazValido");
		c.Cabecalho = "Altera Cabecalho";
		c.Texto = "Altera Texto";
		inicializaTeste();
		Dao.updateCartaz(c);
		respostaTeste();
		Dao.deleteCartaz(c);
	}		


	public static void AtualizaCartazInexistente()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz AtualizaCartazInexistente");
		int id = c.ID;
		c.ID = Dao.getLastID("Cartaz")+1;
		c.Cabecalho = String.Empty;
		c.Texto = String.Empty;
		inicializaTeste();
		Dao.updateCartaz(c);
		respostaTeste();	
		Dao.deleteCartaz(getCartaz(id));
	}		


// delete
// --------------------------------

	public static void ApagaCartazInexistente()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz ApagaCartazInexistente");
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteCartaz(c);
		respostaTeste();			
		Dao.deleteCartaz(getCartaz(id));	
	}		

	public static void ApagaCartazExistente()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz ApagaCartazExistente");
		inicializaTeste();
		Dao.deleteCartaz(c);
		respostaTeste();				
	}		

	public static void ApagaCartazComProgramacao()
	{
		
		Cartaz c = seedCartaz("SUPERCANADA", "cartaz ApagaCartazComProgramacao");
		ItensProgramacao i = new ItensProgramacao();
		i.TipoMidia = "CARTAZ";
		i.Ordem = 1;
		i.CartazID = c.ID;
		i. TempoExibicao = 10000;
		i.ProgramacaoId = 1;
		Dao.setItensProgramacao(i);

		inicializaTeste();
		Dao.deleteCartaz(c);
		respostaTeste();			
		Dao.deleteItensProgramacao(getItensProgramacao(getLastID("ItemProgramacao")));
		Dao.deleteCartaz(c);
	}		

	public static void ApagaCartazSemProgramacao()
	{
		//Cadastrar uma empresa válida

		Cartaz c = seedCartaz("SUPERCANADA", "cartaz ApagaCartazSemProgramacao");
		inicializaTeste();
		Dao.deleteCartaz(c);
		respostaTeste();	
	}		


	/////// Teste Empresa

	public static Empresa seedEmpresa(string Nome, string Cnpj)
	{
		Empresa c = new Empresa();
		c.Nome = Nome;
		c.Cnpj = Cnpj;
		Dao.setEmpresa(c);		
		return Dao.getEmpresa(Cnpj);
	}

	public static void PesquisaEmpresaComCNPJErrado()
	{

		//Cadastrar uma empresa válida
		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");

		//TesteCNPJErrado
		string CNPJ = "12014";

		inicializaTeste();
		Empresa e = getEmpresa(CNPJ);
		respostaTeste();
		Dao.deleteEmpresa(c);
	}

	public static void PesquisaEmpresaComCNPJCerto()
	{

		//Cadastrar uma empresa válida
		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");

		inicializaTeste();
		Empresa e = getEmpresa(c.Cnpj);

		respostaTeste();

		Dao.deleteEmpresa(c);

	}		

	public static void PesquisaListaEmpresas()
	{

		Empresa c1 = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		Empresa c2 = seedEmpresa("SUPERCANADA","13.014.206/0002-01");
		Empresa c3 = seedEmpresa("SUPERCANADA","13.014.206/0003-84");

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<Empresa> empresas = Dao.listEmpresas();

		respostaTeste();				
		Dao.deleteEmpresa(c1);
		Dao.deleteEmpresa(c2);
		Dao.deleteEmpresa(c3);
	}		

	public static void PesquisaListaNenhumaEmpresas()
	{
		//Cadastrar uma empresa válida
		List<Empresa> empresas = Dao.listEmpresas();
		foreach(Empresa c in empresas)
		{
			Dao.deleteEmpresa(c);
		}

		inicializaTeste();

		empresas = Dao.listEmpresas();
		respostaTeste();				
	}		

	public static void SetaEmpresaInvalido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();

		Empresa c = seedEmpresa(String.Empty,String.Empty);

		respostaTeste();				
	}		

	public static void SetaEmpresaValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();

		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		
		respostaTeste();	
		Dao.deleteEmpresa(c);			
	}		

	public static void SetaEmpresaJaSalva()
	{
		//Cadastrar uma empresa válida


		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		
		inicializaTeste();
		Dao.setEmpresa(c);

		respostaTeste();
		Dao.deleteEmpresa(c);		
	}		


// update
// --------------------------------

	public static void AtualizaEmpresaInvalida()
	{
		//Cadastrar uma empresa válida


		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		c.Nome = String.Empty;
		inicializaTeste();
		Dao.updateEmpresa(c);
		respostaTeste();	
		Dao.deleteEmpresa(c);			
	}		

	public static void AtualizaEmpresaValida()
	{
		//Cadastrar uma empresa válida


		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		c.Nome = "String.Empty";
		inicializaTeste();
		Dao.updateEmpresa(c);
		respostaTeste();
		Dao.deleteEmpresa(c);
	}		

	public static void AtualizaEmpresaInexistente()
	{
		//Cadastrar uma empresa válida

		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		string cnpj = c.Cnpj;
		c.Nome = String.Empty;
		c.Cnpj = String.Empty;
		inicializaTeste();
		Dao.updateEmpresa(c);
		respostaTeste();	
		Dao.deleteEmpresa(getEmpresa(cnpj));			
	}		

// delete
// --------------------------------

	public static void ApagaEmpresaInexistente()
	{
		//Cadastrar uma empresa válida

		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		string cnpj = c.Cnpj;
		c.Cnpj=String.Empty;
		inicializaTeste();
		Dao.deleteEmpresa(c);
		respostaTeste();	
		Dao.deleteEmpresa(getEmpresa(cnpj));			
	}		

	public static void ApagaEmpresaExistente()
	{
		//Cadastrar uma empresa válida

		Empresa c = seedEmpresa("SUPERCANADA","13.014.206/0001-12");
		inicializaTeste();
		Dao.deleteEmpresa(c);
		respostaTeste();				
	}		



////////////////// TESTE IMAGEM

	public static Imagem seedImagem(string Endereco, int Altura, int Largura)
	{
		Imagem c = new Imagem();
		c.Endereco = Endereco;
		c.Altura = Altura;
		c.Largura = Largura;
		Dao.setImagem(c);		
		int  ID = Dao.getLastID("Imagem");
		return getImagem(ID);
	}

	public static void PesquisaImagemComIDErrado()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);

		//TesteCNPJErrado
		int ID = 99;

		inicializaTeste();
		Imagem e = getImagem(ID);
		respostaTeste();
		Dao.deleteImagem(getImagem(c.ID));
	}

	public static void PesquisaImagemComIDCerto()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
//		debug+=Dao.mensagemSucesso.mensagem + "|71|";

		//TesteCNPJErrado
		int  ID = c.ID;
//		debug+=Dao.mensagemSucesso.mensagem + "|75|";

		inicializaTeste();
		Imagem e = getImagem(ID);
		respostaTeste();
		Dao.deleteImagem(c);
	}		

	public static void PesquisaListaImagens()
	{
		Imagem c1 = seedImagem("C:\\PICTURES\\TESTE1.JPG", 100, 200);
		Imagem c2 = seedImagem("C:\\PICTURES\\TESTE2.JPG", 100, 200);
		Imagem c3 = seedImagem("C:\\PICTURES\\TESTE3.JPG", 100, 200);

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<Imagem> imagens = Dao.listaImagens();
		respostaTeste();				
		Dao.deleteImagem(c1);
		Dao.deleteImagem(c2);
		Dao.deleteImagem(c3);
	}		

	public static void PesquisaListaNenhumImagens()
	{
		//Cadastrar uma empresa válida
		List<Imagem> imagens = Dao.listaImagens();
		foreach(Imagem c in imagens)
		{
			Dao.deleteImagem(c);
		}

		inicializaTeste();
		imagens = Dao.listaImagens();
		respostaTeste();				
	}		

	public static void SetaImagemInvalido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Imagem c = new Imagem();
		Dao.setImagem(c);
		respostaTeste();				
	}		

	public static void SetaImagemValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);

		respostaTeste();	
		Dao.deleteImagem(c);			
	}		


	public static void SetaImagemJaSalvo()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);

		inicializaTeste();
		Dao.setImagem(c);
		respostaTeste();	
		Dao.deleteImagem(c);			
	}		


// update
// --------------------------------

	public static void AtualizaImagemInvalido()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
		c.Endereco = String.Empty;
		c.Altura = 0;
		c.Largura = 0;
		inicializaTeste();
		Dao.updateImagem(c);
		respostaTeste();	
		Dao.deleteImagem(c);			
	}		

	public static void AtualizaImagemValido()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
		c.Endereco = "c:\\pictures\\outro.jpg";
		c.Largura = 300;
		c.Altura = 100;
		inicializaTeste();
		Dao.updateImagem(c);
		respostaTeste();
		Dao.deleteImagem(c);
	}		


	public static void AtualizaImagemInexistente()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
		int id = c.ID;
		c.ID = Dao.getLastID("Imagem")+1;
		c.Endereco = String.Empty;
		c.Altura = 0;
		c.Largura = 0;
		inicializaTeste();
		Dao.updateImagem(c);
		respostaTeste();	
		Dao.deleteImagem(getImagem(id));			
	}		


// delete
// --------------------------------

	public static void ApagaImagemInexistente()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteImagem(c);
		respostaTeste();				
		Dao.deleteImagem(getImagem(id));
	}		

	public static void ApagaImagemExistente()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
		inicializaTeste();
		Dao.deleteImagem(c);
		respostaTeste();				
	}		

	public static void ApagaImagemComProgramacao()
	{
		
		Imagem c = seedImagem("C:\\PICTURES\\DELETETESTE.JPG", 100, 200);
		ItensProgramacao i = new ItensProgramacao();
		i.TipoMidia = "IMAGEM";
		i.Ordem = 1;
		i.ImagemID = c.ID;
		i. TempoExibicao = 10000;
		i.ProgramacaoId = 1;
		Dao.setItensProgramacao(i);

		inicializaTeste();
		Dao.deleteImagem(c);
		respostaTeste();	
		Dao.deleteItensProgramacao(getItensProgramacao(getLastID("ItemProgramacao")));	
		Dao.deleteImagem(c);
	}		

	public static void ApagaImagemSemProgramacao()
	{
		//Cadastrar uma empresa válida
		Imagem c = seedImagem("C:\\PICTURES\\TESTE.JPG", 100, 200);
		inicializaTeste();
		Dao.deleteImagem(c);
		respostaTeste();	
	}		

////////////////// TESTE TABELAS

	public static Tabelas seedTabelas(string Cabecalho)
	{
		Tabelas c = new Tabelas();
		c.Cabecalho = Cabecalho;
		c.FonteCabecalho = "Arial";
		c.TamanhoCabecalho = 100;
		Dao.setTabelas(c);		
		int  ID = Dao.getLastID("Tabelas");
		return getTabelas(ID);
	}

	public static void PesquisaTabelasComIDErrado()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA PESQUISA");

		//TesteCNPJErrado
		int ID = 99;

		inicializaTeste();
		Tabelas e = getTabelas(ID);
		respostaTeste();
		Dao.deleteTabelas(getTabelas(Dao.getLastID("Tabelas")));
	}

	public static void PesquisaTabelasComIDCerto()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA PESQUISA");
//		debug+=Dao.mensagemSucesso.mensagem + "|71|";

		//TesteCNPJErrado
		int  ID = c.ID;
//		debug+=Dao.mensagemSucesso.mensagem + "|75|";

		inicializaTeste();
		Tabelas e = getTabelas(ID);
		respostaTeste();
		Dao.deleteTabelas(c);
	}		

	public static void PesquisaListaTabelas()
	{
		Tabelas c1 = seedTabelas("TABELA 1");
		Tabelas c2 = seedTabelas("TABELA 2");
		Tabelas c3 = seedTabelas("TABELA 3");

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<Tabelas> tabelas = Dao.listTabelas();
		respostaTeste();				
		Dao.deleteTabelas(c1);
		Dao.deleteTabelas(c2);
		Dao.deleteTabelas(c3);
	}		

	public static void PesquisaListaNenhumTabelas()
	{
		//Cadastrar uma empresa válida
		List<Tabelas> tabelas = Dao.listTabelas();
		foreach(Tabelas c in tabelas)
		{
			Dao.deleteTabelas(c);
		}

		inicializaTeste();
		tabelas = Dao.listTabelas();
		respostaTeste();				
	}		

	public static void SetaTabelasInvalido()
	{
		//Cadastrar uma empresa válida
		Tabelas c = new Tabelas();
		inicializaTeste();
		Dao.setTabelas(c);
		respostaTeste();				
		Dao.deleteTabelas(c);
	}		

	public static void SetaTabelasValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Tabelas c = seedTabelas("TABELA VALIDA");
		respostaTeste();	
		Dao.deleteTabelas(c);			
	}		


	public static void SetaTabelasJaSalvo()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA JA SALVA");
		inicializaTeste();
		Dao.setTabelas(c);
		respostaTeste();	
		Dao.deleteTabelas(c);			
	}		


// update
// --------------------------------

	public static void AtualizaTabelasInvalido()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA INVALIDA");
		c.Cabecalho = String.Empty;
		inicializaTeste();
		Dao.updateTabelas(c);
		respostaTeste();	
		Dao.deleteTabelas(c);			
	}		

	public static void AtualizaTabelasValido()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA VALIDA");
		c.Cabecalho = "Altera Cabecalho";
		inicializaTeste();
		Dao.updateTabelas(c);
		respostaTeste();
		Dao.deleteTabelas(c);
	}		


	public static void AtualizaTabelasInexistente()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA INEXISTENTE");
		int id = c.ID;
		c.ID = Dao.getLastID("Tabelas")+1;
		c.Cabecalho = String.Empty;
		inicializaTeste();
		Dao.updateTabelas(c);
		respostaTeste();	
		Dao.deleteTabelas(getTabelas(id));
	}		


// delete
// --------------------------------

	public static void ApagaTabelasInexistente()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA DELETE INEXISTENTE");
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteTabelas(c);
		respostaTeste();				
		Dao.deleteTabelas(getTabelas(id));
	}		

	public static void ApagaTabelasExistente()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA EXISTENTE");
		inicializaTeste();
		Dao.deleteTabelas(c);
		respostaTeste();				
	}		

	public static void ApagaTabelasComProgramacao()
	{
		
		Tabelas c = seedTabelas("TABELA COM PROGRAMACAO");
		ItensProgramacao i = new ItensProgramacao();
		i.TipoMidia = "TABELA";
		i.Ordem = 1;
		i.TabelaID = c.ID;
		i. TempoExibicao = 10000;
		i.ProgramacaoId = 1;
		Dao.setItensProgramacao(i);

		inicializaTeste();
		Dao.deleteTabelas(c);
		respostaTeste();			
		Dao.deleteItensProgramacao(getItensProgramacao(getLastID("ItemProgramacao")));
		Dao.deleteTabelas(c);
	}		

	public static void ApagaTabelasSemProgramacao()
	{
		//Cadastrar uma empresa válida
		Tabelas c = seedTabelas("TABELA SEM PROGRAMACAO");
		inicializaTeste();
		Dao.deleteTabelas(c);
		respostaTeste();	
	}		

	public static void ListaTabelasEItensSemTabela()
	{
		Tabelas c = new Tabelas();
		inicializaTeste();
		List<Tabelas> tabelas = Dao.listTabelasEItens();
		respostaTeste();
	}

	public static void ListaTabelasEItensComTabelaInvalida()
	{
		Tabelas c = seedTabelas("TABELA E ITENS TABELA INVALIDA");
		int id = c.ID;
		c.ID=0;
		inicializaTeste();
		List<Tabelas> tabelas = Dao.listTabelasEItens();
		respostaTeste();
		Dao.deleteTabelas(getTabelas(id));
	}

	public static void ListaTabelasEItensComTabelaSemItens()
	{
		Tabelas c = seedTabelas("TABELA E ITENS SEM ITENS");
		inicializaTeste();
		List<Tabelas> tabelas = Dao.listTabelasEItens();
		respostaTeste();
		Dao.deleteTabelas(c);
	}

	public static void ListaTabelasEItensComTabelaEItens()
	{
		Tabelas c = seedTabelas("TABELA E ITENS COM ITENS");
		ItensTabela i1 = seedItensTabelas("ITEM DE TABELA PESQUISA 1", 5.50m, c.ID);
		i1.TabelaID = c.ID;

		ItensTabela i2 = seedItensTabelas("ITEM DE TABELA PESQUISA 2", 5.50m, c.ID);
		i2.TabelaID = c.ID;

		inicializaTeste();
		List<Tabelas> tabelas = Dao.listTabelasEItens();
		respostaTeste();
		Dao.deleteTabelas(c);
	}

////////////////// TESTE ITENSTABELAS

	public static ItensTabela seedItensTabelas(string Produto, decimal Valor, int TabelaID)
	{
		ItensTabela c = new ItensTabela();
		c.TabelaID = TabelaID;
		c.Produto = Produto;
		c.FonteProduto = "Arial";
		c.TamanhoProduto = 20;
		c.Valor = Valor;
		c.FonteValor = "Arial";
		c.TamanhoValor = 20;
		Dao.setItensTabelas(c);		
		int  ID = Dao.getLastID("ItensTabela");
		return getItensTabelas(ID);
	}

	public static ItensTabela seedItensTabelas(string Produto, decimal Valor)
	{
		Tabelas t = seedTabelas("TABELA TESTE");

		ItensTabela c = new ItensTabela();
		c.TabelaID = t.ID;
		c.Produto = Produto;
		c.FonteProduto = "Arial";
		c.TamanhoProduto = 20;
		c.Valor = Valor;
		c.FonteValor = "Arial";
		c.TamanhoValor = 20;
		Dao.setItensTabelas(c);		
		int  ID = Dao.getLastID("ItensTabela");
		return getItensTabelas(ID);
	}

	public static void PesquisaItensTabelasComIDErrado()
	{
		//Cadastrar uma empresa válida
		ItensTabela c = seedItensTabelas("ITEM DE TABELA PESQUISA", 5.50m);

		//TesteCNPJErrado
		int ID = 99;

		inicializaTeste();
		ItensTabela e = getItensTabelas(ID);
		respostaTeste();
		Dao.deleteItensTabelas(getItensTabelas(Dao.getLastID("ItensTabela")));
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}

	public static void PesquisaItensTabelasComIDCerto()
	{
		//Cadastrar uma empresa válida
		ItensTabela c = seedItensTabelas("ITEM DE TABELA PESQUISA", 5.50m);
//		debug+=Dao.mensagemSucesso.mensagem + "|71|";

		//TesteCNPJErrado
		int  ID = c.ID;
//		debug+=Dao.mensagemSucesso.mensagem + "|75|";

		inicializaTeste();
		ItensTabela e = getItensTabelas(ID);
		respostaTeste();
		Dao.deleteItensTabelas(e);
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		

	public static void PesquisaListaItensTabelas()
	{

		ItensTabela c1 = seedItensTabelas("ITEM DE TABELA PESQUISA 1", 5.50m);
		ItensTabela c2 = seedItensTabelas("ITEM DE TABELA PESQUISA 2", 6.50m);
		ItensTabela c3 = seedItensTabelas("ITEM DE TABELA PESQUISA 3", 7.50m);

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<ItensTabela> itenstabelas = Dao.listItensTabelas();
		respostaTeste();		

		Dao.deleteItensTabelas(c1);
		Dao.deleteTabelas(getTabelas(c1.TabelaID));
		Dao.deleteItensTabelas(c2);
		Dao.deleteTabelas(getTabelas(c2.TabelaID));
		Dao.deleteItensTabelas(c3);
		Dao.deleteTabelas(getTabelas(c3.TabelaID));

	}		

	public static void PesquisaListaNenhumItensTabelas()
	{
		//Cadastrar uma empresa válida
		List<ItensTabela> itenstabelas = Dao.listItensTabelas();
		foreach(ItensTabela c in itenstabelas)
		{
			Dao.deleteItensTabelas(c);
			Dao.deleteTabelas(getTabelas(c.TabelaID));
		}

		inicializaTeste();
		itenstabelas = Dao.listItensTabelas();
		respostaTeste();				
	}		

	public static void SetaItensTabelasInvalido()
	{
		//Cadastrar uma empresa válida
		ItensTabela c = seedItensTabelas("ITEM DE TABELA CREATE", 5.50m);
		int tabelaid = c.TabelaID;
		int id = c.ID;
		inicializaTeste();
		c.TabelaID = 0;
		Dao.setItensTabelas(c);
		respostaTeste();				
		Dao.deleteItensTabelas(getItensTabelas(id));
		Dao.deleteTabelas(getTabelas(tabelaid));

	}		

	public static void SetaItensTabelasValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		ItensTabela c = seedItensTabelas("ITEM DE TABELA CREATE", 5.50m);
		respostaTeste();				
		Dao.deleteItensTabelas(c);
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		


	public static void SetaItensTabelasJaSalvo()
	{
		//Cadastrar uma empresa válida
		ItensTabela c = seedItensTabelas("ITEM DE TABELA CREATE", 5.50m);
		inicializaTeste();
		Dao.setItensTabelas(c);
		respostaTeste();		
		Dao.deleteItensTabelas(c);		
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		


// update
// --------------------------------

	public static void AtualizaItensTabelasInvalido()
	{
		//Cadastrar uma empresa válida

		ItensTabela c = seedItensTabelas("ITEM DE TABELA UPDATE", 5.50m);
		int tabelaid = c.TabelaID;
		int id = c.ID;
		c.TabelaID = 0;
		inicializaTeste();
		Dao.updateItensTabelas(c);
		respostaTeste();				
		Dao.deleteItensTabelas(getItensTabelas(id));
		Dao.deleteTabelas(getTabelas(tabelaid));
	}		

	public static void AtualizaItensTabelasValido()
	{
		//Cadastrar uma empresa válida

		ItensTabela c = seedItensTabelas("ITEM DE TABELA UPDATE", 5.50m);
		c.Produto = "Outro Produto";
		inicializaTeste();
		Dao.updateItensTabelas(c);
		respostaTeste();
		Dao.deleteItensTabelas(c);
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		


	public static void AtualizaItensTabelasInexistente()
	{
		//Cadastrar uma empresa válida

		ItensTabela c = seedItensTabelas("ITEM DE TABELA UPDATE", 5.50m);
		int id = c.ID;
		c.ID = Dao.getLastID("ItensTabela")+1;
		c.Produto = String.Empty;
		inicializaTeste();
		Dao.updateItensTabelas(c);
		respostaTeste();				
		Dao.deleteItensTabelas(getItensTabelas(id));
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		


// delete
// --------------------------------

	public static void ApagaItensTabelasInexistente()
	{
		//Cadastrar uma empresa válida

		ItensTabela c = seedItensTabelas("ITEM DE TABELA DELETE", 5.50m);
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteItensTabelas(c);
		respostaTeste();				
		Dao.deleteItensTabelas(getItensTabelas(id));
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		

	public static void ApagaItensTabelasExistente()
	{
		//Cadastrar uma empresa válida

		ItensTabela c = seedItensTabelas("ITEM DE TABELA DELETE", 5.50m);
		inicializaTeste();
		Dao.deleteItensTabelas(c);
		respostaTeste();				
		Dao.deleteTabelas(getTabelas(c.TabelaID));
	}		

	public static void ListaItensPorTabelaTabelaInvalida()
	{

		ItensTabela c = seedItensTabelas("LISTA ITENS TABELA INVALIDA", 5.50m);
		int tabelaid = c.TabelaID;
		c.TabelaID = 0;
		inicializaTeste();
		List<ItensTabela> itenstabelas = listItensTabelasPorTabela(c.TabelaID);
		respostaTeste();				
		Dao.deleteTabelas(getTabelas(tabelaid));
	}

	public static void ListaItensPorTabelaTabelaSemItens()
	{

		inicializaTeste();
		Tabelas c = seedTabelas("LISTA ITENS POR TABELA SEM ITENS");
		List<ItensTabela> itenstabelas = listItensTabelasPorTabela(c.ID);
		respostaTeste();				
		Dao.deleteTabelas(getTabelas(c.ID));
	}

	public static void ListaItensPorTabelaTabelaComItens()
	{
		Tabelas t = seedTabelas("LISTA ITENS POR TABELA SEM ITENS");
		ItensTabela c1 = seedItensTabelas("LISTA ITENS POR TABELA SEM ITENS", 5.50m, t.ID);
		ItensTabela c2 = seedItensTabelas("LISTA ITENS POR TABELA SEM ITENS", 5.50m, t.ID);
		inicializaTeste();
		List<ItensTabela> itenstabelas = listItensTabelasPorTabela(t.ID);
		respostaTeste();				
		Dao.deleteTabelas(getTabelas(t.ID));
		
	}


	//////////// TESTE REGISTRO

	public static Registro seedRegistro(string LicencaAtiva, string Cnpj)
	{
		Registro c = new Registro();
		c.LicencaAtiva = LicencaAtiva;
		c.Cnpj = Cnpj;
		Dao.setRegistro(c);		
		return Dao.getRegistro(Cnpj);
	}

	public static void TestaValidaLicenca()
	{
		DateTime validade = DateTime.Now;
		string licenca = "SUPERCANADAVALIDO";
		inicializaTeste();
		bool validacao = Dao.validaLicenca(licenca, validade);
		respostaTeste();
	}

	public static void PesquisaRegistroComCNPJErrado()
	{

		//Cadastrar uma empresa válida
		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");

		//TesteCNPJErrado
		string CNPJ = "12014";

		inicializaTeste();
		Registro e = getRegistro(CNPJ);
		respostaTeste();
		Dao.deleteRegistro(c);
	}

	public static void PesquisaRegistroComCNPJCerto()
	{

		//Cadastrar uma empresa válida
		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		string CNPJ = "13014206000112";

		inicializaTeste();
		Registro e = getRegistro(CNPJ);

		respostaTeste();

		Dao.deleteRegistro(c);

	}		

	public static void SetaRegistroInvalido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();

		Registro c = seedRegistro(String.Empty,String.Empty);

		respostaTeste();				

	}		

	public static void SetaRegistroValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();

		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		
		respostaTeste();				
		Dao.deleteRegistro(c);
	}		

	public static void SetaRegistroValidoExpirado()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();

		Registro c = seedRegistro("SUPERCANADAINVALIDO","13.014.206/0001-12");
		
		respostaTeste();				
	}		

	public static void SetaRegistroJaSalva()
	{
		//Cadastrar uma empresa válida


		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		
		inicializaTeste();
		Dao.setRegistro(c);

		respostaTeste();
		Dao.deleteRegistro(c);
				
	}		


// update
// --------------------------------

	public static void AtualizaRegistroInvalida()
	{
		//Cadastrar uma empresa válida


		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		c.LicencaAtiva = String.Empty;
		inicializaTeste();
		Dao.updateRegistro(c);
		respostaTeste();
		Dao.deleteRegistro(c);				
	}		

	public static void AtualizaRegistroValida()
	{
		//Cadastrar uma empresa válida

		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		c.LicencaAtiva = "SUPERCANADAOUTRAVALIDADE";
		inicializaTeste();
		Dao.updateRegistro(c);
		respostaTeste();
		Dao.deleteRegistro(c);				
	}		

	public static void AtualizaRegistroExpirado()
	{
		//Cadastrar uma empresa válida

		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		c.LicencaAtiva = "SUPERCANADAINVALIDO";
		inicializaTeste();
		Dao.updateRegistro(c);
		respostaTeste();
		Dao.deleteRegistro(c);				
	}		

	public static void AtualizaRegistroInexistente()
	{
		//Cadastrar uma empresa válida

		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		c.Cnpj = String.Empty;
		inicializaTeste();
		Dao.updateRegistro(c);
		respostaTeste();				
		Dao.deleteRegistro(c);				
	}		

// delete
// --------------------------------

	public static void ApagaRegistroInexistente()
	{
		//Cadastrar uma empresa válida

		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		c.Cnpj=String.Empty;
		inicializaTeste();
		Dao.deleteRegistro(c);
		respostaTeste();				
	}		

	public static void ApagaRegistroExistente()
	{
		//Cadastrar uma empresa válida

		Registro c = seedRegistro("SUPERCANADAVALIDO","13.014.206/0001-12");
		inicializaTeste();
		Dao.deleteRegistro(c);
		respostaTeste();				
	}		




//*****
	////////////////// TESTE TABELAS

	public static Programacao seedProgramacao(string Descricao)
	{
		Programacao c = new Programacao();
		c.Descricao = Descricao;
		c.PeriodoInicial = DateTime.Now.AddDays(1);
		c.PeriodoFinal = DateTime.Now.AddDays(3);
		Dao.setProgramacao(c);		
		int  ID = Dao.getLastID("Programacao");
		return getProgramacao(ID);
	}

	public static void PesquisaProgramacaoComIDErrado()
	{
		Programacao c = seedProgramacao("PROGRAMAÇÃO COM ID ERRADO");


		int ID = 99;

		inicializaTeste();
		Programacao e = getProgramacao(ID);
		respostaTeste();
		Dao.deleteProgramacao(getProgramacao(Dao.getLastID("Programacao")));
	}

	public static void PesquisaProgramacaoComIDCerto()
	{
		Programacao c = seedProgramacao("PROGRAMACAO COM ID CERTO");

		int  ID = c.ID;

		inicializaTeste();
		Programacao e = getProgramacao(ID);
		respostaTeste();
		Dao.deleteProgramacao(c);
	}		

	public static void PesquisaListaProgramacao()
	{
		Programacao c1 = seedProgramacao("PROGRAMAÇÃO 1");
		Programacao c2 = seedProgramacao("PROGRAMAÇÃO 2");
		Programacao c3 = seedProgramacao("PROGRAMAÇÃO 3");

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<Programacao> programacoes = Dao.listProgramacao();
		respostaTeste();				
		Dao.deleteProgramacao(c1);
		Dao.deleteProgramacao(c2);
		Dao.deleteProgramacao(c3);
	}		

	public static void PesquisaListaNenhumProgramacao()
	{
		//Cadastrar uma empresa válida
		List<Programacao> programacoes = Dao.listProgramacao();
		foreach(Programacao c in programacoes)
		{
			Dao.deleteProgramacao(c);
		}

		inicializaTeste();
		programacoes = Dao.listProgramacao();
		respostaTeste();				
	}		

	public static void SetaProgramacaoInvalido()
	{
		
		Programacao c = new Programacao();
		inicializaTeste();
		Dao.setProgramacao(c);
		respostaTeste();				
		Dao.deleteProgramacao(c);
	}		

	public static void SetaProgramacaoValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Programacao c = seedProgramacao("PROGRAMACAO VALIDA");
		respostaTeste();	
		Dao.deleteProgramacao(c);			
	}		


	public static void SetaProgramacaoJaSalvo()
	{
		
		Programacao c = seedProgramacao("PROGRAMAÇÃO JA SALVA");
		inicializaTeste();
		Dao.setProgramacao(c);
		respostaTeste();	
		Dao.deleteProgramacao(c);			
	}		


// update
// --------------------------------

	public static void AtualizaProgramacaoInvalido()
	{
		//Cadastrar uma empresa válida
		Programacao c = seedProgramacao("PROGRAMAÇÃO INVALIDA");
		c.Descricao = String.Empty;
		inicializaTeste();
		Dao.updateProgramacao(c);
		respostaTeste();	
		Dao.deleteProgramacao(c);			
	}		

	public static void AtualizaProgramacaoValido()
	{
		//Cadastrar uma empresa válida
		Programacao c = seedProgramacao("PROGRAMAÇÃO VALIDA");
		c.Descricao = "Altera Descricao";
		inicializaTeste();
		Dao.updateProgramacao(c);
		respostaTeste();
		Dao.deleteProgramacao(c);
	}		


	public static void AtualizaProgramacaoInexistente()
	{
		//Cadastrar uma empresa válida
		Programacao c = seedProgramacao("PROGRAMAÇÃO INEXISTENTE");
		int id = c.ID;
		c.ID = Dao.getLastID("Programacao")+1;
		c.Descricao = String.Empty;
		inicializaTeste();
		Dao.updateProgramacao(c);
		respostaTeste();	
		Dao.deleteProgramacao(getProgramacao(id));
	}		


// delete
// --------------------------------

	public static void ApagaProgramacaoInexistente()
	{
		//Cadastrar uma empresa válida
		Programacao c = seedProgramacao("PROGRAMAÇÃO DELETE INEXISTENTE");
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteProgramacao(c);
		respostaTeste();				
		Dao.deleteProgramacao(getProgramacao(id));
	}		

	public static void ApagaProgramacaoExistente()
	{
		//Cadastrar uma empresa válida
		Programacao c = seedProgramacao("PROGRAMAÇÃO EXISTENTE");
		inicializaTeste();
		Dao.deleteProgramacao(c);
		respostaTeste();				
	}		

	public static void ListaProgramacaoEItensSemProgramacao()
	{
		Programacao c = new Programacao();
		inicializaTeste();
		List<Programacao> programacoes = Dao.listProgramacaoEItens();
		respostaTeste();
	}

	public static void ListaProgramacaoEItensComProgramacaoInvalida()
	{
		Programacao c = seedProgramacao("PROGRAMAÇÃO E ITENS PROGRAMAÇÃO INVALIDA");
		int id = c.ID;
		c.ID=0;
		inicializaTeste();
		List<Programacao> programacoes = Dao.listProgramacaoEItens();
		respostaTeste();
		Dao.deleteProgramacao(getProgramacao(id));
	}

	public static void ListaProgramacaoEItensComProgramacaoSemItens()
	{
		Programacao c = seedProgramacao("PROGRAMAÇÃO E ITENS SEM ITENS");
		inicializaTeste();
		List<Programacao> programacoes = Dao.listProgramacaoEItens();
		respostaTeste();
		Dao.deleteProgramacao(c);
	}

	public static void ListaProgramacaoEItensComProgramacaoEItens()
	{
		Programacao c = seedProgramacao("PROGRAMAÇÃO E ITENS COM ITENS");
		ItensProgramacao i1 = seedItensProgramacao(c.ID, 1, "IMAGEM", 1, 10000);
		//ItensProgramacao i2 = seedItensProgramacao(c.ID, 2, "CARTAZ", 1, 10000);

		inicializaTeste();
		List<Programacao> programacoes = Dao.listProgramacaoEItens();
		respostaTeste();
		Dao.deleteProgramacao(c);
	}

////////////////// TESTE ITENSTABELAS

	public static ItensProgramacao seedItensProgramacao(int Ordem, string TipoMidia, int MidiaId, int TempoExibicao)
	{
		Programacao p = seedProgramacao("PROGRAMAÇÃO DE ITENS");

		ItensProgramacao c = new ItensProgramacao();
		c.ProgramacaoId = p.ID;
		c.Ordem = Ordem;
		c.TipoMidia = TipoMidia;
		c.ImagemID=0;
		c.CartazID=0;
		c.TabelaID=0;
		switch(TipoMidia)
		{
			case "IMAGEM": c.ImagemID = MidiaId; break;
			case "CARTAZ": c.CartazID = MidiaId; break;
			case "TABELA": c.TabelaID = MidiaId; break;
		}
		c.TempoExibicao = TempoExibicao;
		Dao.setItensProgramacao(c);		
		int  ID = Dao.getLastID("ItemProgramacao");
		return getItensProgramacao(ID);
	}

	public static ItensProgramacao seedItensProgramacao(int Programacao, int Ordem, string TipoMidia, int MidiaId, int TempoExibicao)
	{
		ItensProgramacao c = new ItensProgramacao();
		c.ProgramacaoId = Programacao;
		c.Ordem = Ordem;
		c.TipoMidia = TipoMidia;

		switch(TipoMidia)
		{
			case "IMAGEM": c.ImagemID = MidiaId; break;
			case "CARTAZ": c.CartazID = MidiaId; break;
			case "TABELA": c.TabelaID = MidiaId; break;
		}
		c.TempoExibicao = TempoExibicao;
		Dao.setItensProgramacao(c);		
		int  ID = Dao.getLastID("ItemProgramacao");
		return getItensProgramacao(ID);
	}

	public static ItensProgramacao TesteSeedItensProgramacao()
	{
		Programacao p = seedProgramacao("PROGRAMAÇÃO DE ITENS");

		ItensProgramacao c = new ItensProgramacao();
		c.ProgramacaoId = p.ID;
		c.Ordem = 1;
		c.TipoMidia = "IMAGEM";
		c.ImagemID=0;
		c.CartazID=0;
		c.TabelaID=0;
		switch(c.TipoMidia)
		{
			case "IMAGEM": c.ImagemID = 1; break;
			case "CARTAZ": c.CartazID = 1; break;
			case "TABELA": c.TabelaID = 1; break;
		}
		c.TempoExibicao = 10000;
		inicializaTeste();
		Dao.setItensProgramacao(c);		
		respostaTeste();
		int  ID = Dao.getLastID("ItemProgramacao");
		return getItensProgramacao(ID);
	}

	public static void PesquisaItensProgramacaoComIDErrado()
	{
		//Cadastrar uma empresa válida
		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);

		//TesteCNPJErrado
		int ID = 99;

		inicializaTeste();
		ItensProgramacao e = getItensProgramacao(ID);
		respostaTeste();
		Dao.deleteItensProgramacao(getItensProgramacao(Dao.getLastID("ItemProgramacao")));
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}

	public static void PesquisaItensProgramacaoComIDCerto()
	{
		//Cadastrar uma empresa válida
		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
//		debug+=Dao.mensagemSucesso.mensagem + "|71|";

		//TesteCNPJErrado
		int  ID = c.ID;
//		debug+=Dao.mensagemSucesso.mensagem + "|75|";

		inicializaTeste();
		ItensProgramacao e = getItensProgramacao(ID);
		respostaTeste();
		Dao.deleteItensProgramacao(e);
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		

	public static void PesquisaListaItensProgramacao()
	{

		ItensProgramacao c1 = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		ItensProgramacao c2 = seedItensProgramacao(2, "IMAGEM", 1, 10000);
		ItensProgramacao c3 = seedItensProgramacao(3, "TABELA", 1, 10000);

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<ItensProgramacao> itens = Dao.listItensProgramacao();
		respostaTeste();		

		Dao.deleteItensProgramacao(c1);
		Dao.deleteProgramacao(getProgramacao(c1.ProgramacaoId));
		Dao.deleteItensProgramacao(c2);
		Dao.deleteProgramacao(getProgramacao(c2.ProgramacaoId));
		Dao.deleteItensProgramacao(c3);
		Dao.deleteProgramacao(getProgramacao(c3.ProgramacaoId));

	}		

	public static void PesquisaListaNenhumItensProgramacao()
	{
		//Cadastrar uma empresa válida
		List<ItensProgramacao> itens = Dao.listItensProgramacao();
		foreach(ItensProgramacao c in itens)
		{
			Dao.deleteItensProgramacao(c);
			Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
		}

		inicializaTeste();
		itens = Dao.listItensProgramacao();
		respostaTeste();				
	}		

	public static void SetaItensProgramacaoInvalido()
	{
		//Cadastrar uma empresa válida
		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		int programacaoid = c.ProgramacaoId;
		int id = c.ID;
		inicializaTeste();
		c.ProgramacaoId = 0;
		Dao.setItensProgramacao(c);
		respostaTeste();				
		Dao.deleteItensProgramacao(getItensProgramacao(id));
		Dao.deleteProgramacao(getProgramacao(programacaoid));

	}		

	public static void SetaItensProgramacaoValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		respostaTeste();				
		Dao.deleteItensProgramacao(c);
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		


	public static void SetaItensProgramacaoJaSalvo()
	{
		//Cadastrar uma empresa válida
		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		inicializaTeste();
		Dao.setItensProgramacao(c);
		respostaTeste();		
		Dao.deleteItensProgramacao(c);		
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		


// update
// --------------------------------

	public static void AtualizaItensProgramacaoInvalido()
	{
		//Cadastrar uma empresa válida

		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		int programacaoid = c.ProgramacaoId;
		int id = c.ID;
		c.ProgramacaoId = 0;
		inicializaTeste();
		Dao.updateItensProgramacao(c);
		respostaTeste();				
		Dao.deleteItensProgramacao(getItensProgramacao(id));
		Dao.deleteProgramacao(getProgramacao(programacaoid));
	}		

	public static void AtualizaItensProgramacaoValido()
	{
		//Cadastrar uma empresa válida

		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		c.TempoExibicao = 50000;
		inicializaTeste();
		Dao.updateItensProgramacao(c);
		respostaTeste();
		Dao.deleteItensProgramacao(c);
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		


	public static void AtualizaItensProgramacaoInexistente()
	{
		//Cadastrar uma empresa válida

		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		int id = c.ID;
		c.ID = Dao.getLastID("ItemProgramacao")+1;
		c.TipoMidia = String.Empty;
		inicializaTeste();
		Dao.updateItensProgramacao(c);
		respostaTeste();				
		Dao.deleteItensProgramacao(getItensProgramacao(id));
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		


// delete
// --------------------------------

	public static void ApagaItensProgramacaoInexistente()
	{
		//Cadastrar uma empresa válida

		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteItensProgramacao(c);
		respostaTeste();				
		Dao.deleteItensProgramacao(getItensProgramacao(id));
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		

	public static void ApagaItensProgramacaoExistente()
	{
		//Cadastrar uma empresa válida

		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		inicializaTeste();
		Dao.deleteItensProgramacao(c);
		respostaTeste();				
		Dao.deleteProgramacao(getProgramacao(c.ProgramacaoId));
	}		

	public static void ListaItensPorProgramacaoProgramacaoInvalida()
	{

		ItensProgramacao c = seedItensProgramacao(1, "CARTAZ", 1, 10000);
		int id = c.ProgramacaoId;
		c.ProgramacaoId = 0;
		inicializaTeste();
		List<ItensProgramacao> itens = listItensProgramacaoPorProgramacao(c.ProgramacaoId);
		respostaTeste();				
		Dao.deleteProgramacao(getProgramacao(id));
	}

	public static void ListaItensPorProgramacaoProgramacaoSemItens()
	{

		inicializaTeste();
		Programacao c = seedProgramacao("PROGRAMAÇÃO SEM ITENS");
		List<ItensProgramacao> itens = listItensProgramacaoPorProgramacao(c.ID);
		respostaTeste();				
		Dao.deleteProgramacao(getProgramacao(c.ID));
	}

	public static void ListaItensPorProgramacaoProgramacaoComItens()
	{
		inicializaTeste();
		Programacao t = seedProgramacao("PROGRAMACAO COM ITENS");
		ItensProgramacao c1 = seedItensProgramacao(t.ID, 1, "CARTAZ", 1, 10000);
		//ItensProgramacao c2 = seedItensProgramacao(t.ID, 2, "IMAGEM", 1, 10000);
		//List<ItensProgramacao> itens = listItensProgramacaoPorProgramacao(t.ID);
		respostaTeste();				
		Dao.deleteProgramacao(getProgramacao(t.ID));
		
	}

////////////////// TESTE FEED

	public static Feed seedFeed(string Noticia, int Ordem)
	{
		Feed c = new Feed();
		c.Noticia = Noticia;
		c.Ordem = Ordem;
		Dao.setFeed(c);		
		int  ID = Dao.getLastID("Feed");
		return getFeed(ID);
	}

	public static void PesquisaFeedComIDErrado()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ESTA COM ID ERRADO", 1);

		//TesteCNPJErrado
		int ID = 99;

		inicializaTeste();
		Feed e = getFeed(ID);
		respostaTeste();
		Dao.deleteFeed(getFeed(c.ID));
	}

	public static void PesquisaFeedComIDCerto()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ESTA COM ID CERTO", 1);
//		debug+=Dao.mensagemSucesso.mensagem + "|71|";

		//TesteCNPJErrado
		int  ID = c.ID;
//		debug+=Dao.mensagemSucesso.mensagem + "|75|";

		inicializaTeste();
		Feed e = getFeed(ID);
		respostaTeste();
		Dao.deleteFeed(c);
	}		

	public static void PesquisaListaFeeds()
	{
		Feed c1 = seedFeed("ESSA NOTICIA ordem 1", 1);
		Feed c2 = seedFeed("ESSA NOTICIA ordem 2", 2);
		Feed c3 = seedFeed("ESSA NOTICIA ordem 3", 3);

		//Cadastrar uma empresa válida
		inicializaTeste();
		List<Feed> feeds = Dao.listFeeds();
		respostaTeste();				
		Dao.deleteFeed(c1);
		Dao.deleteFeed(c2);
		Dao.deleteFeed(c3);
	}		

	public static void PesquisaListaNenhumFeeds()
	{
		//Cadastrar uma empresa válida
		List<Feed> feeds = Dao.listFeeds();
		foreach(Feed c in feeds)
		{
			Dao.deleteFeed(c);
		}

		inicializaTeste();
		feeds = Dao.listFeeds();
		respostaTeste();				
	}		

	public static void SetaFeedInvalido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Feed c = new Feed();
		Dao.setFeed(c);
		respostaTeste();				
	}		

	public static void SetaFeedValido()
	{
		//Cadastrar uma empresa válida
		inicializaTeste();
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);

		respostaTeste();	
		Dao.deleteFeed(c);			
	}		


	public static void SetaFeedJaSalvo()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);

		inicializaTeste();
		Dao.setFeed(c);
		respostaTeste();	
		Dao.deleteFeed(c);			
	}		


// update
// --------------------------------

	public static void AtualizaFeedInvalido()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		c.Noticia = String.Empty;
		c.Ordem = 0;
		inicializaTeste();
		Dao.updateFeed(c);
		respostaTeste();	
		Dao.deleteFeed(c);			
	}		

	public static void AtualizaFeedValido()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		c.Noticia = "MUDANÇA DE NOTÍCIA";
		c.Ordem = 2;
		inicializaTeste();
		Dao.updateFeed(c);
		respostaTeste();
		Dao.deleteFeed(c);
	}		


	public static void AtualizaFeedInexistente()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		int id = c.ID;
		c.ID = Dao.getLastID("Feed")+1;
		c.Noticia = String.Empty;
		c.Ordem = 0;

		inicializaTeste();
		Dao.updateFeed(c);
		respostaTeste();	
		Dao.deleteFeed(getFeed(id));			
	}		


// delete
// --------------------------------

	public static void ApagaFeedInexistente()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		int id = c.ID;
		c.ID+=2;
		inicializaTeste();
		Dao.deleteFeed(c);
		respostaTeste();				
		Dao.deleteFeed(getFeed(id));
	}		

	public static void ApagaFeedExistente()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		inicializaTeste();
		Dao.deleteFeed(c);
		respostaTeste();				
	}		

	public static void ApagaFeedComProgramacao()
	{
		
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		ItensProgramacao i = new ItensProgramacao();
		i.TipoMidia = "FEED";
		i.Ordem = 1;
		i.FeedID = c.ID;
		i. TempoExibicao = 10000;
		i.ProgramacaoId = 1;
		Dao.setItensProgramacao(i);

		inicializaTeste();
		Dao.deleteFeed(c);
		respostaTeste();	
		Dao.deleteItensProgramacao(getItensProgramacao(getLastID("ItemProgramacao")));	
		Dao.deleteFeed(c);
	}		

	public static void ApagaFeedSemProgramacao()
	{
		//Cadastrar uma empresa válida
		Feed c = seedFeed("ESSA NOTICIA ordem 1", 1);
		inicializaTeste();
		Dao.deleteFeed(c);
		respostaTeste();	
	}		


}