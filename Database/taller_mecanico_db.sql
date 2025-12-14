-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 14-12-2025 a las 02:44:23
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `taller_mecanico_db`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clientes`
--

CREATE TABLE `clientes` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Created_at` datetime DEFAULT current_timestamp(),
  `Updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `Dni` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `clientes`
--

INSERT INTO `clientes` (`Id`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Created_at`, `Updated_at`, `Dni`) VALUES
(1, 'Carlos', 'López', '351-1111111', 'carlos@cliente.com', '2025-11-05 22:58:39', '2025-12-10 18:37:15', '12345678'),
(2, 'Ana', 'Gómez', '351-2222222', 'ana@cliente.com', '2025-11-05 22:58:39', '2025-12-07 13:13:49', '12123123'),
(3, 'Martin Nahuel', 'Becerra', '02664304069', 'martinbecerrasl7@gmail.com', '2025-12-10 14:07:35', '2025-12-10 14:07:35', '47266622');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleados`
--

CREATE TABLE `empleados` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(20) DEFAULT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `empleados`
--

INSERT INTO `empleados` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `usuario_id`, `created_at`, `updated_at`) VALUES
(1, 'Admin', 'Taller', '11111111', '351-000000', 1, '2025-11-05 22:58:39', '2025-11-05 22:58:39'),
(2, 'Juan', 'Pérez', '30123456', '351-1234567', 2, '2025-11-05 22:58:39', '2025-11-05 22:58:39'),
(3, 'Martin Nahuel', 'Becerra', '47266622', '02664304069', 3, '2025-12-11 14:57:56', '2025-12-11 15:32:23'),
(6, 'Santiago Agustin', 'Becerra', '46072720', '026641697443', NULL, '2025-12-11 15:58:26', '2025-12-11 15:58:26'),
(7, 'Maria Carolina', 'Becerra', '38731849', '02664334455', NULL, '2025-12-11 15:58:55', '2025-12-11 15:58:55'),
(8, 'Ariel Ramon', 'Becerra', '23448180', '02664334760', 5, '2025-12-11 15:59:19', '2025-12-11 19:31:09');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `herramientas`
--

CREATE TABLE `herramientas` (
  `Id` int(11) NOT NULL,
  `Codigo` varchar(50) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Estado` int(11) NOT NULL,
  `Ubicacion` varchar(100) DEFAULT NULL,
  `Created_at` datetime DEFAULT current_timestamp(),
  `Updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `herramientas`
--

INSERT INTO `herramientas` (`Id`, `Codigo`, `Nombre`, `Estado`, `Ubicacion`, `Created_at`, `Updated_at`) VALUES
(1, 'H001', 'Llave inglesa 10mm', 0, 'Estante A1', '2025-12-04 16:07:56', '2025-12-13 21:36:34'),
(2, 'H002', 'Extractor de bujías', 1, 'Cajón B2', '2025-11-05 22:58:39', '2025-12-13 21:36:33');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagenherramientas`
--

CREATE TABLE `imagenherramientas` (
  `Id` int(11) NOT NULL,
  `HerramientaId` int(11) NOT NULL,
  `Url` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `imagenherramientas`
--

INSERT INTO `imagenherramientas` (`Id`, `HerramientaId`, `Url`) VALUES
(1, 1, '/uploads/herramientas/62854a68-0958-419e-8771-532263c41fe4.webp'),
(2, 1, '/uploads/herramientas/933fff84-b8da-45ce-a7f0-4abe0ba617aa.jpeg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ordenes_trabajo`
--

CREATE TABLE `ordenes_trabajo` (
  `id` int(11) NOT NULL,
  `descripcion_falla` text NOT NULL,
  `fecha_ingreso` datetime NOT NULL DEFAULT current_timestamp(),
  `fecha_estimada_entrega` datetime DEFAULT NULL,
  `estado` int(11) NOT NULL DEFAULT 0,
  `horas_estimadas` decimal(5,2) DEFAULT NULL,
  `vehiculo_id` int(11) NOT NULL,
  `empleado_id` int(11) NOT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `ordenes_trabajo`
--

INSERT INTO `ordenes_trabajo` (`id`, `descripcion_falla`, `fecha_ingreso`, `fecha_estimada_entrega`, `estado`, `horas_estimadas`, `vehiculo_id`, `empleado_id`, `created_at`, `updated_at`) VALUES
(1, 'Cambio de aceite y filtro', '2025-11-05 22:58:39', '2025-11-10 12:00:00', 0, 2.50, 1, 2, '2025-11-05 22:58:39', '2025-12-10 10:44:27'),
(2, 'Le gotea aceite', '2025-12-10 00:00:00', '2025-12-12 00:00:00', 1, 12.00, 3, 2, '2025-12-10 17:27:14', '2025-12-11 15:40:19');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ot_herramientas`
--

CREATE TABLE `ot_herramientas` (
  `id` int(11) NOT NULL,
  `ot_id` int(11) NOT NULL,
  `herramienta_id` int(11) NOT NULL,
  `empleado_id` int(11) NOT NULL,
  `fecha_prestamo` datetime NOT NULL,
  `fecha_devolucion` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ot_herramientas`
--

INSERT INTO `ot_herramientas` (`id`, `ot_id`, `herramienta_id`, `empleado_id`, `fecha_prestamo`, `fecha_devolucion`) VALUES
(1, 2, 1, 2, '2025-12-13 21:32:50', '2025-12-13 21:36:34'),
(2, 2, 2, 2, '2025-12-13 21:33:12', '2025-12-13 21:36:33'),
(3, 2, 2, 2, '2025-12-13 21:36:39', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ot_repuestos`
--

CREATE TABLE `ot_repuestos` (
  `id` int(11) NOT NULL,
  `ot_id` int(11) NOT NULL,
  `repuesto_id` int(11) NOT NULL,
  `empleado_id` int(11) NOT NULL,
  `cantidad_usada` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ot_repuestos`
--

INSERT INTO `ot_repuestos` (`id`, `ot_id`, `repuesto_id`, `empleado_id`, `cantidad_usada`) VALUES
(1, 2, 1, 2, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedidos_repuestos`
--

CREATE TABLE `pedidos_repuestos` (
  `Id` int(11) NOT NULL,
  `Fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `Estado` int(11) NOT NULL DEFAULT 0,
  `Proveedor_id` int(11) NOT NULL,
  `Empleado_id` int(11) NOT NULL,
  `Created_at` datetime DEFAULT current_timestamp(),
  `Updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `fecha_pedido` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `pedidos_repuestos`
--

INSERT INTO `pedidos_repuestos` (`Id`, `Fecha`, `Estado`, `Proveedor_id`, `Empleado_id`, `Created_at`, `Updated_at`, `fecha_pedido`) VALUES
(1, '2025-11-04 10:00:00', 2, 1, 2, '2025-11-05 22:58:39', '2025-12-10 11:45:40', '2025-12-07 22:01:01'),
(2, '2025-12-10 11:41:29', 2, 1, 1, '2025-12-10 11:41:29', '2025-12-11 19:00:01', '2025-12-10 00:00:00'),
(3, '2025-12-10 11:44:38', 0, 1, 1, '2025-12-10 11:44:38', '2025-12-10 17:26:31', '2025-12-10 00:00:00'),
(4, '2025-12-10 13:46:17', 2, 1, 1, '2025-12-10 13:46:17', '2025-12-11 18:59:22', '2025-12-10 00:00:00'),
(5, '2025-12-10 16:41:43', 2, 1, 1, '2025-12-10 16:41:43', '2025-12-11 18:59:14', '2025-12-10 00:00:00'),
(6, '2025-12-11 16:38:59', 1, 1, 3, '2025-12-11 16:38:58', '2025-12-11 18:59:35', '2025-12-11 00:00:00'),
(7, '2025-12-11 18:06:18', 2, 1, 2, '2025-12-11 18:06:18', '2025-12-11 18:59:06', '2025-12-11 00:00:00'),
(8, '2025-12-11 20:27:42', 0, 1, 3, '2025-12-11 20:27:42', '2025-12-11 20:27:42', '2025-12-11 00:00:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedido_repuestos`
--

CREATE TABLE `pedido_repuestos` (
  `pedido_id` int(11) NOT NULL,
  `repuesto_id` int(11) NOT NULL,
  `cantidad_solicitada` int(11) NOT NULL CHECK (`cantidad_solicitada` > 0),
  `cantidad_recibida` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `pedido_repuestos`
--

INSERT INTO `pedido_repuestos` (`pedido_id`, `repuesto_id`, `cantidad_solicitada`, `cantidad_recibida`) VALUES
(1, 2, 5, 0),
(3, 1, 2, 0),
(3, 3, 2, 0),
(6, 3, 2, 0),
(8, 1, 2, 0),
(8, 2, 2, 0),
(8, 3, 2, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores`
--

CREATE TABLE `proveedores` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Contacto` varchar(100) DEFAULT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Condiciones_compra` text DEFAULT NULL,
  `Created_at` datetime DEFAULT current_timestamp(),
  `Updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `proveedores`
--

INSERT INTO `proveedores` (`Id`, `Nombre`, `Contacto`, `Telefono`, `Condiciones_compra`, `Created_at`, `Updated_at`) VALUES
(1, 'AutoParts SRL', 'ventas@autoparts.com', '351-4444444', 'Pago a 30 días', '2025-11-05 22:58:39', '2025-11-05 22:58:39'),
(2, 'Arcor SRL', 'arcorsrl@gmail.com', '3549-304069', 'Pago con cheque menor o igual a 30 Días', '2025-12-10 14:02:36', '2025-12-10 14:02:36');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `repuestos`
--

CREATE TABLE `repuestos` (
  `id` int(11) NOT NULL,
  `codigo` varchar(50) NOT NULL,
  `descripcion` varchar(200) NOT NULL,
  `cantidad_stock` int(11) NOT NULL DEFAULT 0 CHECK (`cantidad_stock` >= 0),
  `precio_unitario` decimal(10,2) NOT NULL CHECK (`precio_unitario` > 0),
  `stock_minimo` int(11) NOT NULL DEFAULT 0,
  `proveedor_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `repuestos`
--

INSERT INTO `repuestos` (`id`, `codigo`, `descripcion`, `cantidad_stock`, `precio_unitario`, `stock_minimo`, `proveedor_id`, `created_at`, `updated_at`) VALUES
(1, 'R001', 'Filtro de aceite Toyota', 13, 12.50, 5, 1, '2025-11-05 22:58:39', '2025-12-13 22:43:09'),
(2, 'R002', 'Pastillas de freno Fiat', 7, 45.00, 3, 1, '2025-11-05 22:58:39', '2025-12-13 22:26:09'),
(3, 'H003', 'Balizas Ford ', 6, 15.00, 4, 2, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `rol` enum('Administrador','Empleado') NOT NULL DEFAULT 'Empleado',
  `avatar_path` varchar(500) DEFAULT '/uploads/avatars/default.jpg',
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `email`, `password_hash`, `rol`, `avatar_path`, `created_at`, `updated_at`) VALUES
(1, 'admin@taller.local', 'AQAAAAIAAYagAAAAEDSnXVDl9Vqy+GQON2Jw7Qpuive1C1Kf+uClaw5LtFWkA6oRfaZ3CQxGZJECcKoKjA==', 'Administrador', '/uploads/avatars/9db6db2a-51bf-47a3-a8fc-d08837f6dec4.jpg', '2025-12-10 10:38:41', '2025-12-10 16:25:25'),
(2, 'empleado@taller.local', 'AQAAAAIAAYagAAAAEJnbdPIVJ2sZrK8EtO5RpIthyk7Ci+RmQ01conSWInbHjlEX4htKpUTJMaEL2+Mtkg==', 'Empleado', '/uploads/avatars/8b0a0b7b-9994-4e24-a8e6-e8a501b1c1ec.jpg', '2025-12-10 16:25:01', '2025-12-10 16:30:05'),
(3, 'martinbecerrasl7@gmail.com', 'AQAAAAIAAYagAAAAEJYGSeKzEofy3hQ8eGcVPJHFnFobgFXeA8ALiNb2MDCdEtLC8X9ATSVpbn7Tsed/eg==', 'Administrador', '/uploads/avatars/35cca01c-f537-4bfd-af99-23059dfae55f.jpg', '2025-12-11 15:13:11', '2025-12-11 15:33:04'),
(5, 'asd', 'AQAAAAIAAYagAAAAEImsgxEyW9NO4WzSJ+4xZMZ0xONKGwr8kiDAHzjrrTew7JmolyLCrMI+Uo6nYkWHfw==', 'Empleado', '/uploads/avatars/default.jpg', '2025-12-11 19:31:09', '2025-12-11 19:31:09');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vehiculos`
--

CREATE TABLE `vehiculos` (
  `id` int(11) NOT NULL,
  `patente` varchar(10) NOT NULL,
  `marca` varchar(50) NOT NULL,
  `modelo` varchar(50) NOT NULL,
  `anio` year(4) NOT NULL CHECK (`anio` >= 1900),
  `tipo` int(11) NOT NULL,
  `observaciones` text DEFAULT NULL,
  `cliente_id` int(11) NOT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `vehiculos`
--

INSERT INTO `vehiculos` (`id`, `patente`, `marca`, `modelo`, `anio`, `tipo`, `observaciones`, `cliente_id`, `created_at`, `updated_at`) VALUES
(1, 'ABC123', 'Toyota', 'Corolla', '2020', 0, 'Cambio de aceite', 1, '2025-11-05 22:58:39', '2025-12-07 13:27:25'),
(2, 'DEF456', 'Fiat', 'Cronos', '2022', 0, 'Frenos delanteros', 2, '2025-11-05 22:58:39', '2025-12-07 13:27:25'),
(3, 'AA411LL', 'Chevrolet', 'Onix LTZ', '2016', 0, 'Ninguna', 3, '2025-12-10 14:08:08', '2025-12-11 19:29:45');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Clientes_Dni` (`Dni`);

--
-- Indices de la tabla `empleados`
--
ALTER TABLE `empleados`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `usuario_id` (`usuario_id`),
  ADD UNIQUE KEY `dni` (`Dni`);

--
-- Indices de la tabla `herramientas`
--
ALTER TABLE `herramientas`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `codigo` (`Codigo`),
  ADD KEY `idx_herramientas_estado` (`Estado`);

--
-- Indices de la tabla `imagenherramientas`
--
ALTER TABLE `imagenherramientas`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_ImagenHerramientas_Herramientas` (`HerramientaId`);

--
-- Indices de la tabla `ordenes_trabajo`
--
ALTER TABLE `ordenes_trabajo`
  ADD PRIMARY KEY (`id`),
  ADD KEY `vehiculo_id` (`vehiculo_id`),
  ADD KEY `empleado_id` (`empleado_id`),
  ADD KEY `idx_ot_estado_fecha` (`estado`,`fecha_ingreso`);

--
-- Indices de la tabla `ot_herramientas`
--
ALTER TABLE `ot_herramientas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_ot_id` (`ot_id`),
  ADD KEY `idx_herramienta_id` (`herramienta_id`),
  ADD KEY `idx_empleado_id` (`empleado_id`);

--
-- Indices de la tabla `ot_repuestos`
--
ALTER TABLE `ot_repuestos`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_ot_repuesto` (`ot_id`,`repuesto_id`),
  ADD KEY `fk_otrep_repuesto` (`repuesto_id`),
  ADD KEY `fk_otrep_empleado` (`empleado_id`);

--
-- Indices de la tabla `pedidos_repuestos`
--
ALTER TABLE `pedidos_repuestos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `proveedor_id` (`Proveedor_id`),
  ADD KEY `empleado_id` (`Empleado_id`),
  ADD KEY `idx_pedidos_estado` (`Estado`,`Proveedor_id`);

--
-- Indices de la tabla `pedido_repuestos`
--
ALTER TABLE `pedido_repuestos`
  ADD PRIMARY KEY (`pedido_id`,`repuesto_id`),
  ADD KEY `repuesto_id` (`repuesto_id`);

--
-- Indices de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `repuestos`
--
ALTER TABLE `repuestos`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `codigo` (`codigo`),
  ADD KEY `proveedor_id` (`proveedor_id`),
  ADD KEY `idx_repuestos_stock` (`cantidad_stock`,`codigo`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `vehiculos`
--
ALTER TABLE `vehiculos`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `patente` (`patente`),
  ADD KEY `cliente_id` (`cliente_id`),
  ADD KEY `idx_vehiculos_patente` (`patente`);

--
-- Indices de la tabla `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `clientes`
--
ALTER TABLE `clientes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `empleados`
--
ALTER TABLE `empleados`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `herramientas`
--
ALTER TABLE `herramientas`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `imagenherramientas`
--
ALTER TABLE `imagenherramientas`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `ordenes_trabajo`
--
ALTER TABLE `ordenes_trabajo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `ot_herramientas`
--
ALTER TABLE `ot_herramientas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `ot_repuestos`
--
ALTER TABLE `ot_repuestos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `pedidos_repuestos`
--
ALTER TABLE `pedidos_repuestos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `repuestos`
--
ALTER TABLE `repuestos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `vehiculos`
--
ALTER TABLE `vehiculos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `imagenherramientas`
--
ALTER TABLE `imagenherramientas`
  ADD CONSTRAINT `FK_ImagenHerramientas_Herramientas` FOREIGN KEY (`HerramientaId`) REFERENCES `herramientas` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `ordenes_trabajo`
--
ALTER TABLE `ordenes_trabajo`
  ADD CONSTRAINT `ordenes_trabajo_ibfk_1` FOREIGN KEY (`vehiculo_id`) REFERENCES `vehiculos` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `ordenes_trabajo_ibfk_2` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`Id`);

--
-- Filtros para la tabla `ot_herramientas`
--
ALTER TABLE `ot_herramientas`
  ADD CONSTRAINT `fk_ot_herramientas_empleado` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_ot_herramientas_herramienta` FOREIGN KEY (`herramienta_id`) REFERENCES `herramientas` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_ot_herramientas_ot` FOREIGN KEY (`ot_id`) REFERENCES `ordenes_trabajo` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `ot_repuestos`
--
ALTER TABLE `ot_repuestos`
  ADD CONSTRAINT `fk_otrep_empleado` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`Id`),
  ADD CONSTRAINT `fk_otrep_ot` FOREIGN KEY (`ot_id`) REFERENCES `ordenes_trabajo` (`id`),
  ADD CONSTRAINT `fk_otrep_repuesto` FOREIGN KEY (`repuesto_id`) REFERENCES `repuestos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
