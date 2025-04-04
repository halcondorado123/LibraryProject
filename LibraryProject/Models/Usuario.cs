namespace LibraryProject.Models
{
    public class Usuario
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Pais Pais { get; set; }
        public int Edad { get; set; }
        public string Salario { get; set; }
    }
}
