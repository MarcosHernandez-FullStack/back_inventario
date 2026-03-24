-- ============================================================
-- db_reset.sql — Limpia datos y reinicia identidades
-- Ejecutar sobre la BD existente (no la elimina)
-- ============================================================
USE db_tienda;
GO

-- Eliminar datos respetando FK: Producto -> Categoria -> Usuario
DELETE FROM dbo.Producto;
DELETE FROM dbo.Categoria;
DELETE FROM dbo.Usuario;
GO

-- Reiniciar contadores IDENTITY desde 1
DBCC CHECKIDENT ('dbo.Producto',  RESEED, 0);
DBCC CHECKIDENT ('dbo.Categoria', RESEED, 0);
DBCC CHECKIDENT ('dbo.Usuario',   RESEED, 0);
GO

-- ── Seed ─────────────────────────────────────────────────────
INSERT INTO dbo.Usuario (Nombres, Apellidos, Correo, Contrasena, Rol, CreadoPor)
VALUES (
    'Administrador', 'Sistema',
    'admin@inventario.com',
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', N'Admin123'), 2),
    'ADMINISTRADOR', 0
);

INSERT INTO dbo.Usuario (Nombres, Apellidos, Correo, Contrasena, Rol, CreadoPor)
VALUES (
    'Empleado', 'Prueba',
    'empleado@inventario.com',
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', N'Empleado123'), 2),
    'EMPLEADO', 1
);

INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Computadoras',   1);
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Monitores',      1);
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Periféricos',    1);
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Almacenamiento', 1);
INSERT INTO dbo.Categoria (Nombre, CreadoPor) VALUES ('Audio',          1);

INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Laptop HP 15"',     'Intel Core i5, 8GB RAM, 512GB SSD', 2899.90, 12, 1, 1);
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Monitor LG 27"',   'Full HD IPS, 75Hz',                  749.00,   8, 2, 1);
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Teclado Mecánico', 'Switch Blue, retroiluminado',         189.90,   3, 3, 1);
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Mouse Logitech MX','Inalámbrico, ergonómico',             229.00,  20, 3, 1);
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Disco SSD 1TB',    'Samsung 870 EVO SATA',                399.00,   5, 4, 1);
INSERT INTO dbo.Producto (Nombre, Descripcion, Precio, Cantidad, IdCategoria, CreadoPor)
VALUES ('Auriculares Sony', 'Noise Cancelling WH-1000XM5',         899.00,   4, 5, 1);
GO

PRINT 'Reset completado. Credenciales:';
PRINT '  Admin:    admin@inventario.com    / Admin123';
PRINT '  Empleado: empleado@inventario.com / Empleado123';
