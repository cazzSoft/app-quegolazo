USE [skor]
GO

/****** Object:  View [crmMYL].[VW_partidos]    Script Date: 2018/05/11 10:18:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [crmMYL].[VW_partidos]
AS
SELECT        dbo.Partidos.id, dbo.Partidos.nombre, dbo.Equipos.nombre AS eq1, dbo.Partidos.score1 AS s1, Equipos_1.nombre AS eq2, dbo.Partidos.score2 AS s2
FROM            dbo.Equipos INNER JOIN
                         dbo.Partidos ON dbo.Equipos.id = dbo.Partidos.idEquipo1 INNER JOIN
                         dbo.Equipos AS Equipos_1 ON dbo.Partidos.idEquipo2 = Equipos_1.id
GO

