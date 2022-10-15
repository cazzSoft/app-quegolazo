USE [skor]
GO

/****** Object:  UserDefinedFunction [dbo].[FN_PuntosCartillas]    Script Date: 2018/05/11 10:46:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: Ivan
-- Description:	Recalcula puntos totales en cartillas.. Correr después de un partido
-- TODO: Filtrar cartillas que tengan ese partido
-- =============================================
ALTER FUNCTION [dbo].[FN_PuntosCartillas]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT        idCartillaUsuario, SUM(puntos) AS total
FROM            pronosticos
GROUP BY idCartillaUsuario

)
GO

