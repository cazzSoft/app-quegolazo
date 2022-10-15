USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[SP_PROC_CalificaPorPartido]    Script Date: 2018/05/11 10:10:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ivan
-- Create date: <Create Date,,>
-- Description:	Calcula los puntos de pronosticos, una vez acabado un partido
-- =============================================
ALTER PROCEDURE [dbo].[SP_PROC_CalificaPorPartido] 
@idPartido int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

--TODO: Bajar puntos en cartilla?? o reclacularlas todas

UPDATE pronosticos set puntos = 
--eqs * (ptsGanador + ptsScore)

(CASE WHEN pr.idEquipo1 = pt.idEquipo1 AND pr.idEquipo2 = pt.idEquipo2 THEN 1 ELSE 0 END) *
(
(CASE WHEN pr.scoreP1 = pt.score1 AND pr.scoreP2 = pt.score2 THEN 1 ELSE 0 END) 
+
(CASE WHEN (pr.scoreP1 - pr.scoreP2) = (pt.score1 - pt.score2) THEN 1 ELSE 0 END)
+
(--acierta empate
CASE WHEN (pr.scoreP1 - pr.scoreP2)= 0 AND (pt.score1 - pt.score2) = 0  THEN 1 
--Acierta gana 1
WHEN (pr.scoreP1 - pr.scoreP2)> 0 AND (pt.score1 - pt.score2) > 0  THEN 1 
--Acierta gana 2
WHEN (pr.scoreP1 - pr.scoreP2) < 0 AND (pt.score1 - pt.score2) < 0  THEN 1 
ELSE 0 END ))

FROM            pronosticos AS pr INNER JOIN
                         partidos AS pt ON pr.idPartido = pt.id
WHERE        (pr.idPartido = @idPartido)


/*
SELECT        pr.id, pr.idEquipo1, pr.idEquipo2, pr.scoreP1, pr.scoreP2, pt.idEquipo1 AS Expr1, pt.idEquipo2 AS Expr2, pt.score1, pt.score2,
CASE WHEN pr.idEquipo1 = pt.idEquipo1 AND pr.idEquipo2 = pt.idEquipo2 THEN 1 ELSE 0 END AS eqs,
CASE WHEN pr.scoreP1 = pt.score1 AND pr.scoreP2 = pt.score2 THEN 1 ELSE 0 END AS ptsScore,
--acierta empate
CASE WHEN (pr.scoreP1 - pr.scoreP2)= 0 AND (pt.score1 - pt.score2) = 0  THEN 3 
--Acierta gana 1
WHEN (pr.scoreP1 - pr.scoreP2)> 0 AND (pt.score1 - pt.score2) > 0  THEN 3 
--Acierta gana 2
WHEN (pr.scoreP1 - pr.scoreP2) < 0 AND (pt.score1 - pt.score2) < 0  THEN 3 
ELSE 0 END AS ptsganador

FROM            pronosticos AS pr INNER JOIN
                         crmMYL.partidos AS pt ON pr.idPartido = pt.id
WHERE        (pr.idPartido = 1)

)
*/

END
GO

