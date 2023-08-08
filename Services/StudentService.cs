using SQLite;
using SQLiteDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace SQLiteDemo.Services
{
    public class StudentService : IStudentService
    {

        public List<StudentModel> _data = new List<StudentModel>();
        //private string _jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data.JSON");
        private string _jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Data.JSON");

        private async Task SetUpDb()
        {
            if (!File.Exists(_jsonFilePath))
            {
                List<StudentModel> students = new List<StudentModel>
                {
                };
                string jsonData = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, jsonData);
            }
        }

        public async Task<int> AddStudent(StudentModel student)
        {
            await SetUpDb();
            //string jsonString = File.ReadAllText(_jsonFilePath);
            //_data = JsonSerializer.Deserialize<List<StudentModel>>(jsonString);
            _data.Add(student);
            SaveData();
            return 1;
        }

       
        public async Task<int> DeleteStudent(StudentModel student)
        {
            await SetUpDb();
            //string jsonContent = File.ReadAllText(_jsonFilePath);
            //List<StudentModel> studentss = JsonSerializer.Deserialize<List<StudentModel>>(jsonContent);
            //_data = JsonSerializer.Deserialize<List<StudentModel>>(jsonContent);
            StudentModel itemToDelete = _data.FirstOrDefault(student);
            if (itemToDelete != null)
            {
                _data.Remove(itemToDelete);
                SaveData();
            }
            return 1;
        }

        public async Task<List<StudentModel>> GetStudentList()
        {
            await SetUpDb();
            string json = File.ReadAllText(_jsonFilePath);
            try
            {
                List<StudentModel> studentList = await ReadStudentDataFromJsonFileAsync(_jsonFilePath);
                return studentList;
                //return _data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveData()
        {
            if (File.Exists(_jsonFilePath))
            {
                string jsonString = File.ReadAllText(_jsonFilePath);
                string jsonstr = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, jsonstr);
            }
            else
            {
                _data = new List<StudentModel>();
            }
        }

        public async Task<int> UpdateStudent(StudentModel studentModel)
        {
            await SetUpDb();
            SaveData();
            return 1;
        }


        public static async Task<List<StudentModel>> ReadStudentDataFromJsonFileAsync(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return await JsonSerializer.DeserializeAsync<List<StudentModel>>(fs, options);
            }
        }
    }
}
