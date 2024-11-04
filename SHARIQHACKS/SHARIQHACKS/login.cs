using KeyAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SHARIQHACKS
{
    internal class login
    {
        public static api KeyAuthApp = new api(
    name: "KARACHI X CHEATS", // Application Name
    ownerid: "soejfry8CH", // Owner ID
    secret: "1d976154ef636b1d5d4ad5e518b59ea1327a274dfa709b2ca229dcd78e6631d5", // Application Secret
    version: "1.0"
);

        public static bool islogin = false;

        public void init()
        {
            login.KeyAuthApp.init();
            if (!login.KeyAuthApp.checkblack())
                return;
            Console.WriteLine("You Are Blacklisted By The Developer");
        }

        public void checklogin()
        {
            if (!login.islogin)
                return;
            Console.WriteLine("Don't Try To Break The Program");
        }

        public void initlogin(string username, string password)
        {
            login.KeyAuthApp.login(username, password);
            if (login.islogin)
            {
                Console.WriteLine("Don't Try To Break The Program");
            }
            else
            {
                if (login.KeyAuthApp.response.success)
                {
                    Console.WriteLine("Successfully Logged In");
                    login.islogin = true;
                }
                else
                {
                    login.islogin = false;
                    Console.WriteLine(login.KeyAuthApp.response.message);
                }
                login.KeyAuthApp.log("Someone Opened The GTC - Aimbot | Hwid = " + WindowsIdentity.GetCurrent().User.Value + " | Username = " + username + " | Password = " + password + " | Response = " + login.KeyAuthApp.response.message);
            }
        }

        public void autolog(string username, string password)
        {
            login.KeyAuthApp.login(username, password);
            if (!login.islogin)
                return;
            Console.WriteLine("Don't Try To Break The Program");
        }

        public static string GetPAttern(string value)
        {
            return login.islogin ? login.KeyAuthApp.var(value) : "0";
        }
    }
}
