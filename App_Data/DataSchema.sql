﻿CREATE TABLE [Cartaz]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [CABECALHO] NVARCHAR(100),
   [FONTE_CABECALHO] NVARCHAR(50),
   [TAMANHO_CABECALHO] INT,
   [TEXTO] NVARCHAR(255),
   [FONTE_TEXTO] NVARCHAR(50),
   [TAMANHO_TEXTO] INT,
   [RODAPE] NVARCHAR(100),
   [FONTE_RODAPE] NVARCHAR(50),
   [TAMANHO_RODAPE] INT
);

CREATE TABLE [Empresa]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [NOME] NVARCHAR(50),
   [CNPJ] NVARCHAR(50),
   [ENDERECO] NVARCHAR(100),
   [FONE] NVARCHAR(20),
   [RESPONSAVEL] NVARCHAR(20)
);

CREATE TABLE [Feed]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [NOTICIA] NVARCHAR(255) NOT NULL,
   [ORDEM] INT NOT NULL
);

CREATE TABLE [Imagem]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [ENDERECO] NVARCHAR(50) NOT NULL,
   [ALTURA] INT NOT NULL,
   [LARGURA] INT NOT NULL
);

CREATE TABLE [ItemProgramacao]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [PROGRAMACAO_ID] INT NOT NULL,
   [ORDEM] INT NOT NULL,
   [TIPO_MIDIA] NVARCHAR(20) NOT NULL,
   [MIDIA_ID] INT NOT NULL,
   [TEMPO_EXIBICAO] INT NOT NULL
);

CREATE TABLE [ItensTabela]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [TABELA_ID] INT NOT NULL,
   [PRODUTO] NVARCHAR(100),
   [FONTE_PRODUTO] NVARCHAR(50),
   [TAMANHO_PRODUTO] INT,
   [VALOR] DECIMAL(15,2),
   [FONTE_VALOR] NVARCHAR(50),
   [TAMANHO_VALOR] INT
);

CREATE TABLE [Licenca]
(
   [LICENCA] NVARCHAR(150) NOT NULL,
   [VIGENCIA] DATETIME
);

CREATE TABLE [Programacao]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [DESCRICAO] NVARCHAR(50),
   [PERIODOINICIAL] DATETIME,
   [PERIODOFINAL] DATETIME
);

CREATE TABLE [Registro]
(
   [CNPJ] NVARCHAR(50) NOT NULL,
   [LICENCAATIVA] NVARCHAR(50)
);

CREATE TABLE [Tabelas]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [CABECALHO] NVARCHAR(50),
   [FONTE_CABECALHO] NVARCHAR(50),
   [TAMANHO_CABECALHO] INT
);

CREATE TABLE [Usuario]
(
   [ID] INT NOT NULL IDENTITY (1,1),
   [NOME] NVARCHAR(50),
   [EMAIL] NVARCHAR(150),
   [SENHA] NVARCHAR(20)
);

ALTER TABLE [Cartaz] ADD CONSTRAINT [PK_Cartaz_ID] PRIMARY KEY ([ID]);

ALTER TABLE [Empresa] ADD CONSTRAINT [PK_Empresa_ID] PRIMARY KEY ([ID]);

ALTER TABLE [Feed] ADD CONSTRAINT [PK_Feed_ID] PRIMARY KEY ([ID]);

ALTER TABLE [Imagem] ADD CONSTRAINT [PK_Imagem_ID] PRIMARY KEY ([ID]);

ALTER TABLE [ItemProgramacao] ADD CONSTRAINT [PK_ItemProgramacao_ID] PRIMARY KEY ([ID]);

ALTER TABLE [ItensTabela] ADD CONSTRAINT [PK_ItensTabela_ID] PRIMARY KEY ([ID]);

ALTER TABLE [Licenca] ADD CONSTRAINT [PK_Licenca_LICENCA] PRIMARY KEY ([LICENCA]);

ALTER TABLE [Programacao] ADD CONSTRAINT [PK_Programacao_ID] PRIMARY KEY ([ID]);

ALTER TABLE [Registro] ADD CONSTRAINT [PK_Registro_CNPJ] PRIMARY KEY ([CNPJ]);

ALTER TABLE [Tabelas] ADD CONSTRAINT [PK_Tabelas_ID] PRIMARY KEY ([ID]);

ALTER TABLE [Usuario] ADD CONSTRAINT [PK_Usuario_ID] PRIMARY KEY ([ID]);
