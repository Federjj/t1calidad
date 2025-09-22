
-- Crear la base de datos
CREATE DATABASE CafeAroma;
GO

USE CafeAroma;
GO

-- Tabla: Roles
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);
GO

-- Tabla: Categorias
CREATE TABLE Categorias (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);
GO

-- Tabla: Clientes
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(200) NULL
);
GO

-- Tabla: Articulos
CREATE TABLE Articulos (
    IdArticulo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    ImagenUrl NVARCHAR(300) NULL,
    CategoriaId INT NOT NULL,
    CONSTRAINT FK_Articulos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(IdCategoria)
);
GO

-- Tabla: Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    RolID INT NOT NULL,
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (RolID) REFERENCES Roles(IdRol)
);
GO

-- Tabla: Pedidos
CREATE TABLE Pedidos (
    IdPedido INT IDENTITY(1,1) PRIMARY KEY,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    Observaciones NVARCHAR(500) NULL,
    FechaPedido DATETIME2 NOT NULL DEFAULT GETDATE(),
    ClienteId INT NOT NULL,
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(IdCliente)
);
GO

-- Tabla: DetallesPedido
CREATE TABLE DetallesPedido (
    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,
    Cantidad INT NOT NULL,
    IdArticulo INT NOT NULL,
    PedidoId INT NOT NULL,
    CONSTRAINT FK_DetallesPedido_Articulos FOREIGN KEY (IdArticulo) REFERENCES Articulos(IdArticulo),
    CONSTRAINT FK_DetallesPedido_Pedidos FOREIGN KEY (PedidoId) REFERENCES Pedidos(IdPedido)
);
GO

-- Insertar datos iniciales para Roles
INSERT INTO Roles (Nombre) VALUES 
('Admin'),
('Usuario');
GO

-- Insertar categorías iniciales
INSERT INTO Categorias (Nombre) VALUES 
('Cafés'),
('Tés'),
('Bebidas Frías'),
('Postres'),
('Sandwiches');
GO

-- Insertar datos de ejemplo para productos
INSERT INTO Articulos (Nombre, Descripcion, Precio, Stock, CategoriaId) VALUES 
('Café Americano', 'Café negro fuerte y aromático', 8.50, 100, 1),
('Café Latte', 'Café con leche vaporizada y espuma', 12.00, 80, 1),
('Té Verde', 'Té verde natural con propiedades antioxidantes', 7.00, 50, 2),
('Capuchino', 'Café espresso con leche vaporizada y espuma', 10.50, 60, 1),
('Brownie de Chocolate', 'Delicioso brownie casero con nueces', 9.00, 30, 4),
('Sandwich de Pollo', 'Sandwich de pollo a la plancha con vegetales', 15.00, 25, 5);
GO

-- Insertar cliente de ejemplo
INSERT INTO Clientes (Nombre, Direccion) VALUES 
('Cliente Ejemplo', 'Av. Principal 123, Lima');
GO

-- Insertar usuario administrador
INSERT INTO Usuarios (Nombre, Apellido, Correo, Password, RolID) VALUES 
('Admin', 'Sistema', 'admin@cafearoma.com', 'admin123', 1);
GO

-- Crear índices para mejorar el rendimiento
CREATE INDEX IX_Articulos_CategoriaId ON Articulos(CategoriaId);
CREATE INDEX IX_Pedidos_ClienteId ON Pedidos(ClienteId);
CREATE INDEX IX_Pedidos_FechaPedido ON Pedidos(FechaPedido);
CREATE INDEX IX_Pedidos_Estado ON Pedidos(Estado);
CREATE INDEX IX_DetallesPedido_PedidoId ON DetallesPedido(PedidoId);
CREATE INDEX IX_DetallesPedido_ArticuloId ON DetallesPedido(IdArticulo);
GO

-- Vista para consultar pedidos con información completa
CREATE VIEW VistaPedidosCompletos AS
SELECT 
    p.IdPedido,
    p.Estado,
    p.Observaciones,
    p.FechaPedido,
    c.Nombre AS ClienteNombre,
    c.Direccion AS ClienteDireccion,
    a.Nombre AS ProductoNombre,
    dp.Cantidad,
    a.Precio,
    (dp.Cantidad * a.Precio) AS Subtotal
FROM Pedidos p
INNER JOIN Clientes c ON p.ClienteId = c.IdCliente
INNER JOIN DetallesPedido dp ON p.IdPedido = dp.PedidoId
INNER JOIN Articulos a ON dp.IdArticulo = a.IdArticulo;
GO

-- Procedimiento almacenado para actualizar stock automáticamente
CREATE PROCEDURE sp_ActualizarStock
    @ArticuloId INT,
    @CantidadVendida INT
AS
BEGIN
    UPDATE Articulos 
    SET Stock = Stock - @CantidadVendida 
    WHERE IdArticulo = @ArticuloId AND Stock >= @CantidadVendida;
    
    IF @@ROWCOUNT = 0
        RAISERROR('Stock insuficiente para el artículo', 16, 1);
END;
GO

-- Procedimiento almacenado para obtener productos disponibles
CREATE PROCEDURE sp_ObtenerProductosDisponibles
AS
BEGIN
    SELECT 
        a.IdArticulo,
        a.Nombre,
        a.Descripcion,
        a.Precio,
        a.Stock,
        a.ImagenUrl,
        c.Nombre AS CategoriaNombre
    FROM Articulos a
    INNER JOIN Categorias c ON a.CategoriaId = c.IdCategoria
    WHERE a.Stock > 0
    ORDER BY c.Nombre, a.Nombre;
END;
GO

-- Mensaje de finalización
PRINT 'Base de datos CafeAroma creada exitosamente!';
PRINT 'Datos iniciales insertados.';
PRINT 'Índices y procedimientos almacenados creados.';