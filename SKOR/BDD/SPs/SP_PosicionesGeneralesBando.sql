CREATE PROCEDURE [dbo].[SP_PosicionesGeneralesBando]
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

WHERE ub.idBando= @idBando AND ub.estaAceptado=1  AND uper.Puntos is not null and uper.posicion is not null

ORDER by posicion

) as cu

UNION

SELECT       
uper.nombreCompleto,  uper.id as idUsuario, uper.ranking, uper.puntos, uper.posicion

FROM            USR_usuariosPersonas as uper

WHERE         (uper.id = @idUsuario)

ORDER BY  cu.posicion 

END



GO


