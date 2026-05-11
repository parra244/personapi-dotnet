-- SQL Server compatible DDL based on the correct MySQL model

CREATE DATABASE persona_db;
GO

USE persona_db;
GO

CREATE TABLE persona (
    cc BIGINT NOT NULL PRIMARY KEY,
    nombre VARCHAR(45) NOT NULL,
    apellido VARCHAR(45) NOT NULL,
    genero CHAR(1) NOT NULL CHECK (genero IN ('M','F')),
    edad INT NULL
);

CREATE TABLE profesion (
    id INT NOT NULL PRIMARY KEY,
    nom VARCHAR(90) NOT NULL,
    des TEXT NULL
);

CREATE TABLE estudios (
    id_prof INT NOT NULL,
    cc_per BIGINT NOT NULL,
    fecha DATE NULL,
    univer VARCHAR(50) NULL,

    CONSTRAINT PK_estudios PRIMARY KEY (id_prof, cc_per),

    CONSTRAINT FK_estudios_profesion
        FOREIGN KEY (id_prof)
        REFERENCES profesion(id),

    CONSTRAINT FK_estudios_persona
        FOREIGN KEY (cc_per)
        REFERENCES persona(cc)
);

CREATE TABLE telefono (
    num VARCHAR(15) NOT NULL PRIMARY KEY,
    oper VARCHAR(45) NOT NULL,
    duenio BIGINT NOT NULL,

    CONSTRAINT FK_telefono_persona
        FOREIGN KEY (duenio)
        REFERENCES persona(cc)
);

-- Datos de prueba

INSERT INTO persona VALUES
(1001, 'Juan', 'Perez', 'M', 25),
(1002, 'Maria', 'Gomez', 'F', 30);

INSERT INTO profesion VALUES
(1, 'Ingeniero', 'Ingenieria de sistemas'),
(2, 'Medico', 'Profesional de la salud');

INSERT INTO estudios VALUES
(1, 1001, '2020-06-15', 'Universidad Nacional'),
(2, 1002, '2018-11-20', 'Universidad del Valle');

INSERT INTO telefono VALUES
('3001112233', 'Claro', 1001),
('3014445566', 'Movistar', 1002);
