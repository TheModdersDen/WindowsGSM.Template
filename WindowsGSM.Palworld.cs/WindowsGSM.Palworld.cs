using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Engine;
using WindowsGSM.GameServer.Query;

namespace WindowsGSM.Plugins
{
    public class WindowsGSMPalworldPlugin : SteamCMDAgent
    {
        // - Plugin Details
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.Palworld", // WindowsGSM.XXXX
            author = "TheModdersDen", // author of plugin
            description = "A WindowsGSM plugin for supporting Palworld Dedicated Server", // description for plugin
            version = "0.0.1", // version of plugin
            url = "https://github.com/TheModdersDen/WindowsGSM.Palworld", // Github repository link (Best practice)
            color = "#ffffff" // Color Hex of the plugin panel
        };


        // ==================== USER CONFIGURABLE VALUES ==================== //

        // The server name (feel free to change this to your liking)
        public string ServerName = "A Cool WindowsGSM Server";

        // The server description (feel free to change this to your liking)
        public string ServerDescription = "A Server Created by WindowsGSM";

        // The admin password (used for RCON. PLEASE CHANGE IT TO SOMETHING ELSE IN YOUR SERVER CONFIG!)
        //
        // DO NOT CHANGE IT HERE UNLESS ABSOLUTELY NECESSARY! 
        // If you need to do this, please change it in the server config file as that is more secure.
        // This is just a template password to help you get started (and prevent unauthorized access to your server).
        //
        // If you have issues changing the admin password, or can't connect to RCON, please see the following link:
        //
        // https://github.com/TheModdersDen/WindowsGSM.Palworld/issues/new/ (and fill out the issue template)        
        public string AdminPassword = "CHANGEME54321";

        // =================== END USER CONFIGURABLE VALUES ================= //

        // - Standard Constructor and properties
        public WindowsGSMPalworldPlugin(ServerConfig serverData) : base(serverData) => base.serverData = _serverData = serverData;
        private readonly ServerConfig _serverData;


        // - Settings properties for SteamCMD installer
        public override bool loginAnonymous => false; // Palworld requires to login steam account to install the server, so loginAnonymous = false
        public override string AppId => "2394010"; // Game server appId, Palworld Dedicated Server is 2394010


        // - Game server Fixed variables
        public override string StartPath => @"PalServer"; // Game server start path
        public string FullName = "Palworld Dedicated Server"; // Game server FullName
        public bool AllowsEmbedConsole = false;  // Does this server support output redirect?
        public int PortIncrements = 2; // This tells WindowsGSM how many ports should skip after installation
        public object QueryMethod = A2S(); // Query method, in this case, it is using A2S


        // - Game server default values

        // The default port for this game is 8211. Feel free to change it to your liking in the WindowsGSM UI.
        public string Port = "8211";

        // This game server does not support QueryPort (as of yet, if at all). So, it is being left as a blank port/string.
        public string QueryPort = "";

        // The default RCON port for this game is 8212. Feel free to change it to your liking in the WindowsGSM UI.
        public string RCONPort = "8212";

        // The default region for the game (leaving blank for now, may use it later)
        public string Region = "";

        // So far, this game does not support custom/default maps. So, it is being left as a blank string.
        public string DefaultMap = "";

        // The default max players for this game is 32. Feel free to change it to your liking in the WindowsGSM UI.
        public string MaxPlayers = "32";

        // The difficulty for the server (currently not used, but may be used in the future)
        public string Difficulty = "None";

        // The server password
        public string ServerPassword = "";

        // Additional server start parameter(s) to help improve performance. If you experience any issues, feel free to remove these parameters using your preferred text editor. 
        public string Additional = $"-useperfthreads -NoAsyncLoadingThread -UseMultithreadForDS -publicport {Port} -publicip {} EpicApp=PalServer";

        // - Verify that the file passed contains data:
        public static async bool DoFilesContainData(String filePath)
        {
            string text = await File.ReadAllTextAsync(filePath);
            return !string.IsNullOrWhiteSpace(text);
        }

        // Get the user's public IP for Palworld's config:
        private static async Task<string> GetPublicIpAddressAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("http://checkip.dyndns.org/");
                var startIndex = response.IndexOf("Current IP Address: ", StringComparison.OrdinalIgnoreCase) + 17;
                var endIndex = response.IndexOf("</body>", StringComparison.OrdinalIgnoreCase);
                return response.Substring(startIndex, endIndex - startIndex).Trim();
            }
        }

        private string PublicIPAddress = "";

        // - Create a default cfg for the game server after installation
        public async void CreateServerCFG()
        {
            PublicIPAddress = await WindowsGSMPalworldPlugin.GetPublicIpAddressAsync();
            Console.WriteLine($"Current public ip address: '{PublicIPAddress}'.");
            Console.WriteLine($"To connect to the server, use the following command: 'open {PublicIPAddress}:{Port}'.\nOr simply enter '{PublicIPAddress}:{Port}' in the server browser.");

            string CFGPath = StartPath + "\Pal\Saved\Config\WindowsServer\";

            if (File.Exists(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath, "PalWorldSettings.ini")))
            {
                // Verify that the config file is valid (not blank or empty)
                string configText = File.ReadAllText(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath, "PalWorldSettings.ini"));
                if (string.IsNullOrWhiteSpace(configText))
                {
                    File.Delete(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath, "PalWorldSettings.ini"));
                    CreateServerCFG();
                }
                else
                {
                    Console.WriteLine("Config file already exists, skipping creation.");
                }
            }
            else
            {
                string configPath = ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath, "PalWorldSettings.ini");
                Directory.CreateDirectory(Path.GetDirectoryName(configPath));

                StringBuilder sb = new StringBuilder();
                sb.Append($"[/Script/Pal.PalGameWorldSettings]\nOptionSettings=(Difficulty={Difficulty},DayTimeSpeedRate=1.000000,NightTimeSpeedRate=1.000000,ExpRate=1.000000,PalCaptureRate=1.000000,PalSpawnNumRate=1.000000,PalDamageRateAttack=1.000000,PalDamageRateDefense=1.000000,PlayerDamageRateAttack=1.000000,PlayerDamageRateDefense=1.000000,PlayerStomachDecreaceRate=1.000000,PlayerStaminaDecreaceRate=1.000000,PlayerAutoHPRegeneRate=1.000000,PlayerAutoHpRegeneRateInSleep=1.000000,PalStomachDecreaceRate=1.000000,PalStaminaDecreaceRate=1.000000,PalAutoHPRegeneRate=1.000000,PalAutoHpRegeneRateInSleep=1.000000,BuildObjectDamageRate=1.000000,BuildObjectDeteriorationDamageRate=1.000000,CollectionDropRate=1.000000,CollectionObjectHpRate=1.000000,CollectionObjectRespawnSpeedRate=1.000000,EnemyDropItemRate=1.000000,DeathPenalty=1,bEnablePlayerToPlayerDamage=False,bEnableFriendlyFire=False,bEnableInvaderEnemy=True,bActiveUNKO=False,bEnableAimAssistPad=True,bEnableAimAssistKeyboard=False,DropItemMaxNum=3000,DropItemMaxNum_UNKO=100,BaseCampMaxNum=128,BaseCampWorkerMaxNum=15,DropItemAliveMaxHours=1.000000,bAutoResetGuildNoOnlinePlayers=False,AutoResetGuildTimeNoOnlinePlayers=72.000000,GuildPlayerMaxNum=20,PalEggDefaultHatchingTime=72.000000,WorkSpeedRate=1.000000,bIsMultiplay=False,bIsPvP=False,bCanPickupOtherGuildDeathPenaltyDrop=False,bEnableNonLoginPenalty=True,bEnableFastTravel=True,bIsStartLocationSelectByMap=False,bExistPlayerAfterLogout=False,bEnableDefenseOtherGuildPlayer=False,CoopPlayerMaxNum={MaxPlayers},ServerPlayerMaxNum={MaxPlayers},ServerName=\"{ServerName}\",ServerDescription=\"{ServerDescription}\",AdminPassword=\"{AdminPassword}\",ServerPassword=\"\",PublicPort={Port},PublicIP=\"{PublicIPAddress}\",RCONEnabled=True,RCONPort={RCONPort},Region=\"\",bUseAuth=True,BanListURL=\"https://api.palworldgame.com/api/banlist.txt\");
            }
        }


        // - Start server function, return its Process to WindowsGSM
        public async Task<Process> Start()
        {
            await Task.Delay(5); // Delay start, otherwise Palworld Dedicated Server will immediately crash if started too quickly

            // Prepare start parameter
            var param = new StringBuilder();

            param.Append(Additional);

            // Prepare config file
            CreateServerCFG();

            // Prepare Process
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath),
                    FileName = ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath, "\PalServer.exe"),
                    Arguments = param.ToString(),
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            // Set up Redirect Input and Output to WindowsGSM Console if EmbedConsole is on
            if (AllowsEmbedConsole)
            {
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                var serverConsole = new ServerConsole(_serverData.ServerID);
                p.OutputDataReceived += serverConsole.AddOutput;
                p.ErrorDataReceived += serverConsole.AddOutput;
            }

            // Start Process
            try
            {
                p.Start();
                if (AllowsEmbedConsole)
                {
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }

                return p;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null; // return null if fail to start
            }
        }

        // - Stop server function
        public async Task Stop(Process p)
        {
            await Task.Delay(5); // Delay stop to give the server time to save
            p.Kill();
        }

        // - Get ini files
        public async Task<Dictionary<string, string>> GetIniFiles()
        {
            Dictionary<string, string> iniFiles = new Dictionary<string, string>();
            string CFGPath = StartPath + "\Pal\Saved\Config\WindowsServer\";

            if (Directory.Exists(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath)))
            {
                string[] files = Directory.GetFiles(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath), "*.ini", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    iniFiles.Add(file.Replace(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath), ""), file);
                }
            }

            return iniFiles;
        }

        // - Attempt to install the server
        public async Task<Process> Install()
        {
            var steamCMD = new Installer.SteamCMD();
            Process p = await steamCMD.Install(_serverData.ServerID, string.Empty, AppId, true, loginAnonymous);
            Error = steamCMD.Error;


            return p;
        }

        // - Check if the installation is successful
        public async Task<bool> IsInstallValid()
        {
            // Check Palworld Dedicated Server binary
            if (await GameServer.CheckForBinary(_serverData.ServerID, StartPath, "PalServer.exe"))
            {
                // Verify that the binary is valid (not blank or empty)
                string binaryText = File.ReadAllText(ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath, "PalServer.exe"));
                if (string.IsNullOrWhiteSpace(binaryText))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            string CFGPath = StartPath + "\Pal\Saved\Config\WindowsServer\";

            // Check Palworld Dedicated Server config
            if (await GameServer.CheckForConfig(_serverData.ServerID, CFGPath, "PalWorldSettings.ini"))
            {
                // Verify that the config file is valid (not blank or empty)
                string configText = File.ReadAllText(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath, "PalWorldSettings.ini"));
                if (string.IsNullOrWhiteSpace(configText))
                {
                    File.Delete(ServerPath.GetServersServerFiles(_serverData.ServerID, CFGPath, "PalWorldSettings.ini"));
                    CreateServerCFG();
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // If all checks fail, return false
            Console.WriteLine("All checks failed, returning false.");
            return false;
        }

        // - Update server function
        public async Task<Process> Update(bool validate = false, string custom = null)
        {
            var (p, error) = await Installer.SteamCMD.UpdateEx(_serverData.ServerID, AppId, validate, custom: custom, loginAnonymous: loginAnonymous);
            Error = error;

            if (p.ExitCode == 0)
            {
                // Prepare config file
                CreateServerCFG();
            }
            else
            {
                // If update fails, return null
                Console.WriteLine($"Update failed (Exitcode: {p.ExitCode})! Please check the logs for more information.");
                Console.WriteLine($"\nError: {error}\n");
                return null;
            }

            return p;
        }

        // - Verify that the import of the game server is valid/successful
        public bool IsImportValid(string path)
        {
            string importPath = Path.Combine(path, StartPath);
            Error = $"Invalid Path! Fail to find {Path.GetFileName(StartPath)}";
            return File.Exists(importPath) && File.Exists(Path.Combine(importPath, "\Pal\Saved\Config\WindowsServer\PalWorldSettings.ini")) && DoFilesContainData(Path.Combine(importPath, "\Pal\Saved\Config\WindowsServer\PalWorldSettings.ini")) && File.Exists(Path.Combine(importPath, "\PalServer.exe")) && DoFilesContainData(Path.Combine(importPath, "\PalServer.exe"));
        }

        // - Import function
        public string GetLocalBuild()
        {
            var steamCMD = new Installer.SteamCMD();
            return steamCMD.GetLocalBuild(_serverData.ServerID, AppId);
        }

        // - Get remote build version
        public async Task<string> GetRemoteBuild()
        {
            var steamCMD = new Installer.SteamCMD();
            return await steamCMD.GetRemoteBuild(AppId);
        }

    }

    // - Backup config files
    private async void BackupConfigFiles(string fileName)
    {
        string tmpConfigFile = ServerPath.GetServersServerFiles(_serverData.ServerID, @"tmp2\");
        string directoryPath = ServerPath.GetServersServerFiles(_serverData.ServerID, @"Pal\Saved\Config\WindowsServer\");
        string filePath = Path.Combine(directoryPath, fileName);
        string copySource = Path.Combine(directoryPath, fileName);
        string copyDestination = Path.Combine(tmpConfigFile, fileName);

        await Task.Run(async () =>
        {
            try
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        // Create tmp2 folder for config files
                        if (!Directory.Exists(tmpConfigFile))
                            Directory.CreateDirectory(tmpConfigFile);

                        // Create the nested directories in the destination path
                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(tmpConfigFile, fileName)));

                        // Copy the file to the destination path
                        File.Copy(copySource, copyDestination, true);
                        Console.WriteLine($"Successfully backed up: '{copySource}' to '{copyDestination}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error backing up config: {ex.Message}");
                        Console.WriteLine($"Please try again or manually backup the file '{filePath}'.");
                    }
                }

                Console.WriteLine($"File '{fileName}' deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting files or backing up config: {ex.Message}");
                Console.WriteLine($"Please try again or manually delete the file '{fileName}'.");
            }
        });

        await Task.Delay(1000);
    }

    // - Restore config files
    private async void RestoreConfigFiles()
    {
        Console.WriteLine("Restoring config files...");
        if (Directory.Exists(ServerPath.GetServersServerFiles(_serverData.ServerID, @"tmp2\")))
        {
            string tmpConfigFile = ServerPath.GetServersServerFiles(_serverData.ServerID, @"tmp2\");
            string directoryPath = ServerPath.GetServersServerFiles(_serverData.ServerID, @"Pal\Saved\Config\WindowsServer\");
            string[] files = Directory.GetFiles(tmpConfigFile, "*.ini", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string copySource = Path.Combine(tmpConfigFile, fileName);
                string copyDestination = Path.Combine(directoryPath, fileName);

                try
                {
                    if (File.Exists(copySource))
                    {
                        // Create the nested directories in the destination path
                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(directoryPath, fileName)));

                        // Copy the file to the destination path
                        File.Copy(copySource, copyDestination, true);
                        Console.WriteLine($"Successfully restored: '{copySource}' to '{copyDestination}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error restoring config: {ex.Message}");
                    Console.WriteLine($"Please try again or manually restore the file '{fileName}'.");
                }
            }

            // Delete tmp2 folder
            Directory.Delete(tmpConfigFile, true);
            Console.WriteLine($"Successfully deleted: '{tmpConfigFile}'.");
        }
        else
        {
            Console.WriteLine($"Error restoring config: '{tmpConfigFile}' does not exist.");
        }
    }

    // - Copy files from one directory to another
    private async Task<bool> CopyFiles(string sourceDirectory, string destinationDirectory)
    {
        try
        {
            await Task.Run(() =>
            {
                // Create the nested directories in the destination path
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(destinationDirectory, fileName)));

                // Copy the file to the destination path
                File.Copy(copySource, copyDestination, true);
                Console.WriteLine($"Successfully copied: '{copySource}' to '{copyDestination}'.");
            });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying files: {ex.Message}");
            Console.WriteLine($"Please try again or manually copy the file '{fileName}'.");
            return false;
        }
    }

    // - Delete files from a directory
    private async Task<bool> DeleteFiles(string directoryPath, string[] fileNames)
    {
        try 
        {
            await Task.Run() =>
            { 
                if (File.Exists(directoryPath))
                {
                    // Walk the directory looking for files to delete that match the names in the fileNames string array
                    foreach (string fileName in fileNames)
                    {
                        string filePath = Path.Combine(directoryPath, fileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Console.WriteLine($"File '{fileName}' deleted successfully.");
                        }
                    }
                    return true;
                }
                else 
                {
                    Console.WriteLine($"Error deleting files: '{directoryPath}' does not exist.");
                    return false;
                }
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error deleting files: {ex.Message}");
            Console.WriteLine($"Please try again or manually delete the files found in the folder '{directoryPath}'.");
            return false;
        }
    }
}