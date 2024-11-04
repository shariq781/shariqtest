using Beyondmem;
using Microsoft.Win32;
using SHARIQHACKS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static string user;
    private static string pass;
    private static bool remme = true;
    private static bool auto = true;
    private static string hwid = WindowsIdentity.GetCurrent().User.Value;
    public static string PID;
    public static MemMirza MemLib = new MemMirza();
    static Dictionary<long, byte[]> originalValues = new Dictionary<long, byte[]>();
    static Dictionary<long, byte[]> headvalues = new Dictionary<long, byte[]>();


    public static void createRegistry()
    {
        RegistryKey subKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\RawAccel");

        subKey.SetValue("Remme", (object)"true");
        subKey.SetValue("Username", (object)(Program.user ?? ""));
        subKey.SetValue("Password", (object)(Program.pass ?? ""));
        subKey.SetValue("Hwid", (object)(Program.hwid ?? ""));
        if (Program.remme)
            subKey.SetValue("Remme", (object)"true");
        else
            subKey.SetValue("Remme", (object)"false");
        if (Program.auto)
            subKey.SetValue("Auto-Login", (object)"true");
        else
            subKey.SetValue("Auto-Login", (object)"false");
        subKey.Close();
    }
    private static async Task load()
    {
        login login = new login();
        login.init();
        login.checklogin();
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\RawAccel");
        if (registryKey != null)
        {
            Program.remme = !((string)registryKey.GetValue("Remme") != "true");
            if (Program.remme)
            {
                Program.user = (string)registryKey.GetValue("Username");
                Program.pass = (string)registryKey.GetValue("Password");
                login.autolog(Program.user, Program.pass);
                if (login.KeyAuthApp.response.success)
                {
                    Console.WriteLine("Successfully Logged In");
                    login.islogin = true;
                }
                else
                {
                    login.islogin = false;
                    Console.WriteLine("Enter Your Username");
                    Program.user = Console.ReadLine();
                    Console.WriteLine("Enter Your Password");
                    Program.pass = Console.ReadLine();
                    Console.WriteLine("Could Not Proceed With Auto Login Enter Your Username & Password Manually");
                    login.initlogin(Program.user, Program.pass);
                }
                login.KeyAuthApp.log("Someone Opened The GTC - Aimbot | Hwid = " + WindowsIdentity.GetCurrent().User.Value + " | Username = " + Program.user + " | Password = " + Program.pass + " | Response = " + login.KeyAuthApp.response.message);
            }
            else
            {
                Program.user = (string)null;
                Program.pass = (string)null;
            }
            registryKey.Close();
        }
        else
        {
            Console.WriteLine("Enter Your Username");
            Program.user = Console.ReadLine();
            Console.WriteLine("Enter Your Password");
            Program.pass = Console.ReadLine();
            login.initlogin(Program.user, Program.pass);
        }
    }
    #region Aimbot INgame On/Off
    static IEnumerable<long> entityAddresses;

    private static async Task GetProcid()
    {
        Int32 porc = Process.GetProcessesByName("HD-Player")[0].Id;
        PID = Convert.ToString(porc);
    }
    private static async Task RWkey(int input)
    {
        MemLib.OpenProcess(Convert.ToInt32(PID));
        if (input == 0)
        {
            try
            {

                if (PID == "0" || PID == null)

                {

                    Int32 porc = Process.GetProcessesByName("HD-Player")[0].Id;
                    PID = Convert.ToString(porc);
                }
                if (originalValues.Keys.Count < 1)
                {
                    entityAddresses = await MemLib.AoBScan2(("FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 BF"), true);

                }
                if (entityAddresses == null)
                {
                    Console.WriteLine("No Value Found");

                    return;
                }


                if (originalValues.Keys.Count < 1)
                {

                    foreach (var num in entityAddresses)
                    {
                        string str = num.ToString("X");
                        {
                            byte[] originalBytes = MemLib.AhReadMeFucker((num + 92L).ToString("X"), 4);

                            originalValues.Add(num, originalBytes);

                            byte[] valueBytes = MemLib.AhReadMeFucker((num + 144L).ToString("X"), 4);

                            headvalues.Add(num, valueBytes);
                            MemLib.WriteMemory((num + 92L).ToString("X"), "int", BitConverter.ToInt32(valueBytes, 0).ToString());

                        }

                    }
                }
                else
                {
                    try
                    {
                        foreach (var entity in headvalues)
                        {
                            MemLib.WriteMemory((entity.Key + 92L).ToString("X"), "int", BitConverter.ToInt32(entity.Value, 0).ToString());

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" error to enable aimbot");
                        Console.WriteLine(ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error To Finding Aimbot Scan");

            }

        }
        else if (input == 1)
        {
            try
            {
                if (originalValues.Keys.Count < 1)
                {
                    RWkey(0);
                    return;
                }
                else
                {
                    foreach (var entity in originalValues)
                    {
                        MemLib.WriteMemory((entity.Key + 0x5c).ToString("X"), "int", BitConverter.ToInt32(entity.Value, 0).ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }

        }
        MemLib.CloseProcess();
    }

    #endregion


    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();


    [DllImport("user32.dll")]

    static extern bool ShowWindow(IntPtr Hwnd, int nCmdShow);


    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    private static void ExtractEmbeddedResource(string resourceName, string outputPath)
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        // Get the embedded resource stream
        using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
        {
            if (resourceStream == null)
            {
                throw new ArgumentException($"Resource '{resourceName}' not found.");
            }

            // Read the embedded resource and save it to the specified path
            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
            {
                byte[] buffer = new byte[resourceStream.Length];
                resourceStream.Read(buffer, 0, buffer.Length);
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
    #region DLL Importing System By Mirza

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    const uint PROCESS_CREATE_THREAD = 0x2;
    const uint PROCESS_QUERY_INFORMATION = 0x400;
    const uint PROCESS_VM_OPERATION = 0x8;
    const uint PROCESS_VM_WRITE = 0x20;
    const uint PROCESS_VM_READ = 0x10;

    const uint MEM_COMMIT = 0x1000;
    const uint PAGE_READWRITE = 4;


    #endregion

    private static void Main(string[] args)
    {

        Program program = new Program();
        Task.Run((Func<Task>)(() => Program.load())).Wait();
        if (login.islogin)
        {
            Program.createRegistry();

            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);


            string apppath = Process.GetCurrentProcess().MainModule.FileName;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            rk.SetValue("SHARIQHACKS.exe", apppath);

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/Fuck/");
            listener.Start();
            Console.WriteLine("Waiting For Response On Localhost...");


            while (true)
            {

                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                string body;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    body = reader.ReadToEnd();
                }

                if (body.Contains("button=Aimbot_Scan"))
                {
                    Console.WriteLine("Aimbot Scan Trigger....");
                    originalValues.Clear();
                    headvalues.Clear();
                    Task.Run(() => RWkey(0).Wait());
                    Task.Run(() => RWkey(1).Wait());

                }
                else if (body.Contains("button=Enable_Aimbot"))
                {
                    Console.WriteLine("Enable Aimbot");
                    Task.Run(() => RWkey(0).Wait());

                }

                else if (body.Contains("button=Disable_Aimbot"))
                {
                    Console.WriteLine("Disable Aimbot");
                    Task.Run(() => RWkey(1).Wait());


                }


                else if (body.Contains("button=Sniper_Scope"))
                {
                    string string_0;


                    Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                    MemLib.OpenProcess(proc);
                    var enumerable = MemLib.AoBScan(0x0000000000010000, 0x00007ffffffeffff, "Your Scan Here", true, true, string.Empty).Result;
                    string_0 = "0x" + enumerable.FirstOrDefault().ToString("X");
                    foreach (long num in enumerable)
                    {
                        MemLib.WriteMemory(num.ToString("X"), "bytes", "Your Replace Here", string.Empty, null);
                    }


                }



                else if (body.Contains("button=3D_Chams"))
                {


                    string processName = "HD-Player"; // Specify your target process name
                    string dllResourceName = "SHARIQHACKS.Box3D.dll"; // Correct resource name

                    // Extract the embedded msdrmi.dll to a temporary file
                    string tempDllPath = Path.Combine(Path.GetTempPath(), "Box3D.dll");
                    ExtractEmbeddedResource(dllResourceName, tempDllPath);

                    Console.WriteLine($"DLL extracted successfully to: {tempDllPath}");



                    Process[] targetProcesses = Process.GetProcessesByName(processName);
                    if (targetProcesses.Length == 0)
                    {
                        Console.WriteLine($"Waiting for {processName}.exe...");
                    }
                    else
                    {
                        Process targetProcess = targetProcesses[0];
                        IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);

                        IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                        IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);

                        IntPtr bytesWritten;
                        WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);

                        CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                    }
                }

                HttpListenerResponse response = context.Response;
                string responseString = "Trigger SuccessFully Done";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();


            }

        }
    }
}
