USE [skor]
GO

/****** Object:  UserDefinedFunction [dbo].[FN_PuntosUsuarios]    Script Date: 2018/05/17 9:35:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: Ivan
-- Description:	SUma los puntos por bando
-- TODO: Filtrar cartillas que hayan cambiado
-- 
-- =============================================
CREATE FUNCTION [dbo].[FN_PuntosBandos]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
SELECT        COUNT(dbo.USR_usuarios.id) AS usuarios, SUM(dbo.USR_usuarios.puntos) AS puntosTotales, dbo.UsuariosBandos.idBando
		FROM            dbo.USR_usuarios INNER JOIN
                         dbo.UsuariosBandos ON dbo.USR_usuarios.id = dbo.UsuariosBandos.idUsuario
GROUP BY dbo.UsuariosBandos.idBando

)

GO


