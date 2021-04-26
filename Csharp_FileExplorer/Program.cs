using System;
using System.Text;
using System.IO;

namespace Csharp_FileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands cm = new Commands();
            string rootPath = Directory.GetCurrentDirectory();
            // kiirjuk a parancsokat
            cm.WriteOutCommands();
            //Megnézzük, hogy milyen parancsot ír be a felhasználó amennyiben exit kilép ha más pedig a Switch dönti el
            bool Run = true;
            while (Run)
            {
                string userInput = Console.ReadLine();
                string[] splittedUserInput = userInput.Split(' ');
                switch (splittedUserInput[0].ToLower())
                {
                    case "exit":
                        Run = false;
                        break;
                    case "ls":
                        cm.list(rootPath);
                        break;
                    case "pwd":
                        cm.PrintWorkingDirectory(rootPath);
                        break;
                    case "commands":
                        cm.WriteOutCommands();
                        break;
                    case "cd":
                        rootPath = cm.ChangeDirectory(rootPath, splittedUserInput[1]);
                        break;
                    case "mkdir":
                        cm.MakeDirectory(rootPath, splittedUserInput[1]);
                        break;
                    case "rmdir":
                        cm.DeleteDirectory(rootPath, splittedUserInput[1]);
                        break;
                    case "mkfil":
                        cm.MakeFile(rootPath, splittedUserInput[1]);
                        break;
                    case "rmfil":
                        cm.RemoveFile(rootPath, splittedUserInput[1]);
                        break;
                    case "sort":
                        cm.SortFiles(rootPath);
                        break;
                    default:
                        break;
                }
            };
        }
    }
    class Commands
    {
        public void WriteOutCommands()
        {
            // egy tömbbe elmentjük a parancsokat
            string[] commands = {
                "exit , kilépés",
                "commands , kilistázza a parancsokat",
                "ls , kilistázza az adot mappa tartalmát",
                "pwd , Kiírja hol vagyunk most",
                "cd , Könyvtár váltás",
                "cd.. , Könyvtár váltás felfelé",
                "mkdir , Könyvtár készítés",
                "rmdir , Könyvtár törlés",
                "mkfil , Fájl készítés",
                "rmfil , Fájl törlés",
                "sort , Rendszerezi az adott mappában a fájlokat"
            };
            foreach (var item in commands)
            {
                Console.WriteLine(item);
            }
        }
        // Készít egy bekért könyvtárat
        public void MakeDirectory(string rootPath, string splittedUserInput)
        {
            string path = Path.GetFullPath(Path.Combine(rootPath, @splittedUserInput));
            try
            {
                if (Directory.Exists(path)) Console.WriteLine("A mappa már létezik ezen az útvonalon.");
                Directory.CreateDirectory(path);
                Console.WriteLine("A mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
        }
        // kitöröl egy bekért könyvtárat
        public void DeleteDirectory(string rootPath, string splittedUserInput)
        {
            string path = Path.GetFullPath(Path.Combine(rootPath, @splittedUserInput));
            try
            {
                Directory.Delete(path);
                Console.WriteLine("A könyvtár törölve lett");
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.Message);
            }
        }
        // át lép egy másik könyvtárba
        public string ChangeDirectory(string rootPath, string splittedUserInput)
        {
            try
            {
                if (splittedUserInput == "..") rootPath = Path.GetFullPath(Path.Combine(rootPath, @"..\"));
                else
                {
                    string[] dirs = Directory.GetDirectories(rootPath);
                    foreach (string dir in dirs)
                    {
                        if (dir.Split('\\')[dir.Split('\\').Length - 1] == splittedUserInput) rootPath = Path.GetFullPath(Path.Combine(rootPath, splittedUserInput));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return rootPath;
        }
        // kilistázza az adott mappa tartalmát
        public void list(string rootPath)
        {
            string[] dirs = Directory.GetDirectories(rootPath);
            string[] files = Directory.GetFiles(rootPath);
            foreach (string dir in dirs)
            {
                Console.WriteLine(dir);
            }
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
        public void PrintWorkingDirectory(string rootPath)
        {
            Console.WriteLine(rootPath);
        }
        public void MakeFile(string rootPath, string splittedUserInput)
        {
            // Bekéri a nevét, kivételt kezel bele tesz egy szöveget.
            string path = Path.GetFullPath(Path.Combine(rootPath, @splittedUserInput));
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("Ez egy szöveg");

                    fs.Write(info, 0, info.Length);
                }
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = " ";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba!");
                Console.WriteLine();
                Console.WriteLine(e.ToString());
            }
        }
        public void RemoveFile(string rootPath, string splittedUserInput)
        {
            // bekéri a nevet, kivitélt kezel
            string path = Path.GetFullPath(Path.Combine(rootPath, @splittedUserInput));
            try
            {
                string[] List = Directory.GetFiles(path, ".txt");
                foreach (string f in List)
                {
                    string fName = f.Substring(path.Length + 1);
                    try
                    {
                        File.Copy(Path.Combine(path, fName), Path.Combine(fName));
                    }
                    catch (IOException copyError)
                    {
                        Console.WriteLine(copyError.Message);
                    }
                }
                foreach (string f in List)
                {
                    File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }
        public void SortFiles(string rootPath)
        {
            Files fs = new Files();
            string CurrentTime = DateTime.Now.ToString(@"-dd-MM-yyyy-(hh-mm-ss)");
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(rootPath, CurrentTime)));
            string[] files = Directory.GetFiles(rootPath);
            foreach (string file in files)
            {
                // Itt lehet bővíteni
                if (file.Contains(".mp4") || file.Contains(".mp3") || file.Contains(".wav") || file.Contains(".avi") || file.Contains(".flv") || file.Contains(".mov") || file.Contains(".mpeg") || file.Contains(".wmv")) fs.mediafajlok(rootPath, CurrentTime);
                if (file.Contains(".png") || file.Contains(".jpg") || file.Contains(".jpeg") || file.Contains(".gif") || file.Contains(".bmp") || file.Contains(".tiff") || file.Contains(".webp")) fs.kepek(rootPath, CurrentTime);
                if (file.Contains(".txt") || file.Contains(".pdf") || file.Contains(".docx") || file.Contains(".ppt") || file.Contains(".pptx") || file.Contains(".rtf") || file.Contains(".tex") || file.Contains(".odt") || file.Contains(".xml") || file.Contains(".doc")) fs.doksik(rootPath, CurrentTime);
                if (file.Contains(".blend") || file.Contains(".blend1") || file.Contains(".mdl") || file.Contains(".fbx") || file.Contains(".obj")) fs.blender(rootPath, CurrentTime);
                if (file.Contains(".torrent")) fs.Torrent(rootPath, CurrentTime);
                if (file.Contains(".py")) fs.Python(rootPath, CurrentTime);
                if (file.Contains(".exe")) fs.programok(rootPath, CurrentTime);
                if (file.Contains(".zip") || file.Contains(".rar") || file.Contains(".iso")) fs.Csomagolt(rootPath, CurrentTime);
                if (file.Contains(".pkt") || file.Contains(".pka")) fs.Network(rootPath, CurrentTime);
            }
        }
    }
    class Files
    {
        public void mediafajlok(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/mediafajlok";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {
                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A médiafájlok mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A médiafájlok mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".mp4") || file.Contains(".mp3") || file.Contains(".wav") || file.Contains(".avi") || file.Contains(".flv") || file.Contains(".mov") || file.Contains(".mpeg") || file.Contains(".wmv"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }

        public void kepek(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/kepek";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {
                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A képek mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A képek mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".png") || file.Contains(".jpg") || file.Contains(".jpeg") || file.Contains(".gif") || file.Contains(".bmp") || file.Contains(".tiff") || file.Contains(".webp"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }

        public void doksik(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/doksik";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {

                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A doksik mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A doksik mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".txt") || file.Contains(".pdf") || file.Contains(".docx") || file.Contains(".ppt") || file.Contains(".pptx") || file.Contains(".rtf") || file.Contains(".tex") || file.Contains(".odt") || file.Contains(".xml") || file.Contains(".doc"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }

        public void blender(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/blender";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {
                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A blender mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A blender mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".blend") || file.Contains(".blend1") || file.Contains(".mdl") || file.Contains(".fbx") || file.Contains(".obj"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }

        public void Torrent(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/Torrent";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {

                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A Torrent mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A Torrent mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".torrent"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }
        public void Csomagolt(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/csomagolt";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {

                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A Csomagolt mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A Csomagolt mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".zip") || file.Contains(".rar") || file.Contains(".iso"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }

        public void programok(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/programok";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {

                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A programok mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A programok mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".exe"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }

        public void Python(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/Python";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {

                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A Python mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A Python mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".py"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }
        public void Network(string path, string CurrentTime)
        {
            string dirName = CurrentTime + "/Network";
            string path2 = Path.GetFullPath(Path.Combine(path, @dirName));
            try
            {
                if (Directory.Exists(path2))
                {
                    Console.WriteLine("A Network mappa már létezik ezen az útvonalon.");
                    return;
                }
                Directory.CreateDirectory(path2);
                Console.WriteLine("A Network mappa sikeresen létrehozva ekkor {0}.", Directory.GetCreationTime(path2));
            }
            catch (Exception e)
            {
                Console.WriteLine("A folyamat nem ment végbe: {0}", e.ToString());
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".pkt") || file.Contains(".pka"))
                {
                    string ideiglenesfajl;
                    string[] filesplitted = file.Split('\\');
                    ideiglenesfajl = filesplitted[filesplitted.Length - 1];
                    File.Move(Path.Combine(@path, @ideiglenesfajl), Path.Combine(@path2, @ideiglenesfajl));
                }
            }
        }
    }
}