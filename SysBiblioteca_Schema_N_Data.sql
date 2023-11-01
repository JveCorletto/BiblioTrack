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

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------- GESTIÓN ADMINISTRATIVA ----------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------

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

CREATE TABLE EstadosMultas(
	IdEstadoMulta INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	EstadoMulta VARCHAR(MAX) NOT NULL
)

SET IDENTITY_INSERT [EstadosMultas] ON 
INSERT [EstadosMultas] ([IdEstadoMulta], [EstadoMulta]) VALUES (1, N'No Pagada')
INSERT [EstadosMultas] ([IdEstadoMulta], [EstadoMulta]) VALUES (2, N'Pendiente')
INSERT [EstadosMultas] ([IdEstadoMulta], [EstadoMulta]) VALUES (3, N'Pagada')
SET IDENTITY_INSERT [EstadosMultas] OFF
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
VALUES	(1, 'André', 'Martínez', '05547481-4', 'jvecorletto@gmail.com', 'Mi Casa', '+50376674238', '02/06/1997'),
		(2, 'Angie', 'Díaz', '01234567-8', 'email@gmail.com', 'Su Casa', '+50301234567', '01/01/2001'),
		(3, 'Gabriela', 'Castillo', '12345678-9', 'email@gmail.com', 'Su Casa', '+50312345678', '01/01/2001'),
		(4, 'Diego', 'Acevedo', '23456789-0', 'email@gmail.com', 'Su Casa', '+50323456789', '01/01/2001'),
		(5, 'Wendy', 'Díaz', '34567890-1', 'email@gmail.com', 'Su Casa', '+50334567890', '01/01/2001'),
		(6, 'Adriana', 'Paola :v', '45678901-2', 'email@gmail.com', 'Su Casa', '+50345678901', '01/01/2001');
SET IDENTITY_INSERT [DatosPersonales] OFF
GO

CREATE TABLE [Usuarios](
	IdUsuario BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	IdRol INT NOT NULL FOREIGN KEY REFERENCES [Roles](IdRol),
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
INSERT [Usuarios] ([IdUsuario], [IdEstado], [IdRol], [IdDatosPersonales], [Usuario], [Contrasenia], [FechaCreacion], [UsuarioCreacion]) 
VALUES	(1, 1, 1, 1, 'jvemartinez', 'MQAyADMANAA=', GETDATE(), 'SysAdmin'),
		(2, 1, 2, 2, 'angie', 'MQAyADMANAA=', GETDATE(), 'SysAdmin'),
		(3, 1, 2, 3, 'gabriela', 'MQAyADMANAA=', GETDATE(), 'SysAdmin'),
		(4, 1, 3, 4, 'diego', 'MQAyADMANAA=', GETDATE(), 'SysAdmin'),
		(5, 1, 3, 5, 'wendy', 'MQAyADMANAA=', GETDATE(), 'SysAdmin'),
		(6, 1, 3, 6, 'adriana', 'MQAyADMANAA=', GETDATE(), 'SysAdmin');
SET IDENTITY_INSERT [Usuarios] OFF
GO

CREATE TABLE [Menus](
	IdMenu BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdParent BIGINT NULL,
	IdSubParent BIGINT NULL,
	Nombre VARCHAR(MAX) NULL,
	[Url] VARCHAR(MAX) NULL,
	Icono VARCHAR(MAX) NULL
)
GO

SET IDENTITY_INSERT [Menus] ON 
-- MENÚS PARA ADMINISTRADOR Y EMPLEADOS
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (1, 0, 0, N'Dashboard', N'/SysBiblioteca/Inicio', N'fa fa-store')
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (2, 0, 0, N'Prestamos y Devoluciones', NULL, N'fa fa-warehouse')
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (3, 0, 0, N'Inventario', NULL, N'fa fa-money-bill-wave')
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (4, 0, 0, N'Reportes', NULL, N'fa fa-money-bill-wave')
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (5, 0, 0, N'Seguridad', NULL, N'fas fa-shield-alt')

-- Módulo de Prestamos y Devoluciones
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (6, 2, 0, N'Prestamos', N'/PrestamosDevoluciones/Prestamos', NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (7, 2, 0, N'Devoluciones', N'/PrestamosDevoluciones/Devoluciones', NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (8, 2, 0, N'Pagos Mora', N'/PrestamosDevoluciones/Pagos', NULL)

-- Módulo de Inventario
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (9, 3, 0, N'Estantería', NULL, NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (10, 3, 0, N'Libros', NULL, NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (11, 3, 0, N'RFID', NULL, NULL)

-- Reportería
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (12, 4, 0, N'Usuarios', N'/Reportes/Usuarios', NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (13, 4, 0, N'Libros', N'/Reportes/Libros', NULL)

-- Seguridad
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (14, 5, 0, N'Usuarios', NULL, NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (15, 5, 0, N'Empleados', NULL, NULL)
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (16, 5, 0, N'Permisos', NULL, NULL)

-- MENÚ PARA USUARIOS NORMALES
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (17, 0, 0, N'Mis Libros', N'/Usuario/MisLibros', N'fa fa-store')
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (18, 0, 0, N'Mi Perfil', N'/Usuario/MiPerfil', N'fa fa-store')
INSERT [Menus] ([IdMenu], [IdParent], [IdSubParent], [Nombre], [Url], [Icono]) VALUES (19, 0, 0, N'Mis Pagos', N'/Usuario/Pagos', N'fa fa-store')
SET IDENTITY_INSERT [Menus] OFF

CREATE TABLE [Link_Rol_Menu](
	IdLinkRolMenu BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdRol INT NOT NULL FOREIGN KEY REFERENCES [Roles](IdRol),
	IdMenu BIGINT NOT NULL FOREIGN KEY REFERENCES [Menus](IdMenu),
	[Create] BIT NOT NULL DEFAULT(0),
	[Read] BIT NOT NULL DEFAULT(0),
	[Update] BIT NOT NULL DEFAULT(0),
	[Delete] BIT NOT NULL DEFAULT(0),
)
GO

SET IDENTITY_INSERT [Link_Rol_Menu] ON 
-- Menu para Rol de Administrador
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(1, 1, 1, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(2, 1, 2, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(3, 1, 3, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(4, 1, 4, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(5, 1, 5, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(6, 1, 6, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(7, 1, 7, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(8, 1, 8, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(9, 1, 9, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(10, 1, 10, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(11, 1, 11, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(12, 1, 12, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(13, 1, 13, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(14, 1, 14, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(15, 1, 15, 1, 1, 1, 1)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(16, 1, 16, 1, 1, 1, 1)

-- Menú para Rol de Empleado
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(17, 2, 1, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(18, 2, 2, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(19, 2, 3, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(20, 2, 4, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(21, 2, 6, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(22, 2, 7, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(23, 2, 8, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(24, 2, 9, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(25, 2, 10, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(26, 2, 11, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(27, 2, 12, 1, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(28, 2, 13, 1, 1, 1, 0)

-- Menú para Rol Usuarios
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(29, 3, 17, 1, 1, 0, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(30, 3, 18, 0, 1, 1, 0)
INSERT INTO [Link_Rol_Menu] (IdLinkRolMenu, IdRol, IdMenu, [Create], [Read], [Update], [Delete]) VALUES(31, 3, 19, 1, 1, 0, 0)
SET IDENTITY_INSERT [Link_Rol_Menu] OFF

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------- GESTIÓN LIBRERÍA ----------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Autores(
	IdAutor BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Autor VARCHAR(MAX) NOT NULL
);

CREATE TABLE Editoriales(
	IdEditorial BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Editorial VARCHAR(MAX) NOT NULL
);

CREATE TABLE Libros(
	IdLibro BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdAutor BIGINT NOT NULL FOREIGN KEY REFERENCES Autores(IdAutor),
	Editorial BIGINT NOT NULL FOREIGN KEY REFERENCES Editoriales(IdEditorial),
	FotoLibro NVARCHAR(MAX) NOT NULL,
	Libro VARCHAR(MAX) NOT NULL,
	[Version] VARCHAR(MAX) NULL,
	ISBN VARCHAR(MAX) NOT NULL,
	AnioPublicacion INT NOT NULL,
	Descripcion VARCHAR(MAX) NOT NULL,
	Cantidad INT NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)

CREATE TABLE Secciones(
	IdSeccion BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Seccion VARCHAR(MAX) NOT NULL,
)

CREATE TABLE Estanterias(
	IdEstanteria BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdSeccion BIGINT NOT NULL FOREIGN KEY REFERENCES Secciones(IdSeccion),
	Estanteria VARCHAR(MAX) NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)

CREATE TABLE Niveles(
	IdNivel BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstanteria BIGINT NOT NULL FOREIGN KEY REFERENCES Estanterias(IdEstanteria),
	Nivel INT NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)

CREATE TABLE Prestamos(
	IdPrestamo BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdUsuario BIGINT NOT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
	IdLibro BIGINT NOT NULL FOREIGN KEY REFERENCES Libros(IdLibro),
	Entregado BIT NOT NULL DEFAULT(0),
	FechaPrestamo DATETIME NOT NULL,
	DiasPrestamo INT NOT NULL,

	FechaDevolucion DATETIME NULL, 
	Finalizado BIT NOT NULL DEFAULT(0),
	EmpleadoValidacion BIGINT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario)
)

CREATE TABLE Multas(
	IdMulta BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdPrestamo BIGINT NOT NULL FOREIGN KEY REFERENCES Prestamos(IdPrestamo),
	IdEstadoMulta INT NOT NULL FOREIGN KEY REFERENCES EstadosMultas(IdEstadoMulta),
	
	EmpleadoValidacion BIGINT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
	ComprobantePago NVARCHAR(MAX) NULL,
	PagoFisico BIT NULL DEFAULT(0),
	FechaValidacion DATETIME NULL
)