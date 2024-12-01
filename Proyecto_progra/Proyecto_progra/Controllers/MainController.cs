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
    }


}