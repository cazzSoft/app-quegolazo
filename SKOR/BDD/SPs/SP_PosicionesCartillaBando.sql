/****** Object:  StoredProcedure [dbo].[SP_PosicionesGeneralesBando]    Script Date: 2018/05/11 11:39:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[SP_PosicionesGeneralesBando]
	@idUsuario int, @cuanto int, @idBando int
AS
BEGIN
	SET NOCOUNT ON;

SELECT * FROM(

--ranking NO SE USAN AUN
--uper.email,  quitado por privacidad por ahora

SELECT     top(@cuanto)  
 uper.nombreCompleto,  uper.id as idUsuario, uper.ranking, uper.puntos, uper.posicion

FROM            USR_usuariosPersonas as uper 
				INNER JOIN [dbo].[UsuariosBandos] ub on ub.Idusuario = uper.id

WHERE ub.idBando= @idBando AND ub.estaAceptado=1  AND uper.Puntos is not null and uper.posicion is not null and uper.posicion > 0

ORDER by posicion

) as cu

UNION

SELECT       
uper.nombreCompleto,  uper.id as idUsuario, uper.ranking, uper.puntos, uper.posicion

FROM            USR_usuariosPersonas as uper

WHERE         (uper.id = @idUsuario) AND uper.Puntos is not null and uper.posicion is not null and uper.posicion > 0

ORDER BY  cu.posicion 

END


GO


