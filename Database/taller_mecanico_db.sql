-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 10-12-2025 a las 15:47:02
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
CREATE DATABASE IF NOT EXISTS `taller_mecanico_db` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `taller_mecanico_db`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clientes`
--

CREATE TABLE `clientes` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `Dni` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `clientes`
--

INSERT INTO `clientes` (`id`, `nombre`, `apellido`, `telefono`, `email`, `created_at`, `updated_at`, `Dni`) VALUES
(1, 'Carlos', 'López', '351-1111111', 'carlos@cliente.com', '2025-11-05 22:58:39', '2025-12-07 13:13:42', '12345678'),
(2, 'Ana', 'Gómez', '351-2222222', 'ana@cliente.com', '2025-11-05 22:58:39', '2025-12-07 13:13:49', '12123123');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleados`
--

CREATE TABLE `empleados` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `dni` varchar(20) DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `usuario_id` int(11) NOT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `empleados`
--

INSERT INTO `empleados` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `usuario_id`, `created_at`, `updated_at`) VALUES
(1, 'Admin', 'Taller', '11111111', '351-000000', 1, '2025-11-05 22:58:39', '2025-11-05 22:58:39'),
(2, 'Juan', 'Pérez', '30123456', '351-1234567', 2, '2025-11-05 22:58:39', '2025-11-05 22:58:39');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `herramientas`
--

CREATE TABLE `herramientas` (
  `id` int(11) NOT NULL,
  `codigo` varchar(50) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `Estado` int(11) NOT NULL,
  `ubicacion` varchar(100) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `herramientas`
--

INSERT INTO `herramientas` (`id`, `codigo`, `nombre`, `Estado`, `ubicacion`, `created_at`, `updated_at`) VALUES
(1, 'H001', 'Llave inglesa 10mm', 1, 'Estante A1', '2025-12-04 16:07:56', '2025-12-04 16:07:56'),
(2, 'H002', 'Extractor de bujías', 1, 'Cajón B2', '2025-11-05 22:58:39', '2025-11-05 22:58:39');

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
(1, 'Cambio de aceite y filtro', '2025-11-05 22:58:39', '2025-11-10 12:00:00', 0, 2.50, 1, 2, '2025-11-05 22:58:39', '2025-12-10 10:44:27');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ot_herramientas`
--

CREATE TABLE `ot_herramientas` (
  `ot_id` int(11) NOT NULL,
  `herramienta_id` int(11) NOT NULL,
  `fecha_prestamo` datetime NOT NULL DEFAULT current_timestamp(),
  `fecha_devolucion` datetime DEFAULT NULL,
  `empleado_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `ot_herramientas`
--

INSERT INTO `ot_herramientas` (`ot_id`, `herramienta_id`, `fecha_prestamo`, `fecha_devolucion`, `empleado_id`) VALUES
(1, 1, '2025-11-05 09:00:00', NULL, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ot_repuestos`
--

CREATE TABLE `ot_repuestos` (
  `ot_id` int(11) NOT NULL,
  `repuesto_id` int(11) NOT NULL,
  `cantidad_usada` int(11) NOT NULL CHECK (`cantidad_usada` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `ot_repuestos`
--

INSERT INTO `ot_repuestos` (`ot_id`, `repuesto_id`, `cantidad_usada`) VALUES
(1, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedidos_repuestos`
--

CREATE TABLE `pedidos_repuestos` (
  `id` int(11) NOT NULL,
  `fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `estado` int(11) NOT NULL DEFAULT 0,
  `proveedor_id` int(11) NOT NULL,
  `empleado_id` int(11) NOT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `fecha_pedido` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `pedidos_repuestos`
--

INSERT INTO `pedidos_repuestos` (`id`, `fecha`, `estado`, `proveedor_id`, `empleado_id`, `created_at`, `updated_at`, `fecha_pedido`) VALUES
(1, '2025-11-04 10:00:00', 2, 1, 2, '2025-11-05 22:58:39', '2025-12-10 11:45:40', '2025-12-07 22:01:01'),
(2, '2025-12-10 11:41:29', 0, 1, 1, '2025-12-10 11:41:29', '2025-12-10 11:41:29', '2025-12-10 00:00:00'),
(3, '2025-12-10 11:44:38', 0, 1, 1, '2025-12-10 11:44:38', '2025-12-10 11:44:38', '2025-12-10 00:00:00');

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
(1, 2, 5, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores`
--

CREATE TABLE `proveedores` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `contacto` varchar(100) DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `condiciones_compra` text DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `proveedores`
--

INSERT INTO `proveedores` (`id`, `nombre`, `contacto`, `telefono`, `condiciones_compra`, `created_at`, `updated_at`) VALUES
(1, 'AutoParts SRL', 'ventas@autoparts.com', '351-4444444', 'Pago a 30 días', '2025-11-05 22:58:39', '2025-11-05 22:58:39');

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
(1, 'R001', 'Filtro de aceite Toyota', 15, 12.50, 5, 1, '2025-11-05 22:58:39', '2025-11-05 22:58:39'),
(2, 'R002', 'Pastillas de freno Fiat', 8, 45.00, 3, 1, '2025-11-05 22:58:39', '2025-11-05 22:58:39');

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
(1, 'admin@taller.local', 'AQAAAAIAAYagAAAAEMrZoSZcmZv1uUC//u6qzF4lG8Ca8mChjyyN3OTnSOuDLo1o9ReDH8GEAa+4rpvMNg==', 'Administrador', '/uploads/avatars/default.jpg', '2025-12-10 10:38:41', '2025-12-10 10:38:41');

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
(2, 'DEF456', 'Fiat', 'Cronos', '2022', 0, 'Frenos delanteros', 2, '2025-11-05 22:58:39', '2025-12-07 13:27:25');

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
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `IX_Clientes_Dni` (`Dni`);

--
-- Indices de la tabla `empleados`
--
ALTER TABLE `empleados`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `usuario_id` (`usuario_id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `herramientas`
--
ALTER TABLE `herramientas`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `codigo` (`codigo`),
  ADD KEY `idx_herramientas_estado` (`Estado`);

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
  ADD PRIMARY KEY (`ot_id`,`herramienta_id`),
  ADD KEY `herramienta_id` (`herramienta_id`),
  ADD KEY `empleado_id` (`empleado_id`);

--
-- Indices de la tabla `ot_repuestos`
--
ALTER TABLE `ot_repuestos`
  ADD PRIMARY KEY (`ot_id`,`repuesto_id`),
  ADD KEY `repuesto_id` (`repuesto_id`);

--
-- Indices de la tabla `pedidos_repuestos`
--
ALTER TABLE `pedidos_repuestos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proveedor_id` (`proveedor_id`),
  ADD KEY `empleado_id` (`empleado_id`),
  ADD KEY `idx_pedidos_estado` (`estado`,`proveedor_id`);

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
  ADD PRIMARY KEY (`id`);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `empleados`
--
ALTER TABLE `empleados`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `herramientas`
--
ALTER TABLE `herramientas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `ordenes_trabajo`
--
ALTER TABLE `ordenes_trabajo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `pedidos_repuestos`
--
ALTER TABLE `pedidos_repuestos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `repuestos`
--
ALTER TABLE `repuestos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `vehiculos`
--
ALTER TABLE `vehiculos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `empleados`
--
ALTER TABLE `empleados`
  ADD CONSTRAINT `empleados_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `ordenes_trabajo`
--
ALTER TABLE `ordenes_trabajo`
  ADD CONSTRAINT `ordenes_trabajo_ibfk_1` FOREIGN KEY (`vehiculo_id`) REFERENCES `vehiculos` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `ordenes_trabajo_ibfk_2` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`id`);

--
-- Filtros para la tabla `ot_herramientas`
--
ALTER TABLE `ot_herramientas`
  ADD CONSTRAINT `ot_herramientas_ibfk_1` FOREIGN KEY (`ot_id`) REFERENCES `ordenes_trabajo` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `ot_herramientas_ibfk_2` FOREIGN KEY (`herramienta_id`) REFERENCES `herramientas` (`id`),
  ADD CONSTRAINT `ot_herramientas_ibfk_3` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`id`);

--
-- Filtros para la tabla `ot_repuestos`
--
ALTER TABLE `ot_repuestos`
  ADD CONSTRAINT `ot_repuestos_ibfk_1` FOREIGN KEY (`ot_id`) REFERENCES `ordenes_trabajo` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `ot_repuestos_ibfk_2` FOREIGN KEY (`repuesto_id`) REFERENCES `repuestos` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `pedidos_repuestos`
--
ALTER TABLE `pedidos_repuestos`
  ADD CONSTRAINT `fk_empleado_pedido` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`id`),
  ADD CONSTRAINT `pedidos_repuestos_ibfk_1` FOREIGN KEY (`proveedor_id`) REFERENCES `proveedores` (`id`),
  ADD CONSTRAINT `pedidos_repuestos_ibfk_2` FOREIGN KEY (`empleado_id`) REFERENCES `empleados` (`id`);

--
-- Filtros para la tabla `pedido_repuestos`
--
ALTER TABLE `pedido_repuestos`
  ADD CONSTRAINT `pedido_repuestos_ibfk_1` FOREIGN KEY (`pedido_id`) REFERENCES `pedidos_repuestos` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `pedido_repuestos_ibfk_2` FOREIGN KEY (`repuesto_id`) REFERENCES `repuestos` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `repuestos`
--
ALTER TABLE `repuestos`
  ADD CONSTRAINT `repuestos_ibfk_1` FOREIGN KEY (`proveedor_id`) REFERENCES `proveedores` (`id`);

--
-- Filtros para la tabla `vehiculos`
--
ALTER TABLE `vehiculos`
  ADD CONSTRAINT `vehiculos_ibfk_1` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
