CREATE TABLE [dbo].[Bandos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idTipo] [int] NOT NULL,
	[idAdmin] [int] NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[codigo] [varchar](8) NOT NULL,
	[esAbierto] [bit] NULL DEFAULT ((0)),
	[puntos] [int] NULL DEFAULT ((0)),
	[miembros] [int] NULL DEFAULT ((0)),
	[posicion] [int] NULL DEFAULT ((0)),
	[descripcion] [nvarchar](150) NULL DEFAULT (''),
	[condiciones] [nvarchar](150) NULL DEFAULT (''),
	[premios] [nvarchar](150) NULL DEFAULT (''),
 CONSTRAINT [PK__bandos__3213E83F4E86AA20] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Bandos]  WITH CHECK ADD  CONSTRAINT [FK_Bandos_USR_usuarios] FOREIGN KEY([idAdmin])
REFERENCES [dbo].[USR_usuarios] ([id])
GO

ALTER TABLE [dbo].[Bandos] CHECK CONSTRAINT [FK_Bandos_USR_usuarios]
GO


