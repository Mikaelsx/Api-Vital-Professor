select * from TiposUsuario

insert into TiposUsuario
values (NEWID(), 'medico')


select * from Pacientes

select * from Consultas

select * from Consultas

select * from Usuarios


select * from Medicos

select * from Especialidades

select * from Enderecos 

select * from Clinicas

select * from Situacoes

select * from MedicosClinicas

insert into Situacoes
values (NEWID(),'Agendadas'), (NEWID(),'Realizadas'), (NEWID(),'Canceladas')


select * from MedicosClinicas

insert into MedicosClinicas
values (NEWID(),'7F5AE2A8-86F1-44A8-BB2B-5CD5B0CA5676', '025AFD49-C195-4B7A-8CB1-843613C8CAC8')

select * from Receitas
insert into Receitas
values (NEWID(), 'dipirona', 'tomar de 12 em 12 horas')

select * from NiveisPrioridade
insert into NiveisPrioridade
values (NEWID(), 0)

UPDATE dbo.Consultas SET PacienteID = '91a65bfe-4413-450f-b063-7d8247311076';
UPDATE dbo.Consultas SET PacienteID = 'FCB94260-A63A-47D1-B5BA-3D8A27D80B2E';

update dbo.Consultas set DataConsulta= (GETDATE())

update dbo.Usuarios set TipoUsuarioID= '97E4A35A-5CFD-4AC3-AEC0-BC240DA1E392' where Nome='vinicius'


update Enderecos set Latitude= '-23.58155781979929' 


-23.58155781979929, -46.59913869991061