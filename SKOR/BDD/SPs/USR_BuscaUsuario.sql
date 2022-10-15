USE [skor]
GO

/****** Object:  StoredProcedure [dbo].[USR_BuscaUsuario]    Script Date: 2018/05/11 10:15:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[USR_BuscaUsuario](@usuario as varchar(50),@password as varchar(50))
as begin
select USR_usuarios.*,MIS_personas.nombre1 as nombreCompleto,MIS_personas.Email
from USR_usuarios inner join MIS_personas on USR_usuarios.idPersona=MIS_personas.id 
--convert(binary(100),nombreUsuario)=convert(binary(100),@usuario)
WHERE nombreUsuario=@usuario AND convert(binary(100),clave)=convert(binary(100),@password)
end
GO

