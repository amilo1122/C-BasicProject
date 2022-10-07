namespace DAL
{
    public class Config
    {
        // Объявляем переменную для хранения ключа
        const string fileFile = @"C:/SQLConnectionString/string.txt";
        // Читаем ключ из файла
        public static string SQLConnectionString = ReadSQLConnectionString(fileFile);

        // Читаем строку подключения к БД
        static string ReadSQLConnectionString(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
    }
}
