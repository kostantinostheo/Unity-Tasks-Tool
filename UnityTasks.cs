using UnityEngine;
using UnityEditor;
using System.IO;

public class UnityTasks : EditorWindow
{
    string[] state_values = { "Open", "In Progress", "Closed" };
    string[] priorities = { "Low", "Medium", "High" };

    private string saveFileName = "UnityTasksSave.txt";
    private string fileFullPath;
    
    bool[] tasksStates = new bool[20];

    int[] tasksStatus = new int[20];
    int[] tasksPriorities = new int[20];
    string[] tasksTitles = new string[20];
    string[] tasksDexcriptions = new string[20];

    int current = 0;

    public Vector2 scrollPosition = Vector2.zero;


    void Awake()
    {
        fileFullPath = Application.persistentDataPath + "/" + saveFileName;

        for (int i = 0; i < 10; i++)
        {
            tasksStates[i] = false;
            tasksStatus[i] = 0;
            tasksPriorities[i] = 0;
            tasksTitles[i] = "";
            tasksDexcriptions[i] = "";
        }

        if (SaveFileExists(fileFullPath))
        {
            string[] saved_data = File.ReadAllLines(fileFullPath);
            current = saved_data.Length;
            for (int y = 0; y < saved_data.Length; y++)
            {
                /* splitted = status | priority | title | description */
                string[] splited = saved_data[y].Split('|');
                int status = int.Parse(splited[0]);
                int priority = int.Parse(splited[1]);

                tasksStates[y] = true;
                tasksStatus[y] = status;            // splited[0]
                tasksPriorities[y] = priority;      // splited[1]
                tasksTitles[y] = splited[2];        // splited[2]
                tasksDexcriptions[y] = splited[3];  // splited[3]

            }
        }
    }

    [MenuItem("Unity Tools/Unity Tasks")]
    static void Init()
    {
        UnityTasks window = (UnityTasks)EditorWindow.GetWindow(typeof(UnityTasks), false, "Unity Tasks");
        window.position = new Rect(new Vector2(0, 0), new Vector2(300, 400));
        window.Show();
    }

    void OnGUI()
    {
        GUIStyle TitleStyle = new GUIStyle();
        TitleStyle.fontSize = 18;
        TitleStyle.fontStyle = FontStyle.Bold;
        TitleStyle.normal.textColor = Color.white;
        GUIStyle SubTitleStyle = new GUIStyle();
        SubTitleStyle.fontSize = 10;
        SubTitleStyle.fontStyle = FontStyle.Normal;
        SubTitleStyle.normal.textColor = Color.white;
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 20;
        myButtonStyle.fontStyle = FontStyle.Bold;
        myButtonStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Space(10);
        GUILayout.Label("Unity Tasks", TitleStyle);
        GUILayout.Label("Unity Tasks Tool is an open source Unity Editor tool created by Konstantinos Theofilis®", SubTitleStyle);
        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", myButtonStyle, GUILayout.Width(30), GUILayout.Height(30)))
        {
            if (current < tasksStates.Length)
            {
                tasksStates[current] = true;
                current++;
            }
        }
        GUILayout.Label("Create a new Task");
        EditorGUI.BeginDisabledGroup(!SaveFileExists(fileFullPath));
        if (GUILayout.Button("Delete Save", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Delete all saved data.",
            "Are you sure you want to delete all the saved Tasks? " +
            "This action cannot be undone.", "Yes", "No"))
            {
                File.Delete(fileFullPath);
                Debug.Log("Succesfully Deleted");
                AssetDatabase.Refresh();
                Repaint();
            }
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndHorizontal();
        Rect rectPos = EditorGUILayout.GetControlRect();
        Rect rectBox = new Rect(rectPos.x, rectPos.y, rectPos.width+20, rectPos.height + 700);
        Rect viewRect = new Rect(rectPos.x, rectPos.y, rectPos.width+20, rectPos.height + 4600f);

        scrollPosition = GUI.BeginScrollView(rectBox, scrollPosition, viewRect, false, true);

        for (int y = 0; y < tasksStatus.Length; y++)
        {
            if (tasksStates[y])
            {

                GUILayout.Space(5);
                GUILayout.Label(new GUIContent("Task title:", "YOUR TOOLTIP HERE"));
                tasksTitles[y] = GUILayout.TextArea(tasksTitles[y]);
                GUILayout.Label("Task Description:");
                tasksDexcriptions[y] = GUILayout.TextArea(tasksDexcriptions[y], GUILayout.Height(50));
                GUILayout.Space(2);
                GUILayout.BeginHorizontal();
                GUILayout.Label("State:");
                GUILayout.Label("Priority:");
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (tasksStatus[y] == 0)
                    GUI.backgroundColor = new Color(0.4f , 0.6f , 1.0f);
                else if (tasksStatus[y] == 1)
                    GUI.backgroundColor = new Color(0.505f, 0.874f, 0.815f);
                else
                    GUI.backgroundColor = new Color(0.43922f, 0.85882f, 0.43922f);
                tasksStatus[y] = EditorGUILayout.Popup(tasksStatus[y], state_values);
                GUI.backgroundColor = Color.white;

                if (tasksPriorities[y] == 0)
                    GUI.backgroundColor = new Color(0.4f, 0.6f, 1.0f);
                else if (tasksPriorities[y] == 1)
                    GUI.backgroundColor = new Color(1, 0.8f, 0.6f);
                else
                    GUI.backgroundColor = new Color(0.960f, 0.278f, 0.278f);
                tasksPriorities[y] = EditorGUILayout.Popup(tasksPriorities[y], priorities);
                GUI.backgroundColor = Color.white;

                GUILayout.EndHorizontal();
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(tasksTitles[y]));
                if (GUILayout.Button("Save"))
                {
                    string text = tasksStatus[y] + "|" + tasksPriorities[y] + "|" + tasksTitles[y] + "|" + tasksDexcriptions[y] + '\n';
                    File.AppendAllText(fileFullPath, text);
                    Debug.Log("Succesfully Saved");
                    AssetDatabase.Refresh();
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.Space(10);
                GUILine(2);
                GUILayout.Space(5);
            }
        }
        GUI.EndScrollView();
    }

    private bool SaveFileExists(string path)
    {
        if (File.Exists(path))
            return true;
        else
            return false;
    }
    void GUILine(int i_height = 1)
    {

        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private string State(int value)
    {
        return state_values[value];
    }
}
