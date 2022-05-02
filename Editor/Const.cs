public class Const
{
    public static string[] state_values = { "Open", "In Progress", "Closed" };
    public static string[] priorities = { "Low", "Medium", "High" };

    public static string saveFileName = "UnityTasksSave.txt";

    public static bool SaveFileExists(string path)
    {
        if (System.IO.File.Exists(path))
            return true;
        else
            return false;
    }
}
