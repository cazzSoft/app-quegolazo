CREATE TABLE [dbo].[Cartillas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idEmpresa] [int] NULL,
	[idAdministrador] [int] NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[fechaCierre] [date] NULL,
	[descripcion] [varchar](250) NULL,
	[premios] [varchar](max) NULL,
	[condiciones] [varchar](max) NULL,
	[estaCerrada] [bit] NULL,
	[urlBanner] [varchar](1000) NULL DEFAULT (NULL),
	[estaTerminada] [bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_cartillas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Cartillas]  WITH CHECK ADD  CONSTRAINT [FK_Empresas_Cartillas] FOREIGN KEY([idEmpresa])
REFERENCES [dbo].[Empresas] ([id])
GO

ALTER TABLE [dbo].[Cartillas] CHECK CONSTRAINT [FK_Empresas_Cartillas]
GO

ALTER TABLE [dbo].[Cartillas]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Cartillas] FOREIGN KEY([idAdministrador])
REFERENCES [dbo].[USR_usuarios] ([id])
GO

ALTER TABLE [dbo].[Cartillas] CHECK CONSTRAINT [FK_Usuarios_Cartillas]
GO


--------------------------------------------------------------------------------

--Modificacion requerimiento agregar estaActiva 09052018
alter table Cartillas add estaActiva bit default 0
