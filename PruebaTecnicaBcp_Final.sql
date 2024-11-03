CREATE DATABASE PruebaTecnicaBcp_Final;

USE PruebaTecnicaBcp_Final;


-- Crear tabla Rol y Usuario

-- Contraseña actual de todos los usuarios es :   pass1234 a pesar de estar SHA256

CREATE TABLE Rol (
    IdRol SMALLINT PRIMARY KEY IDENTITY,
    Nombre VARCHAR(32) 
);

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY,
    CodigoTrabajador VARCHAR(32) NOT NULL,
    Nombre VARCHAR(64),
    Contrasena VARCHAR(128),
    Email VARCHAR(64),
    TelefonoCelular VARCHAR(16),
	Puesto VARCHAR(64),
    IdRol SMALLINT ,
    IdEstado SMALLINT ,
    FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);

CREATE TABLE TipoProducto (
    IdTipo SMALLINT PRIMARY KEY IDENTITY,
    NombreTipo VARCHAR(128) NOT NULL,
    Descripcion VARCHAR(256) NULL
);

CREATE TABLE Producto (
    IdProducto INT PRIMARY KEY IDENTITY,
    Sku VARCHAR(128)UNIQUE,
    Nombre VARCHAR(128),
    IdTipo SMALLINT NULL, -- 
    Etiqueta VARCHAR(128) NULL,
    Precio DECIMAL(18, 2),
    Stock INT,
	IdEstado SMALLINT DEFAULT 1
	FOREIGN KEY (IdTipo) REFERENCES TipoProducto(IdTipo)
);





CREATE TABLE EstadoPedido (
    IdEstadoPedido SMALLINT PRIMARY KEY IDENTITY,
    Nombre VARCHAR(32)
);

CREATE TABLE Pedido (
    IdPedido INT PRIMARY KEY IDENTITY,
    NroPedido VARCHAR(64) UNIQUE,
    FechaPedido DATETIME ,
    FechaDespacho DATETIME ,
    FechaEntrega DATETIME ,
    IdVendedor INT ,
    IdDelivery INT ,
    Total DECIMAL(18, 2) ,
    IdEstadoPedido SMALLINT,
    IdEstado SMALLINT ,
    FOREIGN KEY (IdVendedor) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdDelivery) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdEstadoPedido) REFERENCES EstadoPedido(IdEstadoPedido)
);



CREATE TABLE PedidoDetalle (
    IdPedidoDetalle INT PRIMARY KEY IDENTITY,
    IdPedido INT ,
    IdProducto INT ,
    Cantidad INT ,
    TotalPrecio DECIMAL(18, 2),
    FOREIGN KEY (IdPedido) REFERENCES Pedido(IdPedido),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);




-- Insertando datos en la tabla Rol
INSERT INTO Rol (Nombre) VALUES
('Encargado'),
('Vendedor'),
('Delivery');

Select * from Rol

--Insertar Datos Usuario

insert into Usuario values ('ENC001','Encargado','BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F','encargado@prueba.com','99999999','PRUEBA',1,1)
insert into Usuario values ('VEN002','Vendedor','BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F','vendedor@prueba.com','99999999','PRUEBA',2,1)
insert into Usuario values ('DEL003','Delivery','BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F','delivery@prueba.com','99999999','PRUEBA',3,1)
-- Encargado (IdRol = 1)
INSERT INTO Usuario (CodigoTrabajador, Nombre, Contrasena, Email, TelefonoCelular, Puesto, IdRol, IdEstado)
VALUES 
    ('ENC002', 'Carlos Perez', 'BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F', 'cperez@example.com', '999888777', 'Supervisor de Turno', 1, 1),
    ('ENC003', 'Maria Diaz', 'BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F', 'mdiaz@example.com', '999888776', 'Jefe de Area', 1, 1);

-- Vendedor (IdRol = 2)
INSERT INTO Usuario (CodigoTrabajador, Nombre, Contrasena, Email, TelefonoCelular, Puesto, IdRol, IdEstado)
VALUES 
    ('VEN002', 'Luis Garcia', 'BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F', 'lgarcia@example.com', '999888775', 'Vendedor de Tienda', 2, 1),
    ('VEN003', 'Ana Lopez', 'BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F', 'alopez@example.com', '999888774', 'Vendedor de Mostrador', 2, 1);

-- Delivery (IdRol = 3)
INSERT INTO Usuario (CodigoTrabajador, Nombre, Contrasena, Email, TelefonoCelular, Puesto, IdRol, IdEstado)
VALUES 
    ('DEL002', 'Juan Torres', 'BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F', 'jtorres@example.com', '999888773', 'Repartidor', 3, 1),
    ('DEL003', 'Sofia Vega', 'BD94DCDA26FCCB4E68D6A31F9B5AAC0B571AE266D822620E901EF7EBE3A11D4F', 'svega@example.com', '999888772', 'Mensajero', 3, 1);

Select * from usuario



-- Insertando datos en la tabla EstadoPedido
INSERT INTO EstadoPedido (Nombre) VALUES
('Por atender'), -- Estado para el momento del registro.
('En proceso'), -- Estado que el Encargado colocara después del registro.
('Entregado'); --Estado que el Encargado colocara después del registro.

select * from EstadoPedido


INSERT INTO TipoProducto (NombreTipo, Descripcion) VALUES
('Electrónica', 'Productos relacionados con dispositivos electrónicos, como teléfonos, computadoras y accesorios.'),
('Ropa', 'Artículos de vestimenta y accesorios de moda para hombres, mujeres y niños.'),
('Alimentos', 'Productos comestibles, incluyendo frescos, procesados y bebidas.');

select * from TipoProducto


-- Insertar Productos
INSERT INTO [PruebaTecnicaBcp_Final].[dbo].[Producto] 
    ([Sku], [Nombre], [IdTipo], [Etiqueta], [Precio], [Stock], [IdEstado])
VALUES 
    ('DD001', 'Disco Duro Toshiba 500 GB', 1, '101010', 250, 200, 1),
    ('DD002', 'Disco Duro Seagate 1 TB', 1, '101011', 450, 150, 1),
    ('MM001', 'Memoria RAM Kingston 8 GB', 1, '202020', 120, 300, 1),
    ('MM002', 'Memoria RAM Corsair 16 GB', 1, '202021', 230, 100, 1),
    ('TV001', 'Televisor Samsung 50" LED', 1, '303030', 1200, 80, 1),
    ('TV002', 'Televisor LG 40" LED', 1, '303031', 950, 60, 1),
    ('RP001', 'Ropa Deportiva Nike Talla M', 2, '404040', 70, 250, 1),
    ('RP002', 'Ropa Formal Zara Talla L', 2, '404041', 150, 50, 1),
    ('AL001', 'Cereal Nestlé 500g', 3, '505050', 15, 500, 1),
    ('AL002', 'Arroz Costeño 1kg', 3, '505051', 5, 1000, 1);


SELECT * FROM Producto

Select * from Usuario



