﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;

namespace EasyEvents
{
    public class EasyEvents : Plugin<Config>
    {
        public override string Name => "EasyEvents";
        public override string Author => "PintTheDragon";
        public override Version Version => new Version("1.0.5");
        public override PluginPriority Priority => PluginPriority.Highest;

        public static EasyEvents Singleton;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Singleton = this;
            ScriptStore.LoadScripts();
            ScriptActions.AddEvents();
            Exiled.Events.Handlers.Server.RestartingRound += ScriptActions.Reset;
            ScriptActions.Reset();
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Singleton = null;
            ScriptStore.Scripts = new Dictionary<string, string>();
            ScriptActions.RemoveEvents();
            Exiled.Events.Handlers.Server.RestartingRound -= ScriptActions.Reset;
        }
    }
}