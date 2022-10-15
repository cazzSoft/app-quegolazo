USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_PROC_CreaPronosticos]    Script Date: 2018/05/11 10:10:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SP_PROC_CreaPronosticos] 
	@idCartillaUsuario Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [pronosticos]
	( idPartidoCartilla, idusuario, idCartillaUsuario, idPartido, idEquipo1, idEquipo2)
	
	SELECT
	det.id , cu.idUsuario, @idCartillaUsuario, det.idPartido, p.idEquipo1, p.idEquipo2
	FROM 	 
	[cartillasUsuario] as cu
	INNER join [PartidosCartilla] as det ON det.idCartilla = cu.idCartilla
	INNER join partidos as p ON p.id = det.idPartido
	  
	where cu.id = @idCartillaUsuario
	   
END

GO

