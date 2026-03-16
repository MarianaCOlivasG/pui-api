
-- 1. TABLA: pui_usuarios
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_usuarios (
    id_usuario VARCHAR(36) NOT NULL COMMENT 'Identificador único del usuario (UUID)',
    usuario VARCHAR(100) NOT NULL COMMENT 'Nombre de usuario para login',
    pass VARCHAR(255) NOT NULL COMMENT 'Contraseña (hash recomendable)',
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha de creación del usuario',
    fecha_actualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Última actualización',
    estatus ENUM('ACTIVO', 'INACTIVO') DEFAULT 'ACTIVO' COMMENT 'Estado del usuario',
    PRIMARY KEY (id_usuario),
    UNIQUE INDEX idx_usuario_unique (usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Maestro de usuarios PUI';


-- 2. TABLA: pui_reportes 
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_reportes (
    id_reporte VARCHAR(36) NOT NULL COMMENT 'Identificador unico interno del reporte (UUID)',
    folio_pui VARCHAR(75) NOT NULL COMMENT 'Identificador oficial PUI <FUB>-<UUID4>',
    curp VARCHAR(255) NOT NULL COMMENT 'CURP cifrada AES-256',
    nombre VARCHAR(255) NULL COMMENT 'Nombre cifrado AES-256',
    primer_apellido VARCHAR(255) NULL COMMENT 'Primer apellido cifrado AES-256',
    segundo_apellido VARCHAR(255) NULL COMMENT 'Segundo apellido cifrado AES-256',
    fecha_nacimiento VARCHAR(255) NULL COMMENT 'Fecha nacimiento cifrada o ISO 8601',
    fecha_desaparicion VARCHAR(255) NULL COMMENT 'Fecha desaparicion cifrada o ISO 8601',
    lugar_nacimiento VARCHAR(100) NOT NULL COMMENT 'Entidad federativa, DESCONOCIDO o FORANEO',
    sexo ENUM('H', 'M', 'X') NULL COMMENT 'Sexo: H (Hombre), M (Mujer), X (Otros)',
    telefono VARCHAR(20) NULL COMMENT 'Telefono de contacto',
    correo VARCHAR(100) NULL COMMENT 'Correo electronico',
    direccion TEXT NULL COMMENT 'Direccion detallada de residencia',
    colonia VARCHAR(100) NULL COMMENT 'Colonia de residencia',
    codigo_postal VARCHAR(10) NULL COMMENT 'Codigo postal',
    municipio_alcaldia VARCHAR(100) NULL COMMENT 'Municipio o Alcaldia',
    entidad_federativa VARCHAR(100) NULL COMMENT 'Estado de residencia',
    estatus VARCHAR(20) DEFAULT 'ACTIVO' COMMENT 'ACTIVO o FINALIZADO',
    fecha_activacion DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha activacion PUI',
    fecha_desactivacion DATETIME NULL DEFAULT NULL COMMENT 'Fecha cierre de reporte',
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha registro tecnico local',
    fecha_actualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Ultima modificacion',
    PRIMARY KEY (id_reporte),
    UNIQUE INDEX idx_folio_unique (folio_pui),
    INDEX idx_curp_search (curp),
    INDEX idx_estatus_pui (estatus)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Maestro de reportes PUI';

-- 3. TABLA: pui_coincidencias (Relacionada a Reportes)
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_coincidencias (
    id_coincidencia VARCHAR(36) NOT NULL COMMENT 'Identificador unico de la coincidencia',
    folio_notificacion_pui VARCHAR(75) NOT NULL COMMENT 'ID de notificacion <FUB>-<UUID4>',
    id_reporte VARCHAR(36) NOT NULL COMMENT 'FK: Referencia al reporte PUI',
    fase_busqueda ENUM('1', '2', '3') NOT NULL COMMENT 'Fase: 1-Basica, 2-Historica, 3-Continua',
    curp_encontrada VARCHAR(255) NOT NULL COMMENT 'CURP detectada cifrada AES-256',
    lugar_nacimiento_encontrado VARCHAR(100) NOT NULL COMMENT 'Lugar nacimiento segun CURP detectada',
    guest_id VARCHAR(100) NOT NULL COMMENT 'ID del huesped en sistema PMS',
    tipo_evento VARCHAR(500) DEFAULT 'Hospedaje' COMMENT 'Tipo de operacion (Check-in/Hospedaje)',
    fecha_evento DATE NULL COMMENT 'Fecha del ingreso al hotel',
    descripcion_lugar_evento VARCHAR(500) NULL COMMENT 'Nombre del hotel o sucursal',
    detalle_notificacion_pui JSON NULL COMMENT 'Metadatos: domicilio, fotos, huellas en JSON',
    nivel_coincidencia ENUM('EXACT_CURP', 'NOMBRE_FECHA', 'POSIBLE') NOT NULL COMMENT 'Precision del match',
    notificado_pui BOOLEAN DEFAULT FALSE COMMENT 'Indica si se envio con exito a PUI',
    fecha_notificacion DATETIME NULL DEFAULT NULL COMMENT 'Fecha de confirmacion de la PUI',
    estatus_notificacion ENUM('PENDIENTE', 'ENVIADO', 'ERROR') DEFAULT 'PENDIENTE' COMMENT 'Estado del flujo de envio',
    log_respuesta_pui TEXT NULL COMMENT 'Respuesta o error retornado por la PUI',
    fecha_detectada DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha de deteccion local',
    fecha_actualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id_coincidencia),
    UNIQUE INDEX idx_folio_notif (folio_notificacion_pui),
    CONSTRAINT fk_reporte_match FOREIGN KEY (id_reporte) REFERENCES PUI_identity_mgmt.pui_reportes (id_reporte) ON DELETE CASCADE,
    INDEX idx_estatus_envio (estatus_notificacion),
    INDEX idx_reporte_match (id_reporte)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Registro de hallazgos';

-- 4. TABLA: pui_procesos_busqueda (Monitoreo de Jobs)
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_procesos_busqueda (
    id_proceso VARCHAR(36) NOT NULL COMMENT 'ID unico del job de busqueda',
    id_reporte VARCHAR(36) NULL COMMENT 'FK: Reporte especifico o NULL si es barrido general',
    tipo_busqueda ENUM('HISTORICA', 'CONTINUA') NOT NULL COMMENT 'Tipo de proceso ejecutado',
    registros_evaluados INT DEFAULT 0 COMMENT 'Cantidad de registros analizados',
    coincidencias_detectadas INT DEFAULT 0 COMMENT 'Matches encontrados en el proceso',
    fecha_inicio DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Inicio del proceso',
    fecha_fin DATETIME NULL COMMENT 'Fin del proceso',
    estatus ENUM('PROCESANDO', 'COMPLETADO', 'ERROR') DEFAULT 'PROCESANDO' COMMENT 'Estado del job',
    error_detalle TEXT NULL COMMENT 'Descripcion técnica del fallo',
    PRIMARY KEY (id_proceso),
    CONSTRAINT fk_reporte_busqueda FOREIGN KEY (id_reporte) REFERENCES PUI_identity_mgmt.pui_reportes (id_reporte) ON DELETE SET NULL,
    INDEX idx_tipo_busqueda (tipo_busqueda),
    INDEX idx_estatus_proceso (estatus),
    INDEX idx_fecha_inicio (fecha_inicio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Monitoreo de eficiencia de Jobs';

-- 5. TABLA: pui_reportes_historial (Trazabilidad de cambios)
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_reportes_historial (
    id_historial VARCHAR(36) NOT NULL COMMENT 'ID unico de historial',
    id_reporte VARCHAR(36) NOT NULL COMMENT 'FK: Reporte afectado',
    estatus_anterior VARCHAR(20) NULL COMMENT 'Estado previo',
    estatus_nuevo VARCHAR(20) NOT NULL COMMENT 'Estado actual',
    motivo TEXT NULL COMMENT 'Razon del cambio (Localizacion, Instruccion PUI)',
    fecha_cambio DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha de la transicion',
    PRIMARY KEY (id_historial),
    CONSTRAINT fk_reporte_historial FOREIGN KEY (id_reporte) REFERENCES PUI_identity_mgmt.pui_reportes (id_reporte) ON DELETE CASCADE,
    INDEX idx_historial_reporte (id_reporte),
    INDEX idx_fecha_historial (fecha_cambio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Bitacora de estados';

-- 6. TABLA: pui_eventos (Hitos de Negocio)
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_eventos (
    id_evento VARCHAR(36) NOT NULL COMMENT 'ID unico de auditoria de negocio',
    tipo_evento VARCHAR(50) NOT NULL COMMENT 'Categoria: ACTIVACION, MATCH_DETECTADO, etc',
    id_reporte VARCHAR(36) NULL COMMENT 'FK: Reporte relacionado',
    curp VARCHAR(255) NULL COMMENT 'CURP asociada cifrada',
    resultado VARCHAR(20) NULL COMMENT 'EXITO, ERROR, ADVERTENCIA',
    descripcion TEXT NULL COMMENT 'Detalle del hito de negocio',
    origen VARCHAR(50) NOT NULL COMMENT 'Origen: PUI, API_PAM, JOB',
    fecha_evento DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha del evento',
    PRIMARY KEY (id_evento),
    CONSTRAINT fk_reporte_auditoria FOREIGN KEY (id_reporte) REFERENCES PUI_identity_mgmt.pui_reportes (id_reporte) ON DELETE SET NULL,
    INDEX idx_tipo_evento (tipo_evento),
    INDEX idx_origen_evento (origen),
    INDEX idx_fecha_evento (fecha_evento)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Auditoria de cumplimiento';

-- 7. TABLA: pui_api_logs (Trazabilidad Técnica HTTP)
CREATE TABLE IF NOT EXISTS PUI_identity_mgmt.pui_api_logs (
    id_log VARCHAR(36) NOT NULL COMMENT 'ID unico del log tecnico',
    endpoint VARCHAR(100) NOT NULL COMMENT 'Ruta invocada',
    metodo VARCHAR(10) NOT NULL COMMENT 'GET, POST, etc',
    direccion ENUM('INBOUND', 'OUTBOUND') NOT NULL COMMENT 'Sentido del trafico',
    request_body JSON NULL COMMENT 'JSON de entrada',
    response_body JSON NULL COMMENT 'JSON de salida',
    http_status INT NULL COMMENT 'Codigo HTTP (200, 500, etc)',
    ip_origen VARCHAR(50) NULL COMMENT 'IP del solicitante',
    fecha_request DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha solicitud',
    duracion_ms INT NULL COMMENT 'Latencia en milisegundos',
    PRIMARY KEY (id_log),
    INDEX idx_endpoint_log (endpoint),
    INDEX idx_http_status (http_status),
    INDEX idx_fecha_request (fecha_request)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Logs tecnicos HTTP';