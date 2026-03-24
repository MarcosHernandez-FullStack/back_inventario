-- ============================================================
-- db_inventario — Setup completo
-- Ejecutar sobre SQL Server (crear la BD primero si no existe)
-- ============================================================
USE db_tienda;
GO

-- ── Tablas ──────────────────────────────────────────────────

IF OBJECT_ID('dbo.Producto',  'U') IS NOT NULL DROP TABLE dbo.Producto;
IF OBJECT_ID('dbo.Categoria', 'U') IS NOT NULL DROP TABLE dbo.Categoria;
IF OBJECT_ID('dbo.Usuario',   'U') IS NOT NULL DROP TABLE dbo.Usuario;
GO

CREATE TABLE dbo.Usuario
(
    Id                INT           IDENTITY(1,1) NOT NULL,
    Nombres           NVARCHAR(100) NOT NULL,
    Apellidos         NVARCHAR(100) NOT NULL,
    Celular           NVARCHAR(20)  NULL,
    Correo            NVARCHAR(150) NOT NULL,
    Contrasena        NVARCHAR(64)  NOT NULL,          -- SHA2_256 hex
    Rol               NVARCHAR(50)  NOT NULL,
    Estado            NVARCHAR(10)  NOT NULL CONSTRAINT CK_Usuario_Estado   CHECK (Estado IN ('ACTIVO', 'INACTIVO'))
                                             CONSTRAINT DF_Usuario_Estado   DEFAULT 'ACTIVO',
    FechaCreacion     DATETIME2(0)  NOT NULL CONSTRAINT DF_Usuario_FechaCreacion     DEFAULT (SYSDATETIME()),
    FechaActualizacion DATETIME2(0) NULL,
    CreadoPor         NVARCHAR(100) NOT NULL,
    ActualizadoPor    NVARCHAR(100) NULL,
    CONSTRAINT PK_Usuario PRIMARY KEY (Id),
    CONSTRAINT UQ_Usuario_Correo UNIQUE (Correo)
);
GO

CREATE TABLE dbo.Categoria
(
    Id                INT           IDENTITY(1,1) NOT NULL,
    Nombre            NVARCHAR(100) NOT NULL,
    Estado            NVARCHAR(10)  NOT NULL CONSTRAINT CK_Categoria_Estado CHECK (Estado IN ('ACTIVO','INACTIVO'))
                                             CONSTRAINT DF_Categoria_Estado DEFAULT 'ACTIVO',
    FechaCreacion     DATETIME2(0)  NOT NULL CONSTRAINT DF_Categoria_FechaCreacion DEFAULT (SYSDATETIME()),
    FechaActualizacion DATETIME2(0) NULL,
    CreadoPor         NVARCHAR(100) NOT NULL,
    ActualizadoPor    NVARCHAR(100) NULL,
    CONSTRAINT PK_Categoria PRIMARY KEY (Id)
);
GO

CREATE TABLE dbo.Producto
(
    Id                INT            IDENTITY(1,1) NOT NULL,
    Nombre            NVARCHAR(150)  NOT NULL,
    Descripcion       NVARCHAR(500)  NULL,
    Precio            DECIMAL(10,2)  NOT NULL,
    Cantidad          INT            NOT NULL,
    IdCategoria       INT            NOT NULL,
    Estado            NVARCHAR(10)   NOT NULL CONSTRAINT CK_Producto_Estado    CHECK (Estado IN ('ACTIVO','INACTIVO'))
                                              CONSTRAINT DF_Producto_Estado    DEFAULT 'ACTIVO',
    FechaCreacion     DATETIME2(0)   NOT NULL CONSTRAINT DF_Producto_FechaCreacion DEFAULT (SYSDATETIME()),
    FechaActualizacion DATETIME2(0)  NULL,
    CreadoPor         NVARCHAR(100)  NOT NULL,
    ActualizadoPor    NVARCHAR(100)  NULL,
    CONSTRAINT PK_Producto      PRIMARY KEY (Id),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES dbo.Categoria(Id),
    CONSTRAINT CK_Producto_Precio    CHECK (Precio    >= 0),
    CONSTRAINT CK_Producto_Cantidad  CHECK (Cantidad  >= 0)
);
GO

-- ── Stored Procedures ────────────────────────────────────────

-- ── Auth ────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE dbo.sp_LoginUsuario
    @Correo     NVARCHAR(150),
    @Contrasena NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombres, Apellidos, Correo, Rol
    FROM   dbo.Usuario
    WHERE  Correo     = @Correo
      AND  Contrasena = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @Contrasena), 2)
      AND  Estado     = 'ACTIVO';
END;
GO

-- ── Usuarios ─────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE dbo.sp_ListarUsuarios
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombres, Apellidos, Celular, Correo, Rol, Estado
    FROM   dbo.Usuario
    ORDER  BY Nombres, Apellidos;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ObtenerUsuario
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombres, Apellidos, Celular, Correo, Rol, Estado
    FROM   dbo.Usuario
    WHERE  Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CrearUsuario
    @Nombres    NVARCHAR(100),
    @Apellidos  NVARCHAR(100),
    @Celular    NVARCHAR(20)  = NULL,
    @Correo     NVARCHAR(150),
    @Contrasena NVARCHAR(200),
    @Rol        NVARCHAR(50),
    @CreadoPor  NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Usuario (Nombres, Apellidos, Celular, Correo, Contrasena, Rol, CreadoPor)
    VALUES (
        @Nombres, @Apellidos, @Celular, @Correo,
        CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @Contrasena), 2),
        @Rol, @CreadoPor
    );
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarEstadoUsuario
    @Id            INT,
    @Estado        NVARCHAR(10),
    @ActualizadoPor NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Usuario
    SET    Estado             = @Estado,
           FechaActualizacion = SYSDATETIME(),
           ActualizadoPor     = @ActualizadoPor
    WHERE  Id = @Id;
END;
GO

-- ── Categorías ───────────────────────────────────────────────
CREATE OR ALTER PROCEDURE dbo.sp_ListarCategorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Estado
    FROM   dbo.Categoria
    ORDER  BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ObtenerCategoria
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Estado
    FROM   dbo.Categoria
    WHERE  Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CrearCategoria
    @Nombre    NVARCHAR(100),
    @CreadoPor NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Categoria (Nombre, CreadoPor)
    VALUES (@Nombre, @CreadoPor);
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarCategoria
    @Id             INT,
    @Nombre         NVARCHAR(100),
    @ActualizadoPor NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Categoria
    SET    Nombre             = @Nombre,
           FechaActualizacion = SYSDATETIME(),
           ActualizadoPor     = @ActualizadoPor
    WHERE  Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_EliminarCategoria
    @Id INT
AS
BEGIN
    -- Soft delete: inactiva la categoría
    UPDATE dbo.Categoria
    SET    Estado = 'INACTIVO'
    WHERE  Id = @Id;
END;
GO

-- ── Productos ────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE dbo.sp_ListarProductos
    @Nombre      NVARCHAR(150) = NULL,
    @IdCategoria INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.Cantidad,
           p.IdCategoria, c.Nombre AS NombreCategoria, p.Estado
    FROM   dbo.Producto p
    INNER  JOIN dbo.Categoria c ON c.Id = p.IdCategoria
    WHERE  (@Nombre      IS NULL OR p.Nombre      LIKE '%' + @Nombre + '%')
      AND  (@IdCategoria IS NULL OR p.IdCategoria = @IdCategoria)
    ORDER  BY p.Estado DESC, p.Nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ObtenerProducto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.Cantidad,
           p.IdCategoria, c.Nombre AS NombreCategoria, p.Estado
    FROM   dbo.Producto p
    INNER  JOIN dbo.Categoria c ON c.Id = p.IdCategoria
    WHERE  p.Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CrearProducto
    @Nombre      NVARCHAR(150),
    @Descripcion NVARCHAR(500) = NULL,
    @Precio      DECIMAL(10,2),
    @Cantidad    INT,
    @IdCategoria INT,
    @CreadoPor   NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
    VALUES (@Nombre, @Descripcion, @Precio, @Cantidad, @IdCategoria, @CreadoPor);
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarProducto
    @Id             INT,
    @Nombre         NVARCHAR(150),
    @Descripcion    NVARCHAR(500) = NULL,
    @Precio         DECIMAL(10,2),
    @Cantidad       INT,
    @IdCategoria    INT,
    @ActualizadoPor NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Producto
    SET    Nombre             = @Nombre,
           Descripcion        = @Descripcion,
           Precio             = @Precio,
           Cantidad           = @Cantidad,
           IdCategoria        = @IdCategoria,
           FechaActualizacion = SYSDATETIME(),
           ActualizadoPor     = @ActualizadoPor
    WHERE  Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_EliminarProducto
    @Id INT
AS
BEGIN
    -- Soft delete
    UPDATE dbo.Producto
    SET    Estado = 'INACTIVO'
    WHERE  Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CambiarEstadoProducto
    @Id             INT,
    @Estado         NVARCHAR(10),
    @ActualizadoPor NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Producto
    SET    Estado             = @Estado,
           FechaActualizacion = SYSDATETIME(),
           ActualizadoPor     = @ActualizadoPor
    WHERE  Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ListarProductosBajoStock
    @Umbral INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.Cantidad,
           p.IdCategoria, c.Nombre AS NombreCategoria, p.Estado
    FROM   dbo.Producto p
    INNER  JOIN dbo.Categoria c ON c.Id = p.IdCategoria
    WHERE  p.Estado   = 'ACTIVO'
      AND  p.Cantidad <= @Umbral
    ORDER  BY p.Cantidad;
END;
GO

-- ── Reportes ─────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE dbo.sp_ResumenDashboard
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        (SELECT COUNT(*) FROM dbo.Producto  WHERE Estado = 'ACTIVO') AS TotalProductos,
        (SELECT COUNT(*) FROM dbo.Categoria WHERE Estado = 'ACTIVO') AS TotalCategorias,
        (SELECT COUNT(*) FROM dbo.Usuario   WHERE Estado = 'ACTIVO') AS TotalUsuarios,
        (SELECT ISNULL(SUM(Precio * Cantidad), 0) FROM dbo.Producto WHERE Estado = 'ACTIVO') AS ValorInventario;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ReporteStockPorCategoria
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        c.Nombre                        AS Categoria,
        COUNT(p.Id)                     AS TotalProductos,
        ISNULL(SUM(p.Cantidad), 0)      AS StockTotal,
        ISNULL(SUM(p.Precio * p.Cantidad), 0) AS ValorTotal
    FROM  dbo.Categoria c
    LEFT  JOIN dbo.Producto p ON p.IdCategoria = c.Id AND p.Estado = 'ACTIVO'
    WHERE c.Estado = 'ACTIVO'
    GROUP BY c.Id, c.Nombre
    ORDER BY c.Nombre;
END;
GO

-- ── Seed ─────────────────────────────────────────────────────
-- Admin: admin@inventario.com / Admin123
INSERT INTO dbo.Usuario (Nombres, Apellidos, Correo, Contrasena, Rol, CreadoPor)
VALUES (
    'Administrador', 'Sistema',
    'admin@inventario.com',
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', N'Admin123'), 2),
    'ADMINISTRADOR', 'SISTEMA'
);

-- Empleado: empleado@inventario.com / Empleado123
INSERT INTO dbo.Usuario (Nombres, Apellidos, Correo, Contrasena, Rol, CreadoPor)
VALUES (
    'Empleado', 'Prueba',
    'empleado@inventario.com',
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', N'Empleado123'), 2),
    'EMPLEADO', 'SISTEMA'
);

-- Categorías
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Computadoras',   'SISTEMA');
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Monitores',      'SISTEMA');
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Periféricos',    'SISTEMA');
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Almacenamiento', 'SISTEMA');
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Audio',          'SISTEMA');

-- Productos
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Laptop HP 15"',     'Intel Core i5, 8GB RAM, 512GB SSD', 2899.90, 12, 1, 'SISTEMA');
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Monitor LG 27"',   'Full HD IPS, 75Hz',                  749.00,   8, 2, 'SISTEMA');
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Teclado Mecánico', 'Switch Blue, retroiluminado',         189.90,   3, 3, 'SISTEMA');
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Mouse Logitech MX','Inalámbrico, ergonómico',             229.00,  20, 3, 'SISTEMA');
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Disco SSD 1TB',    'Samsung 870 EVO SATA',                399.00,   5, 4, 'SISTEMA');
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Auriculares Sony', 'Noise Cancelling WH-1000XM5',         899.00,   4, 5, 'SISTEMA');
GO

PRINT 'Setup completado. Credenciales de prueba:';
PRINT '  Admin:   admin@inventario.com   / Admin123';
PRINT '  Empleado: empleado@inventario.com / Empleado123';
