using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Diplomski.Helper
{
    public class ExceptionHandler
    {
       

        public static string GetConstraintExceptionMessage(Exception error)
        {
            string NewMessage = error.InnerException.InnerException.Message;

            int startIndex = NewMessage.IndexOf("'");
            int endIndex = NewMessage.IndexOf("'", startIndex + 1);
            if (startIndex > 0 && endIndex > 0)
            {
                string cnstraintName = NewMessage.Substring(startIndex + 1, endIndex - startIndex - 1);

                switch (cnstraintName)
                {
                    case "UQ_BrojDosijea":
                        NewMessage = "Broj dosijea je već u upotrebi!  Molimo unesite drugi";
                        break;
                    case "UQ_Email":
                        NewMessage = "Email je već u upotrebi!  Molimo unesite drugi";
                        break;
                    case "UQ_RFID":
                        {
                            NewMessage = "RFID je već u upotrebi!  Molimo unesite drugi";
                            break;
                        }

                }
                if (NewMessage == error.Message)
                {
                    NewMessage = "Greška na serveru!" + error.Message;
                }
            }
            return NewMessage;
        }
    }
}