using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GeoDataReporting.Models
{
    public class CatalogManager
    {
        public CatalogManager()
        {
            KeptImgs = new List<string>();
            Timestamp = DateTime.Now.ToString("yyyy.dd.MM HH-mm");
        }
        private dbGeodataEntities db = new dbGeodataEntities();
        public string Company { get; set; }
        public string Timestamp;
        public List<string> KeptImgs;
        // added
        public int CompanyCode { get; private set; }
        public int RepId { get; private set; }
        public string ImgReplacementChar { get; set; }

        public List<string> evaluate(string path, bool FileNamesOnly = false)
        {
            var rep = path.Substring(path.Length - 2, 2);
            //var keptImgs = new List<string>();
            var xlFiles = new List<string>();
            var extraImgs = new List<string>();

            Company = path.Substring(21, 4);

            CompanyCode = Convert.ToInt32(path.Substring(21, 4));
            ImgReplacementChar = db.Database.SqlQuery<String>($"select ImgReplacementChar from mSeller.dbo.tblcompany WHERE CompanyCode='{CompanyCode}' AND ImgCharReplacementEnabled = 1").SingleOrDefault();

            RepId = Convert.ToInt32(rep);

            var csvNames = new Dictionary<string, int>();
            csvNames.Add($"{path}\\Extragroupcodes.csv", 1);
            csvNames.Add($"{path}\\Group1codes.csv", 1);
            csvNames.Add($"{path}\\Group2codes.csv", 1);
            csvNames.Add($"{path}\\Prod.csv", 0);
            csvNames.Add($"{path}\\Prod_Groups.csv", 2);

            foreach(var csvName in csvNames.Keys)
            {
                if (File.Exists(csvName))
                {
                    var imgNames = File.ReadAllLines(csvName).Skip(1).Select(line => GetValueAt(line, csvNames[csvName]));
                    xlFiles.AddRange(imgNames);
                }
            }
            #region old
            //var csvName = $"{path}\\Extragroupcodes.csv";
            //if (File.Exists(csvName))
            //{
            //    var imgNames = File.ReadAllLines(csvName).Skip(1).Select(line => GetValueAt(line, 1));
            //    xlFiles.AddRange(imgNames);
            //}
            //csvName = $"{path}\\Group1codes.csv";
            //if (File.Exists(csvName))
            //{
            //    var imgNames = File.ReadAllLines(csvName).Skip(1).Select(line => GetValueAt(line, 1));
            //    xlFiles.AddRange(imgNames);
            //}
            //csvName = $"{path}\\Group2codes.csv";
            //if (File.Exists(csvName))
            //{
            //    var imgNames = File.ReadAllLines(csvName).Skip(1).Select(line => GetValueAt(line, 1));
            //    xlFiles.AddRange(imgNames);
            //}
            //csvName = $"{path}\\Prod.csv";
            //if (File.Exists(csvName))
            //{
            //    var imgNames = File.ReadAllLines(csvName).Skip(1).Select(line => GetValueAt(line, 0));
            //    xlFiles.AddRange(imgNames);
            //}
            //csvName = $"{path}\\Prod_Groups.csv";
            //if (File.Exists(csvName))
            //{
            //    var imgNames = File.ReadAllLines(csvName).Skip(1).Select(line => GetValueAt(line, 2));
            //    xlFiles.AddRange(imgNames);
            //}
            #endregion
            // Filter empty string
            xlFiles = xlFiles.Where(im => !string.IsNullOrWhiteSpace(im)).ToList();

            var allFiles = Directory.GetFiles($"{path.Substring(0, 45)}Files");

            foreach (var file in allFiles)
            {
                var fileName_ext_Array = Path.GetFileName(file).Split('.');
                var folderImage = fileName_ext_Array[0];
                var folderImageExt = fileName_ext_Array.Length > 1 ? fileName_ext_Array[1] : "File";

                if (isOtherThanJPGImage(file)
                    || !xlFiles.Any(xf =>
                                folderImage.Equals(xf, StringComparison.OrdinalIgnoreCase) || (folderImage.StartsWith($"{xf}~", StringComparison.OrdinalIgnoreCase) && folderImageExt.Equals("jpg", StringComparison.OrdinalIgnoreCase)))
                    )
                {
                    //if (isImage(file))
                    extraImgs.Add($"{(FileNamesOnly ? Path.GetFileName(file) : file)}");
                }
                else
                {
                    // Keep image
                    KeptImgs.Add(Path.GetFileName(file));
                } 
            }   

            //LOGGING
            WriteCsv($"C:\\mSellerUtilities\\CatalogMgr\\{Company}\\{RepId} {Timestamp}_xl.csv", xlFiles);
            WriteCsv($"C:\\mSellerUtilities\\CatalogMgr\\{Company}\\{RepId} {Timestamp}_kept.csv", KeptImgs);

            return extraImgs;
        }
        public List<string> GetRepUsersPath(string CompanyPath)
        {
            var comp = new List<string>();
            if (string.IsNullOrWhiteSpace(CompanyPath) || !System.IO.Directory.Exists(CompanyPath))
                return comp;

            var temp = 0;
            var dir = new DirectoryInfo(CompanyPath).GetDirectories();
            comp = dir.Where(rf => int.TryParse(rf.Name, out temp))
                .Select(r => r.FullName + "|" + r.Name + " - iPad")
                .ToList();
            CompanyPath += "iPhone\\";

            if (Directory.Exists(CompanyPath))
            {
                dir = new DirectoryInfo(CompanyPath).GetDirectories();
                comp.AddRange(
                     dir
                    .Where(rf => int.TryParse(rf.Name, out temp))
                    .Select(r => r.FullName + "|" + r + " - iPhone")
                    .ToList());
            }

            return comp;
        }

        public void DeleteFiles(IEnumerable<string> extraImgs, string deletedByUser)
        {
            var act = new UserActivityTracking();
            act.CompanyCode = CompanyCode;
            act.RepId = RepId;
            act.LoggedInUser = deletedByUser;
            act.CreatedOn = DateTime.Now;
            act.ActivityType = "DEL_PHOTO";
            act.ActivityDetail = extraImgs.Count().ToString();

            db.UserActivityTrackings.Add(act);
            db.SaveChanges();
            if (extraImgs.Count() == 0)
            {
                return;
            }

            WriteCsv($"C:\\mSellerUtilities\\CatalogMgr\\{Company}\\{RepId} {Timestamp}_deleted.csv", extraImgs);

            foreach (var f in extraImgs)
            {
                File.Delete(f);
            }

            // Update Last Deleted by in db
        }
        public List<UserActivityTracking> GetTrackingHistory()
        {
            return db.UserActivityTrackings.ToList();
        }
        private string GetValueAt(string row, int index)
        {
            return EscapeFileName(row.Split('~')[index]);
        }
        private bool isImage(string fileName)
        {
            return fileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith("png", StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith("gif", StringComparison.OrdinalIgnoreCase);
        }
        private bool isOtherThanJPGImage(string fileName)
        {
            return isImage(fileName) && !fileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase);
        }
        private string EscapeFileName(string fileName)
        {
            if(CompanyCode == 18)
            {
                return fileName.Replace(" ", "");
            }
            string from = "/", to = "-";
            if (!string.IsNullOrEmpty(ImgReplacementChar))
            {
                from = ImgReplacementChar.Split('=')[0];
                to = ImgReplacementChar.Split('=')[1];
            }
            //.Replace('\\', '-');
             
            return fileName.Replace(from, to);
        }

        private void WriteCsv(string path, IEnumerable<String> list)
        {
            var content = $"index,ImageName\n" + string.Join("\n", list.Select((f, i) => $"{1 + i},{f}"));
            var dirPath = Path.GetDirectoryName(path);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllText(path, content);
        }
    }
}