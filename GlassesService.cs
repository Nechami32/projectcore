using OurApi.Models;
using OurApi.Interfaces;
using System.Text.Json;
using System;

namespace OurApi.Services
{
    public class GlassesService : Iglass
    {
        List<Glasse> glasses ; // sửa đổi từ "users" thành "glasses"
        int nextId = 3;
        private string fileName;

        public GlassesService()
        {
            fileName = Path.Combine("data", "glasses.json");
            using (var jsonFile = File.OpenText(fileName))
            {
                glasses = JsonSerializer.Deserialize<List<Glasse>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            
        }
        private void saveToFile()
        {

           File.WriteAllText(fileName, JsonSerializer.Serialize(glasses));
        }
        public List<Glasse> GetAll() => glasses; // מחזיר את כל המשקפיים

        public Glasse Get(int id) => glasses.FirstOrDefault(g => g.id == id); // מחזיר משקפיים לפי ID

        public void Add(Glasse glass) // הוספת משקפיים
        {
            glass.id = nextId++;
            glasses.Add(glass);
            saveToFile();
        }

        public void Delete(int id) // מחיקת משקפיים
        {
            var glass = Get(id);
            if (glass is null)
                return;

            glasses.Remove(glass);
            saveToFile();
        }

        public void Update(Glasse g) // עדכון משקפיים
        {
            var index = glasses.FindIndex(gl => gl.id == g.id);
            if (index == -1)
                return;

            glasses[index] = g;
            saveToFile();
        }

         public int Count => glasses.Count; // מחזיר את מספר המשקפיים
    }
}

