using System;
using System.IO;
using System.Linq;
using System.Collections;

namespace FileSync
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            { 
                Console.WriteLine("--------------------------- WICHTIG -----------------------------------");
                Console.WriteLine("Bitte darauf achten, dass während der Synchronisation keine Änderungen");
                Console.WriteLine("an beiden Verzeichnissen und Dateien vorgenommen werden.");
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine(@"Bitte das Backup-Verzeichnis eingeben (wenn leer, dann wird 'N:\Bilder_Seidel\Kamera' angenommen)");
                var tmp = Console.ReadLine();
                if (string.IsNullOrEmpty(tmp))
                    tmp = @"N:\Bilder_Seidel\Kamera";

                string BackupDir = tmp;// @"N:\Bilder_Seidel\Kamera";

                Console.WriteLine(@"Bitte das Original-Verzeichnis eingeben (wenn leer, dann wird 'N:\Transcend\Kamera' angenommen)");
                tmp = Console.ReadLine();
                if (string.IsNullOrEmpty(tmp))
                    tmp = @"N:\Transcend\Kamera";

                string SourceDir = tmp; //@"N:\Transcend\Kamera";

                Console.WriteLine("Start: " + DateTime.Now.ToLongTimeString());
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Lese Verzeichnisse...");
                var listCurrentDir = Directory.GetDirectories(@"N:\Transcend\Kamera", "*.*", SearchOption.AllDirectories);

                var iCountChangesOrg = 0;
                var iCountChangesDest = 0;
                var iCountNewFiles = 0;
                var iCountNewDir = 0;
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Synchronisation wird gestartet...");
                foreach (string dir in listCurrentDir)
                {

                    var targetPath = BackupDir + dir.Replace(SourceDir, "");

                    if (Directory.Exists(targetPath) == false)
                    {
                        Directory.CreateDirectory(targetPath);
                        Console.WriteLine(DateTime.Now.ToLongTimeString() + " Verzeichnis '" + targetPath + "' wurde erfolgreich erstellt");
                        iCountNewDir++;
                    }

                    foreach (string file in Directory.GetFiles(dir))
                    {
                        var fiSource = new FileInfo(file);
                        var targetFile = Path.Combine(targetPath, fiSource.Name);

                        if (File.Exists(targetFile))
                        {
                            var fiDest = new FileInfo(targetFile);

                            var LastWriteTimeSource = new DateTime(fiSource.LastWriteTime.Year, fiSource.LastWriteTime.Month, fiSource.LastWriteTime.Day, fiSource.LastWriteTime.Hour, fiSource.LastWriteTime.Minute, fiSource.LastWriteTime.Second);
                            var LastWriteTimeDest = new DateTime(fiDest.LastWriteTime.Year, fiDest.LastWriteTime.Month, fiDest.LastWriteTime.Day, fiDest.LastWriteTime.Hour, fiDest.LastWriteTime.Minute, fiDest.LastWriteTime.Second);


                            if (LastWriteTimeSource > LastWriteTimeDest)
                            {
                                File.Copy(file, targetFile, true);
                                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Änderung an Original '" + file + "' wurde übernommen!");
                                iCountChangesOrg++;
                            }
                            if (LastWriteTimeSource < LastWriteTimeDest)
                            {
                                File.Copy(targetFile, file, true);
                                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Änderung an Sicherung '" + file + "' wurde übernommen!");
                                iCountChangesDest++;
                            }
                        }
                        else
                        {
                            File.Copy(file, targetFile, true);
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " '" + file + "' wurde nach '" + targetFile + "' kopiert!");
                            iCountNewFiles++;
                        };

                    }
                }

                Console.WriteLine();
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Synchronisation beendet!");
                Console.WriteLine("Ende: " + DateTime.Now.ToLongTimeString());
                var iSum = iCountChangesOrg + iCountChangesDest + iCountNewFiles + iCountNewDir;

                if (iSum > 0)
                {
                    if (iCountChangesOrg > 0)
                        Console.WriteLine(iCountChangesOrg + " Änderungen von Original-Verzeichnis nach Backup-Verzeichnis übernommen!");
                    if (iCountChangesDest > 0)
                        Console.WriteLine(iCountChangesOrg + " Änderungen von Backup-Verzeichnis nach Original-Verzechnis übernommen!");
                    if (iCountNewFiles > 0)
                        Console.WriteLine(iCountNewFiles + " neue Dateien gesichert!");
                    if (iCountNewDir > 0)
                        Console.WriteLine(iCountNewDir + " Vereichnisse erstellt!");
                
                    Console.WriteLine("Press Enter for quit");
                    }
                else
                    Console.WriteLine("Keine Dateien/Verzeichnisse hinzugefügt oder geändert!");

                    Console.WriteLine("Press Enter for quit");
                    Console.ReadLine();

                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Press Enter for quit");
                Console.ReadLine();
            }
        }

        static void DurchsucheVerzeichnis(string pfad)
        {
            try
            {
                // Alle Dateien im aktuellen Verzeichnis durchlaufen ???66666666
                string[] files = Directory.GetFiles(pfad);
                foreach (string file in files)
                {
                    Console.WriteLine($"Datei: {file}");
                }

                // Alle Unterverzeichnisse durchlaufen und rekursiv durchsuchen
                string[] directories = Directory.GetDirectories(pfad);
                foreach (string directory in directories)
                {
                    Console.WriteLine($"Verzeichnis: {directory}");
                    DurchsucheVerzeichnis(directory); // Rekursive Methode
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Zugriff verweigert auf: {pfad} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei: {pfad} - {ex.Message}");
            }
        }
    }
}
