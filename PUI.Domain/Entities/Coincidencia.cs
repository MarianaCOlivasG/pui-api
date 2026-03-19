using PUI.Domain.Entities;
using PUI.Domain.Enums;
using PUI.Domain.Exceptions;
using PUI.Domain.ValueObjects;

public class Coincidencia
{
    public Guid Id { get; private set; }

    public string FolioNotificacionPui { get; private set; } = null!;

    public Guid IdReporte { get; private set; }

    public FaseBusqueda FaseBusqueda { get; private set; }

    public Curp CurpEncontrada { get; private set; } = null!;

    public string? LugarNacimientoEncontrado { get; private set; }

    public string? GuestId { get; private set; }

    public string? TipoEvento { get; private set; }

    public DateTime? FechaEvento { get; private set; }

    public string? DescripcionLugarEvento { get; private set; }

    public string? DetalleNotificacionPui { get; private set; }

    public NivelCoincidencia NivelCoincidencia { get; private set; }

    public bool NotificadoPui { get; private set; }

    public DateTime? FechaNotificacion { get; private set; }

    public EstatusNotificacion EstatusNotificacion { get; private set; }

    public string? LogRespuestaPui { get; private set; }

    public DateTime FechaDetectada { get; private set; }

    public DateTime FechaActualizacion { get; private set; }

    public Reporte Reporte { get; private set; } = null!;

    private Coincidencia() { }

    public static Coincidencia Crear(
        Guid idReporte,
        string folioNotificacionPui,
        Curp curpEncontrada,
        string? tipoEvento,
        DateTime? fechaEvento,
        string? descripcionLugarEvento,
        string? detalleJson = null,
        string? lugarNacimiento = null,
        string? guestId = null,
        NivelCoincidencia nivel = NivelCoincidencia.POSIBLE,
        FaseBusqueda fase = FaseBusqueda.Fase3
    )
    {
        // Validación de reglas de negocio
        if (string.IsNullOrWhiteSpace(folioNotificacionPui))
            throw new ExcepcionReglaNegocio("Folio requerido");

        if (idReporte == Guid.Empty)
            throw new ExcepcionReglaNegocio("IdReporte requerido");

        if (curpEncontrada is null)
            throw new ExcepcionReglaNegocio("CURP requerida");

        return new Coincidencia
        {
            // Se genera nuevo ID
            Id = Guid.NewGuid(),

            // Se asignan datos principales
            IdReporte = idReporte,
            FolioNotificacionPui = folioNotificacionPui,
            CurpEncontrada = curpEncontrada,

            // Datos de la notificación
            TipoEvento = tipoEvento,
            FechaEvento = fechaEvento,
            DescripcionLugarEvento = descripcionLugarEvento,
            DetalleNotificacionPui = detalleJson,

            // Datos adicionales
            LugarNacimientoEncontrado = lugarNacimiento,
            GuestId = guestId,

            // Configuración de negocio
            NivelCoincidencia = nivel,
            FaseBusqueda = fase,

            // Estado inicial
            EstatusNotificacion = EstatusNotificacion.PENDIENTE,
            NotificadoPui = false,

            // Fechas de auditoría
            FechaDetectada = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };
    }

    public void MarcarComoNotificado(string? respuesta)
    {
        NotificadoPui = true;
        FechaNotificacion = DateTime.UtcNow;
        EstatusNotificacion = EstatusNotificacion.ENVIADO;
        LogRespuestaPui = respuesta;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void MarcarError(string error)
    {
        EstatusNotificacion = EstatusNotificacion.ERROR;
        LogRespuestaPui = error;
        FechaActualizacion = DateTime.UtcNow;
    }
}