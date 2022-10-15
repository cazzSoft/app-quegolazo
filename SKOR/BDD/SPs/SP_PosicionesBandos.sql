USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_PosicionesGenerales]    Script Date: 2018/05/17 9:53:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- //todo: hacer union con los grupos del usuario??


CREATE PROCEDURE [dbo].[SP_PosicionesBandos]
	 @cuanto int
AS
BEGIN
	SET NOCOUNT ON;



SELECT    top(@cuanto) 
 bd.posicion, bd.nombre, bd.puntos, bd.miembros ,
 (bd.puntos / bd.miembros) as prom

FROM            bandos as bd

Where Puntos is not null and posicion is not null and posicion >0

ORDER by posicion


END

GO


