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

CREATE TABLE [Roles](
	IdRol INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	Rol VARCHAR(MAX) NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)

CREATE TABLE Generos(
	IdGenero INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Genero VARCHAR(MAX) NOT NULL
)

CREATE TABLE DatosPersonales(
	IdDatosPersonales BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdGenero INT NOT NULL FOREIGN KEY REFERENCES Generos(IdGenero),
	Nombres VARCHAR(MAX) NOT NULL,
	Apellidos VARCHAR(MAX) NOT NULL,
	DUI VARCHAR(MAX) NOT NULL,
	Correo VARCHAR(MAX) NOT NULL,
	Direccion VARCHAR(MAX) NOT NULL,
	Telefono VARCHAR(MAX) NOT NULL,
	FechaNacimiento VARCHAR(MAX) NOT NULL
)

CREATE TABLE [Cargos](
	IdCargo INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	Cargo VARCHAR(MAX) NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)

CREATE TABLE [Usuarios](
	IdUsuario BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdRol INT NOT NULL FOREIGN KEY REFERENCES [Roles](IdRol),
	IdCargo INT NULL FOREIGN KEY REFERENCES [Cargos](IdCargo),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
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

CREATE TABLE [Menus](
	IdMenu BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdParent BIGINT NULL,
	IdSubParent BIGINT NULL,
	Nombre VARCHAR(MAX) NULL,
	[Url] VARCHAR(MAX) NULL,
	Icono VARCHAR(MAX) NULL
)

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

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------- GESTIÓN LIBRERÍA ----------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Autores(
	IdAutor BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Autor VARCHAR(MAX) NOT NULL
)

CREATE TABLE GenerosLiterarios(
	IdGenero BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Genero VARCHAR(MAX) NOT NULL
)

CREATE TABLE Editoriales(
	IdEditorial BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Editorial VARCHAR(MAX) NOT NULL
)

CREATE TABLE Libros(
	IdLibro BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEstado INT NOT NULL FOREIGN KEY REFERENCES [Estados](IdEstado),
	IdEditorial BIGINT NOT NULL FOREIGN KEY REFERENCES Editoriales(IdEditorial),
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

CREATE TABLE AutoresLibros(
	IdAutorLibro BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdLibro BIGINT NOT NULL FOREIGN KEY REFERENCES Libros(IdLibro),
	IdAutor BIGINT NOT NULL FOREIGN KEY REFERENCES Autores(IdAutor)
)

CREATE TABLE GenerosLibros(
	IdGeneroLibro BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdLibro BIGINT NOT NULL FOREIGN KEY REFERENCES Libros(IdLibro),
	IdGenero BIGINT NOT NULL FOREIGN KEY REFERENCES GenerosLiterarios(IdGenero)
)

CREATE TABLE Secciones(
	IdSeccion BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Seccion VARCHAR(MAX) NOT NULL,

	UsuarioCreacion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL,
	UsuarioModificacion VARCHAR(MAX) NULL,
	FechaModificacion DATETIME NULL
)

CREATE TABLE Estanterias(
	IdEstanteria BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdSeccion BIGINT NOT NULL FOREIGN KEY REFERENCES Secciones(IdSeccion),
	Estanteria INT NOT NULL,

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

CREATE TABLE Ubicaciones(
	IdUbicacion BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdLibro BIGINT NOT NULL FOREIGN KEY REFERENCES Libros(IdLibro),
	IdNivel BIGINT NOT NULL FOREIGN KEY REFERENCES Niveles(IdNivel),
)
GO

CREATE TABLE Prestamos(
	IdPrestamo BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdUsuario BIGINT NOT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
	IdLibro BIGINT NOT NULL FOREIGN KEY REFERENCES Libros(IdLibro),
	DiasPrestamo INT NOT NULL,
	FechaPrestamo DATETIME NULL,
	Entregado BIT NOT NULL DEFAULT(0),
	IdUsuarioEntrego BIGINT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),

	FechaDevolucion DATETIME NULL, 
	Finalizado BIT NOT NULL DEFAULT(0),
	IdUsuarioRecibio BIGINT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario)
)

CREATE TABLE EstadosMultas(
	IdEstadoMulta INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	EstadoMulta VARCHAR(MAX) NOT NULL
)

CREATE TABLE Multas(
	IdMulta BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdPrestamo BIGINT NOT NULL FOREIGN KEY REFERENCES Prestamos(IdPrestamo),
	IdEstadoMulta INT NOT NULL FOREIGN KEY REFERENCES EstadosMultas(IdEstadoMulta),

	DiasRetraso INT NOT NULL,
	Monto DECIMAL(10,2) NULL DEFAULT(0.00),
	IdUsuarioValidacion BIGINT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
	ComprobantePago NVARCHAR(MAX) NULL,
	PagoFisico BIT NULL DEFAULT(0),
	FechaValidacion DATETIME NULL
)
GO