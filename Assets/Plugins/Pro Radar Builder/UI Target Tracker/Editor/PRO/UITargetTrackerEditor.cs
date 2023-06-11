using System;
using DaiMangou.ProRadarBuilder;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DaiMangou.ProRadarBuilderEditor
{
    [Serializable]
    public class UITargetTrackerEditor : EditorWindow
    {
        [SerializeField] private UITargetTracker _uiTargetTracker;

        private Texture2D On, Off;
        private Vector2 scrollView2D;

        private Vector2 scrollView3D;

        private Rect ScreenRect;

        [MenuItem("Tools/DaiMangou/UI Target Tracker")]
        private static void Init()
        {
            var win = GetWindow(typeof(UITargetTrackerEditor));
            win.minSize = new Vector2(600, 260);
            win.titleContent.text = "Tracker";
        }

        private GameObject _Selection()
        {
            try
            {
                return Selection.activeGameObject;
            }
            catch
            {
                return null;
            }
        }

        public void OnEnable()
        {
            On =
             GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "PowerOn", "png");

            Off =
               GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "PowerOff", "png");

            titleContent.image = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources",
                "TargetTrackerIcon", "png");
        }

        public void OnGUI()
        {
            ScreenRect = new Rect(0, 0, position.width, position.height);
            if (_Selection())
            {
                if (_Selection().GetComponent<UITargetTracker>())
                    _uiTargetTracker = _Selection().GetComponent<UITargetTracker>();


                if (!_Selection().GetComponent<UITargetTracker>())
                {
                    EditorGUILayout.HelpBox(
                        "There is no object with a UI Target Tracker selected. select a radar with a UI Target Tracker component or add one to the selected gameobject",
                        MessageType.Info);

                    if (GUI.Button( ScreenRect.ToCenter(100, 20), "Add to selected"))
                    {
                        _Selection().AddComponent<UITargetTracker>();
                        _uiTargetTracker = _Selection().GetComponent<UITargetTracker>();

                        if (_uiTargetTracker._2dRadar)
                            if (_uiTargetTracker._2dRadar.Blips.Count != _uiTargetTracker.customUITargetDataset.Count)
                                _uiTargetTracker.customUITargetDataset.Resize(_uiTargetTracker._2dRadar.Blips.Count);

                        if (_uiTargetTracker._3dRadar)
                            if (_uiTargetTracker._3dRadar.Blips.Count != _uiTargetTracker.customUITargetDataset.Count)
                                _uiTargetTracker.customUITargetDataset.Resize(_uiTargetTracker._3dRadar.Blips.Count);

                        return;
                    }

                    return;
                }
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "There is no object with a UI Target Tracker selected. select a radar with a UI Target Tracker component or add one to the selected gameobject",
                    MessageType.Info);

                return;
            }

            #region Header Bar

            GUILayout.BeginHorizontal();


            if (GUILayout.Button("Use Scene Scale: " + (_uiTargetTracker.useSceneScale ? "On" : "Off"),
                EditorStyles.toolbarButton, GUILayout.Width(120)))
                _uiTargetTracker.useSceneScale = !_uiTargetTracker.useSceneScale;

            if (GUILayout.Button("Use Lockon: " + (_uiTargetTracker.useLockon ? "On" : "Off"),
                EditorStyles.toolbarButton, GUILayout.Width(120)))
                _uiTargetTracker.useLockon = !_uiTargetTracker.useLockon;

            GUILayout.Box(" ", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width));


            if (!_uiTargetTracker.canvas)
                if (GUILayout.Button("Fix missing canvas", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    var foundObj = GameObject.Find("Target Trackers Canvas");
                    if (foundObj != null)
                    {
                        _uiTargetTracker.canvas = foundObj.GetComponent<Canvas>();
                        _uiTargetTracker.TargetTrackerParentObject = foundObj;
                    }
                    else
                    {
                        var newTargetTrackerParent = new GameObject("Target Trackers Canvas");
                        newTargetTrackerParent.AddComponent<Canvas>();

                        newTargetTrackerParent.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

                        newTargetTrackerParent.AddComponent<CanvasScaler>();
                        var canvasScaler = newTargetTrackerParent.GetComponent<CanvasScaler>();
                        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

                        newTargetTrackerParent.AddComponent<GraphicRaycaster>();
                        newTargetTrackerParent.transform.localPosition = Vector3.zero;

                        _uiTargetTracker.canvas = newTargetTrackerParent.GetComponent<Canvas>();
                        _uiTargetTracker.TargetTrackerParentObject = newTargetTrackerParent;
                    }
                }

            if (!_uiTargetTracker.radarcamera)
                if (GUILayout.Button("Fix missing camera", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    if (_uiTargetTracker._2dRadar)
                    {
                        if (_uiTargetTracker._2dRadar.RadarDesign.camera != null)
                        {
                            _uiTargetTracker.radarcamera = _uiTargetTracker._2dRadar.RadarDesign.camera;
                        }
                        else
                        {
                            var cam = GameObject.FindGameObjectWithTag(_uiTargetTracker._2dRadar.RadarDesign.CameraTag)
                                .GetComponent<Camera>();
                            if (cam != null)
                                _uiTargetTracker.radarcamera = cam;
                            else
                                _uiTargetTracker.radarcamera = Camera.main;
                        }
                    }
                    else
                    {
                        if (_uiTargetTracker._3dRadar.RadarDesign.camera != null)
                        {
                            _uiTargetTracker.radarcamera = _uiTargetTracker._3dRadar.RadarDesign.camera;
                        }
                        else
                        {
                            var cam = GameObject.FindGameObjectWithTag(_uiTargetTracker._3dRadar.RadarDesign.CameraTag)
                                .GetComponent<Camera>();
                            if (cam != null)
                                _uiTargetTracker.radarcamera = cam;
                            else
                                _uiTargetTracker.radarcamera = Camera.main;
                        }
                    }
                }

            GUILayout.EndHorizontal();

            #endregion




            #region if there are no blips in the selected radar

            if (_uiTargetTracker.customUITargetDataset.Count == 0)
            {
                EditorGUILayout.HelpBox("There are no blips in the selected radar", MessageType.Info);

                if (GUI.Button( ScreenRect.ToCenter(140, 20), "Open Radar Builder"))
                    GetWindow<ProRadarBuilder.ProRadarBuilderEditorWindow>();
            }

            #endregion

            #region 2D

            if (_uiTargetTracker._2dRadar)
            {
                if (_uiTargetTracker._2dRadar.Blips.Count != _uiTargetTracker.customUITargetDataset.Count)
                    _uiTargetTracker.customUITargetDataset.Resize(_uiTargetTracker._2dRadar.Blips.Count);

                if (_uiTargetTracker._2dRadar == null)
                {
                    if (_uiTargetTracker.GetComponent<_2DRadar>())
                        _uiTargetTracker._2dRadar = _uiTargetTracker.GetComponent<_2DRadar>();
                    else
                        return;
                }





                scrollView2D = EditorGUILayout.BeginScrollView(scrollView2D, false, false);



                for (var i = 0; i < _uiTargetTracker.customUITargetDataset.Count; i++)
                {
                    if (_uiTargetTracker.customUITargetDataset[i] == null)
                        _uiTargetTracker.customUITargetDataset[i] = new CustomUITargetData();

                    var targetedObject = _uiTargetTracker.customUITargetDataset[i];


                    EditorGUILayout.Space();
                    GUILayout.BeginHorizontal();


                    targetedObject.showFoldout = EditorGUILayout.Foldout(targetedObject.showFoldout,
                        _uiTargetTracker._2dRadar.Blips[i].Tag, true);

                    if (_uiTargetTracker.customUITargetDataset[i].targetSprite)
                        GUILayout.Box(_uiTargetTracker.customUITargetDataset[i].targetSprite.texture, "Label",
                            GUILayout.Height(20));

                    if (GUILayout.Button(_uiTargetTracker.customUITargetDataset[i].isActive ? On : Off, "Label",
                        GUILayout.Width(20)))
                        _uiTargetTracker.customUITargetDataset[i].isActive =
                            !_uiTargetTracker.customUITargetDataset[i].isActive;

                    GUILayout.EndHorizontal();


                    //  GUI.DrawTexture(GUILayoutUtility.GetLastRect().ToLowerLeft(0,1),textu)
                    if (targetedObject.showFoldout)
                    {
                        Separator();
                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Scale By Distance");
                        targetedObject.scaleByDistance = GUILayout.Toggle(targetedObject.scaleByDistance,
                            GUIContent.none, GUILayout.MaxWidth(100));
                        GUILayout.EndHorizontal();

                        if (targetedObject.scaleByDistance)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Min");
                            targetedObject.minSize =
                                EditorGUILayout.FloatField(targetedObject.minSize, GUILayout.MaxWidth(45));
                            GUILayout.Label("Max");
                            targetedObject.maxSize =
                                EditorGUILayout.FloatField(targetedObject.maxSize, GUILayout.MaxWidth(45));
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Scale");
                            targetedObject.scale =
                                EditorGUILayout.FloatField(targetedObject.scale, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }


                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        targetedObject.showTextSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showTextSettingsFoldout, _uiTargetTracker._2dRadar.Blips[i].Tag + " Text");


                        if (targetedObject.showTextSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font");
                            targetedObject.NameFont = (Font)EditorGUILayout.ObjectField(targetedObject.NameFont, typeof(Font),
                                false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font Size");
                            targetedObject.fontSize =
                                (int)EditorGUILayout.FloatField(targetedObject.fontSize, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("text Color");
                            targetedObject.textColor =
                                EditorGUILayout.ColorField(targetedObject.textColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.textMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.textMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        targetedObject.showImageSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showImageSettingsFoldout,
                            _uiTargetTracker._2dRadar.Blips[i].Tag + " Sprite");


                        if (targetedObject.showImageSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Target Sprite");
                            targetedObject.targetSprite = (Sprite)EditorGUILayout.ObjectField(
                                targetedObject.targetSprite, typeof(Sprite), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Sprite Color");
                            targetedObject.imageColor =
                                EditorGUILayout.ColorField(targetedObject.imageColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.imageMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.imageMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }


                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        targetedObject.showOffScreenIndicatorSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showOffScreenIndicatorSettingsFoldout,
                            "Off Screen Sprite");
                        if (GUILayout.Button(_uiTargetTracker.customUITargetDataset[i].showOffScreenIndicator ? On : Off, "Label",
                 GUILayout.Width(20)))
                            _uiTargetTracker.customUITargetDataset[i].showOffScreenIndicator =
                                !_uiTargetTracker.customUITargetDataset[i].showOffScreenIndicator;
                        GUILayout.EndHorizontal();


                        if (targetedObject.showOffScreenIndicatorSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Off Screen Sprite");
                            targetedObject.offScreenImageSprite = (Sprite)EditorGUILayout.ObjectField(
                                targetedObject.offScreenImageSprite, typeof(Sprite), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Sprite Color");
                            targetedObject.offScreenImageColor =
                                EditorGUILayout.ColorField(targetedObject.offScreenImageColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.OffScreenImageMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.OffScreenImageMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Scale");
                            targetedObject.OffScreenIconScale = EditorGUILayout.DelayedFloatField(
                                targetedObject.OffScreenIconScale, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                            

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Padding");
                            targetedObject.OffScreenImagePadding = EditorGUILayout.DelayedFloatField(
                                targetedObject.OffScreenImagePadding, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }


                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        targetedObject.showDistanceTextSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showDistanceTextSettingsFoldout,
                            _uiTargetTracker._2dRadar.Blips[i].Tag + " Distance Text");
                        if (GUILayout.Button(_uiTargetTracker.customUITargetDataset[i].showDistance ? On : Off, "Label",
                            GUILayout.Width(20)))
                            _uiTargetTracker.customUITargetDataset[i].showDistance =
                                !_uiTargetTracker.customUITargetDataset[i].showDistance;
                        GUILayout.EndHorizontal();

                        if (targetedObject.showDistanceTextSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font");
                            targetedObject.DistanceTextFont = (Font)EditorGUILayout.ObjectField(
                                targetedObject.DistanceTextFont, typeof(Font), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font Size");
                            targetedObject.distanceFontSize =
                                (int)EditorGUILayout.FloatField(targetedObject.distanceFontSize,
                                    GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("text Color");
                            targetedObject.distanceTextColor =
                                EditorGUILayout.ColorField(targetedObject.distanceTextColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.distanceTextMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.distanceTextMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Ignore This Layer"); targetedObject.layer = EditorGUILayout.LayerField(targetedObject.layer, GUILayout.Width(220));
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Optimization Method");
                        targetedObject.optimizationMethod =
                            (OptimizationMethod)EditorGUILayout.EnumPopup(targetedObject.optimizationMethod,
                                GUILayout.MaxWidth(220));
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Text Positioning");
                        targetedObject.textAnchor =
                            (TextAnchor)EditorGUILayout.EnumPopup(targetedObject.textAnchor,
                                GUILayout.MaxWidth(220));
                        GUILayout.EndHorizontal();

                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }


                    EditorGUILayout.Space();
                    Separator();
                }


                EditorGUILayout.EndScrollView();
            }

            #endregion

            #region 3D

            if (_uiTargetTracker._3dRadar)
            {
                if (_uiTargetTracker._3dRadar.Blips.Count != _uiTargetTracker.customUITargetDataset.Count)
                    _uiTargetTracker.customUITargetDataset.Resize(_uiTargetTracker._3dRadar.Blips.Count);

                if (_uiTargetTracker._3dRadar == null)
                {
                    if (_uiTargetTracker.GetComponent<_3DRadar>())
                        _uiTargetTracker._3dRadar = _uiTargetTracker.GetComponent<_3DRadar>();
                    else
                        return;
                }


                scrollView3D = EditorGUILayout.BeginScrollView(scrollView3D, false, false);

                for (var i = 0; i < _uiTargetTracker.customUITargetDataset.Count; i++)
                {
                    if (_uiTargetTracker.customUITargetDataset[i] == null)
                        _uiTargetTracker.customUITargetDataset[i] = new CustomUITargetData();

                    var targetedObject = _uiTargetTracker.customUITargetDataset[i];


                    EditorGUILayout.Space();
                    GUILayout.BeginHorizontal();


                    targetedObject.showFoldout = EditorGUILayout.Foldout(targetedObject.showFoldout,
                        _uiTargetTracker._3dRadar.Blips[i].Tag, true);

                    if (_uiTargetTracker.customUITargetDataset[i].targetSprite)
                        GUILayout.Box(_uiTargetTracker.customUITargetDataset[i].targetSprite.texture, "Label",
                            GUILayout.Height(20));

                    if (GUILayout.Button(_uiTargetTracker.customUITargetDataset[i].isActive ? On : Off, "Label",
                        GUILayout.Width(20)))
                        _uiTargetTracker.customUITargetDataset[i].isActive =
                            !_uiTargetTracker.customUITargetDataset[i].isActive;

                    GUILayout.EndHorizontal();


                    //  GUI.DrawTexture(GUILayoutUtility.GetLastRect().ToLowerLeft(0,1),textu)
                    if (targetedObject.showFoldout)
                    {
                        Separator();
                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Scale By Distance");
                        targetedObject.scaleByDistance = GUILayout.Toggle(targetedObject.scaleByDistance,
                            GUIContent.none, GUILayout.MaxWidth(100));
                        GUILayout.EndHorizontal();

                        if (targetedObject.scaleByDistance)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Min");
                            targetedObject.minSize =
                                EditorGUILayout.FloatField(targetedObject.minSize, GUILayout.MaxWidth(45));
                            GUILayout.Label("Max");
                            targetedObject.maxSize =
                                EditorGUILayout.FloatField(targetedObject.maxSize, GUILayout.MaxWidth(45));
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Scale");
                            targetedObject.scale =
                                EditorGUILayout.FloatField(targetedObject.scale, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }


                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        targetedObject.showTextSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showTextSettingsFoldout, _uiTargetTracker._3dRadar.Blips[i].Tag + " Text");


                        if (targetedObject.showTextSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font");
                            targetedObject.NameFont = (Font)EditorGUILayout.ObjectField(targetedObject.NameFont, typeof(Font),
                                false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font Size");
                            targetedObject.fontSize =
                                (int)EditorGUILayout.FloatField(targetedObject.fontSize, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("text Color");
                            targetedObject.textColor =
                                EditorGUILayout.ColorField(targetedObject.textColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.textMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.textMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        targetedObject.showImageSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showImageSettingsFoldout,
                            _uiTargetTracker._3dRadar.Blips[i].Tag + " Sprite");


                        if (targetedObject.showImageSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Target Sprite");
                            targetedObject.targetSprite = (Sprite)EditorGUILayout.ObjectField(
                                targetedObject.targetSprite, typeof(Sprite), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Sprite Color");
                            targetedObject.imageColor =
                                EditorGUILayout.ColorField(targetedObject.imageColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.imageMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.imageMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }


                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        targetedObject.showOffScreenIndicatorSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showOffScreenIndicatorSettingsFoldout,
                            "Off Screen Sprite");
                        if (GUILayout.Button(_uiTargetTracker.customUITargetDataset[i].showOffScreenIndicator ? On : Off, "Label",
                 GUILayout.Width(20)))
                            _uiTargetTracker.customUITargetDataset[i].showOffScreenIndicator =
                                !_uiTargetTracker.customUITargetDataset[i].showOffScreenIndicator;
                        GUILayout.EndHorizontal();


                        if (targetedObject.showOffScreenIndicatorSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Off Screen Sprite");
                            targetedObject.offScreenImageSprite = (Sprite)EditorGUILayout.ObjectField(
                                targetedObject.offScreenImageSprite, typeof(Sprite), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Sprite Color");
                            targetedObject.offScreenImageColor =
                                EditorGUILayout.ColorField(targetedObject.offScreenImageColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.OffScreenImageMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.OffScreenImageMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Scale");
                            targetedObject.OffScreenIconScale = EditorGUILayout.DelayedFloatField(
                                targetedObject.OffScreenIconScale, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();


                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Padding");
                            targetedObject.OffScreenImagePadding = EditorGUILayout.DelayedFloatField(
                                targetedObject.OffScreenImagePadding, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();
                        }


                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        targetedObject.showDistanceTextSettingsFoldout = EditorGUILayout.Foldout(
                            targetedObject.showDistanceTextSettingsFoldout,
                            _uiTargetTracker._3dRadar.Blips[i].Tag + " Distance Text");
                        if (GUILayout.Button(_uiTargetTracker.customUITargetDataset[i].showDistance ? On : Off, "Label",
                            GUILayout.Width(20)))
                            _uiTargetTracker.customUITargetDataset[i].showDistance =
                                !_uiTargetTracker.customUITargetDataset[i].showDistance;
                        GUILayout.EndHorizontal();

                        if (targetedObject.showDistanceTextSettingsFoldout)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font");
                            targetedObject.DistanceTextFont = (Font)EditorGUILayout.ObjectField(
                                targetedObject.DistanceTextFont, typeof(Font), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Font Size");
                            targetedObject.distanceFontSize =
                                (int)EditorGUILayout.FloatField(targetedObject.distanceFontSize,
                                    GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("text Color");
                            targetedObject.distanceTextColor =
                                EditorGUILayout.ColorField(targetedObject.distanceTextColor, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Material");
                            targetedObject.distanceTextMaterial = (Material)EditorGUILayout.ObjectField(
                                targetedObject.distanceTextMaterial, typeof(Material), false, GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();

                          /*  GUILayout.BeginHorizontal();
                            GUILayout.Label("Text Positioning");
                            targetedObject.textAnchor =
                                (TextAnchor)EditorGUILayout.EnumPopup(targetedObject.textAnchor,
                                    GUILayout.MaxWidth(220));
                            GUILayout.EndHorizontal();*/
                        }

                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Separator();
                        EditorGUILayout.Space();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Ignore This Layer"); targetedObject.layer = EditorGUILayout.LayerField(targetedObject.layer, GUILayout.Width(220));
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Optimization Method");
                        targetedObject.optimizationMethod =
                            (OptimizationMethod)EditorGUILayout.EnumPopup(targetedObject.optimizationMethod,
                                GUILayout.MaxWidth(220));
                        GUILayout.EndHorizontal();




                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }


                    EditorGUILayout.Space();
                    Separator();
                }

                EditorGUILayout.EndScrollView();
            }

            #endregion

            Repaint();
        }

        #region Separator

        private void Separator()
        {
            // var tex = Application.HasProLicense() ? Textures.lightGray : Textures.gray;
            // GUI.DrawTexture(GUILayoutUtility.GetLastRect().ToLowerLeft(Screen.width - 5, 1, -5, 5), tex);

            var col = Application.HasProLicense() ? Color.gray : Colour.colour(10, 10, 10);

            Handles.BeginGUI();
            Handles.DrawBezier(GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, -5, 5).position,
                GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, Screen.width, 5).position,
                GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, -5, 5).position,
                GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, Screen.width, 5).position,
                col, null, 1);
            Handles.EndGUI();
        }

        #endregion

    }
}