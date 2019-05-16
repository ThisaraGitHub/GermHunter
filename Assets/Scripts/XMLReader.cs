using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Text;

public class XMLReader : MonoBehaviour
{
    public TextAsset dictionary;
    public string languageName;
    public int currentLanguage;

    string button1;
    string button2;
    public Text qwe1;
    public Text qwe2;

    List<Dictionary<string, string>> languages = new List<Dictionary<string, string>>();
    Dictionary<string, string> obj;

    private void Awake()
    {
        Reader();
    }

    private void Update()
    {
        languages[currentLanguage].TryGetValue("Name", out languageName);
        languages[currentLanguage].TryGetValue("button1", out button1);
        languages[currentLanguage].TryGetValue("button2", out button2);

        qwe1.text = button1;
        qwe2.text = button2;
    }

    void Reader ()
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dictionary.text);
        XmlNodeList landuageList = xmlDocument.GetElementsByTagName("language");

        foreach(XmlNode languageValue in landuageList)
        {
            XmlNodeList languageContent = languageValue.ChildNodes;
            obj = new Dictionary<string, string>();

            foreach(XmlNode value in languageContent)
            {
                if (value.Name == "Name")
                    obj.Add(value.Name, value.InnerText);
                if (value.Name == "button1")
                    obj.Add(value.Name, value.InnerText);
                if (value.Name == "button2")
                    obj.Add(value.Name, value.InnerText);
            }
            languages.Add(obj);
        }
    }
}
