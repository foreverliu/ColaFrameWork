﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ColaFramework框架中负责UI创建与编辑的编辑器
/// </summary>
public class CreateColaUIEditor
{

    /// <summary>
    /// 快速创建UI模版
    /// </summary>
    [MenuItem("GameObject/UI/ColaUI/UIView", false, 1)]
    public static void CreateColaUIView()
    {
        GameObject uguiRoot = GetOrCreateUGUIRoot();

        //创建新的UI Prefab
        GameObject view = new GameObject("NewUIView", typeof(RectTransform));
        view.layer = LayerMask.NameToLayer("UI");
        view.tag = GloablDefine.UIViewTag;
        string uniqueName = GameObjectUtility.GetUniqueNameForSibling(uguiRoot.transform, view.name);
        view.name = uniqueName;
        Undo.RegisterCreatedObjectUndo(view, "Create" + view.name);
        Undo.SetTransformParent(view.transform, uguiRoot.transform, "Parent" + view.name);
        GameObjectUtility.SetParentAndAlign(view, uguiRoot);

        //设置RectTransform属性
        RectTransform rect = view.GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);

        //设置新建的UIView被选中
        Selection.activeGameObject = view;
    }

    /// <summary>
    /// 导出UI预制件并自动序列化UI组件
    /// </summary>
    [MenuItem("GameObject/UI/ColaUI/ExportUIView", false, 2)]
    public static void ExportUIView()
    {

    }


    /// <summary>
    /// 获取或者创建UGUIRoot（编辑器状态下）
    /// </summary>
    /// <returns></returns>
    private static GameObject GetOrCreateUGUIRoot()
    {
        GameObject uguiRootObj = GameObject.FindWithTag("UIRoot");
        if (uguiRootObj != null)
        {
            Canvas canvas = uguiRootObj.GetComponentInChildren<Canvas>();
            if (null != canvas && canvas.gameObject.activeInHierarchy)
            {
                return canvas.gameObject;
            }
        }

        //如果以上步骤都没有找到，那就从Resource里面加载并实例化一个
        var uguiRootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Arts/Gui/Prefabs/UGUIRoot.prefab");
        GameObject uguiRoot = CommonHelper.InstantiateGoByPrefab(uguiRootPrefab, null);
        GameObject canvasRoot = uguiRoot.GetComponentInChildren<Canvas>().gameObject;
        return canvasRoot;
    }
}
