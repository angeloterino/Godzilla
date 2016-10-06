-- phpMyAdmin SQL Dump
-- version 4.3.11
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 07-08-2015 a las 14:15:31
-- Versión del servidor: 5.6.24
-- Versión de PHP: 5.5.24

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de datos: `jjgestdoc`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `comentarios`
--

CREATE TABLE IF NOT EXISTS `comentarios` (
  `ID` int(11) NOT NULL,
  `CONTRATO_ID` int(20) NOT NULL COMMENT 'Codigo del contrato',
  `TIPO` int(11) NOT NULL,
  `TEXTO` text CHARACTER SET latin1 NOT NULL,
  `FECHA` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `comentarios_tipos`
--

CREATE TABLE IF NOT EXISTS `comentarios_tipos` (
  `ID` int(11) NOT NULL,
  `NOMBRE` text CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE IF NOT EXISTS `contratos` (
  `ID` int(11) NOT NULL,
  `TIPO` int(20) NOT NULL,
  `CODIGO` varchar(20) CHARACTER SET latin1 DEFAULT NULL,
  `NOMBRE` varchar(20) CHARACTER SET latin1 DEFAULT NULL,
  `ADMIN_MANAGER` int(11) DEFAULT NULL,
  `OWNER` int(11) DEFAULT NULL,
  `DPTO` int(11) DEFAULT NULL,
  `TIPO_CONTRATO` text CHARACTER SET latin1,
  `OBJETO_CONTRATO` text CHARACTER SET latin1,
  `PROVEEDOR_ID` int(11) DEFAULT NULL,
  `VENDOR_NUMBER` text CHARACTER SET latin1,
  `ACTIVO` varchar(20) CHARACTER SET latin1 DEFAULT NULL,
  `PROCESO` int(11) DEFAULT NULL,
  `ESTADO` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Disparadores `contratos`
--
DELIMITER $$
CREATE TRIGGER `UPDATE CODIGO` AFTER UPDATE ON `contratos`
 FOR EACH ROW BEGIN
  IF(OLD.CODIGO <> NEW.CODIGO) THEN
    INSERT INTO jjgestdoc.historico_codificacion (CONTRATO_ID, CODIGO_ANTERIOR) VALUES
    (OLD.ID, OLD.CODIGO)
    ;
  END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos_estados`
--

CREATE TABLE IF NOT EXISTS `contratos_estados` (
  `ID` int(11) NOT NULL,
  `DESCRIPCION` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `contratos_estados`
--

INSERT INTO `contratos_estados` (`ID`, `DESCRIPCION`) VALUES
(0, 'PENDIENTE'),
(1, 'ACEPTADO'),
(2, 'DENEGADO');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos_procesos`
--

CREATE TABLE IF NOT EXISTS `contratos_procesos` (
  `ID` int(11) NOT NULL,
  `DESCRIPCION` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `contratos_procesos`
--

INSERT INTO `contratos_procesos` (`ID`, `DESCRIPCION`) VALUES
(0, 'SOLICITUD'),
(1, 'FIRMA'),
(2, 'TRANSFERENCIA'),
(3, 'EXPURGO'),
(4, 'DESTRUCCION');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos_tipo`
--

CREATE TABLE IF NOT EXISTS `contratos_tipo` (
  `ID` int(11) NOT NULL,
  `NOMBRE` text CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `contratos_tipo`
--

INSERT INTO `contratos_tipo` (`ID`, `NOMBRE`) VALUES
(1, 'CONTRATO'),
(2, 'ADENDA');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `departamentos`
--

CREATE TABLE IF NOT EXISTS `departamentos` (
  `ID` int(11) NOT NULL,
  `NOMBRE` text CHARACTER SET latin1 NOT NULL,
  `CODIFICACION` varchar(20) NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `departamentos`
--

INSERT INTO `departamentos` (`ID`, `NOMBRE`, `CODIFICACION`) VALUES
(1, 'Servicios Generales/HR', 'SG'),
(2, 'Marketing', 'MK'),
(3, 'Quality / Quality Internacional', 'X'),
(4, 'Regulatory Affairs', 'RA'),
(5, 'Ventas /  Shopper', 'V'),
(6, 'Supply Chain (HUB/Demand/Customer Service)', 'L'),
(7, 'Finanzas / Contabilidad', 'X'),
(8, 'Dirección General', 'X'),
(9, 'IT/IM', 'X'),
(10, 'OTROS', 'X'),
(11, 'MARKETING - BUSINESS DEV. & PROF. MARKETING', 'MK');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `documentos`
--

CREATE TABLE IF NOT EXISTS `documentos` (
  `ID` int(11) NOT NULL,
  `CONTRATO_ID` int(20) NOT NULL,
  `RUTA` text CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `expurgo`
--

CREATE TABLE IF NOT EXISTS `expurgo` (
  `ID` int(11) NOT NULL,
  `CONTRATO_ID` int(20) NOT NULL,
  `FECHA_SOLICITUD` date NOT NULL,
  `FECHA_EXPURGO` date NOT NULL,
  `ESTADO_ID` int(11) NOT NULL,
  `SOLICITADO` text CHARACTER SET latin1 NOT NULL,
  `CAJA` int(11) DEFAULT NULL,
  `INDICIE` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `fechas`
--

CREATE TABLE IF NOT EXISTS `fechas` (
  `ID` int(11) NOT NULL,
  `CONTRATO_ID` int(11) NOT NULL,
  `FECHA_CODIFICACION` date DEFAULT NULL COMMENT 'Fecha de codificación del contrato',
  `FECHA_ARCHIVO` date DEFAULT NULL COMMENT 'Fecha de entrada en el Archivo General',
  `FECHA_INICIO` date DEFAULT NULL COMMENT 'Fecha de Inicio del servicio',
  `FECHA_FIN` date DEFAULT NULL COMMENT 'Fecha de fin del servicio',
  `FECHA_AVISO_AM` date DEFAULT NULL COMMENT 'Fecha de Aviso al Admin Manager',
  `FECHA_EXPURGO` date DEFAULT NULL COMMENT 'Fecha de paso al estatus de Expurgo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `historico_codificacion`
--

CREATE TABLE IF NOT EXISTS `historico_codificacion` (
  `ID` int(11) NOT NULL,
  `CONTRATO_ID` int(20) NOT NULL COMMENT 'Código actual del contrato',
  `CODIGO_ANTERIOR` varchar(20) NOT NULL COMMENT 'Código previo del contrato'
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Historial de codificación de contratos';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `historico_contratos`
--

CREATE TABLE IF NOT EXISTS `historico_contratos` (
  `ID` int(11) NOT NULL,
  `CODIGO` varchar(20) NOT NULL,
  `FECHA_DESTRUCCION` date NOT NULL,
  `CONTRATO_ID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contratos Destruidos';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores`
--

CREATE TABLE IF NOT EXISTS `proveedores` (
  `ID` int(11) NOT NULL,
  `NOMBRE` text CHARACTER SET latin1 NOT NULL,
  `CIF` text CHARACTER SET latin1,
  `EMAIL` text CHARACTER SET latin1,
  `TELEFONO` text CHARACTER SET latin1,
  `CONTACTO` text CHARACTER SET latin1
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE IF NOT EXISTS `usuarios` (
  `ID` int(11) NOT NULL,
  `NOMBRE` text CHARACTER SET latin1,
  `APELLIDOS` text CHARACTER SET latin1,
  `DEPARTAMENTO_ID` int(11) DEFAULT NULL,
  `EMAIL` text CHARACTER SET latin1,
  `ROL_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`ID`, `NOMBRE`, `APELLIDOS`, `DEPARTAMENTO_ID`, `EMAIL`, `ROL_ID`) VALUES
(1, 'ANA', 'CARDENAS HERNANDEZ', NULL, 'calvare6@its.jnj.co', 2),
(2, 'ALCIRA', 'ALVAREZ', NULL, NULL, 3),
(3, 'asd', 'as', 1, 'ads', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios_roles`
--

CREATE TABLE IF NOT EXISTS `usuarios_roles` (
  `ID` int(11) NOT NULL,
  `NOMBRE` text CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `usuarios_roles`
--

INSERT INTO `usuarios_roles` (`ID`, `NOMBRE`) VALUES
(1, 'ADMINISTRADOR'),
(2, 'AM'),
(3, 'OWNER'),
(4, 'OTROS'),
(5, 'RO');

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_comments`
--
CREATE TABLE IF NOT EXISTS `v_comments` (
`CONTRATO_ID` int(20)
,`TIPO` text
,`TEXTO` text
,`FECHA` date
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_contratos`
--
CREATE TABLE IF NOT EXISTS `v_contratos` (
`ID` int(11)
,`TIPO` text
,`CODIGO` varchar(20)
,`NOMBRE` varchar(20)
,`ADMIN_MANAGER` double
,`OWNER` double
,`DPTO` text
,`FECHA_CODIFICACION` date
,`FECHA_ARCHIVO` date
,`TIPO_CONTRATO` text
,`OBJETO_CONTRATO` text
,`NOMBRE_PROVEEDOR` text
,`CIF_PROVEEDOR` text
,`FECHA_INICIO` date
,`FECHA_FIN` date
,`FECHA_AVISO_AM` date
,`RESUMEN` text
,`COPIA_DOCUMENTO` text
,`VENDOR_NUMBER` text
,`COMENTARIOS_AM` text
,`COMENTARIOS_AG` text
,`ACTIVO` varchar(20)
,`PROCESO` text
,`ESTADO` text
);

-- --------------------------------------------------------

--
-- Estructura para la vista `v_comments`
--
DROP TABLE IF EXISTS `v_comments`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_comments` AS (select `com`.`CONTRATO_ID` AS `CONTRATO_ID`,`tipos`.`NOMBRE` AS `TIPO`,`com`.`TEXTO` AS `TEXTO`,`com`.`FECHA` AS `FECHA` from (`comentarios` `com` join `comentarios_tipos` `tipos` on((`com`.`TIPO` = `tipos`.`ID`))));

-- --------------------------------------------------------

--
-- Estructura para la vista `v_contratos`
--
DROP TABLE IF EXISTS `v_contratos`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_contratos` AS select `ct`.`ID` AS `ID`,`tipos`.`NOMBRE` AS `TIPO`,`ct`.`CODIGO` AS `CODIGO`,`ct`.`NOMBRE` AS `NOMBRE`,((`us`.`NOMBRE` + ' ') + `us`.`APELLIDOS`) AS `ADMIN_MANAGER`,((`ow`.`NOMBRE` + ' ') + `ow`.`APELLIDOS`) AS `OWNER`,`dpo`.`NOMBRE` AS `DPTO`,`fec`.`FECHA_CODIFICACION` AS `FECHA_CODIFICACION`,`fec`.`FECHA_ARCHIVO` AS `FECHA_ARCHIVO`,`ct`.`TIPO_CONTRATO` AS `TIPO_CONTRATO`,`ct`.`OBJETO_CONTRATO` AS `OBJETO_CONTRATO`,`prov`.`NOMBRE` AS `NOMBRE_PROVEEDOR`,`prov`.`CIF` AS `CIF_PROVEEDOR`,`fec`.`FECHA_INICIO` AS `FECHA_INICIO`,`fec`.`FECHA_FIN` AS `FECHA_FIN`,`fec`.`FECHA_AVISO_AM` AS `FECHA_AVISO_AM`,`com_res`.`TEXTO` AS `RESUMEN`,`doc`.`RUTA` AS `COPIA_DOCUMENTO`,`ct`.`VENDOR_NUMBER` AS `VENDOR_NUMBER`,`com_am`.`TEXTO` AS `COMENTARIOS_AM`,`com_ag`.`TEXTO` AS `COMENTARIOS_AG`,`ct`.`ACTIVO` AS `ACTIVO`,`pro`.`DESCRIPCION` AS `PROCESO`,`est`.`DESCRIPCION` AS `ESTADO` from ((((((((((((`contratos` `ct` join `contratos_tipo` `tipos` on((`ct`.`ID` = `tipos`.`ID`))) join `usuarios` `us` on((`us`.`ID` = `ct`.`ADMIN_MANAGER`))) join `usuarios` `ow` on((`ow`.`ID` = `ct`.`OWNER`))) join `departamentos` `dpo` on((`dpo`.`ID` = `ct`.`DPTO`))) join `proveedores` `prov` on((`prov`.`ID` = `ct`.`PROVEEDOR_ID`))) join `fechas` `fec` on((`fec`.`CONTRATO_ID` = `ct`.`ID`))) join `v_comments` `com_res` on(((`com_res`.`CONTRATO_ID` = `ct`.`ID`) and (`com_res`.`TIPO` = 'RESUMEN')))) join `v_comments` `com_ag` on(((`com_ag`.`CONTRATO_ID` = `ct`.`ID`) and (`com_ag`.`TIPO` = 'COMENTARIOS_AG')))) join `v_comments` `com_am` on(((`com_am`.`CONTRATO_ID` = `ct`.`ID`) and (`com_am`.`TIPO` = 'COMENTARIOS_AM')))) join `documentos` `doc` on((`doc`.`CONTRATO_ID` = `ct`.`ID`))) join `contratos_estados` `est` on((`est`.`ID` = `ct`.`ESTADO`))) join `contratos_procesos` `pro` on((`pro`.`ID` = `ct`.`PROCESO`)));

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `comentarios`
--
ALTER TABLE `comentarios`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `comentarios_tipos`
--
ALTER TABLE `comentarios_tipos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `contratos_estados`
--
ALTER TABLE `contratos_estados`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `contratos_procesos`
--
ALTER TABLE `contratos_procesos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `contratos_tipo`
--
ALTER TABLE `contratos_tipo`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `departamentos`
--
ALTER TABLE `departamentos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `documentos`
--
ALTER TABLE `documentos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `expurgo`
--
ALTER TABLE `expurgo`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `fechas`
--
ALTER TABLE `fechas`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `historico_codificacion`
--
ALTER TABLE `historico_codificacion`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `historico_contratos`
--
ALTER TABLE `historico_contratos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `usuarios_roles`
--
ALTER TABLE `usuarios_roles`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `comentarios`
--
ALTER TABLE `comentarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `comentarios_tipos`
--
ALTER TABLE `comentarios_tipos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `contratos_tipo`
--
ALTER TABLE `contratos_tipo`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT de la tabla `departamentos`
--
ALTER TABLE `departamentos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT de la tabla `documentos`
--
ALTER TABLE `documentos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `expurgo`
--
ALTER TABLE `expurgo`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `fechas`
--
ALTER TABLE `fechas`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `historico_codificacion`
--
ALTER TABLE `historico_codificacion`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `historico_contratos`
--
ALTER TABLE `historico_contratos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `usuarios_roles`
--
ALTER TABLE `usuarios_roles`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
