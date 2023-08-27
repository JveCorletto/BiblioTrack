USE master
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SysBiblioteca')
	BEGIN
		DROP DATABASE SysBiblioteca
	END
GO

CREATE DATABASE SysBiblioteca
GO

USE SysBiblioteca
GO

CREATE TABLE [Estados](
	IdEstado INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Estado VARCHAR(MAX) NOT NULL
)
GO

SET IDENTITY_INSERT [Estados] ON 
INSERT [Estados] ([IdEstado], [Estado]) VALUES (1, N'Activo')
INSERT [Estados] ([IdEstado], [Estado]) VALUES (2, N'Inactivo')
SET IDENTITY_INSERT [Estados] OFF
GO

CREATE TABLE [Roles](
	IdRol INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	Rol VARCHAR(MAX) NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)
GO

SET IDENTITY_INSERT [Roles] ON 
INSERT [Roles] ([IdRol], [IdEstado], [Rol], [UsuarioCreacion], [FechaCreacion]) VALUES (1, 1, N'Administrador', 'SysAdmin', GETDATE())
INSERT [Roles] ([IdRol], [IdEstado], [Rol], [UsuarioCreacion], [FechaCreacion]) VALUES (2, 1, N'Empleado', 'SysAdmin', GETDATE())
INSERT [Roles] ([IdRol], [IdEstado], [Rol], [UsuarioCreacion], [FechaCreacion]) VALUES (3, 1, N'Usuario', 'SysAdmin', GETDATE())
SET IDENTITY_INSERT [Roles] OFF
GO

CREATE TABLE [Cargos](
	IdCargo INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	Cargo VARCHAR(MAX) NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)
GO

SET IDENTITY_INSERT [Cargos] ON 
INSERT [Cargos] ([IdCargo], [IdEstado], [Cargo], [UsuarioCreacion], [FechaCreacion]) VALUES (1, 1, N'Gerente', 'SysAdmin', GETDATE())
INSERT [Cargos] ([IdCargo], [IdEstado], [Cargo], [UsuarioCreacion], [FechaCreacion]) VALUES (2, 1, N'Librero', 'SysAdmin', GETDATE())
INSERT [Cargos] ([IdCargo], [IdEstado], [Cargo], [UsuarioCreacion], [FechaCreacion]) VALUES (3, 1, N'Ordenanza', 'SysAdmin', GETDATE())
SET IDENTITY_INSERT [Cargos] OFF
GO

CREATE TABLE DatosPersonales(
	IdDatosPersonales BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombres VARCHAR(MAX) NOT NULL,
	Apellidos VARCHAR(MAX) NOT NULL,
	DUI VARCHAR(MAX) NOT NULL,
	Correo VARCHAR(MAX) NOT NULL,
	Direccion VARCHAR(MAX) NOT NULL,
	Telefono VARCHAR(MAX) NOT NULL,
	FechaNacimiento VARCHAR(MAX) NOT NULL,
)

SET IDENTITY_INSERT [DatosPersonales] ON 
INSERT INTO [DatosPersonales] ([IdDatosPersonales], [Nombres], [Apellidos], [DUI], [Correo], [Direccion], [Telefono], [FechaNacimiento]) 
VALUES	(1, 'Javier André', 'Martínez Melgar', '05547481-4', 'est.j5martinez@gmail.com', 'Mi Casa', '+50376674238', '02/06/1997'),
		(2, 'Rosa Graciela', 'Calederón Juarez', '012345678-9', 'rosagracielacalderon@gmail.com', 'Su Casa', '+50312345678', '26/05/2000');
SET IDENTITY_INSERT [DatosPersonales] OFF
GO

CREATE TABLE [Usuarios](
	IdUsuario BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	IdRol INT NOT NULL FOREIGN KEY REFERENCES [Roles](IdRol),
	IdCargo INT NULL FOREIGN KEY REFERENCES [Cargos](IdCargo),
	IdDatosPersonales BIGINT NOT NULL FOREIGN KEY REFERENCES [DatosPersonales](IdDatosPersonales),

	Usuario VARCHAR(MAX) NOT NULL,
	Contrasenia VARCHAR(MAX) NOT NULL,
	Token VARCHAR(MAX) NULL,
	UltimoAcceso DATETIME NULL,
	ConteoIntentos INT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)
GO

SET IDENTITY_INSERT [Usuarios] ON 
INSERT [Usuarios] ([IdUsuario], [IdEstado], [IdRol], [IdCargo], [IdDatosPersonales], [Usuario], [Contrasenia], [FechaCreacion], [UsuarioCreacion]) 
VALUES	(1, 1, 1, 1, 1, 'jvemartinez', 'MQAyADMANAA=', GETDATE(), 'SysAdmin'),
		(2, 1, 3, 2, 2, 'graciela', 'MQAyADMANAA=', GETDATE(), 'SysAdmin')
SET IDENTITY_INSERT [Usuarios] OFF
GO