/****** Object:  StoredProcedure [dbo].[SP_PosicionesGenerales]    Script Date: 2018/05/11 11:32:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[SP_PosicionesGenerales]
	@idUsuario int, @cuanto int
AS
BEGIN
	SET NOCOUNT ON;

SELECT * FROM(

--ranking NO SE USAN AUN
--uper.email,  quitado por privacidad por ahora

SELECT     top(@cuanto)  
 uper.nombreCompleto,  uper.id as idUsuario, uper.ranking, uper.puntos, uper.posicion

FROM            USR_usuariosPersonas as uper 
Where Puntos is not null and posicion is not null and posicion >0

ORDER by posicion

) as cu

UNION

SELECT       
uper.nombreCompleto,  uper.id as idUsuario, uper.ranking, uper.puntos, uper.posicion

FROM            USR_usuariosPersonas as uper

WHERE         (uper.id = @idUsuario) AND Puntos is not null and posicion is not null and posicion >0

ORDER BY  cu.posicion 

END


GO


