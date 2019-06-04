using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contratos;
using Newtonsoft.Json;

namespace Logica
{
    public class Principal
    {
        /*
         public void EscribirSucursales(List<Sucursal> sucursales)
        {
            string path = @"C:\Datos\ArchivoSucursales.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(sucursales);
                archivo.Write(output);
            }
        }

        public List<Sucursal> ObtenerSucursales()
        {
            string path = @"C:\Datos\ArchivoSucursales.txt";
            List<Sucursal> lista = new List<Sucursal>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Sucursal>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Sucursal>();
            else
                return lista.Where(x => x.Eliminado == false).ToList();
        } 


        public Mensaje AltaDeSucursal(Sucursal sucursal)
        {
            Mensaje mensaje = new Mensaje() { Realizado = false, Descripción="Hay campos vacíos."};
            if (sucursal.Ciudad != "" & sucursal.CodigoPostal != 0 & sucursal.Direccion != "" & sucursal.TasaDeInteres != 0)
            {
                Sucursales = ObtenerSucursales();
                int c = Sucursales.Count() + 1;
                sucursal.ID = c;
                Sucursales.Add(sucursal);
                EscribirSucursales(Sucursales);
                mensaje.Realizado = true;
                mensaje.Descripción = "Se ha completado la acción correctamente.";
            }
            
            return mensaje;
        }
         
         
         */

        public UsuarioLogueado Loguear (string email, string clave)
        {
            UsuarioLogueado usuario = null;
            List<Clave> claves = ObtenerClaves();
            foreach (var item in claves)
            {
                if (item.Email == email & item.Contraseña == clave)
                {
                    usuario.Email = email;
                    usuario.Roles = item.Roles;
                    if (usuario.Roles[0] == Roles.Directora)
                    {
                        List<Directora> directoras = ObtenerDirectoras();
                        foreach (var dire in directoras)
                        {
                            if (dire.Email == email)
                            {
                                usuario.Nombre = dire.Nombre;
                                usuario.Apellido = dire.Apellido;
                            }
                        }
                    }
                }
            }
            return usuario;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            string path = @"C:\Datos\ArchivoUsuarios.txt";
            List<Usuario> lista = new List<Usuario>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Usuario>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Usuario>();
            else
                return lista;
        }

        public void EscribirUsuarios(List<Usuario> usuarios)
        {
            string path = @"C:\Datos\ArchivoUsuarios.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(usuarios);
                archivo.Write(output);
            }
        }

        public Resultado AltaDirectora(Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            Roles rol = Roles.Directora;
            if (rol != usuarioLogueado.RolSeleccionado)
                resultado.Errores.Add("El rol seleccionado no es el de Directora.");
            else

                return resultado;

        }

        public List<Clave> ObtenerClaves()
        {
            string path = @"C:\Datos\ArchivoClaves.txt";
            List<Clave> lista = new List<Clave>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Clave>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Clave>();
            else
                return lista;
        }


        public List<Directora> ObtenerDirectoras()
        {
            string path = @"C:\Datos\ArchivoDirectoras.txt";
            List<Directora> lista = new List<Directora>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Directora>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Directora>();
            else
                return lista;
        }

        public List<Docente> ObtenerDocentes()
        {
            string path = @"C:\Datos\ArchivoDocentes.txt";
            List<Docente> lista = new List<Docente>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Docente>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Docente>();
            else
                return lista;
        }

        public List<Padre> ObtenerPadres()
        {
            string path = @"C:\Datos\ArchivoPadres.txt";
            List<Padre> lista = new List<Padre>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Padre>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Padre>();
            else
                return lista;
        }

        public List<Hijo> ObtenerHijos()
        {
            string path = @"C:\Datos\ArchivoHijos.txt";
            List<Hijo> lista = new List<Hijo>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Hijo>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Hijo>();
            else
                return lista;
        }

    }
}
