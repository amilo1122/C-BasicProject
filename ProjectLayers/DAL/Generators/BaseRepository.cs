
using System.Text;

namespace DAL.Generators
{
    public class BaseRepository
    {
        BaseStorage storage = new BaseStorage();

        public int IncrementCartIndex() => GetIndex("cart");
        public int IncrementOrderIndex() => GetIndex("order");
        public int IncrementOrderItemsIndex() => GetIndex("orderItems");
        public int IncrementCategoryIndex() => GetIndex("category");
        public int IncrementGoodIndex() => GetIndex("good");
                
        private int GetIndex(string nameIndex)
        {
            string pathIndex = nameIndex + "Index.txt";
            var index = storage.GetCurrentIndex(pathIndex);
            if (index != -1)
            {
                storage.UpdateIndex(index + 1, pathIndex);
            }
            else
            {
                storage.UpdateIndex(1, pathIndex);
            }
            return storage.GetCurrentIndex(pathIndex);
        }
    }

    public class BaseStorage
    {     
        public void UpdateIndex(int currentIndex, string path)
        {
            using (FileStream fs = File.Create(path))
            {
                byte[] info;
                if (currentIndex == 1)
                {
                    info = new UTF8Encoding(true).GetBytes("1");
                }
                else
                {
                    info = new UTF8Encoding(true).GetBytes(currentIndex.ToString());
                }
                fs.Write(info, 0, info.Length);
            }
        }

        public int GetCurrentIndex(string path)
        {
            int value;
            if (Int32.TryParse(File.ReadAllText(path), out value))
            {
                return value;
            }
            else
            {
                return -1;
            }
        }
    }
}
