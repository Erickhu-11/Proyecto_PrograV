using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_progra.Models;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace Proyecto_progra.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Mainpage()
        {
            return View();
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }

        public ActionResult CrearConcierto()
        {
            return View();
        }
        public ActionResult BorrarConcierto()
        {
            return View();
        }
        public ActionResult Modificar_Concierto()
        {
            return View();
        }
        public ActionResult MainPageAdmin()
        {
            return View();
        }

        [HttpPost]

        public JsonResult Login(string CORREO, string CLAVE)
        {
            using (ConciertosEntities1 bd = new ConciertosEntities1())
            {
                try
                {
                    var resultado = bd.Database.SqlQuery<int?>(
                        "EXEC SP_VALIDARUSUARIO @CORREO, @CLAVE",
                        new System.Data.SqlClient.SqlParameter("@CORREO", CORREO),
                        new System.Data.SqlClient.SqlParameter("@CLAVE", CLAVE)
                    ).FirstOrDefault();

                    if (resultado != null && resultado > 0)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Login exitoso",
                            data = new
                            {
                                IDUSUARIO = resultado
                            }
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Correo o clave incorrectos"
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ocurrió un error durante el proceso de login",
                        error = ex.Message
                    });
                }
            }
        }

        [HttpPost]
        public JsonResult AgregarConcierto( string nombre_Banda,string genero_Musical,DateTime? fecha_Concierto,TimeSpan? hora_Concierto,string pais,string direccion_Concierto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre_Banda) || string.IsNullOrWhiteSpace(genero_Musical) ||
                    !fecha_Concierto.HasValue || !hora_Concierto.HasValue ||
                    string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(direccion_Concierto))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Todos los campos son obligatorios."
                    });
                }

                using (var bd = new ConciertosEntities1())
                {
                    var registrado = new ObjectParameter("REGISTRADO", typeof(bool));
                    var mensaje = new ObjectParameter("MENSAJE", typeof(string));

                    bd.SP_REGISTRARCONCIERTO(nombre_Banda,genero_Musical,fecha_Concierto,hora_Concierto,pais,direccion_Concierto,registrado,mensaje);

                    bool registradoValue = (bool)registrado.Value;
                    string mensajeValue = (string)mensaje.Value;

                    return Json(new
                    {
                        success = registradoValue,
                        message = mensajeValue
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al registrar el concierto.",
                    error = ex.Message
                });
            }
        }

        public JsonResult ObtenerConciertos()
        {
            using (ConciertosEntities1 bd = new ConciertosEntities1())
            {
                var conciertos = bd.Concierto.Select(c => new
                {
                    c.Nombre_Banda,
                    c.Genero_Musical,
                    c.Fecha_Concierto,
                    c.Hora_Concierto,
                    c.Pais,
                    c.Direccion_Concierto
                }).ToList();

                var conciertosFormateados = conciertos.Select(c => new
                {
                    c.Nombre_Banda,
                    c.Genero_Musical,
                    Fecha_Concierto = c.Fecha_Concierto.ToString("yyyy-MM-dd"),
                    Hora_Concierto = c.Hora_Concierto.ToString(@"hh\:mm"),
                    c.Pais,
                    c.Direccion_Concierto
                }).ToList();

                return Json(new
                {
                    success = true,
                    data = conciertosFormateados
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EliminarConcierto(int id)
        {
            using (ConciertosEntities1 db = new ConciertosEntities1())
            {
                try
                {
                    var paramId = new SqlParameter("@ID", id);
                    var paramEliminado = new SqlParameter
                    {
                        ParameterName = "@ELIMINADO",
                        SqlDbType = System.Data.SqlDbType.Bit,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    var paramMensaje = new SqlParameter
                    {
                        ParameterName = "@MENSAJE",
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Size = 100,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    db.Database.ExecuteSqlCommand(
                        "EXEC SP_ELIMINARCONCIERTO @ID, @ELIMINADO OUTPUT, @MENSAJE OUTPUT",
                        paramId, paramEliminado, paramMensaje
                    );

                    bool eliminado = (bool)paramEliminado.Value;
                    string mensaje = paramMensaje.Value.ToString();

                    return Json(new
                    {
                        success = eliminado,
                        message = mensaje
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ocurrió un error al intentar eliminar el concierto: " + ex.Message
                    });
                }
            }
        }

        [HttpPost]
        public JsonResult ModificarConcierto(int idConcierto, string nombreBanda, string generoBanda, DateTime fechaConcierto, TimeSpan horaConcierto, string pais, string direccionConcierto)
        {
            try
            {
                using (ConciertosEntities1 db = new ConciertosEntities1())
                {
                    var resultadoMensaje = new ObjectParameter("MENSAJE", typeof(string));
                    var resultadoModificado = new ObjectParameter("MODIFICADO", typeof(bool));

                    db.SP_MODIFICARCONCIERTO(idConcierto,nombreBanda,generoBanda,fechaConcierto,horaConcierto,pais,direccionConcierto,resultadoModificado,resultadoMensaje);

                    bool modificado = (bool)resultadoModificado.Value;
                    string mensaje = resultadoMensaje.Value.ToString();

                    return Json(new { success = modificado, message = mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al intentar modificar el concierto: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}