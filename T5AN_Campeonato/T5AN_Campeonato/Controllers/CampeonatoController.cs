using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using T5AN_Campeonato.Models;

namespace T5AN_Campeonato.Controllers
{
    public class CampeonatoController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["CNX"].ConnectionString);

        public ActionResult Index()
        {
            return View();
        }

        List<Consulta> listEquipoxLocalidadxDistrito(string localidad, int distrito)
        {

            List<Consulta> arregloConsulta = new List<Consulta>();
            SqlCommand cmd = new SqlCommand("SP_LISTAEQUIPOXLOCXDIS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@loc", localidad);
            cmd.Parameters.AddWithValue("@dis", distrito);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                arregloConsulta.Add(new Consulta()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    distrito = dr[2].ToString(),
                    localidad = dr[3].ToString(),
                    fecha = DateTime.Parse(dr[4].ToString()).ToString("dd/MM/yyyy"),
                    años = dr[5].ToString()
                });
            }

            dr.Close();
            cn.Close();
            return arregloConsulta;
        }

        List<Equipo> listEquipos()
        {
            List<Equipo> arregloEquipo = new List<Equipo>();
            SqlCommand cmd = new SqlCommand("SP_LISTADOEQUIPOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                arregloEquipo.Add(new Equipo()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    distrito = dr[2].ToString(),
                    localidad = dr[3].ToString(),
                    fecha = DateTime.Parse(dr[4].ToString()).ToString("dd/MM/yyyy"),
                    fundacion = dr[5].ToString()
                });
            }

            dr.Close();
            cn.Close();

            return arregloEquipo;
        }

        List<Equipo_> listEquipos_()
        {
            List<Equipo_> arregloEquipo = new List<Equipo_>();
            SqlCommand cmd = new SqlCommand("SP_LISTADOEQUIPOS_", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                arregloEquipo.Add(new Equipo_()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    distrito = int.Parse(dr[2].ToString()),
                    localidad = dr[3].ToString(),
                    fecha = DateTime.Parse(dr[4].ToString()).ToString("dd/MM/yyyy"),
                    fundacion = dr[5].ToString()
                });
            }

            dr.Close();
            cn.Close();

            return arregloEquipo;
        }

        public ActionResult listadoEquipo()
        {
            return View(listEquipos());
        }

        public ActionResult listadoEquipoxPag(int p = 0)
        {
            List<Equipo> arregloEquipo = listEquipos();
            int filasxPag = 10;
            int n = arregloEquipo.Count;
            int pag = n % filasxPag > 0 ? n / filasxPag + 1 : n / filasxPag;

            ViewBag.pag = pag;
            ViewBag.p = p;
            return View(arregloEquipo.Skip(p * filasxPag).Take(filasxPag));
        }

        List<Distrito> listDistritos()
        {
            List<Distrito> arregloDistritos = new List<Distrito>();
            SqlCommand cmd = new SqlCommand("SP_LISTADODISTRITOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                arregloDistritos.Add(new Distrito()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString()
                });
            }

            dr.Close();
            cn.Close();

            return arregloDistritos;
        }

        public ActionResult nuevoEquipo()
        {
            ViewBag.distrito = new SelectList(listDistritos(), "codigo", "nombre");          
            ViewBag.codigo = codigoActual();

            return View(new Equipo_());
        }

        [HttpPost]
        public ActionResult nuevoEquipo(Equipo_ objetoEquipo)
        {
           if(!ModelState.IsValid)
           {
                return View(objetoEquipo);
           }

            cn.Open();
            ViewBag.mensaje = "";

            SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MANTENIMIENTOEQUIPO", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objetoEquipo.codigo);
                cmd.Parameters.AddWithValue("@nom", objetoEquipo.nombre);
                cmd.Parameters.AddWithValue("@dis", objetoEquipo.distrito);
                cmd.Parameters.AddWithValue("@loc", objetoEquipo.localidad);
                cmd.Parameters.AddWithValue("@fec", objetoEquipo.fecha);
                cmd.Parameters.AddWithValue("@fun", objetoEquipo.fundacion);

                int estado = cmd.ExecuteNonQuery();
                tr.Commit();

                ViewBag.mensaje = estado.ToString() + " Equipo Registrado Correctamente";

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                tr.Rollback();
            }

            cn.Close();

            ViewBag.distrito = new SelectList(listDistritos(), "codigo", "nombre");
            ViewBag.codigo = objetoEquipo.codigo;

            return View(objetoEquipo);
        }

        public ActionResult detalleEquipo(int id)
        {
            Equipo objetoEquipo = listEquipos().Where(e => e.codigo == id).FirstOrDefault();
            return View(objetoEquipo);
        }

        public ActionResult modificaEquipo(int id)
        {
            Equipo_ objetoEquipo = listEquipos_().Where(e => e.codigo == id).FirstOrDefault();
            ViewBag.distrito = new SelectList(listDistritos(), "codigo", "nombre", objetoEquipo.distrito);
            return View(objetoEquipo);
        }

        [HttpPost]
        public ActionResult modificaEquipo(Equipo_ objetoEquipo)
        {
            if (!ModelState.IsValid)
            {
                return View(objetoEquipo);
            }

            cn.Open();
            ViewBag.mensaje = "";

            SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MANTENIMIENTOEQUIPO", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objetoEquipo.codigo);
                cmd.Parameters.AddWithValue("@nom", objetoEquipo.nombre);
                cmd.Parameters.AddWithValue("@dis", objetoEquipo.distrito);
                cmd.Parameters.AddWithValue("@loc", objetoEquipo.localidad);
                cmd.Parameters.AddWithValue("@fec", objetoEquipo.fecha);
                cmd.Parameters.AddWithValue("@fun", objetoEquipo.fundacion);

                int estado = cmd.ExecuteNonQuery();
                tr.Commit();

                ViewBag.mensaje = estado.ToString() + " Equipo Actualizado Correctamente";

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                tr.Rollback();
            }

            cn.Close();
            ViewBag.distrito = new SelectList(listDistritos(), "codigo", "nombre", objetoEquipo.distrito);
            return View(objetoEquipo);
        }

        public ActionResult eliminaEquipo(int id)
        {

            Equipo objetoEquipo = listEquipos().Where(e => e.codigo == id).FirstOrDefault();
            cn.Open();
            SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ELIMINAEQUIPO", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objetoEquipo.codigo);
                int estado = cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                tr.Rollback();
            }

            cn.Close();

            return RedirectToAction("listadoEquipoxPag");
        }

        public ActionResult listadoEquipoxLocalidadxDistrito(string localidad = "", int distrito = 0)
        {
            ViewBag.localidad = localidad;
            ViewBag.distrito = distrito;
            ViewBag.distritos = new SelectList(listDistritos(), "codigo", "nombre");
            return View(listEquipoxLocalidadxDistrito(localidad, distrito));

        }

        public int codigoActual()
        {
            int ultimo = 0;
            SqlCommand cmd = new SqlCommand("SP_SIGUIENTECODIGOEQUIPO", cn);
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ultimo = int.Parse(dr[0].ToString());
 
            }

            dr.Close();
            cn.Close();

            return ultimo;
        }

    }
}