USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_BandosUsuario]    Script Date: 2018/05/06 21:52:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Ivan
-- Create date: <Create Date,,>
-- Description:	Pone la posicion de cierre en la cartillaUsuario
-- =============================================
ALTER PROCEDURE [dbo].[SP_BandosUsuario]
	@idUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT        dbo.bandos.id, bandos.idAdmin, dbo.bandos.nombre, dbo.bandos.idTipo
FROM            dbo.cartillasBandos INNER JOIN
                         dbo.bandos ON dbo.cartillasBandos.idBando = dbo.bandos.id INNER JOIN
                         dbo.CartillasUsuario ON dbo.cartillasBandos.idCartillaUsuario = dbo.CartillasUsuario.id
WHERE        (dbo.CartillasUsuario.idUsuario = @idUsuario)

  
END


GO

