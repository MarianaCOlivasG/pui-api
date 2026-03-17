using PUI.Domain.Enums;
using PUI.Domain.Exceptions;
using PUI.Domain.ValueObjects;

namespace PUI.Domain.Entities
{
    public class Reporte
    {
        public Guid Id { get; private set; }
        public string FolioPui { get; private set; } = null!;

        public string? Nombre { get; private set; }
        public string? PrimerApellido { get; private set; }
        public string? SegundoApellido { get; private set; }

        public string LugarNacimiento { get; private set; } = null!;

        public DateTime? FechaNacimiento { get; private set; }
        public DateTime? FechaDesaparicion { get; private set; }

        public CorreoElectronico? Correo { get; private set; }
        public string? Telefono { get; private set; }

        public string? Direccion { get; private set; }
        public string? Calle { get; private set; }
        public string? Numero { get; private set; }
        public string? Colonia { get; private set; }
        public string? CodigoPostal { get; private set; }
        public string? MunicipioOAlcaldia { get; private set; }
        public string? EntidadFederativa { get; private set; }

        public DateTime FechaActivacion { get; private set; }
        public DateTime? FechaDesactivacion { get; private set; }

        public Curp Curp { get; private set; } = null!;
        public Sexo? Sexo { get; private set; }
        public EstatusReporte Estatus { get; private set; }
        public DateTime? FechaUltimaBusqueda { get; private set; }

        private Reporte() { }

        public Reporte(
            string folioPui,
            Curp curp,
            string lugarNacimiento,
            string? nombre = null,
            string? primerApellido = null,
            string? segundoApellido = null,
            Sexo? sexo = null,
            DateTime? fechaNacimiento = null,
            DateTime? fechaDesaparicion = null,
            CorreoElectronico? correo = null,
            string? telefono = null,
            string? direccion = null,
            string? calle = null,
            string? numero = null,
            string? colonia = null,
            string? codigoPostal = null,
            string? municipioOAlcaldia = null,
            string? entidadFederativa = null
        )
        {
            AplicarReglasDeNegocio(folioPui, curp, lugarNacimiento);

            Id = Guid.NewGuid();
            FolioPui = folioPui;
            Curp = curp;
            LugarNacimiento = lugarNacimiento;

            Nombre = nombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;

            Sexo = sexo;

            FechaNacimiento = fechaNacimiento;
            FechaDesaparicion = fechaDesaparicion;

            Correo = correo;
            Telefono = telefono;

            Direccion = direccion;
            Calle = calle;
            Numero = numero;
            Colonia = colonia;
            CodigoPostal = codigoPostal;
            MunicipioOAlcaldia = municipioOAlcaldia;
            EntidadFederativa = entidadFederativa;

            FechaActivacion = DateTime.UtcNow;
            Estatus = EstatusReporte.Activo;
        }

        public void FinalizarReporte(DateTime fechaDesactivacion)
        {
            if (Estatus == EstatusReporte.Finalizado)
                throw new ExcepcionReglaNegocio("El reporte ya se encuentra finalizado.");

            FechaDesactivacion = fechaDesactivacion;
            Estatus = EstatusReporte.Finalizado;
        }

        private void AplicarReglasDeNegocio(
            string folioPui,
            Curp curp,
            string lugarNacimiento)
        {
            if (string.IsNullOrWhiteSpace(folioPui))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(FolioPui)}' es requerido.");

            if (curp is null)
                throw new ExcepcionReglaNegocio($"El campo '{nameof(Curp)}' es requerido.");

            if (string.IsNullOrWhiteSpace(lugarNacimiento))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(LugarNacimiento)}' es requerido.");
        }

        public void ActualizarFechaUltimaBusqueda(DateTime fecha)
        {
            FechaUltimaBusqueda = fecha;
        }


    }
}