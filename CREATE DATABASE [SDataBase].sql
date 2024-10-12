USE [master]
GO

CREATE DATABASE [SDataBase]
GO

USE [SDataBase]
GO

CREATE TABLE [dbo].[tRol](
	[Consecutivo] [smallint] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tRol] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tUsuario](
	[Consecutivo] [bigint] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](20) NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[CorreoElectronico] [varchar](80) NOT NULL,
	[Contrasenna] [varchar](255) NOT NULL,
	[Activo] [bit] NOT NULL,
	[ConsecutivoRol] [smallint] NOT NULL,
 CONSTRAINT [PK_tUsuario] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[tRol] ON 
GO
INSERT [dbo].[tRol] ([Consecutivo], [NombreRol]) VALUES (1, N'Administradores')
GO
INSERT [dbo].[tRol] ([Consecutivo], [NombreRol]) VALUES (2, N'Clientes')
GO
SET IDENTITY_INSERT [dbo].[tRol] OFF
GO

SET IDENTITY_INSERT [dbo].[tUsuario] ON 
GO
INSERT [dbo].[tUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Activo], [ConsecutivoRol]) VALUES (1, N'304590415', N'Eduardo Calvo Castillo', N'ecalvo90415@ufide.ac.cr', N'90415', 1, 2)
GO
INSERT [dbo].[tUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Activo], [ConsecutivoRol]) VALUES (4, N'304590416', N'Eduardo Calvo Castillo', N'ecalvo90416@ufide.ac.cr', N'90416', 1, 2)
GO
INSERT [dbo].[tUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Activo], [ConsecutivoRol]) VALUES (5, N'304590417', N'Eduardo Calvo Castillo', N'ecalvo90417@ufide.ac.cr', N'90417', 1, 2)
GO
SET IDENTITY_INSERT [dbo].[tUsuario] OFF
GO

ALTER TABLE [dbo].[tUsuario] ADD  CONSTRAINT [UK_Correo] UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tUsuario] ADD  CONSTRAINT [UK_Identificacion] UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tUsuario]  WITH CHECK ADD  CONSTRAINT [FK_tUsuario_tRol] FOREIGN KEY([ConsecutivoRol])
REFERENCES [dbo].[tRol] ([Consecutivo])
GO
ALTER TABLE [dbo].[tUsuario] CHECK CONSTRAINT [FK_tUsuario_tRol]
GO

CREATE PROCEDURE [dbo].[CrearCuenta]
	@Identificacion varchar(20),
	@Nombre varchar(255),
	@CorreoElectronico varchar(80),
	@Contrasenna varchar(255)
AS
BEGIN
	
	DECLARE @Activo BIT = 1
	DECLARE @ConsecutivoRol TINYINT

	SELECT	@ConsecutivoRol = Consecutivo
	FROM	dbo.tRol
	WHERE	NombreRol = 'Clientes'

	IF NOT EXISTS(SELECT 1 FROM dbo.tUsuario
				  WHERE Identificacion = @Identificacion
					OR CorreoElectronico = @CorreoElectronico)
	BEGIN

		INSERT INTO dbo.tUsuario (Identificacion,Nombre,CorreoElectronico,Contrasenna,Activo,ConsecutivoRol)
		VALUES (@Identificacion, @Nombre, @CorreoElectronico, @Contrasenna, @Activo, @ConsecutivoRol)

	END
END
GO