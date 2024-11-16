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
	[UsaClaveTemp] [bit] NOT NULL,
	[Vigencia] [datetime] NOT NULL,
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
INSERT [dbo].[tUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Activo], [ConsecutivoRol], [UsaClaveTemp], [Vigencia]) VALUES (7, N'304590415', N'EDUARDO JOSE CALVO CASTILLO', N'ecalvo90415@ufide.ac.cr', N'vq6iL/cYD92ZbcRTmCttQA==', 1, 1, 0, CAST(N'2024-11-16T08:48:07.430' AS DateTime))
GO
INSERT [dbo].[tUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Activo], [ConsecutivoRol], [UsaClaveTemp], [Vigencia]) VALUES (8, N'304590416', N'FRANCINI DE LOS ANGELES ROMERO ARAYA', N'ecalvo90416@ufide.ac.cr', N'Sgzn/gAnGHGe55pefazs6Q==', 1, 2, 0, CAST(N'2024-11-16T08:48:39.217' AS DateTime))
GO
INSERT [dbo].[tUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Activo], [ConsecutivoRol], [UsaClaveTemp], [Vigencia]) VALUES (9, N'304590417', N'MERCEDES FRANCISCA MAROTO HERNANDEZ', N'ecalvo90417@ufide.ac.cr', N'b+SdtkM0Embs98KX7RA5fA==', 1, 2, 0, CAST(N'2024-11-16T10:16:23.780' AS DateTime))
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

CREATE PROCEDURE [dbo].[ActualizarContrasenna]
	@Consecutivo bigint,
	@Contrasenna varchar(255),
	@UsaClaveTemp bit,
	@Vigencia datetime
AS
BEGIN
	
	UPDATE	dbo.tUsuario
	SET		Contrasenna = @Contrasenna,
			UsaClaveTemp = @UsaClaveTemp,
			Vigencia = @Vigencia
	WHERE	Consecutivo = @Consecutivo

END
GO

CREATE PROCEDURE [dbo].[ActualizarPerfil]
	@Consecutivo		bigint,
	@Identificacion		varchar(20),
	@Nombre				varchar(255),
	@CorreoElectronico	varchar(80),
	@ConsecutivoRol		smallint
AS
BEGIN
	
	IF NOT EXISTS(SELECT 1 FROM dbo.tUsuario
				  WHERE (Identificacion = @Identificacion OR CorreoElectronico = @CorreoElectronico)
					AND Consecutivo != @Consecutivo)
	BEGIN

		UPDATE	dbo.tUsuario
		SET		Identificacion = @Identificacion,
				Nombre = @Nombre,
				CorreoElectronico = @CorreoElectronico,
				ConsecutivoRol = CASE WHEN @ConsecutivoRol != 0
									  THEN @ConsecutivoRol
									  ELSE ConsecutivoRol END
		WHERE	Consecutivo = @Consecutivo

	END

END
GO

CREATE PROCEDURE [dbo].[ConsultarUsuario]
	@Consecutivo BIGINT
AS
BEGIN
	
	SELECT	U.Consecutivo,
			Identificacion,
			Nombre,
			CorreoElectronico,
			Activo,
			ConsecutivoRol,
			R.NombreRol
	  FROM	dbo.tUsuario U
	  INNER JOIN dbo.tRol R ON U.ConsecutivoRol = R.Consecutivo
	  WHERE U.Consecutivo = @Consecutivo

END
GO

CREATE PROCEDURE [dbo].[ConsultarUsuarios]
	
AS
BEGIN
	
	SELECT	U.Consecutivo,
			Identificacion,
			Nombre,
			CorreoElectronico,
			Activo,
			ConsecutivoRol,
			R.NombreRol
	  FROM	dbo.tUsuario U
	  INNER JOIN dbo.tRol R ON U.ConsecutivoRol = R.Consecutivo

END
GO

CREATE PROCEDURE [dbo].[CrearCuenta]
	@Identificacion varchar(20),
	@Nombre varchar(255),
	@CorreoElectronico varchar(80),
	@Contrasenna varchar(255)
AS
BEGIN
	
	DECLARE @Activo BIT = 1,
			@ConsecutivoRol TINYINT,
			@UsaClaveTemp BIT = 0

	SELECT	@ConsecutivoRol = Consecutivo
	FROM	dbo.tRol
	WHERE	NombreRol = 'Clientes'

	IF NOT EXISTS(SELECT 1 FROM dbo.tUsuario
				  WHERE Identificacion = @Identificacion
					OR CorreoElectronico = @CorreoElectronico)
	BEGIN

		INSERT INTO dbo.tUsuario (Identificacion,Nombre,CorreoElectronico,Contrasenna,Activo,ConsecutivoRol,UsaClaveTemp,Vigencia)
		VALUES (@Identificacion, @Nombre, @CorreoElectronico, @Contrasenna, @Activo, @ConsecutivoRol, @UsaClaveTemp, GETDATE())

	END
END
GO

CREATE PROCEDURE [dbo].[IniciarSesion]
	@CorreoElectronico varchar(80),
	@Contrasenna varchar(255)
AS
BEGIN
	
	SELECT	U.Consecutivo,
			Identificacion,
			Nombre,
			CorreoElectronico,
			Activo,
			ConsecutivoRol,
			R.NombreRol,
			UsaClaveTemp,
			Vigencia
	  FROM	dbo.tUsuario U
	  INNER JOIN dbo.tRol R ON U.ConsecutivoRol = R.Consecutivo
	  WHERE CorreoElectronico = @CorreoElectronico
		AND Contrasenna = @Contrasenna
		AND Activo = 1
	
END
GO

CREATE PROCEDURE [dbo].[ValidarUsuario]
	@CorreoElectronico varchar(80)
AS
BEGIN
	
	SELECT	U.Consecutivo,
			Identificacion,
			Nombre,
			CorreoElectronico,
			Activo,
			ConsecutivoRol,
			R.NombreRol
	  FROM	dbo.tUsuario U
	  INNER JOIN dbo.tRol R ON U.ConsecutivoRol = R.Consecutivo
	  WHERE CorreoElectronico = @CorreoElectronico
	
END
GO