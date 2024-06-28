using System;
using System.IO;
using LGF.Editor;
using LGF.Path;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ExcelEditor : EditorWindow
{
    [MenuItem("Tools/Excel/Generate")]
    public static void ShowExample()
    {
        ExcelEditor wnd = GetWindow<ExcelEditor>();
        wnd.titleContent = new GUIContent("ExcelWindow");
    }

    private static string UXMLROOT = "Assets/LGF/Editor/UI/";
    private static string ExcelRoot = Environment.CurrentDirectory + "/数据表/";

    public void CreateGUI()
    {
        Debug.Log(ExcelRoot);

        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UXMLROOT}ExcelWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();

        var searchRoot = labelFromUXML.Q<VisualElement>("SearchElement");
        var scrollRoot = labelFromUXML.Q<ScrollView>("ScrollRoot");
        var field = searchRoot.Q<TextField>("SearchField");
        var createAllBtn = labelFromUXML.Q<Button>("CreateAllBtn");
        // 搜索框发生变化
        field.RegisterValueChangedCallback(Search);

        string[] allExcelFiles = Directory.GetFiles(ExcelRoot, "*.xlsx", SearchOption.AllDirectories);
        allExcelFiles = Array.FindAll(allExcelFiles, s => !s.Contains("~")); // 过滤掉临时文件
        createAllBtn.clicked += () =>
        {
            foreach (string str in allExcelFiles)
            {
                ExcelToJson.Make(ExcelRoot, (file) =>
                    file.EndsWith(str), PathConfig.ExDataPath, PathConfig.ExDataPath);
            }

            EditorUtility.DisplayDialog("提示", "数据生成完毕", "知道了");
            GC.Collect();
        };
        foreach (string str in allExcelFiles)
        {
            var newItem = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UXMLROOT}Item.uxml").Instantiate();

            var nameBtn = newItem.Q<Button>("Btn_Name");
            // 过滤掉路径
            nameBtn.text = Path.GetFileNameWithoutExtension(str);

            nameBtn.clicked += () => { NameBtn_clicked(str); };

            scrollRoot.Add(newItem);
        }

        root.Add(labelFromUXML);
    }

    private void Search(ChangeEvent<string> evt)
    {
        Debug.Log(evt.newValue);
    }

    private void NameBtn_clicked(string str)
    {
        Debug.Log(str);
        ExcelToJson.Make(ExcelRoot, (file) =>
            file.EndsWith(str), PathConfig.ExDataPath, PathConfig.ExDataPath);
        // EditorUtility.DisplayDialog("提示", $"{str}数据生成完毕", "知道了");
        GC.Collect();
    }
}