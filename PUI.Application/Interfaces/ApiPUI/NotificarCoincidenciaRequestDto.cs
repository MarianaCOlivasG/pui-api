
namespace PUI.Application.Interfaces.ApiPUI
{
    public class NotificarCoincidenciaRequestDto
    {
        public required string curp { get; set; } = null!;
        public object? nombre_completo { get; set; }
        public string? nombre { get; set; }
        public string? primer_apellido { get; set; }
        public string? segundo_apellido { get; set; }
        public string? fecha_nacimiento { get; set; }
        public required string lugar_nacimiento { get; set; } = null!;
        public string sexo_asignado { get; set; } = null!;
        public string? telefono { get; set; }
        public string? correo { get; set; }
        public object? domicilio { get; set; }
        public object? fotos { get; set; }
        public string? formato_fotos { get; set; }
        public object? huellas { get; set; }
        public string? formato_huellas { get; set; }
        public required string id { get; set; } = null!;
        public required string institucion_id { get; set; } = null!;
        public string? tipo_evento { get; set; }
        public string? fecha_evento { get; set; }
        public string? descripcion_lugar_evento { get; set; }
        public object? direccion_evento { get; set; }
        public required int fase_busqueda { get; set; } = 0;
    }
}
