using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Localization;
using UnityEngine;
using UnityEngine.UI;

public class CSVReader
{
    public static void Write<T>(T value, string path) where T : BallData
    {
        FieldInfo[] fieldInfos = typeof(T).GetFields();

        using (FileStream filestream = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter filewriter = new StreamWriter(filestream))
            {
                foreach (FieldInfo field in fieldInfos)
                {
                    filewriter.WriteLine(field.GetValue(value) + ",");   
                }
            }
        }
    }

    public static List<string[]> Read(string path)
    {
        var result = new List<string[]>();
        foreach (var line in File.ReadAllLines(path))
        {
            result.Add(line.Split(','));
        }
        return result;
    }

}

public class FileManager : MonoBehaviour
{
    private string jsonFile, jsonFileData, JSONFilesPath, outerJSONFileData, outerJSONPath;

    private string xmlFile, XMLFilesPath, outerXMLPath;

    private string csvFlile, CSVFlilesPath, outerCSVPath;

    private string binFile, BINFilesPath, outerBINPath;

    void Awake()
    {
        string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        string projectName = Application.productName;

        Directory.CreateDirectory(Path.Combine(appDataPath, projectName));

        JSONFilesPath = Path.Combine(appDataPath, projectName, "JSON");
        jsonFile = "/ball_data.json";

        XMLFilesPath = Path.Combine(appDataPath, projectName, "XML");
        xmlFile = "/ball_data.xml";

        CSVFlilesPath = Path.Combine(appDataPath, projectName, "CSV");
        Directory.CreateDirectory(CSVFlilesPath);
        csvFlile = "/ball_data.csv";

        BINFilesPath = Path.Combine(appDataPath, projectName, "BIN");
        Directory.CreateDirectory(BINFilesPath);
        binFile = "/ball_data.dat";
    }

    public void SetOuterPath(TMP_InputField name)
    {
        outerJSONPath = name.text;
        outerXMLPath = name.text;
        outerCSVPath = name.text;
        outerBINPath = name.text;
    }

    public void SaveBINBallData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(BINFilesPath + binFile, FileMode.OpenOrCreate)) 
        {
            formatter.Serialize(stream, BallData.GetInstance());
            stream.Close();
        }
    }
    public void LoadBINBallData()
    {
        if (File.Exists(outerBINPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(outerBINPath, FileMode.Open))
            {
                BallData outerData = formatter.Deserialize(stream) as BallData;
                stream.Close();
                BallData.SetInstance(outerData);
            }
        }
        else 
        {
            Debug.LogError("File not found.");
        }
    }

    public void SaveJSONBallData()
    {
        using (FileStream filestream = new FileStream(JSONFilesPath + jsonFile, FileMode.Create))
        {
            using (StreamWriter filewriter = new StreamWriter(filestream))
            {
                jsonFileData = JsonUtility.ToJson(BallData.GetInstance());
                filewriter.Write(jsonFileData);
            }
        }
    }

    public void LoadJSONBallData()
    {
        using (FileStream filestream = new FileStream(outerJSONPath, FileMode.Open))
        {
            using (StreamReader filereader = new StreamReader(filestream))
            {
                outerJSONFileData = filereader.ReadToEnd();
            }
        }
        BallData.SetInstance(JsonUtility.FromJson<BallData>(outerJSONFileData));
    }

    public void SaveXMLBallData()
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement balls = xmlDoc.CreateElement("balls");
        xmlDoc.AppendChild(balls);

        XmlElement ball = xmlDoc.CreateElement("ball");
        balls.AppendChild(ball);

        XmlElement name = xmlDoc.CreateElement("name");
        name.InnerText = BallData.GetInstance().GetNameBall();
        ball.AppendChild(name);

        XmlElement bounces = xmlDoc.CreateElement("bounces");
        bounces.InnerText = BallData.GetInstance().GetNumBounces().ToString();
        ball.AppendChild(bounces);

        xmlDoc.Save(XMLFilesPath + xmlFile);

    }

    public void LoadXMLBallData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(outerXMLPath);

        XmlNodeList ballList = xmlDoc.GetElementsByTagName("ball");

        foreach (XmlNode ballData in ballList) 
        { 
            XmlNodeList ballContent = ballData.ChildNodes;

            foreach (XmlNode content in ballContent)
            {
                if (content.Name == "name")
                {
                    BallData.GetInstance().SetNameBall(content.InnerText);
                }
                if (content.Name == "bounces")
                {
                    BallData.GetInstance().SetNumBounces(int.Parse(content.InnerText));
                }
            }
        }
    }

    public void SaveCSVBallData()
    {
        CSVReader.Write(BallData.GetInstance(), CSVFlilesPath + csvFlile);
    }

    public void LoadCSVBallData()
    {
        List<string[]> csvValueList = CSVReader.Read(outerCSVPath);

        BallData.GetInstance().SetNameBall(csvValueList[0][0]);
        BallData.GetInstance().SetNumBounces(int.Parse(csvValueList[1][0]));
    }
}