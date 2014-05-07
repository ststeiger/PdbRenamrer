
using System;
using System.Text;


namespace MethodTests
{


    public class FileTextReplacer
    {


        // http://codereview.stackexchange.com/questions/3226/replace-sequence-of-strings-in-binary-file
        public static void ReplaceTextInFile(string inFile, string find, string replace)
        {
            if (find.Length != replace.Length)
                throw new ArgumentException("The lenght of find and replace strings must match!");

            const int chunkPrefix = 1024 * 10;
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            var findBytes = enc.GetBytes(find);
            var replaceBytes = enc.GetBytes(replace);
            long chunkSize = findBytes.Length * chunkPrefix;
            var f = new System.IO.FileInfo(inFile);
            if (f.Length < chunkSize)
                chunkSize = f.Length;

            var readBuffer = new byte[chunkSize];

            using (System.IO.Stream stream = System.IO.File.Open(inFile, System.IO.FileMode.Open))
            {
                int bytesRead;
                while ((bytesRead = stream.Read(readBuffer, 0, readBuffer.Length)) != 0)
                {
                    var replacePositions = new System.Collections.Generic.List<int>();
                    var matches = SearchBytePattern(findBytes, readBuffer, ref replacePositions);
                    if (matches != 0)
                    {
                        foreach (var replacePosition in replacePositions)
                        {
                            var originalPosition = stream.Position;
                            stream.Position = originalPosition - bytesRead + replacePosition;
                            stream.Write(replaceBytes, 0, replaceBytes.Length);
                            stream.Position = originalPosition;
                        } // Next replacePosition
                    } // End if (matches != 0)

                    if (stream.Length == stream.Position)
                        break;

                    var moveBackByHalf = stream.Position - (bytesRead / 2);
                    stream.Position = moveBackByHalf;
                } // Whend

            } // End Using stream

        } // End Sub ReplaceTextInFile


        static public int SearchBytePattern(byte[] pattern, byte[] bytes, ref System.Collections.Generic.List<int> position)
        {
            int matches = 0;

            for (int i = 0; i < bytes.Length; i++)
            {

                if (pattern[0] == bytes[i] && bytes.Length - i >= pattern.Length)
                {

                    bool ismatch = true;
                    for (int j = 1; j < pattern.Length && ismatch == true; j++)
                    {
                        if (bytes[i + j] != pattern[j])
                            ismatch = false;
                    } // Next j

                    if (ismatch)
                    {
                        position.Add(i);
                        matches++;
                        i += pattern.Length - 1;
                    } // End if (ismatch)

                } // End if (pattern[0] == bytes[i] && bytes.Length - i >= pattern.Length)

            } // Next i 

            return matches;
        } // End Function SearchBytePattern


        public static void SizeUnsafeReplaceTextInFile(string strPath, string strTextToSearch, string strTextToReplace)
        {
            if (!System.IO.File.Exists(strPath))
            {
                //throw new System.IO.FileNotFoundException(strPath);
                return;
            }
            byte[] baBuffer = System.IO.File.ReadAllBytes(strPath);
            System.Collections.Generic.List<int> lsReplacePositions = new System.Collections.Generic.List<int>();

            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            byte[] baSearchBytes = enc.GetBytes(strTextToSearch);
            byte[] baReplaceBytes = enc.GetBytes(strTextToReplace);

            var matches = SearchBytePattern(baSearchBytes, baBuffer, ref lsReplacePositions);

            if (matches != 0)
            {

                foreach (var iReplacePosition in lsReplacePositions)
                {

                    for (int i = 0; i < baReplaceBytes.Length; ++i)
                    {
                        baBuffer[iReplacePosition + i] = baReplaceBytes[i];
                    } // Next i

                } // Next iReplacePosition

            } // End if (matches != 0)

            System.IO.File.WriteAllBytes(strPath, baBuffer);

            Array.Clear(baBuffer, 0, baBuffer.Length);
            Array.Clear(baSearchBytes, 0, baSearchBytes.Length);
            Array.Clear(baReplaceBytes, 0, baReplaceBytes.Length);

            baBuffer = null;
            baSearchBytes = null;
            baReplaceBytes = null;
        } // End Sub ReplaceTextInFile


        // MethodTests.FileTextReplacer.Test();
        public static void Test()
        {
            //string str = PartialRegexEscape("[]*abc/%youhuu{}!.*.*");

            string[] astrPdbFiles = new string[] 
            {
                @"D:\Nando.Haemmerli\Desktop\COR-Basic\Releases\currentVersion\bin\Basic.pdb",
                @"D:\Nando.Haemmerli\Desktop\COR-Basic\Releases\currentVersion\bin\Basic_SQL.pdb",
                @"D:\Nando.Haemmerli\Desktop\COR-Basic\Releases\currentVersion\bin\COR-Library.pdb",

                @"D:\Nando.Haemmerli\Desktop\COR-Basic\Releases\raiffeisen\bin\Basic.pdb",
                @"D:\Nando.Haemmerli\Desktop\COR-Basic\Releases\raiffeisen\bin\Basic_SQL.pdb",
                @"D:\Nando.Haemmerli\Desktop\COR-Basic\Releases\raiffeisen\bin\COR-Library.pdb",

                @"D:\Nando.Haemmerli\Desktop\STZH\\Releases\currentVersion\bin\STZH.pdb",
                @"D:\Nando.Haemmerli\Desktop\STZH\\Releases\currentVersion\bin\COR-Library.pdb",

                @"D:\Nando.Haemmerli\Desktop\Portal\Release\CurrentVersion\bin\Portal.pdb",
                @"D:\Nando.Haemmerli\Desktop\Portal\Release\CurrentVersion\bin\Portal_SQL.pdb",

                @"P:\COR_Basic\Release\V2\CurrentVersion\bin\Basic.pdb",
                @"P:\COR_Basic\Release\V2\CurrentVersion\bin\COR-Library.pdb",

                @"P:\SwissRE\Releases\CurrentVersion\bin\SwissRe.pdb",
                @"P:\SwissRE\Releases\CurrentVersion\bin\COR-Library.pdb",
                @"P:\SauterFM\RFC\CurrentVersion\bin\Sauter.pdb",
                @"P:\SauterFM\RFC\CurrentVersion\bin\COR-Library.pdb"
            };

            // SizeUnsafeReplaceTextInFile(strBasicDebug, "Stefan.Steiger", "derPoltergeist");
            // SizeUnsafeReplaceTextInFile(str2, "Stefan.Steiger", "derPoltergeist");


            //ReplaceTextInFile(str, "Stefan.Steiger", "derPoltergeist");
            //ReplaceTextInFile(str2, "Stefan.Steiger", "derPoltergeist");

            System.Collections.Generic.Dictionary<string, string> dictNames = new System.Collections.Generic.Dictionary<string, string>();
            dictNames.Add("Stefan.Steiger", "AutomatedBuild");
         

            // Rico.Luder
            // Ruedi.Angehrn
            // Fabio.Visconti
            // Stefan.Steiger
            // Nando.Haemmerli
            // Thomas.Hartmann
            // Patrick.Sprenger
            // Stefan.Schindler
            // Joerg.Deggelmann


            //Console.WriteLine("Username is: " + Environment.UserName);
            //string userHomePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            // http://stackoverflow.com/questions/1143706/getting-the-path-of-the-home-directory-in-c
            //string strUserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //Console.WriteLine(strProfileName);
            string strHomePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            strHomePath = System.Text.RegularExpressions.Regex.Replace(strHomePath, "Desktop", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string strProfileName = new System.IO.DirectoryInfo(strHomePath).Name;

            string strReplacementName = dictNames[strProfileName];
            foreach (string strFileName in astrPdbFiles)
            {
                SizeUnsafeReplaceTextInFile(strFileName, strProfileName, strReplacementName);
                SizeUnsafeReplaceTextInFile(strFileName, strProfileName.ToLower(), strReplacementName);
            } // Next strFileName 

        } // End Sub Test


    } // End Class FileTextReplacer


} // End Namespace MethodTests
