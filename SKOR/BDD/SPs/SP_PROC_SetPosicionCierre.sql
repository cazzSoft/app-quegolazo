USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_PROC_SetPosicionCierre]    Script Date: 2018/05/11 10:11:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ivan
-- Create date: <Create Date,,>
-- Description:	Pone la posicion de cierre en la cartillaUsuario
-- =============================================
ALTER PROCEDURE [dbo].[SP_PROC_SetPosicionCierre]
	@idCartillaUsuario int, @idCartilla int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @max int;

		
	set @max = (SELECT top 1 posicionCierre from cartillasUsuario where idCartilla = @idCartilla AND estaSellada =1 Order by posicionCierre desc)

	If(@max is null)
		SET @max=0; 



	update cartillasUsuario set posicionCierre = @max + 1 where id= @idCartillaUsuario

END
GO

