USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_RevisaCartilla]    Script Date: 2018/05/11 10:14:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Ivan
-- Create date: <Create Date,,>
-- Description:	Verifica el estado de la cartilla y si el usuario en mención ya tiene una cartillaUsuario y si ya está sellada.
-- =============================================
ALTER PROCEDURE [dbo].[SP_RevisaCartilla] 
	@idCartilla int,
	@idUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT        c.id, c.estaCerrada, 
					cu.idCartillaUsuario, cu.estaSellada
	FROM            dbo.Cartillas AS c 
	LEFT OUTER JOIN
    (SELECT id AS idCartillaUsuario, estaSellada, idCartilla FROM dbo.CartillasUsuario WHERE idUsuario = @idUsuario) AS cu 
	ON c.id = cu.idCartilla

	WHERE c.id = @idCartilla

    
END

GO

