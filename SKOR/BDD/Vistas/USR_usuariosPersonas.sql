USE [skor]
GO

/****** Object:  View [dbo].[USR_usuariosPersonas]    Script Date: 2018/05/11 10:17:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[USR_usuariosPersonas]
AS
SELECT        dbo.MIS_Personas.nombre1 + ' ' + dbo.MIS_Personas.apellido1 AS nombreCompleto, dbo.MIS_Personas.email, dbo.USR_usuarios.nombreUsuario, 
                         dbo.USR_usuarios.id, dbo.USR_usuarios.ranking, dbo.USR_usuarios.puntos, dbo.USR_usuarios.posicion
FROM            dbo.USR_usuarios INNER JOIN
                         dbo.MIS_Personas ON dbo.USR_usuarios.idPersona = dbo.MIS_Personas.id
GO

