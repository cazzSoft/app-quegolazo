USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[USR_BuscaUsuarioId]    Script Date: 2018/05/11 10:15:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[USR_BuscaUsuarioId](@id as int)
as begin
select USR_usuarios.*, (MIS_personas.nombre1 + ' ' + MIS_personas.apellido1) as nombreCompleto,MIS_personas.email 
from USR_usuarios inner join MIS_personas on USR_usuarios.idPersona=MIS_personas.id 
WHERE USR_usuarios.id=@id;
end
GO

