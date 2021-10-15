create database candidatosbd;

use candidatosbd;


create table organizacao_tb(
id int auto_increment primary key,
nome varchar(100)
);

create table users_tb(
id_usuario int auto_increment primary key,
username varchar(150),
normalizedUserName varchar(150),
email varchar(150),
normalizedEmail varchar (150),
passwordhash varchar(300),
confirmedEmail bool,
securityStamp varchar(100),
concurrencyStamp varchar(100),
twoFactorEnable bool,
lockoutEmail bool,
lockoutEnabled bool,
accessFailedCount int,
orgId int,
Constraint foreign key (orgId)
References organizacao_tb(id)
);

create table login_tb(
LoginProvider int auto_increment primary key,
ProviderDisplayName varchar(100),
userId int,
constraint foreign key(userId)
References users_tb(id_usuario)
);

create table user_claims_tb(
id int auto_increment primary key,
userId int,
claimType varchar(100),
claimValue varchar(100),
Constraint Foreign Key(userId)
References users_tb(id_usuario)
);

DELIMITER $$
CREATE DEFINER=CURRENT_USER PROCEDURE add_version_to_actor ( ) 
BEGIN
DECLARE colName TEXT;

SELECT column_name INTO colName
FROM information_schema.columns 
WHERE table_schema = 'candidatosbd'
    AND table_name = 'users_tb'
AND column_name = 'confirmedEmail';
IF colName is null THEN 
    alter table users_tb ADD COLUMN confirmedEmail bool default false;
END IF; 


END$$

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


