USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_AvancesFaseCartilla]    Script Date: 2018/05/06 21:48:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ivan
-- Create date: <Create Date,,>
-- Description:	Descarga los avancesfase de una cartilla
-- =============================================
ALTER PROCEDURE [dbo].[SP_AvancesFaseCartilla]
	@IdCartillaUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  SELECT        dbo.AvancesFase.idGrupo, dbo.AvancesFase.posicion, dbo.Pronosticos.idCartillaUsuario, dbo.AvancesFase.numeroEquipo, dbo.AvancesFase.idPartido
FROM            dbo.AvancesFase INNER JOIN
                         dbo.Pronosticos ON dbo.AvancesFase.idPartido = dbo.Pronosticos.idPartido
WHERE        (dbo.Pronosticos.idCartillaUsuario = @IdCartillaUsuario)
END

GO

