﻿using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BeatSaberPlus.UI
{
    /// <summary>
    /// Main view controller
    /// </summary>
    internal class MainView : SDK.UI.ResourceViewController<MainView>
    {
        /// <summary>
        /// Module buttons
        /// </summary>
        private Dictionary<SDK.IModuleBase, Button> m_ModulesButton = new Dictionary<SDK.IModuleBase, Button>();

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

#pragma warning disable CS0649
        [UIObject("ButtonGrid")]
        public GameObject m_ButtonGrid;
#pragma warning restore CS0649

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// On view creation
        /// </summary>
        protected override sealed void OnViewCreation()
        {
            var l_Layout = m_ButtonGrid.GetComponent<UnityEngine.UI.GridLayoutGroup>();
            l_Layout.constraint         = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
            l_Layout.constraintCount    = 3;

            foreach (var l_Module in Plugin.Instance.Modules.Where(x => x.Type == SDK.IModuleBaseType.Integrated))
            {
                var l_Button = SDK.UI.Button.Create(m_ButtonGrid.transform, l_Module.Name, () => {
                    var l_Items = l_Module.GetSettingsUI();
                    MainViewFlowCoordinator.Instance().ChangeView(l_Items.Item1, l_Items.Item2, l_Items.Item3);
                }, l_Module.Description, 35f, 10f);
                m_ModulesButton.Add(l_Module, l_Button);
            }
        }
        /// <summary>
        /// On view activation
        /// </summary>
        protected override sealed void OnViewActivation()
        {
            /// Refresh button states
            foreach (var l_Current in m_ModulesButton)
                l_Current.Value.interactable = l_Current.Key.IsEnabled;

            /// Show welcome message
#if DEBUG
            if (true)
#else
            if (Config.FirstRun)
#endif
            {
                ShowMessageModal("<color=yellow><b>Welcome to BeatSaberPlus!</b></color>\nBy default all modules are disabled, you can enable/disable\nthem any time by clicking the <b><u>Settings</u></b> button below");
                Config.FirstRun = false;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Go to settings
        /// </summary>
        [UIAction("click-btn-settings")]
        private void OnSettingsPressed()
        {
            MainViewFlowCoordinator.Instance().SwitchToSettingsView();
        }
    }
}
