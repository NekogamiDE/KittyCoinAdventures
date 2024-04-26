using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace IdleEngine.SaveSystem
{
    public static class SaveFileManager
    {
        public static readonly string SavePath = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
        public static readonly string HomeRoot = Application.persistentDataPath;

        private static void EnsureSaveFolder()
        {
            if (!System.IO.Directory.Exists(SavePath))
            {
                System.IO.Directory.CreateDirectory(SavePath);
            }
        }

        public static void Write(string filename, string content)
        {
            EnsureSaveFolder();

            FileStream stream = File.Create(System.IO.Path.Combine(SavePath, filename + ".dat"));
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, content);
            stream.Close();

            //var path = System.IO.Path.Combine(SavePath, filename + ".save");

            //System.IO.File.WriteAllText(path, content);
        }

        public static bool TryLoad(string filename, out string content)
        {
            content = string.Empty;

            var path = System.IO.Path.Combine(SavePath, filename + ".dat");

            if (!System.IO.File.Exists(path))
            {
                return false;
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            content = bf.Deserialize(stream).ToString();
            Debug.Log(content);
            stream.Close();
            //content = System.IO.File.ReadAllText(path);
            return true;
        }

        public static void DeleteSaveFiles()
        {
            System.IO.Directory.Delete(SavePath, true);
        }
    }
}