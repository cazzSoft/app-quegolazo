CREATE TABLE [dbo].[USR_historialLogins](
	[id] [int] NOT NULL,
	[idUsuario] [int] NOT NULL,
	[clave] [varchar](50) NOT NULL,
	[fechaRegistro] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO