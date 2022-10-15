USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_PosicionesCartilla]    Script Date: 2018/05/06 21:53:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SP_PosicionesCartilla]
	@idCartilla int, @idUsuario int, @cuanto int
AS
BEGIN
	SET NOCOUNT ON;

SELECT * FROM(

SELECT     top(@cuanto)  cu.id as idCartillaUsuario, cu.puntos, cu.posicionResultado, cu.posicionCierre,cu.posicionFinal,
 uper.nombreCompleto, uper.email, uper.id as idUsuario, uper.ranking

FROM            cartillasUsuario as cu 
                         INNER JOIN USR_usuariosPersonas as uper ON cu.idUsuario = uper.id
WHERE        (cu.idCartilla = @idCartilla AND cu.estasellada=1) ORDER by posicionfinal

) as cu

UNION

SELECT        cu.id as idCartillaUsuario, cu.puntos, cu.posicionResultado, cu.posicionCierre,cu.posicionFinal,
 uper.nombreCompleto, uper.email, uper.id as idUsuario, uper.ranking

FROM            cartillasUsuario as cu 
                         INNER JOIN USR_usuariosPersonas as uper ON cu.idUsuario = uper.id
WHERE         (cu.idusuario = @idUsuario) AND (cu.idCartilla = @idCartilla) AND  cu.estasellada=1

ORDER BY  cu.posicionFinal

END

GO

