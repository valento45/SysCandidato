create database candidatosbd;

use candidatosbd;

create table users_tb(
id_usuario int auto_increment primary key,
username varchar(150),
normalizedUserName varchar(150),
passwordhash varchar(300)
);

create table vagas_tb(
id_vaga int auto_increment primary key,
descricao varchar(50) not null
);
insert into vagas_tb (descricao)  values ( "Programador JR");
insert into vagas_tb (descricao)  values ( "Programador PL");
insert into vagas_tb (descricao)  values ( "Programador SR");

create table pessoa_tb(
id_pessoa int auto_increment primary key,
nome varchar(50) not null,
sobrenome varchar (60) not null,
cpf int unique not null,
data_nascimento datetime not null
);


create table vaga_candidato_tb(
id int auto_increment primary key,
id_pessoa int not null,
id_vaga int not null,
constraint foreign key (id_pessoa)
references pessoa_tb(id_pessoa),
constraint foreign key(id_vaga)
references vagas_tb(id_vaga)
);

select * from vaga_candidato_tb;
select * from vagas_tb;
select* from pessoa_tb;
select * from users_tb;
