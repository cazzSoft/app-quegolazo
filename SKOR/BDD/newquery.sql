select count(*) from usr_usuarios where fechaCreacion > '20180527'

select * from bandos
UPDATE bandos set nombre = 'USFQ Dragones' where id=23

select idcartilla from CartillasUsuario where fechaSellada = '20180602'

delete from Bandos where id= 24

select nombreCompleto, email from USR_usuariosPersonas where id = 280

select nombre1, email from MIS_Personas where nombre1  != 'A'

select * from cartillas

select top 1 * from partidos order by id desc

select * from rankings

select * from partidos where fecha = '20180601'
select * from equipos where id in (18,64)

update rankings set nombre='Eres el rey de ¡Qué Golazo!' where id =7

select * from cartillas
update cartillas set estaCerrada = 1 where id = 17

select count (*) from USR_usuarios

select top 5 * from USR_usuarios order by puntos desc

select * from USR_historialIngreso where idUsuario = 20