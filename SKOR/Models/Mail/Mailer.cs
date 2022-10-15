using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;

namespace Correos
{
    public abstract class Mailer
    {
        public delegate object BuildContenido();

        //SENDER ------------------------------------------------------------------------
        private string miFrom;
        internal string From { get { return miFrom; } set { miFrom = value; } }

        private string miMailFrom;
        private string MailFrom { get { return miMailFrom; } set { miMailFrom = value; } }

        private string miBCC;
        internal string MailBCC { get { return miBCC; } set { miBCC = value; } }

        private string miReplyTo;
        internal string MailReplyTo { get { return miReplyTo; } set { miReplyTo = value; } }



        //SERVIDOR ------------------------------------------------------------------------
        private SmtpClient miSMTP;
        private string Host { get { return miSMTP.Host; } set { this.miSMTP.Host = value; } }

        private int PuertoSMTP
        {
            get { return miSMTP.Port; }
            set { miSMTP.Port = value; }
        }

        private string usuarioSMTP_;
        private string UsuarioSMTP
        {
            get { return this.usuarioSMTP_; }
            set
            {
                this.usuarioSMTP_ = value;
                this.setUserSMTP();
            }
        }

        private string claveSMTP_;
        private string ClaveSMTP
        {
            get { return this.claveSMTP_; }
            set
            {
                this.claveSMTP_ = value;
                this.setUserSMTP();
            }
        }

        private bool enableSSL
        {
            get { return this.miSMTP.EnableSsl; }
            set { this.miSMTP.EnableSsl = value; }
        }

        //CORREO------------------------------------------------------------------------------------------
        private List<string> miMailTo;
        private List<string> MailTo
        {
            get { return miMailTo; }
            set { miMailTo = value; }
        }

        private List<string> miMailCC;
        private List<string> MailCC
        {
            get { return miMailCC; }
            set { miMailCC = value; }
        }

        private string miTema;
        private string Tema
        {
            get { return miTema; }
            set { miTema = value; }
        }

        private bool correoEsHtml;
        public bool esHTML
        {
            get { return correoEsHtml; }
            set { correoEsHtml = value; }
        }

        private string miMailDebug;
        public string MailDebug
        {
            get { return miMailDebug; }
            set { this.miMailDebug = value; }
        }

        private List<Attachment> miAdjunto;
        public List<Attachment> Adjuntos
        {
            get { return miAdjunto; }
            set { this.miAdjunto = value; }
        }

        /// <summary>
        /// PRIVADO, usado para setear credencial
        /// </summary>
        /// <remarks></remarks>
        private void setUserSMTP()
        {
            if (!string.IsNullOrEmpty(this.UsuarioSMTP) && !string.IsNullOrEmpty(this.ClaveSMTP))
            {
                this.miSMTP.Credentials = new NetworkCredential(this.UsuarioSMTP, this.ClaveSMTP);
            }
        }

        public Mailer()
        {
            this.miSMTP = new SmtpClient();
            this.MailTo = new List<string>();
            this.miAdjunto = new List<Attachment>();

            this.SetConfiguracionAppSettings();
        }


        private void SetConfiguracionServidor(string host, int puerto = 0, bool esSSL = false, string usuario = "", string clave = "")
        {
            this.Host = host;

            if (puerto != 0)
            {
                this.PuertoSMTP = puerto;
            }

            if (!string.IsNullOrEmpty(usuario))
            {
                this.UsuarioSMTP = usuario;
            }

            if (!string.IsNullOrEmpty(clave))
            {
                this.ClaveSMTP = clave;
            }

            this.enableSSL = esSSL;

        }

        private void SetConfiguracionSender(string mailFrom, string from_, string BCC = "")
        {
            this.MailFrom = mailFrom;
            this.From = from_;
            this.MailBCC = BCC;
        }

        /// <summary>
        /// Leo la configuración de los appsettings por defecto
        /// </summary>
        /// <remarks></remarks>
        private void SetConfiguracionAppSettings()
        {
            string host = null;
            int puerto = 0;
            bool ssl = false;
            string usuario = null;
            string clave = null;

            string mailFrom = null;
            string from_ = null;
            string BCC = null;

            host = ConfigurationManager.AppSettings["SMTP"];
            puerto = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_puerto"]);
            ssl = ConfigurationManager.AppSettings["SMTP_ssl"] == "true" ? true : false;
            usuario = ConfigurationManager.AppSettings["SMTP_usuario"];
            clave = ConfigurationManager.AppSettings["SMTP_clave"];

            mailFrom = ConfigurationManager.AppSettings["MailDe"];
            from_ = ConfigurationManager.AppSettings["MailSender"];
            BCC = ConfigurationManager.AppSettings["MailCopias"];

            this.SetConfiguracionServidor(host, puerto, ssl, usuario, clave);
            this.SetConfiguracionSender(mailFrom, from_, BCC);

            this.MailDebug = ConfigurationManager.AppSettings["MailDebug"];
        }

        protected internal object Enviar(string tema, string correoDestinatario, BuildContenido function_, List<string> MailsConCopia = null)
        {
            bool retorno = false;

            this.Tema = tema;
            this.MailTo.Add(correoDestinatario);
            this.MailCC = MailsConCopia;
            retorno = this.SendMailOneByAll(function_);

            return retorno;
        }

        protected internal object Enviar(string tema, List<string> correosDestinatarios, bool UnoPorUno, BuildContenido function_, List<string> MailsConCopia = null)
        {
            bool retorno = false;

            this.Tema = tema;
            this.MailTo = correosDestinatarios;
            this.MailCC = MailsConCopia;

            if (UnoPorUno)
            {
                retorno = this.SendMailOneByOne(function_);
            }
            else
            {
                retorno = this.SendMailOneByAll(function_);
            }

            return retorno;
        }

        private object getEmailToSender(string correo)
        {
            string retorno = null;

            if (string.IsNullOrEmpty(this.MailDebug))
            {
                retorno = correo;
            }
            else
            {
                retorno = this.MailDebug;
            }

            return retorno;
        }


        private bool SendMailOneByAll(BuildContenido funcion)
        {
            MailMessage mensaje = default(MailMessage);
            string correo = null;
            bool estado = false;
            string textoMensaje = null;


            estado = false;
            mensaje = new MailMessage();

            try
            {
                textoMensaje = Convert.ToString(funcion());
                //Me.ConstruirContenido()


                if (this.MailTo.Count > 0 & !string.IsNullOrEmpty(textoMensaje))
                {
                    foreach (string correo_loopVariable in this.MailTo)
                    {
                        correo = correo_loopVariable;
                        if (ValidateEmail(correo))
                        {
                            mensaje.To.Add(this.getEmailToSender(correo).ToString());
                        }
                        else
                        {
                            Console.WriteLine("Correo no válido: " + correo);
                        }
                    }

                    if (mensaje.To.Count == 0)
                    {
                        throw new Exception("Correo sin destinatario");
                    }

                    //
                    if (ValidateEmail(this.MailBCC) && string.IsNullOrEmpty(this.MailDebug))
                    {
                        mensaje.Bcc.Add(new MailAddress(this.MailBCC));
                    }

                    if (((this.MailCC != null)) && this.MailCC.Count > 0 && string.IsNullOrEmpty(this.MailDebug))
                    {
                        foreach (string correoCC in this.MailCC)
                        {
                            if (ValidateEmail(correoCC))
                                mensaje.CC.Add(new MailAddress(correoCC));
                        }
                    }

                    mensaje.Body = textoMensaje;
                    mensaje.IsBodyHtml = this.esHTML;
                    if (ValidateEmail(this.miMailFrom))
                    {
                        mensaje.From = new MailAddress(this.MailFrom, this.From);
                    }

                    mensaje.Subject = this.Tema;
                    if (ValidateEmail(this.MailReplyTo))
                        mensaje.ReplyToList.Add(this.MailReplyTo);

                    if (this.Adjuntos.Count > 0) {
                        foreach (Attachment adj in Adjuntos) {
                            mensaje.Attachments.Add(adj);
                        }
                    }


                    this.miSMTP.Send(mensaje);
                    estado = true;
                }
                else
                {
                    throw new Exception("No hay correos o texto");
                }
                this.limpiar();
            }
            catch (Exception ex)
            {
                estado = false;
                Console.WriteLine("Error en mailer: " + ex.ToString());
            }

            return estado;
        }

        /// <summary>
        /// Envia el mismo correo uno por uno de la lista de envio, tambien se deberia poder crear contenido dinamico, fase 2
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool SendMailOneByOne(BuildContenido funcion)
        {
            MailMessage mensaje = default(MailMessage);
            string correo = null;
            bool estado = false;
            long contador = 0;
            string textoMensaje = null;

            estado = false;

            try
            {
                textoMensaje = Convert.ToString(funcion());
                // Me.ConstruirContenido()


                if (this.MailTo.Count > 0 & !string.IsNullOrEmpty(textoMensaje))
                {
                    contador = 0;
                    foreach (string correo_loopVariable in this.MailTo)
                    {
                        correo = correo_loopVariable;
                        if (ValidateEmail(correo))
                        {
                            mensaje = new MailMessage();

                            mensaje.To.Add(this.getEmailToSender(correo).ToString());
                            //
                            if (contador == 0 && ValidateEmail(this.MailBCC) && string.IsNullOrEmpty(this.MailDebug))
                            {
                                mensaje.Bcc.Add(new MailAddress(this.MailBCC));
                            }

                            if (contador == 0 && ((this.MailCC != null)) && this.MailCC.Count > 0 && string.IsNullOrEmpty(this.MailDebug))
                            {
                                foreach (string correoCC in this.MailCC)
                                {
                                    if (ValidateEmail(correoCC))
                                        mensaje.CC.Add(new MailAddress(correoCC));
                                }
                            }

                            mensaje.Body = textoMensaje;
                            mensaje.IsBodyHtml = this.esHTML;
                            if (ValidateEmail(this.MailFrom))
                            {
                                mensaje.From = new MailAddress(this.MailFrom, this.From);
                            }
                            mensaje.Subject = this.Tema;

                            if (ValidateEmail(this.MailReplyTo))
                                mensaje.ReplyToList.Add(this.MailReplyTo);

                            if (this.Adjuntos.Count > 0)
                            {
                                foreach (Attachment at in this.Adjuntos) {
                                    mensaje.Attachments.Add(at);
                                }
                            }

                            this.miSMTP.Send(mensaje);
                        }
                        else
                        {
                            Console.WriteLine("Correo no válido: " + correo);
                        }
                    }
                    estado = true;
                }
                this.limpiar();
            }
            catch (Exception ex)
            {
                estado = false;
                Console.WriteLine("Error en mailer: " + ex.ToString());
            }


            return estado;
        }


        protected internal string LeerPlantilla(string path)
        {
            StreamReader reader = default(StreamReader);
            string str = null;

            reader = new StreamReader(path, Encoding.Default);
            str = reader.ReadToEnd();
            reader.Close();

            return str;
        }

        private void limpiar()
        {
            this.MailTo = new List<string>();
            this.MailCC = new List<string>();
        }

        public static bool ValidateEmail(string email)
        {
            bool retorno = false;
            try
            {
                retorno = false;
                if (((email != null)) && !string.IsNullOrEmpty(email.Trim()))
                {
                    System.Text.RegularExpressions.Regex emailRegex = new System.Text.RegularExpressions.Regex("^(?<user>[^@]+)@(?<host>.+)$");
                    System.Text.RegularExpressions.Match emailMatch = emailRegex.Match(email);
                    retorno = emailMatch.Success;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error al verificar correo:{0} {1}", email, ex.ToString()));
                retorno = false;
            }

            return retorno;
        }

        /// <summary>
        /// Debe ser usado para construir el texto a ser enviado
        /// </summary>
        /// <remarks></remarks>
        //Public MustOverride Function ConstruirContenido() As String

        /// <summary>
        /// Debe usarse para enviar el correo, colocar mensaje,tema, destinatarios
        /// </summary>
        /// <remarks></remarks>
        public abstract void EnviarCorreo();
    }
}