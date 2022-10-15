USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[USR_buscaUsuarioMail]    Script Date: 2018/05/11 10:16:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[USR_buscaUsuarioMail](@email as varchar(250))
as begin
select USR_usuarios.*, (MIS_personas.nombre1 + ' ' + MIS_personas.apellido1) as nombreCompleto,MIS_personas.email 
from USR_usuarios inner join MIS_personas on USR_usuarios.idPersona=MIS_personas.id 
WHERE MIS_personas.email=@email;
end
GO

