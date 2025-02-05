using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using user_register.Models;



namespace user_register.Data
{
    public class DataAccess
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;

        public static IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<Usuario> ListarUsuarios()
        {

            List<Usuario> usuarios = new List<Usuario>();


            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[listar_usuarios]";
                _connection.Open();


                SqlDataReader reader = _command.ExecuteReader();


                while (reader.Read())
                {
                    Usuario usuario = new Usuario();

                    usuario.Id = Convert.ToInt32(reader["ID"]);
                    usuario.Nome = Convert.ToString(reader["Nome"]);
                    usuario.Sobrenome = Convert.ToString(reader["Sobrenome"]);
                    usuario.Email = Convert.ToString(reader["Email"]);
                    usuario.Cargo = Convert.ToString(reader["Cargo"]);


                    usuarios.Add(usuario);
                }
                _command.Clone();
            }
            return usuarios;
        }

        public bool Cadastrar(Usuario usuario)
        {
            int id = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[inserir_usuario]";

                _command.Parameters.AddWithValue("@Nome", usuario.Nome);
                _command.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                _command.Parameters.AddWithValue("@Email", usuario.Email);
                _command.Parameters.AddWithValue("@Cargo", usuario.Cargo);

                _connection.Open();

                id = _command.ExecuteNonQuery();

                _connection.Close();
            }

            return id > 0 ? true : false;
        }

        public Usuario BuscarUsuarioPorId(int id)
        {
            Usuario usuario = new Usuario();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[listar_usuario_id]";
                _command.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                SqlDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["Id"]);
                    usuario.Nome = Convert.ToString(reader["Nome"]);
                    usuario.Sobrenome = Convert.ToString(reader["Sobrenome"]);
                    usuario.Email = Convert.ToString(reader["Email"]);
                    usuario.Cargo = Convert.ToString(reader["Cargo"]);
                }

                _connection.Close();
            }
            return usuario;
        }

        public bool EditarUsuario(Usuario usuario)
        {

            var id = 0;


            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[editar_usuario]";

                _command.Parameters.AddWithValue("@Id", usuario.Id);
                _command.Parameters.AddWithValue("@Nome", usuario.Nome);
                _command.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                _command.Parameters.AddWithValue("@Email", usuario.Email);
                _command.Parameters.AddWithValue("@Cargo", usuario.Cargo);

                _connection.Open();

                id = _command.ExecuteNonQuery();

                _connection.Close();
            }
            return id > 0 ? true : false;
        }

        public bool RemoverUsuario(int id)
        {
            var result = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[remover_usuario]";

                _command.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                id = _command.ExecuteNonQuery();

                _connection.Close();
            }
            return result > 0 ? true : false;
        }
    }
}
