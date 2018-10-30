using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using SQLite.Net.Async;
//using SQLite.Net;
//using SQLite.Net.Interop;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace hyphenApp
{
    public class BLL
    {
        //private static SQLiteConnection dbConn;

        public string StatusMessage { get; set; }

        public static async Task InitializeDatabaseAsync()
        {
            //dPatient patient = new dPatient
            //{
            //    ID = -1,
            //    UserID = -1,
            //    Name = "",
            //    Gender= "",
            //    Birthday = "",
            //    ProfilePicturePath = ""
            //};
            //await Task.Run(() => InsertPatientRecord(patient));

            //dPest pest = new dPest
            //{
            //    ID = -1,
            //    PatientID = -1,
            //    Severity = -1,
            //    ActionTaken = "",
            //    Notes = "",
            //    Date = ""
            //};

            //await Task.Run(() => InsertPestRecord(pest));


            //string dbPath = App.Device.GetDocumentsFolder () + "/pestDB.db";

            //if (dbConn == null) {
            //	//var sqlitePlatform = DependencyService.Get<ISQLitePlatform> ();
            //	var sqlitePlatform = App.Device.GetSQLitePlatform();
            //	dbConn = new SQLiteConnection (sqlitePlatform, dbPath, false);
            //}
            //dbConn.CreateTable<dUser> ();
            //dbConn.CreateTable<dReminder> ();
            //dbConn.CreateTable<dPestphoto>();
            //dbConn.CreateTable<dPest> ();
            //dbConn.CreateTable<dPatient> ();	
            //dbConn.CreateTable<dDoctor> ();
        }


        /// <summary>
        /// Executes and SQL query.
        /// </summary>
        /// <returns>The execute.</returns>
        /// <param name="query">Query.</param>
        /// <param name="args">Arguments.</param>
        private static int SqlExecute(string query, params object[] args)
        {
            //return dbConn.Execute (query, args);
            return 0;
        }


        /// <summary>
        /// Executes and SQL query and returns a single result.
        /// </summary>
        /// <returns>The execute scale.</returns>
        /// <param name="query">Query.</param>
        /// <param name="args">Arguments.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        //private static T SqlExecuteScalar<T>(string query, params object[] args)
        //{
        //	return dbConn.ExecuteScalar<T> (query, args);
        //}


        /// <summary>
        /// Gets an object from the database.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="ID">I.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static T SqlGet<T>(object ID) where T : class
        {
            T obj = null;
            try
            {
                //obj = dbConn.Get<T> (ID);
            }
            catch
            {
            }

            if (obj == null)
                return null;

            //if (obj is IEncryptedObject)
            //{
            //	((IEncryptedObject)obj).Decrypt ();
            //}
            return obj;
        }


        /// <summary>
        /// Gets an object from the database.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="ID">I.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static List<T> SqlQuery<T>(string query, params object[] args) where T : class
        {
            //var list = dbConn.Query<T> (query, args);
            //if (list != null)
            //{
            //	foreach (var obj in list)
            //	{
            //		if (obj is IEncryptedObject)
            //		{
            //			((IEncryptedObject)obj).Decrypt ();
            //		}
            //	}
            //}
            //return list;
            return null;
        }


        /// <summary>
        /// Inserts an object into the database.
        /// </summary>
        /// <param name="obj">Object.</param>
        private static void SqlInsert(object obj)
        {
            //if (obj is IEncryptedObject)
            //	((IEncryptedObject)obj).Encrypt ();

            ////dbConn.Insert (obj);

            //if (obj is IEncryptedObject)
            //	((IEncryptedObject)obj).Decrypt ();
        }


        /// <summary>
        /// Inserts an object into the database.
        /// </summary>
        /// <param name="obj">Object.</param>
        private static void SqlDelete<T>(object ID)
        {
            //dbConn.Delete<T>(ID);
        }


        /// <summary>
        /// Inserts an object into the database.
        /// </summary>
        /// <param name="obj">Object.</param>
        private static void SqlUpdate(object obj)
        {
            //if (obj is IEncryptedObject)
            //	((IEncryptedObject)obj).Encrypt ();

            ////dbConn.Update (obj);

            //if (obj is IEncryptedObject)
            //	((IEncryptedObject)obj).Decrypt ();
        }












        /// <summary>
        /// Gets a list of doctors given the User ID.
        /// </summary>
        /// <returns>The doctors by user I.</returns>
        /// <param name="userID">User I.</param>
        //public static List<dDoctor> GetDoctorsByUserID(int userID)
        //{
        //    //return SqlQuery<dDoctor> ("SELECT * FROM dDoctor WHERE UserID = ?", userID);
        //    return null;

        //}


        /// <summary>
        /// Gets the doctor by its ID.
        /// </summary>
        /// <param name="id">Identifier.</param>
        //public static dDoctor GetDoctor(int id)
        //{
        //    //return SqlGet<dDoctor>(id);
        //    return null;
        //}


        /// <summary>
        /// Gets a list of patients tied to the given User ID.
        /// </summary>
        /// <returns>The patients by user I.</returns>
        /// <param name="userId">User identifier.</param>
        public static List<dPatient> GetPatientsByUserID(int userId)
        {
            //return SqlQuery<dPatient> ("SELECT * from dPatient WHERE UserID = ?", userId);
            return null;
        }


        /// <summary>
        /// Gets the pest history of a given patient ID and the month/year.
        /// </summary>
        /// <returns>The pest history by patient identifier month and year.</returns>
        /// <param name="patientID">Patient I.</param>
        /// <param name="month">Month.</param>
        /// <param name="year">Year.</param>
        public static List<dPest> xGetPestHistoryByPatientIDMonthAndYear(
            int patientID, int month, int year)
        {
            //string fromDate = year.ToString("0000") + "/" + month.ToString("00")  + "/01";
            //string toDate = year.ToString("0000")  + "/"  + month.ToString("00") + "/31";
            //return SqlQuery<dPest> (
            //	"SELECT * from dPest WHERE PatientID = ? AND Date >= ? AND Date <= ? ORDER BY Date DESC",
            //	patientID, fromDate, toDate);
            return null;
        }

        public static async Task<List<dPest>> GetPestHistoryByPatientIDMonthAndYear(int patientID, int month, int year)
        {
            try
            {
                List<string> listStr = new List<string>();
                List<dPest> pestList = new List<dPest>();
                string str = await Task.Run(() => ReadDataAsync("dPest.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        string date = arrays[5];
                        string strMonth = date.Substring(5, 2);
                        string strYear = date.Substring(0, 4);
                        if (arrays[1] == patientID.ToString().Trim() && strMonth == month.ToString() && strYear == year.ToString())
                        {
                            dPest pest = new dPest();
                            pest.ID = Convert.ToInt32(arrays[0]);
                            pest.PatientID = Convert.ToInt32(arrays[1]);
                            pest.Severity = Convert.ToInt32(arrays[2]);
                            pest.ActionTaken = arrays[3];
                            pest.Notes = arrays[4];
                            pest.Date = arrays[5];

                            pestList.Add(pest);
                        }
                    }
                    catch { }
                }
                return pestList;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the PEST records for the given patient
        /// between the specific periods.
        /// </summary>
        /// <returns>The pest for chart async.</returns>
        /// <param name="patientID">Patient I.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        //public static List<dPest> GetPestByPatientIDAndDateRange(int patientID, string dateFrom, string dateTo)

        public static async Task<List<dPest>> GetPestByPatientIDAndDateRange(int patientID, string dateFrom, string dateTo)
        {
            try
            {
                DateTime fromDate = DateTime.Parse(dateFrom);
                DateTime toDate = DateTime.Parse(dateTo);
                List<string> listStr = new List<string>();
                List<dPest> pestList = new List<dPest>();
                string str = await Task.Run(() => ReadDataAsync("dPest.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        DateTime date = DateTime.Parse(arrays[5]);
                        if (arrays[1] == patientID.ToString().Trim() && date >= fromDate && date <= toDate)
                        {
                            dPest pest = new dPest();
                            pest.ID = Convert.ToInt32(arrays[0]);
                            pest.PatientID = Convert.ToInt32(arrays[1]);
                            pest.Severity = Convert.ToInt32(arrays[2]);
                            pest.ActionTaken = arrays[3];
                            pest.Notes = arrays[4];
                            pest.Date = arrays[5];

                            pestList.Add(pest);
                        }
                    }
                    catch { }
                }
                return pestList;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the PEST record photos.
        /// </summary>
        /// <returns>The pest photos.</returns>
        /// <param name="pestID">Pest I.</param>
        public static List<dPestphoto> GetPestPhotos(int pestID)
        {
            return null;
            //return SqlQuery<dPestphoto> ("SELECT * from dPestphoto WHERE PestID = ?", pestID);
        }


        /// <summary>
        /// Gets the PEST record photos given the patient ID and
        /// a date range.
        /// </summary>
        /// <returns>The pest photos.</returns>
        /// <param name="pestID">Pest I.</param>
        public static List<dPestphoto> xGetPestPhotos(int patientID, string dateFrom, string dateTo)
        {
            return SqlQuery<dPestphoto>(
                "SELECT * from dPestphoto ph, dPest p " +
                "WHERE p.ID = ph.PestID AND p.PatientID = ? AND p.Date >= ? AND p.Date <= ? " +
                "ORDER BY p.Date ASC, ph.ID ASC ",
                patientID, dateFrom, dateTo);
        }


        /// <summary>
        /// Gets the PEST record photos given the patient ID and
        /// a date range.
        /// </summary>
        /// <returns>The pest photos.</returns>
        /// <param name="pestID">Pest I.</param>
        public static string GetPestPhotosEarliestDate(int patientID)
        {
            //return SqlExecuteScalar<string> (
            //	"SELECT MIN(p.Date) from dPestphoto ph, dPest p "+
            //	"WHERE p.ID = ph.PestID AND p.PatientID = ? " , 
            //	patientID);
            return null;
        }





        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="table">Table.</param>
        /// <param name="ID">I.</param>
        public static void DeleteRecord(string table, int ID)
        {
            if (table == "dPatient")
            {
                SqlDelete<dPatient>(ID);
            }
            if (table == "dPest")
            {
                SqlDelete<dPest>(ID);
            }
            if (table == "dUser")
            {
                SqlDelete<dUser>(ID);
            }
            //if(table=="dReminder"){
            //	SqlDelete<dReminder> (ID);
            //}
            if (table == "dPestphoto")
            {
                SqlDelete<dPestphoto>(ID);
            }
            if (table == "dDoctor")
            {
                SqlDelete<dDoctor>(ID);
            }
        }


        /// <summary>
        /// Insert a record into the database.
        /// </summary>
        /// <param name="record">Record.</param>
        public static void InsertRecord(object record)
        {
            //SqlInsert (record);
        }


        /// <summary>
        /// Updates the record into the database.
        /// </summary>
        /// <param name="record">Record.</param>
        public static void UpdateRecord(object record)
        {
            //SqlUpdate (record);
        }


        /// <summary>
        /// Gets the first patient by the User ID.
        /// </summary>
        /// <returns>The first patient by user.</returns>
        /// <param name="userid">Userid.</param>
        public static dPatient GetFirstPatientByUser(int userid)
        {
            //List<dPatient> patients = SqlQuery<dPatient> (
            //	"SELECT * from dPatient WHERE UserID = ?", userid);
            //if (patients.Count > 0)
            //{
            //	return patients [0];
            //}
            return null;

        }

        /// <summary>
        /// Loads the patient record given it's ID.
        /// </summary>
        /// <returns>The patient.</returns>
        /// <param name="patientID">Patient I.</param>
        //public static dPatient GetPatient(int patientID)
        //{
        //	return SqlGet<dPatient> (patientID);
        //}
        public static async Task<dPatient> GetPatient(int patientID)
        {
            try
            {
                List<string> listStr = new List<string>();
                string str = await Task.Run(() => ReadDataAsync("dPatient.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[0] == patientID.ToString().Trim())
                        {
                            dPatient patient = new dPatient();
                            patient.ID = Convert.ToInt32(arrays[0]);
                            patient.UserID = Convert.ToInt32(arrays[1]);
                            patient.Name = arrays[2];
                            patient.Gender = arrays[3];
                            patient.Birthday = arrays[4];
                            patient.ProfilePicturePath = arrays[5];
                            return patient;
                        }
                    }
                    catch { }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }



        public static async void InsertPatientRecord(dPatient p)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            string str = await Task.Run(() => ReadDataAsync("dPatient.txt"));
            if (str == null)
            {
                str = FormatPatientString(p);
                str += "^";
            }
            else
            {
                str = str.Trim();
                if (str.Length == 0)
                {
                    str = FormatPatientString(p);
                    str += "^";
                }
                else
                {
                    string lastStr = FormatPatientString(p);
                    bool bUpdate = false;
                    listStr.Clear();
                    listStr1.Clear();
                    string[] words = str.Split('^');
                    foreach (string s in words)
                    {
                        listStr.Add(s);
                    }
                    foreach (string s in listStr)
                    {
                        try
                        {
                            string[] arrays = s.Split('~');
                            if (Convert.ToInt32(arrays[0]) == p.ID)
                            {
                                listStr1.Add(lastStr);
                                bUpdate = true;
                            }
                            else
                            {
                                listStr1.Add(s);
                            }
                        }
                        catch { }
                    }
                    if (!bUpdate)
                    {
                        string[] arrays = lastStr.Split('~');
                        if (arrays[0].Length > 0)
                        {
                            listStr1.Add(lastStr);
                        }
                    }
                    string outStr = "";
                    foreach (string s in listStr1)
                    {
                        outStr += s;
                        outStr += "^";
                    }
                    str = outStr;
                }
            }

            if (str != null && str.Length > 0)
            {
                await Task.Run(() => SaveDataAsync(str, "dPatient.txt"));
            }
        }


        //public int ID { get; set; }

        //public int Enabled { get; set; }
        //public int PatientID { get; set; }

        //public string Notes { get; set; }
        //public int Frequency { get; set; }
        //public string FrequencyType { get; set; }
        //public string TimesOfDay { get; set; }
        //public string StartDate { get; set; }
        //public string EndDate { get; set; }
        //public string DaysOfWeek { get; set; }
        //public string ReminderType { get; set; }

        /// <summary>
        /// Loads the reminder given the reminder ID.
        /// </summary>
        /// <returns>The reminder.</returns>
        /// <param name="id">Identifier.</param>
        //public static dReminder GetReminder(int reminderID)
        //{
        //    //return SqlGet<dReminder>(reminderID);
        //    return null;
        //}

        public static async Task<dReminder> GetReminder(int reminderID)
        {
            try
            {
                List<string> listStr = new List<string>();
                List<int> idList = new List<int>();
                string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[0] == reminderID.ToString().Trim())
                        {
                            dReminder reminder = new dReminder();
                            reminder.ID = Convert.ToInt32(arrays[0]);
                            reminder.Enabled = Convert.ToInt32(arrays[1]);
                            reminder.PatientID = Convert.ToInt32(arrays[2]);
                            reminder.Notes = arrays[3];
                            reminder.Frequency = Convert.ToInt32(arrays[4]);
                            reminder.FrequencyType = arrays[5];
                            reminder.TimesOfDay = arrays[6];
                            reminder.StartDate = arrays[7];
                            reminder.EndDate = arrays[8];
                            reminder.DaysOfWeek = arrays[9];
                            reminder.ReminderType = arrays[10];

                            return (reminder);
                        }
                    }
                    catch { }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        public static async Task<int> GetRemindersNextID()
        {
            try
            {
                List<string> listStr = new List<string>();
                List<int> idList = new List<int>();
                string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));

                if (str == null)
                    return 1;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        idList.Add(Convert.ToInt32(arrays[0]));
                    }
                    catch { }
                }
                return idList.Max() + 1;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Loads all reminders that are attached to a patient.
        /// </summary>
        /// <returns>The reminder.</returns>
        /// <param name="id">Identifier.</param>
        //public static List<dReminder> GetAllReminders()
        //{
        //    //return SqlQuery<dReminder>("SELECT r.* FROM dReminder r, dPatient p WHERE p.ID = r.PatientID");
        //    return null;
        //}
        public static async Task<List<dReminder>> GetAllReminders()
        {
            try
            {
                List<string> listStr = new List<string>();
                List<dReminder> reminderList = new List<dReminder>();
                string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        dReminder reminder = new dReminder();
                        reminder.ID = Convert.ToInt32(arrays[0]);
                        reminder.Enabled = Convert.ToInt32(arrays[1]);
                        reminder.PatientID = Convert.ToInt32(arrays[2]);
                        reminder.Notes = arrays[3];
                        reminder.Frequency = Convert.ToInt32(arrays[4]);
                        reminder.FrequencyType = arrays[5];
                        reminder.TimesOfDay = arrays[6];
                        reminder.StartDate = arrays[7];
                        reminder.EndDate = arrays[8];
                        reminder.DaysOfWeek = arrays[9];
                        reminder.ReminderType = arrays[10];

                        reminderList.Add(reminder);
                    }
                    catch { }
                }
                return reminderList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the list of reminders associated with a patient ID
        /// </summary>
        /// <returns>The reminders by patient I.</returns>
        /// <param name="patientID">Patient I.</param>

        public static async Task<List<dReminder>> GetRemindersByPatientID(int patientID)
        {
            try
            {
                List<string> listStr = new List<string>();
                List<dReminder> reminderList = new List<dReminder>();
                string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[2] == patientID.ToString().Trim())
                        {
                            dReminder reminder = new dReminder();
                            reminder.ID = Convert.ToInt32(arrays[0]);
                            reminder.Enabled = Convert.ToInt32(arrays[1]);
                            reminder.PatientID = Convert.ToInt32(arrays[2]);
                            reminder.Notes = arrays[3];
                            reminder.Frequency = Convert.ToInt32(arrays[4]);
                            reminder.FrequencyType = arrays[5];
                            reminder.TimesOfDay = arrays[6];
                            reminder.StartDate = arrays[7];
                            reminder.EndDate = arrays[8];
                            reminder.DaysOfWeek = arrays[9];
                            reminder.ReminderType = arrays[10];

                            reminderList.Add(reminder);
                        }
                    }
                    catch { }
                }
                return reminderList;
            }
            catch
            {
                return null;
            }
        }

        public static async void InsertReminderRecord(dReminder reminder)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));
            if (str == null)
            {
                str = FormatReminderString(reminder);
                str += "^";
            }
            else
            {
                str = str.Trim();
                if (str.Length == 0)
                {
                    str = FormatReminderString(reminder);
                    str += "^";
                }
                else
                {
                    string lastStr = FormatReminderString(reminder);
                    bool bUpdate = false;
                    listStr.Clear();
                    listStr1.Clear();
                    string[] words = str.Split('^');
                    foreach (string s in words)
                    {
                        listStr.Add(s);
                    }
                    foreach (string s in listStr)
                    {
                        try
                        {
                            string[] arrays = s.Split('~');
                            if (arrays[0] == reminder.ID.ToString() && arrays[2] == reminder.PatientID.ToString())
                            {
                                listStr1.Add(lastStr);
                                bUpdate = true;
                            }
                            else
                            {
                                listStr1.Add(s);
                            }
                        }
                        catch { }
                    }
                    if (!bUpdate)
                    {
                        string[] arrays = lastStr.Split('~');
                        if (arrays[1].Length > 0 && arrays[5].Length > 0)
                        {
                            listStr1.Add(lastStr);
                        }
                    }
                    string outStr = "";
                    foreach (string s in listStr1)
                    {
                        outStr += s;
                        outStr += "^";
                    }
                    str = outStr;
                }
            }

            if (str != null && str.Length > 0)
            {
                await Task.Run(() => SaveDataAsync(str, "dReminder.txt"));
            }
        }

        public static async void UpdateReminderRecord(dReminder reminder)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();
            string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));

            listStr.Clear();
            listStr1.Clear();

            string[] words = str.Split('^');
            foreach (string s in words)
            {
                listStr.Add(s);
            }
            foreach (string s in listStr)
            {
                string[] arrays = s.Split('~');
                if (arrays[0] == reminder.ID.ToString() && arrays[2] == reminder.PatientID.ToString())
                {
                    string lastStr = FormatReminderString(reminder);
                    listStr1.Add(lastStr);
                }
                else
                {
                    listStr1.Add(s);
                }
            }
            string outStr = "";
            foreach (string s in listStr1)
            {
                outStr += s;
                outStr += "^";
            }
            await Task.Run(() => SaveDataAsync(outStr, "dReminder.txt"));
        }

        public static async void DeleteReminderRecord(int reminderID)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            listStr.Clear();
            listStr1.Clear();

            string str = await Task.Run(() => ReadDataAsync("dReminder.txt"));

            string[] words = str.Split('^');
            foreach (string s in words)
            {
                listStr.Add(s);
            }
            foreach (string p in listStr)
            {
                try
                {
                    string[] arrays = p.Split('~');
                    if (arrays[0] == reminderID.ToString())
                    {
                    }
                    else
                    {
                        listStr1.Add(p);
                    }
                }
                catch { }
            }
            string outStr = "";
            foreach (string s in listStr1)
            {
                outStr += s;
                outStr += "^";
            }
            await Task.Run(() => SaveDataAsync(outStr, "dReminder.txt"));
        }


        //public int ID { get; set; }
        //public int UserID { get; set; }
        //public string Name { get; set; }
        //public string Clinic { get; set; }
        //public string Email { get; set; }
        //public string Address { get; set; }
        //public string Information { get; set; }

        //public static List<dDoctor> GetDoctorsByUserID(int userID)
        //{
        //    //return SqlQuery<dDoctor> ("SELECT * FROM dDoctor WHERE UserID = ?", userID);
        //    return null;

        //}

        public static async Task<List<dDoctor>> GetDoctorsByUserID(int userID)
        {
            try
            {
                List<string> listStr = new List<string>();
                List<dDoctor> doctorList = new List<dDoctor>();
                string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[1] == userID.ToString().Trim())
                        {
                            dDoctor doctor = new dDoctor();
                            doctor.ID = Convert.ToInt32(arrays[0]);
                            doctor.UserID = Convert.ToInt32(arrays[1]);
                            doctor.Name = arrays[2];
                            doctor.Clinic = arrays[3];
                            doctor.Email = arrays[4];
                            doctor.Address = arrays[5];
                            doctor.Information = arrays[6];

                            doctorList.Add(doctor);
                        }
                    }
                    catch { }
                }
                return doctorList;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<dDoctor>> GetAllDoctors()
        {
            try
            {
                List<string> listStr = new List<string>();
                List<dDoctor> doctorList = new List<dDoctor>();
                string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        dDoctor doctor = new dDoctor();
                        doctor.ID = Convert.ToInt32(arrays[0]);
                        doctor.UserID = Convert.ToInt32(arrays[1]);
                        doctor.Name = arrays[2];
                        doctor.Clinic = arrays[3];
                        doctor.Email = arrays[4];
                        doctor.Address = arrays[5];
                        doctor.Information = arrays[6];

                        doctorList.Add(doctor);
                    }
                    catch { }
                }
                return doctorList;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<dDoctor> GetDoctor(int doctorID)
        {
            try
            {
                List<string> listStr = new List<string>();
                List<int> idList = new List<int>();
                string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[0] == doctorID.ToString().Trim())
                        {
                            dDoctor doctor = new dDoctor();
                            doctor.ID = Convert.ToInt32(arrays[0]);
                            doctor.UserID = Convert.ToInt32(arrays[1]);
                            doctor.Name = arrays[2];
                            doctor.Clinic = arrays[3];
                            doctor.Email = arrays[4];
                            doctor.Address = arrays[5];
                            doctor.Information = arrays[6];
                           
                            return (doctor);
                        }
                    }
                    catch { }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        public static async Task<int> GetDoctorsNextID()
        {
            try
            {
                List<string> listStr = new List<string>();
                List<int> idList = new List<int>();
                string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));

                if (str == null)
                    return 1;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        idList.Add(Convert.ToInt32(arrays[0]));
                    }
                    catch { }
                }
                return idList.Max() + 1;
            }
            catch
            {
                return 1;
            }
        }

        public static async void InsertDoctorRecord(dDoctor doctor)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));
            if (str == null)
            {
                str = FormatDoctorString(doctor);
                str += "^";
            }
            else
            {
                str = str.Trim();
                if (str.Length == 0)
                {
                    str = FormatDoctorString(doctor);
                    str += "^";
                }
                else
                {
                    string lastStr = FormatDoctorString(doctor);
                    bool bUpdate = false;
                    listStr.Clear();
                    listStr1.Clear();
                    string[] words = str.Split('^');
                    foreach (string s in words)
                    {
                        listStr.Add(s);
                    }
                    foreach (string s in listStr)
                    {
                        try
                        {
                            string[] arrays = s.Split('~');
                            if (arrays[0] == doctor.ID.ToString())
                            {
                                listStr1.Add(lastStr);
                                bUpdate = true;
                            }
                            else
                            {
                                listStr1.Add(s);
                            }
                        }
                        catch { }
                    }
                    if (!bUpdate)
                    {
                        string[] arrays = lastStr.Split('~');
                        if (arrays[1].Length > 0 && arrays[5].Length > 0)
                        {
                            listStr1.Add(lastStr);
                        }
                    }
                    string outStr = "";
                    foreach (string s in listStr1)
                    {
                        outStr += s;
                        outStr += "^";
                    }
                    str = outStr;
                }
            }

            if (str != null && str.Length > 0)
            {
                await Task.Run(() => SaveDataAsync(str, "dDoctor.txt"));
            }
        }

        public static async void DeleteDoctorRecord(int doctorID)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            listStr.Clear();
            listStr1.Clear();

            string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));

            string[] words = str.Split('^');
            foreach (string s in words)
            {
                listStr.Add(s);
            }
            foreach (string p in listStr)
            {
                try
                {
                    string[] arrays = p.Split('~');
                    if (arrays[0] == doctorID.ToString())
                    {
                    }
                    else
                    {
                        listStr1.Add(p);
                    }
                }
                catch { }
            }
            string outStr = "";
            foreach (string s in listStr1)
            {
                outStr += s;
                outStr += "^";
            }
            await Task.Run(() => SaveDataAsync(outStr, "dDoctor.txt"));
        }

        public static string FormatDoctorString(dDoctor doctor)
        {
            string str = doctor.ID.ToString();
            str += "~";
            str += doctor.UserID.ToString();
            str += "~";
            str += doctor.Name;
            str += "~";
            str += doctor.Clinic;
            str += "~";
            str += doctor.Email;
            str += "~";
            str += doctor.Address;
            str += "~";
            str += doctor.Information;

            return str;
        }

        public static string FormatReminderString(dReminder reminder)
        {
            string str = reminder.ID.ToString();
            str += "~";
            str += reminder.Enabled.ToString();
            str += "~";
            str += reminder.PatientID.ToString();
            str += "~";
            str += reminder.Notes;
            str += "~";
            str += reminder.Frequency.ToString();
            str += "~";
            str += reminder.FrequencyType;
            str += "~";
            str += reminder.TimesOfDay;
            str += "~";
            str += reminder.StartDate;
            str += "~";
            str += reminder.EndDate;
            str += "~";
            str += reminder.DaysOfWeek;
            str += "~";
            str += reminder.ReminderType;

            return str;
        }

        public static string FormatPatientString(dPatient patient)
        {
            string str = patient.ID.ToString();
            str += "~";
            str += patient.UserID.ToString();
            str += "~";
            str += patient.Name;
            str += "~";
            str += patient.Gender;
            str += "~";
            str += patient.Birthday;
            str += "~";
            str += patient.ProfilePicturePath;

            return str;
        }

        public static string FormatPestString(dPest pest)
        {
            string str = pest.ID.ToString();
            str += "~";
            str += pest.PatientID.ToString();
            str += "~";
            str += pest.Severity.ToString();
            str += "~";
            str += pest.ActionTaken;
            str += "~";
            str += pest.Notes;
            str += "~";
            str += pest.Date;
            //str += "~";
            //str += pest.Day;
            //str += "~";
            //str += pest.Month;
            return str;
        }

        public static string FormatPestPhotoString(dPestphoto p)
        {
            string str = p.ID.ToString();
            str += "~";
            str += p.PestID.ToString();
            str += "~";
            str += p.PatientID.ToString();
            str += "~";
            str += p.PestDate;
            str += "~";
            str += p.FilePath;
            return str;
        }

        public static async Task<List<dPestphoto>> GetPestPhotos(int patientID, string date)
        {
            try
            {
                List<string> listStr = new List<string>();
                List<dPestphoto> photoList = new List<dPestphoto>();
                string str = await Task.Run(() => ReadDataAsync("dPestphoto.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[2] == patientID.ToString().Trim() && arrays[3] == date.Trim())
                        {
                            dPestphoto pestphoto = new dPestphoto();
                            pestphoto.ID = Convert.ToInt32(arrays[0]);
                            pestphoto.PestID = Convert.ToInt32(arrays[1]);
                            pestphoto.PatientID = Convert.ToInt32(arrays[2]);
                            pestphoto.PestDate = arrays[3];
                            pestphoto.FilePath = arrays[4];
                            //pest.Day = arrays[6];
                            //pest.Month = arrays[7];
                            photoList.Add(pestphoto);
                        }
                    }
                    catch { }
                }
                return photoList;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<dPestphoto>> GetPestPhotos(int patientID, string dateFrom, string dateTo)
        {
            try
            {
                DateTime fromDate = DateTime.Parse(dateFrom);
                DateTime toDate = DateTime.Parse(dateTo);
                List<string> listStr = new List<string>();
                List<dPestphoto> photoList = new List<dPestphoto>();
                string str = await Task.Run(() => ReadDataAsync("dPestphoto.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        DateTime date = DateTime.Parse(arrays[3]);
                        if (arrays[2] == patientID.ToString().Trim() && date >= fromDate && date <= toDate)
                        {
                            dPestphoto pestphoto = new dPestphoto();
                            pestphoto.ID = Convert.ToInt32(arrays[0]);
                            pestphoto.PestID = Convert.ToInt32(arrays[1]);
                            pestphoto.PatientID = Convert.ToInt32(arrays[2]);
                            pestphoto.PestDate = arrays[3];
                            pestphoto.FilePath = arrays[4];
                            photoList.Add(pestphoto);
                        }
                    }
                    catch { }
                }
                return photoList;
            }
            catch
            {
                return null;
            }
        }

        public static async void InsertPestPhotoRecord(dPestphoto p)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            string str = await Task.Run(() => ReadDataAsync("dPestphoto.txt"));
            if (str == null)
            {
                str = FormatPestPhotoString(p);
                str += "^";
            }
            else
            {
                str = str.Trim();
                if (str.Length == 0)
                {
                    str = FormatPestPhotoString(p);
                    str += "^";
                }
                else
                {
                    string lastStr = FormatPestPhotoString(p);
                    bool bUpdate = false;
                    listStr.Clear();
                    listStr1.Clear();
                    string[] words = str.Split('^');
                    foreach (string s in words)
                    {
                        listStr.Add(s);
                    }
                    foreach (string s in listStr)
                    {
                        try
                        {
                            string[] arrays = s.Split('~');
                            if (arrays[2] == p.PestDate)
                            {
                                listStr1.Add(lastStr);
                                bUpdate = true;
                            }
                            else
                            {
                                listStr1.Add(s);
                            }
                        }
                        catch { }
                    }
                    if (!bUpdate)
                    {
                        string[] arrays = lastStr.Split('~');
                        if (arrays[2].Length > 0)
                        {
                            listStr1.Add(lastStr);
                        }
                    }
                    string outStr = "";
                    foreach (string s in listStr1)
                    {
                        outStr += s;
                        outStr += "^";
                    }
                    str = outStr;
                }
            }

            if (str != null && str.Length > 0)
            {
                await Task.Run(() => SaveDataAsync(str, "dPestphoto.txt"));
            }
        }

        public static async void InsertPestRecord(dPest pest)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            string str = await Task.Run(() => ReadDataAsync("dPest.txt"));
            if (str == null)
            {
                str = FormatPestString(pest);
                str += "^";
            }
            else
            {
                str = str.Trim();
                if (str.Length == 0)
                {
                    str = FormatPestString(pest);
                    str += "^";
                }
                else
                {
                    string lastStr = FormatPestString(pest);
                    bool bUpdate = false;
                    listStr.Clear();
                    listStr1.Clear();
                    string[] words = str.Split('^');
                    foreach (string s in words)
                    {
                        listStr.Add(s);
                    }
                    foreach (string s in listStr)
                    {
                        try
                        {
                            string[] arrays = s.Split('~');
                            if (arrays[1] == pest.PatientID.ToString() && arrays[5] == pest.Date.Trim())
                            {
                                listStr1.Add(lastStr);
                                bUpdate = true;
                            }
                            else
                            {
                                listStr1.Add(s);
                            }
                        }
                        catch { }
                    }
                    if (!bUpdate)
                    {
                        string[] arrays = lastStr.Split('~');
                        if (arrays[1].Length > 0 && arrays[5].Length > 0)
                        {
                            listStr1.Add(lastStr);
                        }
                    }
                    string outStr = "";
                    foreach (string s in listStr1)
                    {
                        outStr += s;
                        outStr += "^";
                    }
                    str = outStr;
                }
            }

            if (str != null && str.Length > 0)
            {
                await Task.Run(() => SaveDataAsync(str, "dPest.txt"));
            }
        }

        public static async void UpdatePestRecord(dPest pest)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();
            string str = await Task.Run(() => ReadDataAsync("dPest.txt"));

            listStr.Clear();
            listStr1.Clear();

            string[] words = str.Split('^');
            foreach (string s in words)
            {
                listStr.Add(s);
            }
            foreach (string s in listStr)
            {
                string[] arrays = s.Split('~');
                if (arrays[1] == pest.PatientID.ToString() && arrays[5] == pest.Date.Trim())
                {
                    string lastStr = FormatPestString(pest);
                    listStr1.Add(lastStr);
                }
                else
                {
                    listStr1.Add(s);
                }
            }
            string outStr = "";
            foreach (string s in listStr1)
            {
                outStr += s;
                outStr += "^";
            }
            await Task.Run(() => SaveDataAsync(outStr, "dPest.txt"));
        }

        public static async void DeletePestRecord(int patientID, string date)
        {
            List<string> listStr = new List<string>();
            List<string> listStr1 = new List<string>();

            listStr.Clear();
            listStr1.Clear();

            string str = await Task.Run(() => ReadDataAsync("dPest.txt"));

            string[] words = str.Split('^');
            foreach (string s in words)
            {
                listStr.Add(s);
            }
            foreach (string p in listStr)
            {
                try
                {
                    string[] arrays = p.Split('~');
                    if (arrays[1] == patientID.ToString() && arrays[5] == date.Trim())
                    {
                    }
                    else
                    {
                        listStr1.Add(p);
                    }
                }
                catch { }
            }
            string outStr = "";
            foreach (string s in listStr1)
            {
                outStr += s;
                outStr += "^";
            }
            await Task.Run(() => SaveDataAsync(outStr, "dPest.txt"));
        }


        /// <summary>
        /// Checks if a PEST record for the patient exists
        /// </summary>
        /// <returns>The if pest exists.</returns>
        /// <param name="date">Date.</param>
        /// <param name="patientID">Patient I.</param>
        public static async Task<dPest> GetPest(int patientID, string date)
        {
            try
            {
                List<string> listStr = new List<string>();
                string str = await Task.Run(() => ReadDataAsync("dPest.txt"));

                if (str == null)
                    return null;

                string[] words = str.Split('^');
                foreach (string s in words)
                {
                    listStr.Add(s);
                }
                foreach (string s in listStr)
                {
                    try
                    {
                        string[] arrays = s.Split('~');
                        if (arrays[1] == patientID.ToString().Trim() && arrays[5] == date.Trim())
                        {
                            dPest pest = new dPest();
                            pest.ID = Convert.ToInt32(arrays[0]);
                            pest.PatientID = Convert.ToInt32(arrays[1]);
                            pest.Severity = Convert.ToInt32(arrays[2]);
                            pest.ActionTaken = arrays[3];
                            pest.Notes = arrays[4];
                            pest.Date = arrays[5];
                            //pest.Day = arrays[6];
                            //pest.Month = arrays[7];
                            return pest;
                        }
                    }
                    catch { }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> IsPatient()
        {
            try
            {
                string str = await Task.Run(() => ReadDataAsync("dPatient.txt"));

                if (str == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> IsPest()
        {
            try
            {
                string str = await Task.Run(() => ReadDataAsync("dPest.txt"));

                if (str == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the total number of users in the database.
        /// </summary>
        /// <returns>The user count.</returns>
        public static int GetUserCount()
        {
            //return SqlExecuteScalar<int> ("SELECT COUNT(*) FROM dUser");
            return 0;

        }


        /// <summary>
        /// Gets the user by his/her login ID.
        /// </summary>
        /// <returns>The user by login.</returns>
        /// <param name="login">Login.</param>
        public static dUser GetUserByLogin(string login)
        {
            var userRecords = SqlQuery<dUser>("SELECT * FROM dUser WHERE Login = ?", login);
            if (userRecords == null || userRecords.Count == 0)
                return null;

            return userRecords[0];
        }

        public static async Task<string> GetUserEmailID()
        {
            string email = await Task.Run(() => ReadDataAsync());

            return email;
        }

        public static async Task<bool> InsertUserEmailID(string email)
        {
            await Task.Run(() => SaveDataAsync(email));
            return true;
        }

        public static async Task SaveDataAsync(string str)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "email.txt");
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(str.Trim());
            }
        }

        public static async Task<string> ReadDataAsync()
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "email.txt");

            if (backingFile == null || !File.Exists(backingFile))
            {
                return null;
            }

            var str = "";
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    //if (int.TryParse(line, out var newcount))
                    //{
                    //    count = newcount;
                    //}
                    str = line;
                }
            }
            return str;
        }

        public static async Task SaveDataAsync(string str, string filename)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(str.Trim());
            }
        }

        public static async Task<string> ReadDataAsync(string filename)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);

            if (backingFile == null || !File.Exists(backingFile))
            {
                return null;
            }

            var str = "";
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    //if (int.TryParse(line, out var newcount))
                    //{
                    //    count = newcount;
                    //}
                    str = line;
                }
            }
            return str;
        }

        public static dUser GetUserEmail()
        {
            var userRecords = SqlQuery<dUser>("SELECT * FROM dUser");
            if (userRecords == null || userRecords.Count == 0)
                return null;

            return userRecords[0];
        }

        /// <summary>
        /// Gets the user record by the user ID.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="userID">User I.</param>
        public static dUser GetUser(int userID)
        {
            try
            {
                return SqlGet<dUser>(userID);
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Checks if the given user name and password can be used to login
        /// to the app. Returns the user record if so, null otherwise.
        /// </summary>
        /// <returns>The access.</returns>
        /// <param name="user">User.</param>
        /// <param name="pass">Pass.</param>
        public static dUser CheckAccess(string login, string pass)
        {
            var userRecord = GetUserByLogin(login);

            if (userRecord == null)
                return null;

            //if (App.Device.Hash (login + pass) == userRecord.Password)
            //	return userRecord;

            return null;
        }



        /// <summary>
        /// Deletes the photos belonging to a single pest record.
        /// </summary>
        /// <param name="pestId">Pest identifier.</param>
        public static void DeleteRecordPhotos(int pestId)
        {
            SqlExecute("DELETE FROM dPestphoto WHERE PestID = ?", pestId);
        }



        /// <summary>
        /// Gets the photo ID given the photo's file path.
        /// </summary>
        /// <returns>The photo I.</returns>
        /// <param name="photopath">Photopath.</param>
        public static int GetPhotoID(string photopath)
        {
            //return SqlExecuteScalar<int>(
            //	"SELECT ID FROM dPestphoto WHERE FilePath = ?", photopath);
            return 0;
        }



        /// <summary>
        /// Deletes the pest record and photos of a given patient ID.
        /// </summary>
        /// <param name="patientID">Patient I.</param>
        public static void DeletePestRecordAndPhotos(int patientID)
        {
            SqlExecute(
                "DELETE FROM dPestphoto WHERE ID IN " +
                "(SELECT ph.ID FROM dPestPhoto ph, dPest p WHERE ph.PestID = p.ID AND p.PatientID = ?)",
                patientID);

            SqlExecute(
                "DELETE FROM dPest WHERE PatientID = ?",
                patientID);
        }

    }
}